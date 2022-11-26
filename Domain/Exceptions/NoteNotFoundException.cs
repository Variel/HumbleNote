namespace HumbleNote.Domain.Exceptions;

public class NoteNotFoundException : Exception
{
    public NoteNotFoundException() : base("노트가 존재하지 않습니다") { }
}