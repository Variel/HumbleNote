using HumbleNote.Domain.Models.Transfer;
using HumbleNote.Persistence.Models;

namespace HumbleNote.Domain.Models.Results;

public class ListNoteResult
{
    public ListNoteStatus Status { get; set; }
    public IEnumerable<NoteDto>? Notes { get; set; }

    public ListNoteResult(ListNoteStatus status, IEnumerable<Note>? notes)
    {
        Status = status;

        if (notes != null)
        {
            Notes = notes.Select(note => new NoteDto(note));
        }
    }
}

public enum ListNoteStatus
{
    Succeeded,
    InvalidCursor
}