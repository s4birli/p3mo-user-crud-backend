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
            await using var browser = await playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions
            {
                Headless = true,
                Args = new[] { "--disable-web-security" } // Helps with CORS during local testing
            });
            
            // Create a new page
            var page = await browser.NewPageAsync();
            
            // Navigate to the frontend page for this user
            _logger.LogInformation("Navigating to {Url}", $"{frontendUrl}/user/{userId}");
            await page.GotoAsync($"{frontendUrl}/user/{userId}");
            
            // Wait for network to be idle
            await page.WaitForLoadStateAsync(LoadState.NetworkIdle);
            
            // Wait for any skeleton loaders or loading indicators to disappear
            // Adjust the selector based on your actual skeleton UI implementation
            try
            {
                // Wait for skeleton loaders to disappear (adjust this selector to match your skeleton UI)
                await page.WaitForSelectorAsync(".skeleton-loader", new PageWaitForSelectorOptions
                {
                    State = WaitForSelectorState.Hidden,
                    Timeout = 10000 // 10 seconds timeout
                });
                
                // Additional wait to ensure all animations are complete
                await Task.Delay(1000);
                
                _logger.LogInformation("Page fully loaded, generating PDF");
            }
            catch (TimeoutException)
            {
                _logger.LogWarning("Skeleton UI did not disappear within timeout, proceeding with PDF generation");
                // Continue anyway, in case the selector is wrong or the skeleton UI is not present
            }

            // Take a screenshot for debugging (optional)
            // var screenshotBytes = await page.ScreenshotAsync();
            // System.IO.File.WriteAllBytes($"user-{userId}-screenshot.png", screenshotBytes);

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

            _logger.LogInformation("PDF generated successfully for user {UserId}", userId);
            return pdfBytes;
        }
    }
} 