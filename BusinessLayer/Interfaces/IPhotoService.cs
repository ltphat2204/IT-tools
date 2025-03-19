﻿using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Http;

namespace BusinessLayer.Interfaces
{
    interface IPhotoService
    {
        Task<ImageUploadResult> AddPhotoAsync(IFormFile file);

        Task<DeletionResult> DeletePhotoAsync(string publicUrl);
    }
}
