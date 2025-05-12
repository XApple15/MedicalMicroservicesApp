namespace DoctorService.Application.Service
{
    public interface IFileService
    {
        Task<string> SaveFileAsync(IFormFile file, string subDirectory);
        Task<Stream> GetFileStreamAsync(string filePath);
        Task DeleteFileAsync(string filePath);
    }
}
