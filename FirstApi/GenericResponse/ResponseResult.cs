namespace FirstApi.GenericResponse
{
    public class ResponseResult<T>
    {
        public T? Data { get; set; }
        public string? Message { get; set; }
        public bool Status { get; set; } = false;

        public static ResponseResult<T> Success(T? data,string Message)
        {
            return new ResponseResult<T>
            {
                Data = data,
                Message = Message,
                Status = true
            };
        }

        public static ResponseResult<T> Failure(T? Data,string Message)
        {
            return new ResponseResult<T>
            {
                Data = Data,
                Message = Message,
                Status = false
            };
        }
    }
}
