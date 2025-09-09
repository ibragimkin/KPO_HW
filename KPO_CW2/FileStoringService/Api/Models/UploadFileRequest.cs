// File: FileStoringService.API/Models/UploadFileRequest.cs
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace FileStoringService.API.Models
{
    /// <summary>
    /// Модель для приёма multipart/form-data с полем "file".
    /// </summary>
    public class UploadFileRequest
    {
        /// <summary>
        /// Сам загружаемый .txt-файл.
        /// </summary>
        [Required]
        [DataType(DataType.Upload)]
        public IFormFile? File { get; set; }
    }
}
