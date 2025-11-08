using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaurants.Domain.Interfaces
{
    public interface IBlobStorageService
    {
        Task<string> UploadToBlobAsync(Stream data, string fileName);
        string? GetBlobSasUrl(string? blobUrl);
    }
}
