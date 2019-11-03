using System.Collections;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(MapGenerator))]
public class MapGeneratorEditor : Editor
{
    public override void OnInspectorGUI()
    {
        // Draw the normal inspector - adds controls for "MapGenerator" fields
        DrawDefaultInspector();

        MapGenerator mapGen = (MapGenerator) this.target;
        if (GUILayout.Button("Rebuild Map from File"))
        {
            mapGen.BuildMapFromFilePath();
        }

        if (GUILayout.Button("Clear Map"))
        {
            mapGen.DestroyMapTiles();
        }
    }
}
