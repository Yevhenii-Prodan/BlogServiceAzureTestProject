using System.Threading.Tasks;
using BlogService.SaveFunction.Models;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using Microsoft.WindowsAzure.Storage.Blob;
using Newtonsoft.Json;

namespace BlogService.SaveFunction
{
    public static class ModerationFunction
    {
        [FunctionName("ModerationFunction")]
        public static async Task RunAsync([EventHubTrigger("bloghub", Connection = "EventHubListenerConnection")]
            string fileName,
            [Blob("blog-blob-storage", Connection = "AzureWebJobsStorage")]
            CloudBlobContainer outputContainer
            , ILogger log)
        {

            var blobReference = outputContainer.GetBlockBlobReference(fileName);

            var postModel = JsonConvert.DeserializeObject<BlogPostModel>(await blobReference.DownloadTextAsync());  

            postModel.IsModerated = true;
            postModel.ModerationSucceed = !postModel.Content.Contains("***StopWord***");

            await blobReference.UploadTextAsync(JsonConvert.SerializeObject(postModel));


        }

    }
}