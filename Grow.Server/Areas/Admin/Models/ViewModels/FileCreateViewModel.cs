using Grow.Data.Entities;
using Grow.Server.Model;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Grow.Server.Areas.Admin.Models.ViewModels
{
    public class FileCreateViewModel
    {
        [Required]
        public FileCategory Category { get; set; }

        public string FileName { get; set; }

        [Required]
        public IFormFile FileData { get; set; }

        public string AltText { get; set; }
        
        public File ToFile(StorageConnector storage)
        {
            // Create file
            File file;
            using (var stream = FileData.OpenReadStream())
            {
                file = storage.Create(Category.ToString().ToLower(), FileData.FileName, stream);
            }

            // Fill fields
            file.AltText = AltText;
            if (!string.IsNullOrEmpty(FileName))
                file.Name = FileName;

            return file;
        }
    }
}
