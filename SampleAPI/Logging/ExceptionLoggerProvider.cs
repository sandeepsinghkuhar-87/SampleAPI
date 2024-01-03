namespace SampleAPI.Logging
{
    public class ExceptionLoggerProvider : ILoggerProvider
    {
        private readonly IServiceProvider _serviceProvider;

        public ExceptionLoggerProvider(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public ILogger CreateLogger(string categoryName)
        {
            return new ExceptionLogger(_serviceProvider);
        }

        public void Dispose()
        {
        }
    }
}
