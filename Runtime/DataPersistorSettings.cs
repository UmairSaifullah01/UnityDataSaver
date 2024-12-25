
namespace THEBADDEST.DataManagement
{


	using UnityEngine;

	[CreateAssetMenu(menuName = "THEBADDEST/DataManagement/DataPersistorSettings", fileName = "DataPersistorSettings", order = 0)]
	public class DataPersistorSettings : ScriptableObject
	{
		[HideInInspector]public string dataSaverType = "BinaryDataSaver";
		public string GameKey       = "gamedata";
	}


}