using Grow.Data.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Grow.Data.Helpers.Attributes
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class FileCategoryAttribute : Attribute
    {
        public FileCategory Category { get; }

        public FileCategoryAttribute(FileCategory category)
        {
            Category = category;
        }
    }
}
