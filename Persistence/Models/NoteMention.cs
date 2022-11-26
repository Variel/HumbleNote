using System.ComponentModel.DataAnnotations;
using HumbleNote.Common;

namespace HumbleNote.Persistence.Models;

public class NoteMention
{
    [MaxLength(UlidHelper.StringLength)]
    public string NoteId { get; set; }
    public Note? Note { get; set; }


    [MaxLength(UlidHelper.StringLength)]
    public string MentionedNoteId { get; set; }
    public Note? MentionedNote { get; set; }


    public NoteMention(string noteId, string mentionedNoteId)
    {
        NoteId = noteId;
        MentionedNoteId = mentionedNoteId;
    }
}