namespace TodoListAPI.Services
{
    /// <summary>
    /// No-op email sender for development. Replace with a real implementation (e.g., SendGrid, SMTP) in production.
    /// </summary>
    public class NoOpEmailSender : IEmailSender
    {
        private readonly ILogger<NoOpEmailSender> _logger;

        public NoOpEmailSender(ILogger<NoOpEmailSender> logger)
        {
            _logger = logger;
        }

        public Task SendAsync(string to, string subject, string body)
        {
            _logger.LogWarning("NoOpEmailSender: Would send email to {To}, subject: {Subject}", to, subject);
            return Task.CompletedTask;
        }
    }
}
