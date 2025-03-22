using Microsoft.Playwright;

namespace p3mo_user_crud_backend.Helpers
{
    public static class PlaywrightHelper
    {
        public static async Task InstallBrowsersAsync()
        {
            await Task.Run(() =>
            {
                var exitCode = Microsoft.Playwright.Program.Main(new[] { "install", "--with-deps" });
                if (exitCode != 0)
                {
                    throw new Exception($"Playwright exited with code {exitCode}");
                }
            });
        }

        public static async Task EnsureBrowsersInstalledAsync(IServiceProvider serviceProvider)
        {
            var logger = serviceProvider.GetRequiredService<ILogger<IPlaywright>>();

            try
            {
                logger.LogInformation("Ensuring Playwright browsers are installed");
                await InstallBrowsersAsync();
                logger.LogInformation("Playwright browsers installed successfully");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Failed to install Playwright browsers");
                throw;
            }
        }
    }
}