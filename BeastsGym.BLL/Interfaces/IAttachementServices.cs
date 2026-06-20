using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeastsGym.BLL.Interfaces
{
    public interface IAttachementServices
    {
        Task<string?> UploadAsync(Stream fileStream, string fileName, string folderName, CancellationToken ct);

        bool Delete(string fileName, string folderName);

        (Stream stream, string contentType)? GetFile(string fileName, string folderName);
    }
}
