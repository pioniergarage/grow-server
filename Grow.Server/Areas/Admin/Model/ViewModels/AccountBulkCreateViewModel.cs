using Grow.Data;
using Grow.Data.Entities;
using Grow.Server.Model.Helpers;
using Grow.Server.Model.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Grow.Server.Areas.Admin.Model.ViewModels
{
    public class AccountBulkCreateViewModel
    {
        [Required]
        public string Input { get; set; }
        public ICollection<string> TeamNames { get; set; }
        public IDictionary<string, string> Emails { get; set; }
        public IDictionary<string, string> TemporaryPasswords { get; set; }

        public void ParseInput()
        {
            Emails = new Dictionary<string, string>();
            TeamNames = new List<string>();
            var errors = new StringBuilder();

            using (StringReader sr = new StringReader(Input))
            {
                var line = string.Empty;
                const string regex = @"\s*([\w@.]+)\s*,\s*(.*)\s*";
                var lineCounter = 0;

                while ((line = sr.ReadLine()) != null)
                {
                    // Next line
                    lineCounter++;
                    if (string.IsNullOrWhiteSpace(line))
                        continue;

                    // Try to evaluate regex 
                    var match = Regex.Match(line, regex);
                    if (!match.Success)
                    {
                        errors.Append("Could not parse line ").Append(lineCounter).AppendLine();
                        continue;
                    }

                    // Extract email (#1) and team name (#2)
                    var email = match.Groups[1].Value.Trim().ToLower();
                    var teamName = match.Groups[2].Value.Trim();
                    if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(teamName))
                    {
                        errors.Append("Data is missing in line ").Append(lineCounter).AppendLine();
                        continue;
                    }

                    // Add found data to collections
                    if (Emails.ContainsKey(email))
                        errors.Append("Duplicate email found in line ").Append(lineCounter).AppendLine();
                    else
                        Emails.Add(email, teamName);
                    if (!TeamNames.Any(t => t.Equals(teamName, StringComparison.InvariantCultureIgnoreCase)))
                        TeamNames.Add(teamName);
                }
            }

            if (errors.Length > 0)
                throw new InvalidOperationException(errors.ToString());
        }

        public void CreateTeams(GrowDbContext context, int contestId)
        {
            foreach (var team in TeamNames)
            {
                if (context.Teams.Any(t => t.Name.Equals(team, StringComparison.InvariantCultureIgnoreCase)))
                    continue;
                context.Teams.Add(new Team()
                {
                    Name = team,
                    ContestId = contestId,
                    IsActive = true
                });
            }
            context.SaveChanges();
        }

        public async Task CreateAccounts(GrowDbContext context, AccountVmMapper mapper)
        {
            var errors = new List<string>();
            var random = new Random();
            TemporaryPasswords = new Dictionary<string, string>();

            foreach (var (email, teamName) in Emails)
            {
                var teamId = context.Teams.FirstOrDefault(t => t.Name.Equals(teamName, StringComparison.InvariantCultureIgnoreCase))?.Id;
                if (teamId == null)
                {
                    errors.Add($"Email {email} was skipped since corresponding team was not found in the database");
                    continue;
                }

                var account = new AccountEditViewModel()
                {
                    Email = email,
                    Name = email,
                    Password = random.Next(268435456, int.MaxValue).ToString("X"), //produces 8-char hex string
                    IsAdmin = false,
                    IsSuperAdmin = false,
                    IsActive = true,
                    TeamId = teamId
                };

                var result = await mapper.CreateAccountAsync(account).ConfigureAwait(false);
                if (!result.Succeeded)
                    errors.Add(result.Errors.First().Description);
                else
                    TemporaryPasswords.Add(email, account.Password);
            }
        }
    }
}
