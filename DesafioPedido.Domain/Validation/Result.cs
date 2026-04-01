namespace DesafioPedido.Domain.Validation
{
    public class Result<T>
    {
        public bool Success { get; private set; }
        public T Data { get; private set; }
        public string Error { get; private set; }

        private Result(T data)
        {
            Success = true;
            Data = data;
        }

        private Result(string error)
        {
            Success = false;
            Error = error;
        }

        public static Result<T> Ok(T data) => new Result<T>(data);
        public static Result<T> Fail(string error) => new Result<T>(error);
    }
}
