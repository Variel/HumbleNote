namespace HumbleNote.Domain.Exceptions;

public class NoteHierarchyException : Exception
{
    public NoteHierarchyException(string subMessage) : base("노트 계층 관계를 만들 수 없습니다 : " + subMessage) { }
}