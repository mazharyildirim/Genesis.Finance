namespace Genesis.CoreApi.Shared
{
    public class NProcessResult
    {
        public bool IsSuccess { get; set; }
        public string? Message { get; set; }
    }
    public class NProcessResult<T> : NProcessResult
    {
        public T? ResultData { get; set; }
    }
}
