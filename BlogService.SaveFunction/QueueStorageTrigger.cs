using System.Threading.Tasks;
using System;
using System.IO;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;
using Microsoft.WindowsAzure.Storage.Blob;

namespace BlogService.SaveFunction
{
    public static class QueueStorageTrigger
    {
        [FunctionName("QueueStorageTrigger")]
        public static async Task RunAsync([QueueTrigger("blog-queue", Connection = "")] 
            string myQueueItem, 
            [Blob("blog-blob-storage", Connection = "AzureWebJobsStorage")] CloudBlobContainer outputContainer,
            ILogger log)
        {
            var blobName = Guid.NewGuid()+".json";
            
            await outputContainer.CreateIfNotExistsAsync();
            var cloudBlockBlob = outputContainer.GetBlockBlobReference(blobName);
            await cloudBlockBlob.UploadTextAsync(myQueueItem);
            
            log.LogInformation($"C# Queue trigger function processed: {myQueueItem}");
            
        }
    }
}