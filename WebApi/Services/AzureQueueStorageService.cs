using System.Threading.Tasks;
using Azure.Storage.Queues;

namespace WebApi.Services
{
    public class AzureQueueStorageService
    {
        private readonly string _connectionString = "DefaultEndpointsProtocol=https;AccountName=blogstoragetestproject;AccountKey=HOLIbxjpKBzvftnpOhA2VKkQLhZXuVDi+Rrc7zTqNqOAdXBIXc43ivVjtyZfw65sutW6seMNpDL/9GxzTJQV0A==;EndpointSuffix=core.windows.net";
        private readonly string _queueName = "blog-queue";

        public Task PushMessage(string message)
        {
            var queue = new QueueClient(_connectionString, _queueName);

            return queue.SendMessageAsync(message);
        }
    }
}