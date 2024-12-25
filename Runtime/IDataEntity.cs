namespace THEBADDEST.DataManagement
{
	public interface IDataEntity
	{

		string Key { get;}

		void SaveData(Data data);

		Data LoadData();

	}
}