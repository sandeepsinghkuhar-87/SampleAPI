namespace SampleAPI.Entities
{
    public class ExceptionLog
    {
        public int LogId { get; set; }
        public string LogMessage { get; set; }
        public string StackTrace { get; set; }
        public DateTime LogTime { get; set; }
    }
}
