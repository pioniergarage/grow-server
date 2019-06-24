using Grow.Data.Entities;
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

        public StorageConnector(AppSettings settings)
        {
            var connectionString = settings.StorageConnectionString;
            if (!CloudStorageAccount.TryParse(connectionString, out CloudStorageAccount storageAccount))
            {
                Trace.TraceError("Could not connect to storage account");
                return;
            }

            CloudBlobClient = storageAccount.CreateCloudBlobClient();
            
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

        public static string GetFolderNameForEntityType<T>() where T : BaseDbEntity
        {
            FileCategory container;
            switch (typeof(T).Name)
            {
                case nameof(Event):
                    container = FileCategory.Events;
                    break;
                case nameof(Partner):
                    container = FileCategory.Partners;
                    break;
                case nameof(Person):
                case nameof(Judge):
                case nameof(Organizer):
                case nameof(Mentor):
                    container = FileCategory.People;
                    break;
                case nameof(Team):
                    container = FileCategory.Teams;
                    break;
                default:
                    container = FileCategory.Misc;
                    break;
            }
            return container.ToString().ToLower();
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
