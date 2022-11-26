using HumbleNote.Persistence.Models;
using MediatR;

namespace HumbleNote.Domain.Models.Events;

public class NoteCreatedEvent : INotification
{
    public string NoteId { get; set; }
    public string UserId { get; set; }
    public string Content { get; set; }
    public string? ParentNoteId { get; set; }
    public string? OldVersionNoteId { get; set; }
    public DateTimeOffset Timestamp { get; set; }


    public NoteCreatedEvent(Note note)
    {
        NoteId = note.Id;
        UserId = note.UserId;
        Content = note.Content;
        ParentNoteId = note.ParentNoteId;
        OldVersionNoteId = note.OldVersionNoteId;
        Timestamp = note.Timestamp;
    }
}