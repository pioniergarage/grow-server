using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Grow.Server.Model.Attributes
{
    /// <summary>
    /// Validates that a given file property or field contains only files with a valid extension.
    /// 
    /// Can only be applied to properties or fields of type IFormFile or IEnumerable<IFormFile>.
    /// 
    /// Usage example: [FormFileExtensions("jpg,png")]
    /// </summary>
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public class FormFileExtensionsAttribute : ValidationAttribute
    {
        public string Extensions
        {
            get
            {
                return string.Join(',', AllowedExtensions);
            }
            set
            {
                if (value == null)
                    throw new ArgumentNullException(nameof(Extensions));

                AllowedExtensions = value
                    .Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                    .Select(s => s.Trim(' ', '.').ToLower())
                    .ToList();
            }
        }

        private List<string> AllowedExtensions { get; set; } 
            = new List<string> { "jpg", "jpeg", "gif", "png" };

        public override bool IsValid(object value)
        {
            // Process single file
            if (value is IFormFile file)
            {
                var fileName = file.FileName;

                return IsValidFileName(fileName);
            }

            // Process collection of files
            if (value is IEnumerable<IFormFile> files)
            {
                var allAreValid = true;
                foreach (var singleFile in files)
                {
                    var fileName = singleFile.FileName;
                    allAreValid &= IsValidFileName(fileName);
                }
                return allAreValid;
            }

            return true;
        }

        private bool IsValidFileName(string fileName)
        {
            return AllowedExtensions.Any(y => fileName.EndsWith("." + y, StringComparison.CurrentCultureIgnoreCase));
        }
    }
}
