namespace dotnet_echo_microservice.Models;

public class BasePayload<T>
{
    public Pattern pattern { get; set; }
    public string id { get; set; }
    public T data { get; set; }
}

public class Pattern
{
    public string cmd { get; set; }
}