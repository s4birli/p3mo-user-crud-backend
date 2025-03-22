using Microsoft.AspNetCore.Mvc;
using p3mo_user_crud_backend.Services;

namespace p3mo_user_crud_backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PdfController : ControllerBase
    {
        private readonly IPdfService _pdfService;
        private readonly IConfiguration _configuration;
        private readonly ILogger<PdfController> _logger;

        public PdfController(
            IPdfService pdfService, 
            IConfiguration configuration,
            ILogger<PdfController> logger)
        {
            _pdfService = pdfService;
            _configuration = configuration;
            _logger = logger;
        }

        [HttpGet("{userId}")]
        public async Task<ActionResult> GenerateUserPdf(int userId)
        {
            try
            {
                var frontendUrl = _configuration["FrontendUrl"] ?? "http://localhost:5500";
                var pdfBytes = await _pdfService.GenerateUserPdfAsync(userId, frontendUrl);
                return File(pdfBytes, "application/pdf", $"user-{userId}.pdf");
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while generating PDF for user with ID {UserId}", userId);
                return StatusCode(500, $"Internal server error occurred while generating PDF for user with ID {userId}");
            }
        }
    }
} 