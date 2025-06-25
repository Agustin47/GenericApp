namespace Framework.Database;

public interface IFilter
{
    string FieldName { get; set; }
    object Value { get; set; }
}