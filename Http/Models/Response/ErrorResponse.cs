namespace HumbleNote.Http.Models.Response;

public class ErrorResponse : Response
{
  public string Message { get; set; }

  public override bool Success => false;

  public ErrorResponse(string message)
  {
    Message = message;
  }
}