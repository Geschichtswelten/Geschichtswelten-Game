using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class TerrainDataClass
{
    public string[] names;
    public List<TreeInstance[]> treeInstances;
    public List<TreePrototype[]> treePrototypes;

    public TerrainDataClass(string[] names, List<TreeInstance[]> treeInstances, List<TreePrototype[]> treePrototypes)
    {
        this.names = names;
        this.treeInstances = treeInstances;
        this.treePrototypes = treePrototypes;
    }
}
