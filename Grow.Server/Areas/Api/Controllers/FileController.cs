using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Grow.Data.Entities;
using Grow.Server.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Grow.Data;
using Microsoft.Extensions.Options;
using Grow.Server.Model.Helpers;
using Microsoft.AspNetCore.Authorization;

namespace Grow.Server.Areas.Api.Controllers
{
    [Authorize(Roles = Constants.ADMIN_ROLE_NAME)]
    public class FileController : ApiController<File>
    {
        private StorageConnector Storage => _storage.Value;
        private readonly Lazy<StorageConnector> _storage;

        public FileController(GrowDbContext context, IOptions<AppSettings> settings, ILogger logger) : base(context, logger)
        {
            _storage = new Lazy<StorageConnector>(() => new StorageConnector(settings.Value, Logger));
        }
        
        public override ActionResult<IEnumerable<File>> Find(string category, string search = null)
        {
            IQueryable<File> query = Context.Files;
            if (category != null)
                query = query.Where(e => e.Category.Equals(category, StringComparison.CurrentCultureIgnoreCase));
            if (search != null)
                query = query.Where(e => e.Name.Contains(search, StringComparison.CurrentCultureIgnoreCase));
            return Ok(query.Take(10));
        }

        [HttpPost]
        public ActionResult<File> Upload(IList<IFormFile> fileData, string category = "misc")
        {
            if (!Storage.FolderExists(category))
            {
                return NotFound();
            }

            var files = new List<File>();
            foreach (var formFile in fileData)
            {
                string filename = ContentDispositionHeaderValue.Parse(formFile.ContentDisposition).FileName.Trim('"');

                using (var stream = formFile.OpenReadStream())
                {
                    var file = Storage.Create(category, filename, stream);
                    files.Add(file);
                }
            }

            Context.Files.AddRange(files);
            Context.SaveChanges();

            return Ok(files);
        }
    }
}