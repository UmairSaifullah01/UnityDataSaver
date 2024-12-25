## DataPersistor

This project seems to be a utility for data persistence in Unity, offering different methods of saving, such as binary, JSON, XML, and Unity's native format.

## Technologies Used:

- C#
- Unity

## Project Architecture

### Runtime/

Contains C# scripts that implement the data persistence logic.

- `Data.cs`: Probably defines the data structure to be persisted.
- `DataPersistor.cs`: The main class that manages the persistence.
- `DataPersistorSettings.cs`: Persistence system settings.
- `DataSaver/`: Implementations of different saving methods.
    - `BinaryDataSaver.cs`: Saving in binary format.
    - `IDataSaver.cs`: Interface for the different types of `DataSaver`.
    - `JsonDataSaver.cs`: Saving in JSON format.
    - `UnityDataSaver.cs`: Saving using Unity's serialization system.
    - `XmlDataSaver.cs`: Saving in XML format.
- `IDataEntity.cs`: Interface that entities to be persisted must implement.

### Editor/

Contains scripts that extend the Unity Editor interface.

- `DataPersistorSettingsEditor.cs`: Custom editor for persistence settings.

### Resources/

Resources used by the project at runtime.

- `DataPersistorSettings.asset`: Persistence settings file.

### Root Files

- `.git/`: Git version control system directory.
- `CHANGELOG.md`: Project change history.
- `README.md`: This documentation file.
- `package.json`: Package manifest file, probably for dependency management.


## Additional Notes

The project structure suggests a well-organized solution for data persistence in Unity, offering flexibility in the choice of saving format. The inclusion of a custom editor for settings shows attention to detail in the user experience. It would be interesting to add usage examples to the README to facilitate the adoption of the utility.