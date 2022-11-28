using HumbleNote.Domain.Models.Commands;
using HumbleNote.Domain.Models.Results;
using HumbleNote.Domain.Models.Transfer;
using HumbleNote.Domain.Services;
using HumbleNote.Http.Models.Request;
using Microsoft.AspNetCore.Mvc;

namespace HumbleNote.Controllers;

[ApiController]
[Route("notes")]
public class NoteContorller : ApiControllerBase
{
  private readonly INoteService _noteService;
  private readonly ILogger<NoteContorller> _logger;

  const string userId = "01GJWW8R1CMPB4E1N1M6CN5QWV";

  public NoteContorller(ILogger<NoteContorller> logger, INoteService noteService)
  {
    _logger = logger;
    _noteService = noteService;
  }

  [HttpGet(Name = "List notes")]
  public async Task<IEnumerable<NoteDto>?> ListNotes(string? cursor = null)
  {
    var result = await _noteService.ListNoteAsync(userId, cursor,
      HttpContext.RequestAborted);

    return result.Notes;
  }

  [HttpPost(Name = "Post new note")]
  public async Task<NoteDto?> PostNewNote(PostNoteRequest model)
  {
    var result = await _noteService.CreateNoteAsync(
      new CreateNoteCommand(userId, model.Content),
      HttpContext.RequestAborted);

    return result.Note;
  }



  [HttpGet("{parentId}/children", Name = "Get child notes")]
  public async Task<ActionResult<IEnumerable<NoteDto>>> GetChildrenNotes(string parentId, string? cursor = null)
  {
    var result = await _noteService.ListChildrenAsync(userId,
      parentId,
      cursor,
      HttpContext.RequestAborted);

    return result.Status switch
    {
      ListChildrenStatus.Succeeded => Ok(result.Children),
      ListChildrenStatus.InvalidCursor => BadRequest("올바르지 않은 cursor입니다"),
      ListChildrenStatus.ParentNotFound => NotFound(),
      _ => BadRequest()
    };
  }


  [HttpPost("{parentId}/children", Name = "Post child note")]
  public async Task<ActionResult<NoteDto>> PostChildNote(string parentId, string content)
  {
    var result = await _noteService.CreateNoteAsync(
      new CreateNoteCommand(userId, content)
      {
        ParentNoteId = parentId
      },
      HttpContext.RequestAborted);

    return result.Status switch
    {
      CreateNoteStatus.Succeeded => Ok(result.Note),
      CreateNoteStatus.ParentNotFound => NotFound(),
      _ => BadRequest()
    };
  }



  [HttpPost("{oldVersionId}/revision", Name = "Revision a note")]
  public async Task<ActionResult<NoteDto>> RevisionNote(string content, string oldVersionId)
  {
    var result = await _noteService.CreateNoteAsync(
        new CreateNoteCommand(userId, content)
        {
          OldVersionNoteId = oldVersionId
        },
        HttpContext.RequestAborted);

    return result.Status switch
    {
      CreateNoteStatus.Succeeded => Ok(result.Note),
      CreateNoteStatus.OldVersionNotFound => NotFound(),
      _ => BadRequest()
    };
  }
}
