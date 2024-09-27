using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Configuration;
using Azure.Storage.Blobs;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Azure.CognitiveServices.Vision.ComputerVision;
using Microsoft.Azure.CognitiveServices.Vision.ComputerVision.Models;
using SearchPic.Models;

namespace SearchPic.Controllers
{
    public class ImageController : Controller
    {
        private readonly BlobServiceClient _blobServiceClient;
        private readonly Container _cosmosContainer;
        private readonly ComputerVisionClient _visionClient;
        public ImageController(IConfiguration configuration)
        {
            _visionClient = new ComputerVisionClient(new ApiKeyServiceClientCredentials(configuration["CognitiveServicesKey"]))
            {
                Endpoint = configuration["CognitiveServicesEndpoint"]
            };
        }
        private async Task<ImageAnalysisResult> AnalyzeImage(string imageUrl)
        {
            var features = new List<VisualFeatureTypes?> { VisualFeatureTypes.Description, VisualFeatureTypes.Tags };
            var result = await _visionClient.AnalyzeImageAsync(imageUrl, features);
            return new ImageAnalysisResult
            {
                Description = result.Description.Captions.FirstOrDefault()?.Text ?? "No description available",
                Keywords = result.Tags.Select(tag => tag.Name).ToList()
            };
        }
    }
}