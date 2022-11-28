using HumbleNote.Domain.Models.Transfer;
using HumbleNote.Persistence.Models;

namespace HumbleNote.Domain.Models.Results;

public class CreateNoteResult
{
  public CreateNoteStatus Status { get; set; }
  public NoteDto? Note { get; set; }

  public CreateNoteResult(CreateNoteStatus status, Note? note = default)
  {
    Status = status;

    if (note != null)
    {
      Note = new NoteDto(note);
    }
  }
}

public enum CreateNoteStatus
{
  Succeeded,
  ParentNotFound,
  OldVersionNotFound
}