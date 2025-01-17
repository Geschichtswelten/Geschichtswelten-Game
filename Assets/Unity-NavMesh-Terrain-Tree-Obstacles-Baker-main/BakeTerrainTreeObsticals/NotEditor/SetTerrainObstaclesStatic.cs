using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;

public class SetTerrainObstaclesStatic : MonoBehaviour
{

    // Start is called before the first frame update
    static TreeInstance[] Obstacle;
    static float width;
    static float lenght;
    static float hight;
    static bool isError;
    public static GameObject[] treeObjects;
    public static void BakeTreeObstacles(List<Terrain> terrains)
    {
        GameObject tree1 = AssetDatabase.LoadAssetAtPath("Assets/Visual Design Cafe/Nature Renderer Demo/Realistic/Art/Trees/Conifer.prefab",typeof(GameObject)) as GameObject;
        GameObject tree2 = AssetDatabase.LoadAssetAtPath("Assets/Forst/Conifers [BOTD]/Render Pipeline Support/URP/Prefabs/PF Conifer Bare BOTD URP.prefab", typeof(GameObject)) as GameObject;
        GameObject tree3 = AssetDatabase.LoadAssetAtPath("Assets/Forst/Conifers [BOTD]/Render Pipeline Support/URP/Prefabs/PF Conifer Medium BOTD URP.prefab", typeof(GameObject)) as GameObject;
        GameObject tree4 = AssetDatabase.LoadAssetAtPath("Assets/Forst/Conifers [BOTD]/Render Pipeline Support/URP/Prefabs/PF Conifer Small BOTD URP.prefab", typeof(GameObject)) as GameObject;
        GameObject tree5 = AssetDatabase.LoadAssetAtPath("Assets/Forst/Conifers [BOTD]/Render Pipeline Support/URP/Prefabs/PF Conifer Tall BOTD URP.prefab", typeof(GameObject)) as GameObject;
        GameObject tree6 = AssetDatabase.LoadAssetAtPath("Assets/Imports/Forst/Lux Instant Vegetation/Demo/Content/URP/Prefabs/PF_TreeIt fully procedural Fake LOD URP.prefab", typeof(GameObject)) as GameObject;
        treeObjects = new GameObject[6]{ tree1, tree2, tree3, tree4, tree5, tree6};

        foreach (Terrain terrain in terrains)
        {

            Obstacle = terrain.terrainData.treeInstances;

            lenght = terrain.terrainData.size.z;
            width = terrain.terrainData.size.x;
            hight = terrain.terrainData.size.y;
            Debug.Log("Terrain Size is :" + width + " , " + hight + " , " + lenght);

            int i = 0;
            

            Debug.Log("Adding " + Obstacle.Length + " navMeshObstacle Components for Trees");
            foreach (TreeInstance tree in Obstacle)
            {
                Vector3 worldPosition = Vector3.Scale(tree.position, terrain.terrainData.size) + terrain.transform.position;
                Quaternion tempRot = Quaternion.AngleAxis(tree.rotation * Mathf.Rad2Deg, Vector3.up);

                GameObject obs = (GameObject)Instantiate(treeObjects[Random.Range(0, treeObjects.Length)],worldPosition, tempRot );

                obs.AddComponent<NavMeshObstacle>();
                NavMeshObstacle obsElement = obs.GetComponent<NavMeshObstacle>();
                obsElement.carving = true;
                obsElement.carveOnlyStationary = true;

                if (terrain.terrainData.treePrototypes[tree.prototypeIndex].prefab.GetComponent<Collider>() == null)
                {
                    isError = true;
                    Debug.LogError("ERROR  There is no CapsuleCollider or BoxCollider attached to ''" + terrain.terrainData.treePrototypes[tree.prototypeIndex].prefab.name + "'' please add one of them.");
                    break;
                }
                Collider coll = terrain.terrainData.treePrototypes[tree.prototypeIndex].prefab.GetComponent<Collider>();
                if (coll.GetType() == typeof(CapsuleCollider) || coll.GetType() == typeof(BoxCollider))
                {

                    if (coll.GetType() == typeof(CapsuleCollider))
                    {
                        CapsuleCollider capsuleColl = terrain.terrainData.treePrototypes[tree.prototypeIndex].prefab.GetComponent<CapsuleCollider>();
                        obsElement.shape = NavMeshObstacleShape.Capsule;
                        obsElement.center = capsuleColl.center;
                        obsElement.radius = capsuleColl.radius;
                        obsElement.height = capsuleColl.height;

                    }
                    else if (coll.GetType() == typeof(BoxCollider))
                    {
                        BoxCollider boxColl = terrain.terrainData.treePrototypes[tree.prototypeIndex].prefab.GetComponent<BoxCollider>();
                        obsElement.shape = NavMeshObstacleShape.Box;
                        obsElement.center = boxColl.center;
                        obsElement.size = boxColl.size;
                    }

                }
                else
                {
                    isError = true;
                    Debug.LogError("ERROR  There is no CapsuleCollider or BoxCollider attached to ''" + terrain.terrainData.treePrototypes[tree.prototypeIndex].prefab.name + "'' please add one of them.");
                    break;
                }


                i++;
            }
            if (!isError) Debug.Log("All " + Obstacle.Length + " NavMeshObstacles were succesfully added to your Scene, Horray !");
        }
    }
}

