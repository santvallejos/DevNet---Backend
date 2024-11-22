using DevNet_BusinessLayer.Services;
using System;
using System.IO;
using System.Threading.Tasks;

namespace DevNet_WebAPI.Services
{
    public class FileHandlingService : IFileHandlingService
    {
        private readonly string _uploadDirectory;

        public FileHandlingService(string uploadDirectory)
        {
            if (string.IsNullOrWhiteSpace(uploadDirectory))
                throw new ArgumentException("Upload directory cannot be null or empty.");

            _uploadDirectory = uploadDirectory;

            // Crear el directorio si no existe
            if (!Directory.Exists(_uploadDirectory))
            {
                Directory.CreateDirectory(_uploadDirectory);
            }
        }

        public async Task<string> SaveProfileImageAsync(string base64Image)
        {
            if (string.IsNullOrWhiteSpace(base64Image))
                throw new ArgumentException("Image data cannot be null or empty.");

            var fileName = $"{Guid.NewGuid()}.png"; // Genera un nombre único para el archivo
            var filePath = Path.Combine(_uploadDirectory, fileName);

            try
            {
                // Convertir la cadena Base64 a bytes y guardar el archivo
                byte[] imageBytes = Convert.FromBase64String(base64Image);
                await File.WriteAllBytesAsync(filePath, imageBytes);

                return fileName; // Retorna el nombre del archivo
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Error saving the image file.", ex);
            }
        }

        public void DeleteProfileImage(string fileName)
        {
            if (string.IsNullOrWhiteSpace(fileName))
                throw new ArgumentException("File name cannot be null or empty.");

            var filePath = Path.Combine(_uploadDirectory, fileName);

            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }
        }
    }
}
