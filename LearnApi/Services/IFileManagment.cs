using LearnApi.Models;

namespace LearnApi.Services
{
    public interface IFileManagment
    {
        Task<ApiResponse> Upload(IFormFile file,string filePath);
        Task<ApiResponse> UploadMultipleFiles(IFormFileCollection files, string path);
        Task<ApiResponse> GetMultipleFiles(List<string> fileNames,string path);
        Task<ApiResponse> GetSingleFile(string fileName,string path);
        string GetMainFilePath(string path);

        Task<ApiResponse> RemoveSingleFile(string fileName, string path);
        Task<ApiResponse> RemoveMultipleFiles(List<string>filesName, string path);
    }
}
