namespace Framework.Database;

public interface ISorting
{
    string FieldName { get; set; }
    bool Ascending { get; set; }
}