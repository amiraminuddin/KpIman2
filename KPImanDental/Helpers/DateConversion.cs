namespace KPImanDental.Helpers
{
    public static class DateConversion
    {
        public static string ConvertDateTime(DateTime Date, string Format)
        {
            return Date.ToString(Format);
        }
    }
}
