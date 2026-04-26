namespace ActivityAssistent.Web.Services
{
    public interface IThemeService
    {
        Task<bool> IsDarkModeAsync();
        Task SetDarkModeAsync(bool isDark);
    }

    public class ThemeService : IThemeService
    {
        // Variabele in PascalCase, standaard op true (donker)
        private bool IsDarkMode = true;

        public Task<bool> IsDarkModeAsync()
        {
            return Task.FromResult(IsDarkMode);
        }

        public Task SetDarkModeAsync(bool isDark)
        {
            IsDarkMode = isDark;
            return Task.CompletedTask;
        }
    }
}