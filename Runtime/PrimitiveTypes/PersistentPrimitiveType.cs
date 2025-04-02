using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;


namespace THEBADDEST.DataManagement.PrimitiveTypes
{
    public abstract class PersistentPrimitiveType<T> : INotifyPropertyChanged, IEquatable<T>
    {
        protected readonly T defaultValue;
        protected readonly string key;
        protected T value;

        public event PropertyChangedEventHandler PropertyChanged;

        public T Value
        {
            get => value;
            set => SetValue(value);
        }

        protected PersistentPrimitiveType(string key, T defaultValue)
        {
            if (string.IsNullOrEmpty(key))
                throw new ArgumentException("Key cannot be null or empty", nameof(key));

            this.key = key;
            this.defaultValue = defaultValue;
            this.value = defaultValue;

            Load();
        }

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public virtual void SetValue(T newValue)
        {
            if (!ValidateValue(newValue))
            {
                throw new ArgumentException($"Invalid value: {newValue}");
            }

            if (EqualityComparer<T>.Default.Equals(value, newValue)) return;

            value = newValue;
            OnPropertyChanged(nameof(Value));
            Save();
        }

        public virtual T GetValue() => value;

        protected virtual bool ValidateValue(T value)
        {
            return true;
        }

        public void ResetToDefault()
        {
            SetValue(defaultValue);
        }

        public virtual bool Equals(T other)
        {
            return EqualityComparer<T>.Default.Equals(value, other);
        }

        public override string ToString()
        {
            return value?.ToString() ?? string.Empty;
        }

        protected abstract void Save();
        protected abstract void Load();
    }
}