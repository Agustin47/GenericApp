using Framework.Database;

namespace GenericWebApp.Models;

public class QueryBaseModel
{
    public FilterModel[]? Filters { get; set; }
    public SortingModel? Sorting { get; set; }
    public PagingModel? Paging { get; set; }
}

public class FilterModel : IFilter{
    public string FieldName { get; set; }
    public object Value { get; set; }
}

public class SortingModel : ISorting
{
    public string FieldName { get; set; }
    public bool Ascending { get; set; }
}

public class PagingModel : IPaging
{
    public int Size { get; set; }
    public int Index { get; set; }
}