namespace THEBADDEST.DataManagement
{
	public interface IDataElement
	{

		string dataTag { get;}

		Data SaveData();

		void LoadData(Data data);

	}
}