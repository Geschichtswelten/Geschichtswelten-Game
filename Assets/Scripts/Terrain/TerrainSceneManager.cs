using Sirenix.Config;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.SceneManagement;

public class TerrainSceneManager : MonoBehaviour
{
    [Header("Player")]
    [SerializeField] private GameObject player;

    [Header("Terrain Data")]
    [Header("Read only")]
    [SerializeField] private int chunk;
    [SerializeField] private int scene;
    [SerializeField] private string sceneName;

    private Vector3 playerPosition;
    private Scene activeScene;
    private List<Scene> sceneList;

    private void Awake()
    {
        sceneList = new List<Scene>();
    }



    private void Update()
    {
        playerPosition = player.transform.position;
        GetChunk();
        GetScene();
        
    }

    private void GetChunk()
    {
        //get relative player Position
        int p_x = (int) Mathf.Ceil(playerPosition.x / 100F);
        int p_z = (int) Mathf.Ceil(playerPosition.z / 100F);

        chunk = (20 * p_z) + p_x;
        chunk = chunk == 0 ? 1 : chunk;
        
    }

    private void GetScene()
    {
        //wood camp?
        if (chunk == 175 || chunk == 176 || chunk == 195 || chunk == 196)
        {
            scene = 175;
            activeScene = SceneManager.GetSceneAt(chunk);
            sceneName = activeScene.name;
            return;
        }

        //arminius camp?
        if (chunk == 182 || chunk == 183 || chunk == 202 || chunk == 203)
        {
            scene = 181;
            activeScene = SceneManager.GetSceneAt(chunk);
            sceneName = activeScene.name;
            return;
        }

        //copper camp?
        if (chunk == 312 || chunk == 313 || chunk == 332 || chunk == 333)
        {
            scene = 306;
            activeScene = SceneManager.GetSceneAt(chunk);
            sceneName = activeScene.name;
            return;
        }

        scene = chunk;
        //WRONG
        //Wood
        scene -= chunk > 175 ? 1 : 0;
        scene -= chunk > 195 ? 1 : 0;
        //Arminius
        scene -= chunk > 182 ? 1 : 0;
        scene -= chunk > 202 ? 1 : 0;
        //Copper
        scene -= chunk > 312 ? 1 : 0;
        scene -= chunk > 332 ? 1 : 0;

        activeScene = SceneManager.GetSceneAt(chunk);
        sceneName = activeScene.name;
    }





    }
