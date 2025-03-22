using Microsoft.EntityFrameworkCore;
using Microsoft.Playwright;
using p3mo_user_crud_backend.Data;

namespace p3mo_user_crud_backend.Services
{
    public class PdfService : IPdfService
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<PdfService> _logger;

        public PdfService(
            ApplicationDbContext context, 
            ILogger<PdfService> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<byte[]> GenerateUserPdfAsync(int userId, string frontendUrl)
        {
            _logger.LogInformation("Generating PDF for user {UserId}", userId);
            
            var user = await _context.Users
                .Include(u => u.UserDetails)
                .ThenInclude(ud => ud.Role)
                .FirstOrDefaultAsync(u => u.Id == userId);
                
            if (user == null)
                throw new KeyNotFoundException($"User with ID {userId} not found");
            
            // Initialize Playwright
            using var playwright = await Microsoft.Playwright.Playwright.CreateAsync();
            await using var browser = await playwright.Chromium.LaunchAsync();
            
            // Create a new page
            var page = await browser.NewPageAsync();
            
            // Navigate to the frontend page for this user
            await page.GotoAsync($"{frontendUrl}/users/{userId}");
            
            // Wait for the page to load completely
            await page.WaitForLoadStateAsync(LoadState.NetworkIdle);
            
            // Generate the PDF
            var pdfBytes = await page.PdfAsync(new PagePdfOptions
            {
                Format = "A4",
                PrintBackground = true,
                DisplayHeaderFooter = true,
                HeaderTemplate = "<div style='font-size:10px; text-align:center; width:100%;'>User Profile Report</div>",
                FooterTemplate = "<div style='font-size:8px; text-align:center; width:100%;'>Generated on " + 
                    DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "</div>"
            });
            
            return pdfBytes;
        }
    }
} 