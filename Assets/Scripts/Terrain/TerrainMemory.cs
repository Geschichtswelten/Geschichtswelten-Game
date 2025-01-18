using UnityEditor;
using UnityEngine;

public class TerrainMemory : MonoBehaviour
{
    private TerrainData data;
    private Terrain terrain;


    private void Awake()
    {
        terrain = GetComponent<Terrain>();
        data = TerrainDataCloner.Clone(terrain.terrainData);
        terrain.terrainData = data;
        var heights = terrain.terrainData.GetHeights(0, 0, 0, 0);
        terrain.terrainData.SetHeights(0, 0, heights);
    }


}
