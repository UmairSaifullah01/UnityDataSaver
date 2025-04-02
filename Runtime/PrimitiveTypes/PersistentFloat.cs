using UnityEngine;


namespace THEBADDEST.DataManagement.PrimitiveTypes
{
    public class PersistentFloat : PersistentPrimitiveType<float>
    {
        private readonly float minValue;
        private readonly float maxValue;
        private readonly bool hasConstraints;

        public PersistentFloat(string key, float defaultValue = 0f, float minValue = float.MinValue, float maxValue = float.MaxValue)
            : base(key, defaultValue)
        {
            this.minValue       = minValue;
            this.maxValue       = maxValue;
            this.hasConstraints = !Mathf.Approximately(minValue, float.MinValue) || !Mathf.Approximately(maxValue, float.MaxValue);
        }

        protected override bool ValidateValue(float value)
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
            if (DataPersistor.CanGet(key, out float savedValue))
            {
                SetValue(savedValue);
            }
            else
            {
                SetValue(defaultValue);
            }
        }

        // Arithmetic operators
        public static float operator +(PersistentFloat lhs, PersistentFloat rhs) => lhs.value + rhs.value;
        public static float operator -(PersistentFloat lhs, PersistentFloat rhs) => lhs.value - rhs.value;
        public static float operator *(PersistentFloat lhs, PersistentFloat rhs) => lhs.value * rhs.value;
        public static float operator /(PersistentFloat lhs, PersistentFloat rhs) => lhs.value / rhs.value;

        // Comparison operators
        public static bool operator ==(PersistentFloat lhs, PersistentFloat rhs) => lhs?.value == rhs?.value;
        public static bool operator !=(PersistentFloat lhs, PersistentFloat rhs) => !(lhs == rhs);
        public static bool operator >(PersistentFloat lhs, PersistentFloat rhs) => lhs.value > rhs.value;
        public static bool operator <(PersistentFloat lhs, PersistentFloat rhs) => lhs.value < rhs.value;
        public static bool operator >=(PersistentFloat lhs, PersistentFloat rhs) => lhs.value >= rhs.value;
        public static bool operator <=(PersistentFloat lhs, PersistentFloat rhs) => lhs.value <= rhs.value;

        // Implicit conversion operators
        public static implicit operator float(PersistentFloat primitive) => primitive.value;

        // Utility methods
        public void Add(float amount) => SetValue(value + amount);
        public void Subtract(float amount) => SetValue(value - amount);
        public void Multiply(float amount) => SetValue(value * amount);
        public void Divide(float amount) => SetValue(value / amount);

        public override bool Equals(object obj)
        {
            if (obj is PersistentFloat other) return Mathf.Approximately(value, other.value);
            if (obj is float floatValue) return Mathf.Approximately(value,      floatValue);
            return false;
        }

        public override int GetHashCode()
        {
            return value.GetHashCode();
        }
    }

    // Alias for shorter usage
    public class PFloat : PersistentFloat
    {
        public PFloat(string key, float defaultValue = 0f, float minValue = float.MinValue, float maxValue = float.MaxValue)
            : base(key, defaultValue, minValue, maxValue)
        {
        }
    }
}