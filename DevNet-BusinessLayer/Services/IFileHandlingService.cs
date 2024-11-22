using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevNet_BusinessLayer.Services
{
    public interface IFileHandlingService
    {
        Task<string> SaveProfileImageAsync(string base64Image);
        void DeleteProfileImage(string imageUrl);
    }

}
