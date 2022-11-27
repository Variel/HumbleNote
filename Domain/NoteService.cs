using HumbleNote.Domain.Exceptions;
using HumbleNote.Domain.Extensions;
using HumbleNote.Domain.Models.Commands;
using HumbleNote.Domain.Models.Events;
using HumbleNote.Domain.Models.Results;
using HumbleNote.Persistence.Models;
using HumbleNote.Services;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HumbleNote.Domain;

public class NoteService : INoteService
{
    private readonly DatabaseContext _database;
    private readonly IMediator _mediator;

    public NoteService(DatabaseContext database, IMediator mediator)
    {
        _database = database;
        _mediator = mediator;
    }

    public async Task<CreateNoteResult> CreateNoteAsync(CreateNoteCommand command,
                                                        CancellationToken cancellationToken
                                                            = default)
    {
        using var transaction = await _database.Database.BeginTransactionAsync(cancellationToken);

        var note = command.ToNote();
        _database.Notes.Add(note);

        await _database.SaveChangesAsync(cancellationToken);

        try
        {
            await _mediator.Publish(new NoteCreatedEvent(note), cancellationToken);
        }
        catch (NoteHierarchyException)
        {
            await transaction.RollbackAsync(cancellationToken);
            return new CreateNoteResult(CreateNoteStatus.ParentNotFound);
        }
        catch (NoteRevisionException)
        {
            await transaction.RollbackAsync(cancellationToken);
            return new CreateNoteResult(CreateNoteStatus.OldVersionNotFound);
        }

        await transaction.CommitAsync(cancellationToken);
        return new CreateNoteResult(CreateNoteStatus.Succeeded, note);
    }

    public async Task<ListNoteResult> ListNoteAsync(string userId, string? cursor = null,
                                                    CancellationToken cancellationToken = default)
    {
        IQueryable<Note> filterQuery = _database.Notes.OrderByDescending(n => n.Id);

        if (cursor == null)
        {
            filterQuery = filterQuery.Where(n => n.UserId == userId);
        }
        else
        {
            filterQuery = filterQuery.OrderByDescending(n => n.Id)
                                     .Where(n => n.UserId == userId
                                              && n.Id.CompareTo(cursor) < 0);
        }

        var notes = await filterQuery.Take(10).ToArrayAsync();

        return new ListNoteResult(ListNoteStatus.Succeeded, notes);
    }
}