using UnityEngine;

public class TerrainMemory : MonoBehaviour
{
    private TreeInstance[] treeInstances;
    private Terrain terrain;


    private void Awake()
    {
        terrain = GetComponent<Terrain>();
        treeInstances = terrain.terrainData.treeInstances;
    }

    public void RestoreTrees()
    {
        terrain.terrainData.treeInstances = treeInstances;
    }


}
