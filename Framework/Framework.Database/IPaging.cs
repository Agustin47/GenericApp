namespace Framework.Database;

public interface IPaging
{
    int Size { get; set; }
    int Index { get; set; }
}