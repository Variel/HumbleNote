using HumbleNote.Domain.Exceptions;
using HumbleNote.Domain.Extensions;
using HumbleNote.Domain.Models.Commands;
using HumbleNote.Domain.Models.Events;
using HumbleNote.Domain.Models.Results;
using HumbleNote.Services;
using MediatR;

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
        catch (NoteRivisionException)
        {
            await transaction.RollbackAsync(cancellationToken);
            return new CreateNoteResult(CreateNoteStatus.OldVersionNotFound);
        }

        await transaction.CommitAsync(cancellationToken);
        return new CreateNoteResult(CreateNoteStatus.Succeeded, note);
    }
}