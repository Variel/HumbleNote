namespace HumbleNote.Domain.Models.Commands;

public class CreateNoteCommand
{
    public string UserId { get; set; }
    public string Content { get; set; }

    // If exists, this note is child note
    public string? ParentNoteId { get; set; }

    // If exists, this note is a new revision of an old note
    public string? OldVersionNoteId { get; set; }


    public CreateNoteCommand(string userId, string content)
    {
        UserId = userId;
        Content = content;
    }
}