using Grow.Data.Entities;
using Grow.Server.Model.Helpers;
using Microsoft.Azure.Storage;
using Microsoft.Azure.Storage.Auth;
using Microsoft.Azure.Storage.Blob;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace Grow.Server.Model
{
    public class StorageConnector
    {
        public CloudBlobClient CloudBlobClient { get; set; }

        public StorageConnector(AppSettings settings, ILogger logger)
        {
            var connectionString = settings.StorageConnectionString;
            if (!CloudStorageAccount.TryParse(connectionString, out CloudStorageAccount storageAccount))
            {
                Trace.TraceError("Could not connect to storage account");
                return;
            }

            try
            {
                CloudBlobClient = storageAccount.CreateCloudBlobClient();
            }
            catch (Exception e)
            {
                logger.LogError("Could not connect to storage", e);
            }

            EnsureContainersAreCreated();
        }

        public bool IsInStorage(string uri)
        {
            return uri.StartsWith(CloudBlobClient.BaseUri.AbsoluteUri, StringComparison.OrdinalIgnoreCase)
                && CloudBlobClient.GetBlobReferenceFromServer(new Uri(uri)) != null;
        }

        public bool FolderExists(string category)
        {
            return Enum.TryParse(category, true, out FileCategory container);
        }

        public bool FileExists(string category, string fileName)
        {
            var cloudBlobContainer = GetContainerFor(category);
            CloudBlockBlob cloudBlockBlob = cloudBlobContainer.GetBlockBlobReference(fileName);
            return cloudBlockBlob.Exists();
        }

        public File Create(string category, string fileName, System.IO.Stream imageData)
        {
            fileName = ProcessFileName(category, fileName);

            var cloudBlobContainer = GetContainerFor(category);
            CloudBlockBlob cloudBlockBlob = cloudBlobContainer.GetBlockBlobReference(fileName);
            cloudBlockBlob.UploadFromStream(imageData);

            return new File()
            {
                Name = fileName,
                Extension = fileName.Split('.').Last(),
                Category = category.ToLower(),
                IsActive = true,
                Url = cloudBlockBlob.Uri.AbsoluteUri
            };
        }
        
        public void Delete(File file)
        {
            var url = file.Url;
            var fileName = System.IO.Path.GetFileName(url);

            if (FileExists(file.Category, fileName))
            {
                var cloudBlobContainer = GetContainerFor(file.Category);
                CloudBlockBlob cloudBlockBlob = cloudBlobContainer.GetBlockBlobReference(fileName);
                cloudBlockBlob.Delete();
            }
        }

        private string ProcessFileName(string category, string fileName)
        {
            if (fileName.Split('.').Length < 2)
                throw new ArgumentException("File was uploaded without extension");

            var randomSuffix = "";
            if (FileExists(category, fileName))
            {
                randomSuffix = "_" + new Random().Next(4096, 65535).ToString("X4");
            }

            var fileNameWithoutExt = System.IO.Path.GetFileNameWithoutExtension(fileName);
            var fileExtension = fileName.Split('.').Last();

            return string.Format("{0}{1}.{2}",
                fileNameWithoutExt,
                randomSuffix,
                fileExtension
            );
        }

        private void EnsureContainersAreCreated()
        {
            foreach (var containerName in Enum.GetNames(typeof(FileCategory)))
            {
                CloudBlobContainer cloudBlobContainer = CloudBlobClient.GetContainerReference(containerName.ToLower());
                if (!cloudBlobContainer.Exists())
                {
                    cloudBlobContainer.Create();
                    BlobContainerPermissions permissions = new BlobContainerPermissions
                    {
                        PublicAccess = BlobContainerPublicAccessType.Blob
                    };
                    cloudBlobContainer.SetPermissions(permissions);
                }
            }
        }

        private CloudBlobContainer GetContainerFor(string folder)
        {
            if (!Enum.TryParse(folder, true, out FileCategory container))
            {
                throw new ArgumentException("Given folder does not exist");
            }
            var containerName = GetContainerName(container);
            return CloudBlobClient.GetContainerReference(containerName);
        }

        private string GetContainerName(FileCategory container)
        {
            return container.ToString().ToLower();
        }
    }
}
