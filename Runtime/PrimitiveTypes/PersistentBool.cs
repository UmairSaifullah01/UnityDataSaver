namespace THEBADDEST.DataManagement.PrimitiveTypes
{
    public class PersistentBool : PersistentPrimitiveType<bool>
    {
        public PersistentBool(string key, bool defaultValue = false)
            : base(key, defaultValue)
        {
        }

        // Boolean operations
        public void Toggle() => SetValue(!value);

        public void And(bool other) => SetValue(value && other);
        public void Or(bool other) => SetValue(value || other);
        public void Xor(bool other) => SetValue(value ^ other);
        public void Not() => SetValue(!value);

        protected override void Save()
        {
            DataPersistor.Save(key,value ? 1 : 0);
        }

        protected override void Load()
        {
            if (DataPersistor.CanGet(key, out int savedValue))
            {
                SetValue(savedValue==1);
            }
            else
            {
                SetValue(defaultValue);
            }
        }

        // Comparison operators
        public static bool operator ==(PersistentBool lhs, PersistentBool rhs) => lhs?.value == rhs?.value;
        public static bool operator !=(PersistentBool lhs, PersistentBool rhs) => !(lhs == rhs);

        // Logical operators
        public static bool operator &(PersistentBool lhs, PersistentBool rhs) => lhs.value && rhs.value;
        public static bool operator |(PersistentBool lhs, PersistentBool rhs) => lhs.value || rhs.value;
        public static bool operator ^(PersistentBool lhs, PersistentBool rhs) => lhs.value ^ rhs.value;
        public static bool operator !(PersistentBool value) => !value.value;

        // Implicit conversion operators
        public static implicit operator bool(PersistentBool primitive) => primitive.value;

        public override bool Equals(object obj)
        {
            if (obj is PersistentBool other) return value == other.value;
            if (obj is bool boolValue) return value == boolValue;
            return false;
        }

        public override int GetHashCode()
        {
            return value.GetHashCode();
        }
    }

    // Alias for shorter usage
    public class PBool : PersistentBool
    {
        public PBool(string key, bool defaultValue = false)
            : base(key, defaultValue)
        {
        }
    }
}