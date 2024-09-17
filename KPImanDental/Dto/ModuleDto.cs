namespace KPImanDental.Dto
{
    public class ModuleDto
    {
        public long Id { get; set; }
        public int RowNumber { get; set; }
        public string ModuleName { get; set; }
        public string ModuleDescription { get; set; }
        public bool IsActive { get; set; }
        public byte[] ModuleIcon { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime UpdatedOn { get; set; } 
    }
}
