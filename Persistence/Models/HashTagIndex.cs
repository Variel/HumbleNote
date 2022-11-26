using System.ComponentModel.DataAnnotations;
using HumbleNote.Common;

namespace HumbleNote.Persistence.Models;

public class HashTagIndex
{
    [MaxLength(UlidHelper.StringLength)]
    public string UserId { get; set; }

    [MaxLength(50)]
    public string HashTag { get; set; }

    [MaxLength(UlidHelper.StringLength)]
    public string NoteId { get; set; }
    public Note? Note { get; set; }


    public DateTimeOffset Timestamp { get; set; }


    public HashTagIndex(string userId, string hashTag, string noteId, DateTimeOffset timestamp)
    {
        UserId = userId;
        HashTag = hashTag;
        NoteId = noteId;
        Timestamp = timestamp;
    }
}