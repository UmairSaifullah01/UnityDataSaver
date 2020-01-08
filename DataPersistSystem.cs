using System;
using System.Collections.Generic;
using UMGS;
using UnityEngine;

namespace UMDataManagement
{
    public class DataPersistSystem : Singleton<DataPersistSystem>
    {
        private Dictionary<string, object> allData;
        private IDataSaver sdataSaver;

        protected override void Awake()
        {
            base.Awake();
            Initialize(true);
            
        }


        public object this[string key] => allData[key];

        public void Initialize(bool autoLoad)
        {
            allData = new Dictionary<string, object>();
            sdataSaver = new BinaryDataSaver();
            if (autoLoad)
                Load();
        }

        public void Initialize(IDataSaver dataSaver, bool autoLoad)
        {
            allData = new Dictionary<string, object>();
            sdataSaver = dataSaver;
            if (autoLoad)
                Load();
        }

        public bool CanGet<T>(string key, out T dataObject)
        {
            if (allData.ContainsKey(key))
            {
                dataObject = (T) allData[key];
                return true;
            }
            else
            {
                dataObject = default(T);
                return false;
            }
        }

        public bool Contains(string key)
        {
            return allData.ContainsKey(key);
        }

        public void Delete(string key)
        {
            allData.Remove(key);
        }

        public T Get<T>(string key)
        {
            return (T) allData[key];
        }

        public void Add<T>(string key, T dataObject)
        {
            if (allData.ContainsKey(key))
            {
                allData[key] = dataObject;
            }
            else
            {
                allData.Add(key, dataObject);
            }
        }

        public void Load()
        {
            if (!sdataSaver.Contains("UjsavedData"))
                return;

            List<Entry> serializedData = sdataSaver.Get<List<Entry>>("UjsavedData");
            foreach (var entry in serializedData)
            {
                allData[entry.Key] = entry.Value;
            }
        }

        private void OnApplicationQuit()
        {
            Save();
        }

        public void Save()
        {
            List<Entry> serializedData = new List<Entry>(allData.Count);

            foreach (string key in allData.Keys)
            {
                serializedData.Add(new Entry(key, allData[key]));
            }

            sdataSaver.Save("UjsavedData", serializedData);
        }

        [System.Serializable]
        public class Entry
        {
            public string Key;
            public object Value;

            public Entry()
            {
            }

            public Entry(string key, object value)
            {
                Key = key;
                Value = value;
            }
        }
    }
}