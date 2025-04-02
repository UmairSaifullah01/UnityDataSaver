namespace THEBADDEST.DataManagement.PrimitiveTypes
{
    public class PersistentInt : PersistentPrimitiveType<int>
    {
        private readonly int minValue;
        private readonly int maxValue;
        private readonly bool hasConstraints;

        public PersistentInt(string key, int defaultValue = 0, int minValue = int.MinValue, int maxValue = int.MaxValue)
            : base(key, defaultValue)
        {
            this.minValue = minValue;
            this.maxValue = maxValue;
            this.hasConstraints = minValue != int.MinValue || maxValue != int.MaxValue;
        }

        protected override bool ValidateValue(int value)
        {
            if (!hasConstraints) return true;
            return value >= minValue && value <= maxValue;
        }

        protected override void Save()
        {
            DataPersistor.Save(key, value);
        }

        protected override void Load()
        {
            if (DataPersistor.CanGet(key, out int savedValue))
            {
                SetValue(savedValue);
            }
            else
            {
                SetValue(defaultValue);
            }
        }

        // Arithmetic operators
        public static int operator +(PersistentInt lhs, PersistentInt rhs) => lhs.value + rhs.value;
        public static int operator -(PersistentInt lhs, PersistentInt rhs) => lhs.value - rhs.value;
        public static int operator *(PersistentInt lhs, PersistentInt rhs) => lhs.value * rhs.value;
        public static int operator /(PersistentInt lhs, PersistentInt rhs) => lhs.value / rhs.value;

        // Comparison operators
        public static bool operator ==(PersistentInt lhs, PersistentInt rhs) => lhs?.value == rhs?.value;
        public static bool operator !=(PersistentInt lhs, PersistentInt rhs) => !(lhs == rhs);
        public static bool operator >(PersistentInt lhs, PersistentInt rhs) => lhs.value > rhs.value;
        public static bool operator <(PersistentInt lhs, PersistentInt rhs) => lhs.value < rhs.value;
        public static bool operator >=(PersistentInt lhs, PersistentInt rhs) => lhs.value >= rhs.value;
        public static bool operator <=(PersistentInt lhs, PersistentInt rhs) => lhs.value <= rhs.value;

        // Implicit conversion operators
        public static implicit operator int(PersistentInt primitive) => primitive.value;

        // Utility methods
        public void Increment() => SetValue(value + 1);
        public void Decrement() => SetValue(value - 1);
        public void Add(int amount) => SetValue(value + amount);
        public void Subtract(int amount) => SetValue(value - amount);
        public void Multiply(int amount) => SetValue(value * amount);
        public void Divide(int amount) => SetValue(value / amount);

        public override bool Equals(object obj)
        {
            if (obj is PersistentInt other) return value == other.value;
            if (obj is int intValue) return value == intValue;
            return false;
        }

        public override int GetHashCode()
        {
            return value.GetHashCode();
        }
    }

    // Alias for shorter usage
    public class PInt : PersistentInt
    {
        public PInt(string key, int defaultValue = 0, int minValue = int.MinValue, int maxValue = int.MaxValue)
            : base(key, defaultValue, minValue, maxValue)
        {
        }
    }
}