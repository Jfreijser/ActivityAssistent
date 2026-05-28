namespace ActivityAssistent.App.Services
{
    public interface IErrorService
    {
        string LastError { get; set; }
        void LogError(Exception ex);
    }

    public class ErrorService : IErrorService
    {
        public string LastError { get; set; } = string.Empty;

        public void LogError(Exception ex)
        {
            LastError = ex.Message;
            Console.WriteLine($"Error: {ex.Message}");
        }
    }
}