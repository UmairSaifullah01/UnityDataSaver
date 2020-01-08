using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

namespace UMDataManagement
{
    public class BinaryDataSaver : IDataSaver
    {
        /// <summary>
        /// holds data path 
        /// </summary>
        private string persistentDataPath;

        public BinaryDataSaver(string path = "")
        {
            persistentDataPath = string.IsNullOrEmpty(path) ? Application.persistentDataPath : path;
        }

        /// <summary>
        /// CanGet is used for get data if data exists
        /// </summary>
        /// <param name="key">data saved as named</param>
        /// <param name="dataObject">carries data</param>
        /// <typeparam name="T">type of data</typeparam>
        /// <returns>Return true if it finds data</returns>
        public bool CanGet<T>(string key, out T dataObject)
        {
            if (Contains(key))
            {
                using (var stream = new FileStream($"{persistentDataPath}/{key}.data", FileMode.Open))
                {
                    var formatter = new BinaryFormatter();
                    dataObject = (T) formatter.Deserialize(stream);
                    return true;
                }
            }
            else
            {
                dataObject = default(T);
                return false;
            }
        }

        /// <summary>
        /// Contains used to check is data file exist with name of key
        /// </summary>
        /// <param name="key">data saved as named</param>
        /// <returns></returns>
        public bool Contains(string key)
        {
            return File.Exists($"{persistentDataPath}/{key}.data");
        }

        public void Delete(string key)
        {
            File.Delete($"{persistentDataPath}/{key}.data");
        }

        public T Get<T>(string key)
        {
            using (var stream = new FileStream($"{persistentDataPath}/{key}.data", FileMode.Open))
            {
                var formatter = new BinaryFormatter();
                return (T) formatter.Deserialize(stream);
            }
        }

        public void Save<T>(string key, T dataObject)
        {
            using (var stream = new FileStream($"{persistentDataPath}/{key}.data", FileMode.Create))
            {
                var formatter = new BinaryFormatter();
                formatter.Serialize(stream, dataObject);
            }
        }
    }
}