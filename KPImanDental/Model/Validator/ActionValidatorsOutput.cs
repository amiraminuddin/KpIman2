namespace KPImanDental.Model.Validator
{
    public class ActionValidatorsOutput
    {
        public string ActionCode { get; set; } //Each action should have unique code
        public bool IsDisabled { get; set; }
        public bool IsLocked { get; set; }
        public bool IsVisible { get; set; }
        public bool IsViewOnly { get; set; } //Fo edit button -> make all field read-only and no save/update operation.
        public string LockedMessage { get; set; }
    }
}
