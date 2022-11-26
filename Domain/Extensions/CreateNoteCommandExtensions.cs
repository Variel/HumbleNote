using HumbleNote.Domain.Models.Commands;
using HumbleNote.Persistence.Models;

namespace HumbleNote.Domain.Extensions;

public static class CreateNoteCommandExtensions
{
    public static Note ToNote(this CreateNoteCommand self) => new Note(self.Content, self.UserId)
    {
        OldVersionNoteId = self.OldVersionNoteId,
        ParentNoteId = self.ParentNoteId,
    };
}