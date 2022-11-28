using HumbleNote.Domain.Models.Transfer;
using HumbleNote.Persistence.Models;

namespace HumbleNote.Domain.Models.Results;

public class ListChildrenResult
{
  public ListChildrenStatus Status { get; set; }
  public IEnumerable<NoteDto>? Children { get; set; }

  public ListChildrenResult(ListChildrenStatus status, IEnumerable<Note>? children)
  {
    Status = status;

    if (children != null)
    {
      Children = children.Select(note => new NoteDto(note));
    }
  }
}

public enum ListChildrenStatus
{
  Succeeded,
  InvalidCursor,
  ParentNotFound
}