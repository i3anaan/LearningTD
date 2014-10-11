using UnityEngine;
using UnityEditor;
using System.Collections;

public class XMLWaveHandler : EditorWindow {


    //DeserializedLevelsLoader deserializedLevelsLoader;
    //DeserializedLevelsCrossChecker deserializedLevelsCrossChecker;

    [MenuItem("Window/XML Wave handler")]
    public static void ShowWindow()
    {
        //Show existing window instance. If one doesn't exist, make one.
        EditorWindow.GetWindow(typeof(XMLWaveHandler));
    }

    void OnGUI()
    {
        // create one DeserializedLevelsLoader and Saver instance
        //if (deserializedLevelsLoader == null) deserializedLevelsLoader = new DeserializedLevelsLoader();
        //if (deserializedLevelsCrossChecker == null) deserializedLevelsCrossChecker = new DeserializedLevelsCrossChecker();


        // Import section
        GUILayout.Label("Import", EditorStyles.boldLabel);

        if (GUILayout.Button("Import Levels.xml into the scene"))
            DeserializedLevelLoader.updateWaveSpawner();


        // Export section
        GUILayout.Label("Export", EditorStyles.boldLabel);
        if (GUILayout.Button("Export WaveSpawner (And all of its children) to XML")) { 
            Debug.Log("Export button pressed!");
            WaveSpawner ws = GameObject.FindObjectOfType<WaveSpawner>();
            DeserializedLevelSaver.saveWaveSpawner(ws);
        }


        // Delete section
        //GUILayout.Label("Delete", EditorStyles.boldLabel);
        //GUILayout.Label("Delete " + DeserializedLevelsLoader.xmlItemsGOName + " and " + DeserializedLevelsSaver.xmlItemsToExportGOName + " GameObjects from scene", EditorStyles.wordWrappedLabel);
        //if (GUILayout.Button("Delete"))
        //{
        //    DestroyImmediate(GameObject.Find(DeserializedLevelsLoader.xmlItemsGOName));
        //    DestroyImmediate(GameObject.Find(DeserializedLevelsSaver.xmlItemsToExportGOName));
        //}


        // Cross check section
        //GUILayout.Label("Cross Check", EditorStyles.boldLabel);
        //GUILayout.Label("Cross check /Resources/Prefabs and Levels.xml if there are any item prefabs that exist only in one but not the other", EditorStyles.wordWrappedLabel);
        //if (GUILayout.Button("Cross Check"))
        //    deserializedLevelsCrossChecker.crossCheck();
    }

}
