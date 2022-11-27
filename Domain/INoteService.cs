using HumbleNote.Domain.Models.Commands;
using HumbleNote.Domain.Models.Results;

namespace HumbleNote.Domain;

public interface INoteService
{
    Task<CreateNoteResult> CreateNoteAsync(CreateNoteCommand command,
                                           CancellationToken cancellationToken = default);

    Task<ListNoteResult> ListNoteAsync(string userId, string? cursor = null,
                                       CancellationToken cancellationToken = default);
}