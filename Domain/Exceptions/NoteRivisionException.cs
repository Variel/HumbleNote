namespace HumbleNote.Domain.Exceptions;

public class NoteRivisionException : Exception
{
    public NoteRivisionException(string subMessage) : base("노트 리비전을 만들 수 없습니다 : " + subMessage) { }
}