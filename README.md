# DataPersistor

**DataPersistor** is a Unity utility designed for data persistence, allowing developers to save and load data using various formats such as binary, JSON, XML, and Unity's native serialization. This flexibility makes it easy to manage game data efficiently.

## Table of Contents

- [Technologies Used](#technologies-used)
- [Project Architecture](#project-architecture)
- [Usage Examples](#usage-examples)
- [Installation](#installation)
- [Additional Notes](#additional-notes)
- [Changelog](#changelog)

## Technologies Used

- **C#**
- **Unity**

## Project Architecture

### Runtime/

Contains C# scripts that implement the data persistence logic.

- **Data.cs**: Defines the data structure to be persisted.
- **DataPersistor.cs**: The main class that manages the persistence.
- **DataPersistorSettings.cs**: Persistence system settings.
- **DataSaver/**: Implementations of different saving methods.
  - **BinaryDataSaver.cs**: Saving in binary format.
  - **IDataSaver.cs**: Interface for the different types of `DataSaver`.
  - **JsonDataSaver.cs**: Saving in JSON format.
  - **UnityDataSaver.cs**: Saving using Unity's serialization system.
  - **XmlDataSaver.cs**: Saving in XML format.
- **IDataEntity.cs**: Interface that entities to be persisted must implement.

### Editor/

Contains scripts that extend the Unity Editor interface.

- **DataPersistorSettingsEditor.cs**: Custom editor for persistence settings.

### Resources/

Resources used by the project at runtime.

- **DataPersistorSettings.asset**: Persistence settings file.

### Root Files

- **.git/**: Git version control system directory.
- **CHANGELOG.md**: Project change history.
- **README.md**: This documentation file.
- **package.json**: Package manifest file, probably for dependency management.

## Usage Examples

### Saving Data

To save data using the DataPersistor, you can create a class that implements `IDataEntity` and use the `Save` method:

```csharp
using UnityEngine;

[System.Serializable]
public class GameData : IDataEntity
{
    public int playerScore;
    public string playerName;

    public string Key => "GameData"; // Unique key for saving
}

public class GameManager : MonoBehaviour
{
    void Start()
    {
        GameData data = new GameData { playerScore = 100, playerName = "Player1" };
        DataPersistor.Save(data.Key, data);
    }
}
```

### Loading Data

```csharp
public class GameManager : MonoBehaviour
{
    void Start()
    {
        GameData data = DataPersistor.Load<GameData>("GameData");
        if (data != null)
        {
            Debug.Log($"Loaded data: {data.playerName} with score {data.playerScore}");
            }
        }
    }
}
```

### Removing Data

```csharp
DataPersistor.Delete("GameData");
```

### Installation

1. Clone the repository or download the package.
2. Import the package into your Unity project.
3. Configure the DataPersistorSettings in the Unity Editor to choose your preferred data saver type.
4. Use the DataPersistor class to save, load, and delete data.

### Additional Notes

The project structure is designed for easy extensibility and maintainability. The inclusion of a custom editor for settings enhances the user experience.

Consider contributing to the project by adding more data saver implementations or improving the documentation.
