namespace p3mo_user_crud_backend.Services
{
    public interface IPdfService
    {
        Task<byte[]> GenerateUserPdfAsync(int userId, string frontendUrl);
    }
} 