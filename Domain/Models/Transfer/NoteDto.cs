using HumbleNote.Persistence.Models;

namespace HumbleNote.Domain.Models.Transfer;

public class NoteDto
{
  public string Id { get; set; }
  public string UserId { get; set; }
  public string Content { get; set; }
  public string? ParentNoteId { get; set; }
  public string? OldVersionNoteId { get; set; }
  public DateTimeOffset Timestamp { get; set; }


  public NoteDto? Parent { get; set; }
  public NoteDto? Root { get; set; }
  public IEnumerable<NoteDto>? Children { get; set; }


  public NoteDto(Note note)
  {
    Id = note.Id;
    UserId = note.UserId;
    Content = note.Content;
    ParentNoteId = note.ParentNoteId;
    OldVersionNoteId = note.OldVersionNoteId;
    Timestamp = note.Timestamp;

    if (note.ParentNote is not null)
    {
      Parent = new NoteDto(note.ParentNote);
    }

    if (note.RootNote is not null)
    {
      Root = new NoteDto(note.RootNote);
    }

    if (note.ChildrenNotes is not null)
    {
      Children = note.ChildrenNotes.Select(n => new NoteDto(n));
    }
  }
}