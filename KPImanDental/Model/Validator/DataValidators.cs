using KPImanDental.Enums;

namespace KPImanDental.Model.Validator
{
    public class DataValidators<T>
    {
        public T Data {  get; set; }
        public ValidatorTriggerType TriggerType { get; set; }
    }
}
