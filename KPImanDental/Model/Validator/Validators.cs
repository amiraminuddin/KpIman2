using KPImanDental.Enums;

namespace KPImanDental.Model.Validator
{
    public class Validators
    {
        public string Field { get; set; }  // Corresponds to a field in Dto
        public bool IsValid { get; set; }   // Validation result
        public string Message { get; set; } // Validation message

        public ValidatorsType ValidatorsType { get; set; }
    }
}
