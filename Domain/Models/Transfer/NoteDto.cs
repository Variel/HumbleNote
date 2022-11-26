using HumbleNote.Persistence.Models;

namespace HumbleNote.Domain.Models.Transfer;

public class NoteDto
{
    public string NoteId { get; set; }
    public string UserId { get; set; }
    public string Content { get; set; }
    public string? ParentNoteId { get; set; }
    public string? OldVersionNoteId { get; set; }
    public DateTimeOffset Timestamp { get; set; }


    public NoteDto(Note note)
    {
        NoteId = note.Id;
        UserId = note.UserId;
        Content = note.Content;
        ParentNoteId = note.ParentNoteId;
        OldVersionNoteId = note.OldVersionNoteId;
        Timestamp = note.Timestamp;
    }
}