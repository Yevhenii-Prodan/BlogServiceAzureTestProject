using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using WebApi.Models;
using WebApi.Services;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class BlogController : ControllerBase
    {

        private readonly ILogger<BlogController> _logger;
        private readonly AzureQueueStorageService _queueStorageService;

        public BlogController(ILogger<BlogController> logger, AzureQueueStorageService queueStorageService)
        {
            _logger = logger;
            _queueStorageService = queueStorageService;
        }

        [HttpPost]
        public async Task<IActionResult> Post(BlogPostModel model)
        {
            await _queueStorageService.PushMessage(Base64Encode(JsonConvert.SerializeObject(model)));
            
            
            return Ok("Your post will be validated and then published.");
        }
        
        
        public static string Base64Encode(string plainText) {
            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
            return System.Convert.ToBase64String(plainTextBytes);
        }
    }
}