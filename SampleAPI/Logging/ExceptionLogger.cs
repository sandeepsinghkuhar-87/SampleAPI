using Microsoft.EntityFrameworkCore;
using SampleAPI.Entities;

namespace SampleAPI.Logging
{
    public class ExceptionLogger : ILogger
    {
        private readonly IServiceProvider _serviceProvider;

        public ExceptionLogger(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public IDisposable BeginScope<TState>(TState state)
        {
            return null;
        }

        public bool IsEnabled(LogLevel logLevel)
        {
            return true;
        }

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<OrderDbContext>();

                if (exception != null)
                {
                    var exceptionLog = new ExceptionLog
                    {
                        LogMessage = exception.Message,
                        StackTrace = exception.StackTrace,
                        LogTime = DateTime.UtcNow
                    };

                    dbContext.ExceptionLogs.Add(exceptionLog);
                    dbContext.SaveChanges();
                }
            }

           
        }
    }
}
