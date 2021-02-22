using System.Threading.Tasks;
using System;
using System.IO;
using BlogService.SaveFunction.Models;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;
using Microsoft.WindowsAzure.Storage.Blob;
using Newtonsoft.Json;

namespace BlogService.SaveFunction
{
    public static class StoreToBlobFunction
    {
        [FunctionName("StoreToBlobFunction")]
        public static async Task RunAsync([QueueTrigger("blog-queue", Connection = "AzureWebJobsStorage")] 
            string blogPost, 
            [Blob("blog-blob-storage", Connection = "AzureWebJobsStorage")] CloudBlobContainer outputContainer,
            [EventHub("bloghub", Connection = "EventHubPublisherConnection")]IAsyncCollector<string> outputEvents,
            ILogger log)
        {
            var blobName = Guid.NewGuid()+".json";
            
            await outputContainer.CreateIfNotExistsAsync();
            var cloudBlockBlob = outputContainer.GetBlockBlobReference(blobName);


            var postObject = JsonConvert.DeserializeObject<BlogPostModel>(blogPost);
            postObject.IsModerated = false;
            postObject.ModerationSucceed = false;
            
            
            await cloudBlockBlob.UploadTextAsync(JsonConvert.SerializeObject(postObject));
            
            await outputEvents.AddAsync(blobName);
            
            
        }
    }
}