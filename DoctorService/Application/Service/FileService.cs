namespace DoctorService.Application.Service
{
    public class FileService: IFileService
    {
        private readonly string _baseStoragePath;
        private readonly string _baseStorageUrl;

        public FileService(IConfiguration configuration)
        {
            _baseStoragePath = configuration["FileStorage:Path"]
                ?? Path.Combine(Directory.GetCurrentDirectory(), "FileStorage");
            _baseStorageUrl = configuration["FileStorage:BaseUrl"]
                ?? "/files";
        }

        public async Task<string> SaveFileAsync(IFormFile file, string subDirectory)
        {
            string directory = Path.Combine(_baseStoragePath, subDirectory);
            if (!Directory.Exists(directory))
                Directory.CreateDirectory(directory);

            string uniqueFileName = $"{Guid.NewGuid()}_{Path.GetFileName(file.FileName)}";
            string filePath = Path.Combine(directory, uniqueFileName);

            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(fileStream);
            }

            return Path.Combine(_baseStorageUrl, subDirectory, uniqueFileName).Replace("\\", "/");
        }

        public Task<Stream> GetFileStreamAsync(string filePath)
        {
            string systemPath = filePath;
            if (filePath.StartsWith(_baseStorageUrl))
            {
                systemPath = filePath.Replace(_baseStorageUrl, _baseStoragePath).Replace("/", Path.DirectorySeparatorChar.ToString());
            }
            else if (!Path.IsPathRooted(filePath))
            {
                systemPath = Path.Combine(_baseStoragePath, filePath);
            }

            if (!File.Exists(systemPath))
                throw new FileNotFoundException("File not found", systemPath);

            return Task.FromResult<Stream>(new FileStream(systemPath, FileMode.Open, FileAccess.Read));
        }

        public Task DeleteFileAsync(string filePath)
        {
            string systemPath = filePath;
            if (filePath.StartsWith(_baseStorageUrl))
            {
                systemPath = filePath.Replace(_baseStorageUrl, _baseStoragePath).Replace("/", Path.DirectorySeparatorChar.ToString());
            }
            else if (!Path.IsPathRooted(filePath))
            {
                systemPath = Path.Combine(_baseStoragePath, filePath);
            }

            if (File.Exists(systemPath))
                File.Delete(systemPath);

            return Task.CompletedTask;
        }
    }
}
