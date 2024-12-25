using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;


namespace THEBADDEST.DataManagement
{


	[CustomEditor(typeof(DataPersistorSettings))]
	public class DataPersistorSettingsEditor : Editor
	{
		private DataPersistorSettings settings;
		private Type[]          dataSavers;
		private string[]              dataSaverNames;

		private void OnEnable()
		{
			settings       = (DataPersistorSettings)target;
			dataSavers     = GetIDataSaverImplementations();
			dataSaverNames = new string[dataSavers.Length];

			for (int i = 0; i < dataSavers.Length; i++)
			{
				dataSaverNames[i] = dataSavers[i].Name;
			}
		}

		public override void OnInspectorGUI()
		{
			base.OnInspectorGUI();

			EditorGUI.BeginChangeCheck();

			int currentIndex = Array.IndexOf(dataSaverNames, settings.dataSaverType);
			int newIndex     = EditorGUILayout.Popup("Data Saver Type", currentIndex, dataSaverNames);

			if (EditorGUI.EndChangeCheck())
			{
				settings.dataSaverType = dataSaverNames[newIndex];
				EditorUtility.SetDirty(settings);
			}
		}

		private Type[] GetIDataSaverImplementations()
		{
			Assembly[]       assemblies = AppDomain.CurrentDomain.GetAssemblies();
			List<Type> dataSavers = new List<Type>();

			foreach (Assembly assembly in assemblies)
			{
				Type[] types = assembly.GetTypes();

				foreach (Type type in types)
				{
					if (typeof(IDataSaver).IsAssignableFrom(type) && !type.IsAbstract && !type.IsInterface)
					{
						dataSavers.Add(type);
					}
				}
			}

			return dataSavers.ToArray();
		}
	}


}