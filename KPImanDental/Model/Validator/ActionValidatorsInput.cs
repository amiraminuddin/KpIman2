namespace KPImanDental.Model.Validator
{
    public class ActionValidatorsInput<T>
    {
        public T Data { get; set; } //Get Data for parameter formula evaluator
        public List<string> ActionCode { get; set; } //List of action
    }
}
