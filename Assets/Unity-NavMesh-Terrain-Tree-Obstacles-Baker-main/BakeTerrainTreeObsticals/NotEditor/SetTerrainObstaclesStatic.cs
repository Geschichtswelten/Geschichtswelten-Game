using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Actions;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

public class SetTerrainObstaclesStatic : MonoBehaviour
{

    // Start is called before the first frame update
    static TreeInstance[] Obstacle;
    static float width;
    static float lenght;
    static float hight;
    static bool isError;

    public static void SetScripts(List<Terrain> terrains)
    {
        foreach (Terrain terrain in terrains)
        {
            terrain.gameObject.AddComponent<TerrainMemory>();
            EditorUtility.SetDirty(terrain);
        }
    }

    public static void AddMushrooms(List<Terrain> terrains, GameObject shrooms)
    {
        foreach (Terrain terrain in terrains)
        {
            SceneManager.SetActiveScene(terrain.gameObject.scene);
            for (int i = 0; i < 6; i++)
            {
                TerrainCollider tC = terrain.GetComponent<TerrainCollider>();
                
              
                
                    var worldPos = new Vector3(Random.Range(tC.bounds.center.x - tC.bounds.extents.x, tC.bounds.center.x + tC.bounds.extents.x), 20f, Random.Range(tC.bounds.center.z - tC.bounds.extents.z, tC.bounds.center.z + tC.bounds.extents.z));
                    var instPoint = terrain.SampleHeight(worldPos);
                    var instPos = new Vector3(worldPos.x, instPoint, worldPos.z);
                
                var sh = Instantiate(shrooms, instPos, Quaternion.identity);
                EditorUtility.SetDirty(sh);
            }
        }
    }


    public static void BakeTreeObstacles(List<Terrain> terrains)
    {
        foreach (Terrain terrain in terrains)
        {

            Obstacle = terrain.terrainData.treeInstances;

            lenght = terrain.terrainData.size.z;
            width = terrain.terrainData.size.x;
            hight = terrain.terrainData.size.y;
            Debug.Log("Terrain Size is :" + width + " , " + hight + " , " + lenght);

            int i = 0;
            GameObject parent = new GameObject("Tree_Obstacles");

            Debug.Log("Adding " + Obstacle.Length + " navMeshObstacle Components for Trees");
            foreach (TreeInstance tree in Obstacle)
            {
                Vector3 worldPosition = Vector3.Scale(tree.position, terrain.terrainData.size) + terrain.transform.position;
                Quaternion tempRot = Quaternion.AngleAxis(tree.rotation * Mathf.Rad2Deg, Vector3.up);

                GameObject obs = new GameObject("Obstacle" + i);
                obs.transform.SetParent(parent.transform);
                obs.transform.position = worldPosition; // Use the adjusted world position
                obs.transform.rotation = tempRot;

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

