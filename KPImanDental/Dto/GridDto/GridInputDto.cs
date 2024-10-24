namespace KPImanDental.Dto.GridDto
{
    public class GridInputDto
    {
        public int CurrentPage { get; set; }
        public int PageSize { get; set; } = 5;

        public List<SortMeta> SortMeta { get; set; }
    }

    public class SortMeta
    {
        public string Field { get; set; }
        public int Order { get; set; }
    }
}
