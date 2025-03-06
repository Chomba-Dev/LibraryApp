namespace App.Shared.Responses;

public class ServiceResponse<T>
{
    public T Data { get; set; }
    public bool Success { get; set; } = false;
    public string Message { get; set; } = string.Empty;
    
    public ServiceResponse(){}

    public ServiceResponse(T data, bool success, string message)
    {
        Data = data;
        Success = success;
        Message = message;
    }
}