using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Xml.Serialization;
using UnityEngine;

public class DataHandler
{
    private static ISerializer mSerializer;
    private static IDataPersister mDataPersister;
    private static bool isInitialized = false;

    public static void Initialize(ISerializer serializer, IDataPersister dataPersister)
    {
        mSerializer = serializer;
        mDataPersister = dataPersister;
        isInitialized = true;
    }

    public static void Save<T>(string name, T dataObject)
    {
        if (isInitialized)
        {
            var value = mSerializer.Serialize(dataObject);
            mDataPersister.Save(name, value);
        }
        else
        {
            Debug.Log("Not Initialized");
        }
    }

    public static T Get<T>(string name)
    {
        if (isInitialized)
        {
            var value = mDataPersister.Retrieve(name);
            if (value != null) return mSerializer.Deserialize<T>(value);
            Debug.Log("No data Found with  name: " + name);
            return default(T);
        }
        else
        {
            Debug.Log("Not Initialized");
            return default(T);
        }
    }
}

public interface ISerializer
{
    string Serialize<T>(T data);
    T Deserialize<T>(string serializedData);
}

public interface IDataPersister
{
    void Save(string name, string serializedData);
    string Retrieve(string name);
}

public class UnitySerializer : ISerializer
{
    public string Serialize<T>(T data)
    {
        return JsonUtility.ToJson(data);
    }

    public T Deserialize<T>(string serializedData)
    {
        return JsonUtility.FromJson<T>(serializedData);
    }
}

public class UnityPersister : IDataPersister
{
    public void Save(string name, string serializedData)
    {
        PlayerPrefs.SetString(name, serializedData);
    }

    public string Retrieve(string name)
    {
        return PlayerPrefs.GetString(name);
    }
}
public class FilePersister : IDataPersister
{
    private string persistentDataPath;

    public FilePersister()
    {
        persistentDataPath = Application.persistentDataPath;
    }

    public void Save(string name, string serializedData)
    {
        if (File.Exists($"{persistentDataPath}/{name}.data"))
        {
            File.Delete($"{persistentDataPath}/{name}.data");
        }

        using (var stream = File.CreateText($"{persistentDataPath}/{name}.data"))
        {
            stream.Write(serializedData);
        }
    }

    public string Retrieve(string name)
    {
        var value = "";
        if (!File.Exists($"{persistentDataPath}/{name}.data")) return value;
        using (var stream = File.OpenText($"{persistentDataPath}/{name}.data"))
        {
            value = stream.ReadToEnd();
        }

        return value;
    }
}
public class BinaryPersister : IDataPersister, ISerializer
{
    private string persistentDataPath;
    private FileStream stream;
    private Action serialization;

    public BinaryPersister()
    {
        persistentDataPath = Application.persistentDataPath;
    }

    public void Save(string name, string serializedData)
    {
        stream = new FileStream($"{persistentDataPath}/{name}.data", FileMode.Create);
        serialization?.Invoke();
        stream.Close();
    }

    public string Retrieve(string name)
    {
        stream = new FileStream($"{persistentDataPath}/{name}.data", FileMode.Open);
        return "Binary Type: Cant Show It";
    }

    public string Serialize<T>(T data)
    {
        serialization = () =>
        {
            var formatter = new BinaryFormatter();
            formatter.Serialize(stream, data);
        };
        return "Binary Type: Cant Show It";
    }

    public T Deserialize<T>(string serializedData)
    {
        using (stream)
        {
            var formatter = new BinaryFormatter();
            return (T) formatter.Deserialize(stream);
        }
    }
}

//public class XmlPersister:ISerializer,IDataPersister
//{
//    public string Serialize<T>(T data)
//    {
//        XmlSerializer serializer = new XmlSerializer(typeof(T));
//        serializer.Serialize(data);
//        return "";
//    }
//
//    public T Deserialize<T>(string serializedData)
//    {
//        throw new NotImplementedException();
//    }
//
//    public void Save(string name, string serializedData)
//    {
//        throw new NotImplementedException();
//    }
//
//    public string Retrieve(string name)
//    {
//        throw new NotImplementedException();
//    }
//}