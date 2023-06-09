﻿using System.IO;
using UnityEditor;
using UnityEngine;

namespace Opencoding.Console.Editor
{
	[CustomEditor(typeof(DebugConsole))]
	public class DebugConsoleInspector : UnityEditor.Editor
	{
		public override void OnInspectorGUI()
		{
			DebugConsole myTarget = (DebugConsole)target;
			#if UNITY_2018_3_OR_NEWER
			if (PrefabUtility.GetPrefabAssetType(myTarget.gameObject) != PrefabAssetType.NotAPrefab)
				GUILayout.Label("This prefab should be placed in the first scene of your game", "helpbox");
			#else
			if (PrefabUtility.GetPrefabType(myTarget.gameObject) == PrefabType.Prefab)
				GUILayout.Label("This prefab should be placed in the first scene of your game", "helpbox");
			#endif

			GUILayout.BeginHorizontal();
			if (GUILayout.Button("Help..."))
			{
				Application.OpenURL("http://www.opencoding.net/TouchConsolePro/getting_started.php");
			}

			if(GUILayout.Button("Settings...")) 
			{
				Selection.activeObject = myTarget.Settings;
			}
			GUILayout.EndHorizontal();
		}
	}
}