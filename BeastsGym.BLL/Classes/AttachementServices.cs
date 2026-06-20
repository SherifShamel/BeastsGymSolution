using BeastsGym.BLL.Interfaces;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeastsGym.BLL.Classes
{
    public class AttachementServices : IAttachementServices
    {
        private readonly long maxFileSize = 5 * 1024 * 1024; // 5 MB
        private readonly string[] allowedExtensions = { ".jpg", ".jpeg", ".png" };
        private readonly ILogger<AttachementServices> logger;
        private readonly IWebHostEnvironment webHostEnvironment;



        public AttachementServices(ILogger<AttachementServices> logger, IWebHostEnvironment webHostEnvironment)
        {
            this.logger = logger;
            this.webHostEnvironment = webHostEnvironment;

        }
        public bool Delete(string fileName, string folderName)
        {
            if (string.IsNullOrEmpty(fileName) || string.IsNullOrEmpty(folderName)) return false;

            try
            {
                var fullPath = Path.Combine(webHostEnvironment.ContentRootPath, folderName, fileName);
                if (File.Exists(fullPath))
                {
                    File.Delete(fullPath);
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred while deleting the file.");
                return false;
            }
        }

        public (Stream stream, string contentType)? GetFile(string fileName, string folderName)
        {
            throw new NotImplementedException();
        }

        public async Task<string?> UploadAsync(Stream fileStream, string fileName, string folderName, CancellationToken ct)
        {
            if (fileStream is null || !fileStream.CanRead) return null;

            if (fileStream.Length == 0) return null;
            if (fileStream.Length > maxFileSize)
            {
                logger.LogError("File size exceeds the maximum allowed size of 5 MB.");
                return null;
            }

            var extention = Path.GetExtension(fileName).ToLowerInvariant();
            if (string.IsNullOrEmpty(extention) || !allowedExtensions.Contains(extention))
            {
                logger.LogError("File extension is not allowed.");
                return null;
            }

            var UploadedFolder = Path.Combine(webHostEnvironment.ContentRootPath, folderName);

            Directory.CreateDirectory(UploadedFolder);

            var StoredFileName = $"{Guid.NewGuid()}{extention}";

            var FilePath = Path.Combine(UploadedFolder, StoredFileName);

            try
            {
                await using var fs = new FileStream(FilePath, FileMode.CreateNew, FileAccess.Write, FileShare.None);
                await fileStream.CopyToAsync(fs, ct);
                return StoredFileName;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred while uploading the file.");
                return null;
            }
        }
    }
}
