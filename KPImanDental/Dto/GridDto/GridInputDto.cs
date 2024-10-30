namespace KPImanDental.Dto.GridDto
{
    public class GridInputDto
    {
        public int CurrentPage { get; set; }
        public int PageSize { get; set; } = 5;
        public string FilterValue { get; set; }
        public List<FilterColumn> FilterColumn { get; set; }
        public List<SortMeta> SortMeta { get; set; }

        public List<WhereClause> WhereCondition { get; set; }

    }

    public class SortMeta
    {
        public string Field { get; set; }
        public int Order { get; set; }
    }

    public class FilterColumn
    {
        public string Field { get; set; }
    }

    public class WhereClause
    {
        public string Field { get; set; }
        public string Value { get; set; }
    }
}
