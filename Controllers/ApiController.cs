using HumbleNote.Http.Models.Response;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;

namespace HumbleNote.Controllers;

public class ApiControllerBase : ControllerBase
{
  public new OkObjectResult Ok()
  {
    return base.Ok(new SuccessResponse());
  }

  public override OkObjectResult Ok([ActionResultObjectValue] object? value)
  {
    return base.Ok(new SuccessDataResponse(value));
  }

  public override CreatedResult Created(string uri, [ActionResultObjectValue] object? value)
  {
    return base.Created(uri, new SuccessDataResponse(value));
  }

  public override CreatedResult Created(Uri uri, [ActionResultObjectValue] object? value)
  {
    return base.Created(uri, value);
  }

  public new BadRequestObjectResult BadRequest()
  {
    return base.BadRequest(new ErrorResponse("올바르지 않은 요청입니다"));
  }

  public override BadRequestObjectResult BadRequest([ActionResultObjectValue] object? value)
  {
    return base.BadRequest(new ErrorDataResponse("올바르지 않은 요청입니다", value));
  }

  public BadRequestObjectResult BadRequest(string message)
  {
    return base.BadRequest(new ErrorResponse(message));
  }

  public BadRequestObjectResult BadRequest(string message, object? value)
  {
    return base.BadRequest(new ErrorDataResponse(message, value));
  }
}