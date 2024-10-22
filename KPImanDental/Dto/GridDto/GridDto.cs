namespace KPImanDental.Dto.GridDto
{
    public class GridDto<T>
    {
        public int TotalData { get; set; }
        public T Data { get; set; }
        public int PageSize {  get; set; }
        public int TotalPages
        {
            get
            {
                return (int)Math.Ceiling(TotalData / (double)PageSize);

            }
        }
    }
}
