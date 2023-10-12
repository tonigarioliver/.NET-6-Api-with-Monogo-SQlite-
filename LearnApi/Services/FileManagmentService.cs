
using LearnApi.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace LearnApi.Services
{
    public class FileManagementService : IFileManagment
    {
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly ILogger<FileManagementService> _logger;
        private readonly string _rootFolder;

        public FileManagementService(IWebHostEnvironment webHostEnvironment, string rootFolder, ILogger<FileManagementService> logger)
        {
            _webHostEnvironment = webHostEnvironment;
            _rootFolder = rootFolder;
            _logger = logger;
        }

        public string GetMainFilePath(string path)
        {
            return Path.Combine(_webHostEnvironment.WebRootPath, _rootFolder, path);
        }

        public async Task<ApiResponse> GetMultipleFiles(List<string> fileNames, string path)
        {
            ApiResponse response = new ApiResponse();
            List<ApiResponse> responses = new List<ApiResponse>();

            foreach (var fileName in fileNames)
            {
                responses.Add(await HandleFileAsync(fileName,null, path, HandleFileAction.GetFile));
            }

            HandleFileResponses(response, responses);
            return response;
        }

        public async Task<ApiResponse> GetSingleFile(string fileName, string path)
        {
            ApiResponse response = new ApiResponse();
            List<ApiResponse> responses = new List<ApiResponse>();

            responses.Add(await HandleFileAsync(fileName,null, path, HandleFileAction.GetFile));

            HandleFileResponses(response, responses);
            return response;
        }

        public async Task<ApiResponse> RemoveMultipleFiles(List<string> fileNames, string path)
        {
            ApiResponse response = new ApiResponse();
            List<ApiResponse> responses = new List<ApiResponse>();

            foreach (var fileName in fileNames)
            {
                responses.Add(await HandleFileAsync(fileName,null, path, HandleFileAction.RemoveFile));
            }

            HandleFileResponses(response, responses);
            return response;
        }

        public async Task<ApiResponse> RemoveSingleFile(string fileName, string path)
        {
            ApiResponse response = new ApiResponse();
            List<ApiResponse> responses = new List<ApiResponse>();

            responses.Add(await HandleFileAsync(fileName,null, path, HandleFileAction.RemoveFile));

            HandleFileResponses(response, responses);
            return response;
        }

        public async Task<ApiResponse> Upload(IFormFile file, string path)
        {
            ApiResponse response = new ApiResponse();
            List<ApiResponse> responses = new List<ApiResponse>();

            responses.Add(await HandleFileAsync(file.FileName, file, path, HandleFileAction.UploadFile));

            HandleFileResponses(response, responses);
            return response;
        }

        public async Task<ApiResponse> UploadMultipleFiles(IFormFileCollection files, string path)
        {
            ApiResponse response = new ApiResponse();
            List<ApiResponse> responses = new List<ApiResponse>();

            foreach (var file in files)
            {
                responses.Add(await HandleFileAsync(file.FileName,file, path, HandleFileAction.UploadFile));
            }

            HandleFileResponses(response, responses);
            return response;
        }

        private async Task<ApiResponse> HandleFileAsync(string fileName, IFormFile file, string path, HandleFileAction action)
        {
            ApiResponse response = new ApiResponse();
            string imageUrl = string.Empty;

            try
            {
                string filePath = GetMainFilePath(path);
                string imagePath = Path.Combine(filePath, $"{fileName}");

                if (action == HandleFileAction.UploadFile)
                {
                    if (!Directory.Exists(filePath))
                    {
                        Directory.CreateDirectory(filePath);
                    }
                    if (File.Exists(imagePath))
                    {
                        File.Delete(imagePath);
                    }

                    using (FileStream fileStream = File.Create(imagePath))
                    {
                        await file.CopyToAsync(fileStream);
                        response.StatusCode = StatusCodes.Status200OK;
                        response.Result = "pass";
                    }
                }
                else if (action == HandleFileAction.GetFile)
                {
                    if (File.Exists(imagePath))
                    {
                        imageUrl = $"/{_rootFolder}/{path}/{fileName}";
                        response.StatusCode = StatusCodes.Status200OK;
                        response.Result = imageUrl;
                    }
                    else
                    {
                        response.StatusCode = StatusCodes.Status404NotFound;
                        response.Result = "FileNotFound";
                    }
                }
                else if (action == HandleFileAction.RemoveFile)
                {
                    if (File.Exists(imagePath))
                    {
                        File.Delete(imagePath);
                    }
                    else
                    {
                        response.StatusCode = StatusCodes.Status404NotFound;
                        response.Result = "FileNotFound";
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while handling a file.");
                response.Errormessage = ex.Message;
                response.StatusCode = StatusCodes.Status500InternalServerError;
            }

            return response;
        }

        private void HandleFileResponses(ApiResponse response, List<ApiResponse> responses)
        {
            var errorResponses = responses.Where(r => r.StatusCode == StatusCodes.Status500InternalServerError).ToList();
            var notFoundResponses = responses.Where(r => r.StatusCode == StatusCodes.Status404NotFound).ToList();

            if (errorResponses.Any())
            {
                response.Errormessage = string.Join(", ", errorResponses.Select(r => r.Errormessage));
                response.StatusCode = StatusCodes.Status500InternalServerError;
            }
            else if (notFoundResponses.Any())
            {
                response.Errormessage = string.Join(", ", notFoundResponses.Select(r => r.Errormessage));
                response.StatusCode = StatusCodes.Status404NotFound;
            }
            else
            {
                response.Result = string.Join(", ", responses.Select(r => r.Result));
                response.StatusCode = StatusCodes.Status200OK;
            }
        }

        private enum HandleFileAction
        {
            GetFile,
            RemoveFile,
            UploadFile
        }

    }
}

