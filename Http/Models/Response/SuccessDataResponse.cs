namespace HumbleNote.Http.Models.Response;

public class SuccessDataResponse : Response
{
  public object? Data { get; set; }

  public override bool Success => true;

  public SuccessDataResponse(object? data) => Data = data;
}