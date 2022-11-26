using HumbleNote.Domain.Exceptions;
using HumbleNote.Domain.Models.Events;
using HumbleNote.Services;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HumbleNote.Domain.EventHandlers;

public class OnNoteCreatedCreateHierarchyHandler : INotificationHandler<NoteCreatedEvent>
{
    private readonly DatabaseContext _database;

    public OnNoteCreatedCreateHierarchyHandler(DatabaseContext database)
    {
        _database = database;
    }

    public async Task Handle(NoteCreatedEvent notification, CancellationToken cancellationToken)
    {
        if (notification.ParentNoteId is null)
        {
            return;
        }

        var createdNote
            = await _database.Notes.FirstOrDefaultAsync(n => n.Id == notification.NoteId,
                                                        cancellationToken);
        if (createdNote is null)
        {
            throw new NoteNotFoundException();
        }

        var parentNote
            = await _database.Notes.FirstOrDefaultAsync(
                n => n.Id == notification.ParentNoteId && n.UserId == notification.UserId,
                cancellationToken);
        if (parentNote is null)
        {
            throw new NoteHierarchyException("부모 노트가 존재하지 않습니다");
        }

        createdNote.RootNoteId = parentNote.RootNoteId ?? parentNote.Id;

        await _database.SaveChangesAsync(cancellationToken);
    }
}