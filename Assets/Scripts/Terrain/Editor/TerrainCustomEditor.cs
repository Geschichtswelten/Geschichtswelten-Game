using UnityEngine;
using UnityEditor;


[CustomEditor(typeof(RaiseTerrainHeightmap))]
public class TerrainCustomEditor : Editor
{


    // public RaiseHeights raiseHeights;

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        RaiseTerrainHeightmap RaiseTerrain = (RaiseTerrainHeightmap)target;
        if (GUILayout.Button("Raise Terrain Heightmap"))
        { RaiseTerrain.RaiseHeights(); }
    }
}
