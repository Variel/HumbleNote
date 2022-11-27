using HumbleNote.Domain.Exceptions;
using HumbleNote.Domain.Models.Events;
using HumbleNote.Services;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HumbleNote.Domain.EventHandlers;

public class OnNoteCreatedCreateRevisionHandler : INotificationHandler<NoteCreatedEvent>
{
    private readonly DatabaseContext _database;

    public OnNoteCreatedCreateRevisionHandler(DatabaseContext database)
    {
        _database = database;
    }

    public async Task Handle(NoteCreatedEvent notification, CancellationToken cancellationToken)
    {
        if (notification.OldVersionNoteId is null)
        {
            return;
        }

        var createdNote = await _database.Notes.FirstOrDefaultAsync(
            n => n.Id == notification.NoteId,
            cancellationToken);
        if (createdNote is null)
        {
            throw new NoteNotFoundException();
        }

        var oldVersionNote = await _database.Notes.FirstOrDefaultAsync(
            n => n.Id == notification.OldVersionNoteId && n.UserId == notification.UserId,
            cancellationToken);
        if (oldVersionNote is null)
        {
            throw new NoteHierarchyException("이전 버전 노트가 존재하지 않습니다");
        }

        createdNote.OldVersionNoteId = oldVersionNote.Id;
        createdNote.RootNoteId = oldVersionNote.RootNoteId;
        createdNote.ParentNoteId = oldVersionNote.ParentNoteId;
        
        oldVersionNote.ParentNoteId = null;
        oldVersionNote.RootNoteId = null;
        oldVersionNote.NewVersionNoteId = createdNote.Id;

        await _database.SaveChangesAsync(cancellationToken);
    }
}