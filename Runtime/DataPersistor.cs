using System.Collections.Generic;
using System.Linq;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace THEBADDEST.DataManagement
{


	public static class DataPersistor
	{

		private static  string GameKey = "gamedata";

		private static readonly Dictionary<string, object> DataDictionary = new Dictionary<string, object>();

		private static IDataSaver dataSaver;
		private static bool       isInitialized;

		/// <summary>
		/// initialize data system
		/// </summary>
		static void Initialize()
		{
			Debug.Log($"Data Initialized with BinaryDataSaver {Application.persistentDataPath}");
			var path = Application.persistentDataPath;
			var settings      = Resources.Load<DataPersistorSettings>("DataPersistorSettings");
			GameKey = settings.GameKey;
			switch (settings.dataSaverType)
			{
				case "BinaryDataSaver":
					dataSaver = new BinaryDataSaver(path);
					break;
				case "JsonDataSaver":
					dataSaver = new JsonDataSaver(path);				
					break;
				case "XmlDataSaver":
					dataSaver = new XmlDataSaver(path);
					break;
				case "UnityDataSaver":
					dataSaver = new UnityDataSaver();
					break;
				default:
					dataSaver     = new BinaryDataSaver();
					break;
			}
			
			isInitialized = true;
			Load();
		}

		/// <summary>
		/// initialize data system with desired IDataSaver type data saver
		/// </summary>
		/// <param name="dataSaver"></param>
		public static void Initialize(IDataSaver dataSaver)
		{
			DataPersistor.dataSaver = dataSaver;
			isInitialized         = true;
		}

		/// <summary>
		/// CanGet is used for get data if data exists
		/// </summary>
		/// <param name="key">data saved as named</param>
		/// <param name="dataObject">carries data</param>
		/// <typeparam name="T">type of data</typeparam>
		/// <returns>Return true if it finds data</returns>
		public static bool CanGet<T>(string key, out T dataObject)
		{
			if (!isInitialized)
				Initialize();
			if (DataDictionary.ContainsKey(key))
			{
				dataObject = (T)DataDictionary[key];
				return true;
			}

			dataObject = default(T);
			return false;
		}

		/// <summary>
		/// Contains used to check is data exist with name of key
		/// </summary>
		/// <param name="key">data saved as named</param>
		/// <returns></returns>
		public static bool Contains(string key)
		{
			if (!isInitialized)
				Initialize();
			return DataDictionary.ContainsKey(key);
		}

		/// <summary>
		/// Delete saved data by key(data saved name)
		/// </summary>
		/// <param name="key"></param>
		public static void Delete(string key)
		{
			if (!isInitialized)
				Initialize();
			DataDictionary.Remove(key);
		}

		/// <summary>
		/// Used to get data by key
		/// </summary>
		/// <param name="key"></param>
		/// <typeparam name="T"></typeparam>
		/// <returns></returns>
		public static T Get<T>(string key)
		{
			if (!isInitialized)
				Initialize();
			if (DataDictionary.ContainsKey(key))
				return (T)DataDictionary[key];
			return default(T);
		}
		/// <summary>
		/// Get Data of DataElement
		/// </summary>
		/// <param name="dataElement"></param>
		public static void Get(IDataElement dataElement)
		{
			if (!isInitialized)
				Initialize();
			Data data = Get<Data>(dataElement.dataTag);
			dataElement.LoadData(data);
			if (data == null)
				Debug.Log("Data not found");
		}
		/// <summary>
		/// Used to saved data by key 
		/// </summary>
		/// <param name="key"></param>
		/// <param name="dataObject"></param>
		/// <typeparam name="T"></typeparam>
		public static void Save<T>(string key, T dataObject)
		{
			if (!isInitialized)
				Initialize();
			DataDictionary[key] = dataObject;
		}

		/// <summary>
		/// Save Data of IDataElement 
		/// </summary>
		/// <param name="dataElement"></param>
		public static void Save(IDataElement dataElement)
		{
			if (!isInitialized)
				Initialize();
			Save(dataElement.dataTag, dataElement.SaveData());
		}
        /// <summary>
        /// Serializes the current data in the DataDictionary and saves it to a file using the dataSaver.
        /// </summary>
        /// <remarks>
        /// Converts each key-value pair in the DataDictionary into an Entry object and stores them in a list.
        /// The list is then saved to a file identified by GameKey.
        /// </remarks>
		public static void Persist()
		{
			if(!isInitialized)
				Initialize();
			var serializedData = new List<Entry>(DataDictionary.Count);
			serializedData.AddRange(DataDictionary.Keys.Select(key => new Entry(key, DataDictionary[key])));
			dataSaver.Save(GameKey, serializedData);
		}

		/// <summary>
		/// Loads data from the file identified by GameKey into the DataDictionary.
		/// </summary>
		/// <remarks>
		/// Converts each Entry object from the loaded list into a key-value pair and stores them in the DataDictionary.
		/// </remarks>
		public static void Load()
		{
			if (!dataSaver.Contains(GameKey))
				return;
			var serializedData = dataSaver.Get<List<Entry>>(GameKey);
			foreach (Entry entry in serializedData)
			{
				DataDictionary[entry.Key] = entry.Value;
			}
		}
		#if UNITY_EDITOR
		[MenuItem ("Tools/THEBADDEST/DataManagement/DeleteData")]
		public static void DeleteAllData ()
		{
			if(!isInitialized)
				Initialize();
			DataDictionary.Clear();
			Persist();
			dataSaver.Delete(GameKey);
			Debug.Log("Data Deleted");
		}
		#endif
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
			Key   = key;
			Value = value;
		}

	}
	
}