namespace HumbleNote.Http.Models.Response;

public class ErrorDataResponse : Response
{
  public object? Data { get; set; }
  public string Message { get; set; }

  public override bool Success => false;

  public ErrorDataResponse(string message, object? data)
  {
    Message = message;
    Data = data;
  }
}