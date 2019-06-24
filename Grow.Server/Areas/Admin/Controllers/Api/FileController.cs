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

namespace Grow.Server.Areas.Admin.Controllers.Api
{
    public class FileController : ApiController<File>
    {
        private readonly StorageConnector _storage;

        public FileController(GrowDbContext context, IOptions<AppSettings> settings, ILogger logger) : base(context, logger)
        {
            _storage = new StorageConnector(settings.Value, logger);
        }
        
        public ActionResult<IEnumerable<File>> Find(string folder, string search = null)
        {
            IQueryable<File> query = Context.Files
                .Where(e => e.Category.Equals(folder, StringComparison.CurrentCultureIgnoreCase));
            if (search != null)
                query = query.Where(e => e.Name.Contains(search, StringComparison.CurrentCultureIgnoreCase));
            return Ok(query);
        }

        [HttpPost]
        public ActionResult<File> Upload(IList<IFormFile> fileData, string category = "misc")
        {
            if (!_storage.FolderExists(category))
            {
                return NotFound();
            }

            var files = new List<File>();
            foreach (var formFile in fileData)
            {
                string filename = ContentDispositionHeaderValue.Parse(formFile.ContentDisposition).FileName.Trim('"');

                if (_storage.FileExists(category, filename))
                {
                    return Conflict();
                }

                using (var stream = formFile.OpenReadStream())
                {
                    var file = _storage.Create(category, filename, stream);
                    files.Add(file);
                }
            }

            Context.Files.AddRange(files);
            Context.SaveChanges();

            return Ok(files);
        }
    }
}