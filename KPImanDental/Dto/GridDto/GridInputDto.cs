namespace KPImanDental.Dto.GridDto
{
    public class GridInputDto
    {
        public int CurrentPage { get; set; }
        public int PageSize { get; set; } = 5;

        public string SortableInput { get; set; }
        public string SortableMode { get; set; }
    }
}
