namespace KPImanDental.Dto.ChartDto
{
    public class OrganizationChartDto
    {
        public string Label { get; set; }
        public string Type { get; set; }
        public string StyleClass { get; set; }
        public bool Expanded { get; set; }
        public OrganizationChartDataDto Data { get; set; }
        public List<OrganizationChartDto> Children {  get; set; } = new List<OrganizationChartDto>();
    }

    public class OrganizationChartDataDto
    {
        public string Name { get; set; }
        public string ProfilePicture { get; set; }
    }
}
