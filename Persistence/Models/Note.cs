using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using HumbleNote.Common;

namespace HumbleNote.Persistence.Models;

public class Note
{
    [MaxLength(UlidHelper.StringLength)]
    public string Id { get; set; } = Ulid.NewUlid().ToString();

    [MaxLength(UlidHelper.StringLength)]
    public string UserId { get; set; }
    public User? User { get; set; }


    public string Content { get; set; }



    // 하위 노트일 경우 최상위 노트에 대한 참조
    [MaxLength(UlidHelper.StringLength)]
    public string? RootNoteId { get; set; }
    public Note? RootNote { get; set; }


    // 하위 노트일 경우 부모 노트에 대한 참조
    [MaxLength(UlidHelper.StringLength)]
    public string? ParentNoteId { get; set; }
    public Note? ParentNote { get; set; }

    public ICollection<Note> ChildrenNotes { get; set; } = new HashSet<Note>();


    // 수정이 있었을 경우 새로 바뀐 노트에 대한 참조

    [MaxLength(UlidHelper.StringLength)]
    public string? NewVersionNoteId { get; set; }
    public Note? NewVersionNote { get; set; }

    [MaxLength(UlidHelper.StringLength)]
    public string? OldVersionNoteId { get; set; }
    public Note? OldVersionNote { get; set; }


    public ICollection<NoteMention> NoteMentions { get; set; } = new HashSet<NoteMention>();
    public ICollection<NoteMention> NoteMentionBackLinks { get; set; } = new HashSet<NoteMention>();


    public DateTimeOffset Timestamp { get; set; } = DateTimeOffset.UtcNow;

    public DateTimeOffset LastActivatedAt { get; set; } = DateTimeOffset.UtcNow;



    public Note(string content, string userId)
    {
        Content = content;
        UserId = userId;
    }
}