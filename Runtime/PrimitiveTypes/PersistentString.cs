using System.Text.RegularExpressions;


namespace THEBADDEST.DataManagement.PrimitiveTypes
{
    public class PersistentString : PersistentPrimitiveType<string>
    {
        private readonly int maxLength;
        private readonly string validationPattern;

        public PersistentString(string key, string defaultValue = "", int maxLength = int.MaxValue, string validationPattern = null)
            : base(key, defaultValue)
        {
            this.maxLength = maxLength;
            this.validationPattern = validationPattern;
        }

        // String manipulation methods
        public void Append(string text) => SetValue(value + text);
        public void Prepend(string text) => SetValue(text + value);
        public void Insert(int index, string text) => SetValue(value.Insert(index, text));
        public void Remove(int startIndex, int count) => SetValue(value.Remove(startIndex, count));
        public void Replace(string oldValue, string newValue) => SetValue(value.Replace(oldValue, newValue));
        public void Clear() => SetValue(string.Empty);
        public void Trim() => SetValue(value.Trim());
        public void ToUpper() => SetValue(value.ToUpper());
        public void ToLower() => SetValue(value.ToLower());

        // Properties
        public int Length => value?.Length ?? 0;
        public bool IsEmpty => string.IsNullOrEmpty(value);
        public bool IsNullOrWhiteSpace => string.IsNullOrWhiteSpace(value);

        protected override bool ValidateValue(string value)
        {
            if (value == null) return false;
            if (value.Length > maxLength) return false;
            if (!string.IsNullOrEmpty(validationPattern) && !Regex.IsMatch(value, validationPattern)) return false;
            return true;
        }

        protected override void Save()
        {
            DataPersistor.Save(key, value);
        }

        protected override void Load()
        {
            if (DataPersistor.CanGet(key, out string savedValue))
            {
                SetValue(savedValue);
            }
            else
            {
                SetValue(defaultValue);
            }
        }

        // Comparison operators
        public static bool operator ==(PersistentString lhs, PersistentString rhs) => lhs?.value == rhs?.value;
        public static bool operator !=(PersistentString lhs, PersistentString rhs) => !(lhs == rhs);

        // Implicit conversion operators
        public static implicit operator string(PersistentString primitive) => primitive.value;

        public override bool Equals(object obj)
        {
            if (obj is PersistentString other) return value == other.value;
            if (obj is string stringValue) return value == stringValue;
            return false;
        }

        public override int GetHashCode()
        {
            return value?.GetHashCode() ?? 0;
        }
    }

    // Alias for shorter usage
    public class PString : PersistentString
    {
        public PString(string key, string defaultValue = "", int maxLength = int.MaxValue, string validationPattern = null)
            : base(key, defaultValue, maxLength, validationPattern)
        {
        }
    }
}