namespace Revalsys.Properties
{
    public class Response<T>
    {
        public int ReturnCode { get; set; } = 0;
        public string? ReturnMessage { get; set; } = string.Empty;
        public string? ResponseTime { get; set; } = string.Empty;
        public long RecordCount { get; set; } = 0;
        public T? Headers { get; set; } = default(T?);
        public T? Data { get; set; } = default(T?);
    }
}