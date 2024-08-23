namespace GymManagment.Shared;

public class Response<T>
{
    public string Message { get; set; }
    public T Data { get; set; }
    public int Status { get; set; }

    public Response(string message, T data, int status)
    {
        Message = message;
        Data = data;
        Status = status;
    }
}