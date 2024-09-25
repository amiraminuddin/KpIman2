using KPImanDental.Enums;

namespace KPImanDental.Model
{
    public class DeletionCondition<T>
    {
        public T Entity { get; set; }

        public int DependenciesCount { get; set; }
        public bool HasDependencies => DependenciesCount > 0;

        public string Message { get; set; }

        public MessageType MessageType { get; set; }

        public bool CanDelete => DeleteCondition();

        public bool DeleteCondition()
        {
            return !HasDependencies;
        }
    }
}
