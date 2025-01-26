using System;
using Sirenix.Config;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

public class TerrainSceneManager : MonoBehaviour
{
    [Header("Player")]
    [SerializeField] private GameObject player;
    [SerializeField] private float spawnRadius;
    [SerializeField] private GameObject enemy;
    [SerializeField] private float spawnInterval;

    [Header("Terrain Data")]
    [Header("Read only")]
    [SerializeField] private int chunk;
    [SerializeField] private int scene;

    private int lastScene;
    private Vector3 playerPosition;
    private int[] sceneList;
    int[] scenesToBeLoaded;
    private float _spawnIntervalSlice;
    private void Start()
    {
        sceneList = new int[33];
        scenesToBeLoaded = new int[25];
        lastScene = 0;
        Application.backgroundLoadingPriority = ThreadPriority.Low;
        _spawnIntervalSlice = spawnInterval;
        StartCoroutine(nameof(cor));
    }

    private void Update()
    {
        _spawnIntervalSlice -= Time.deltaTime;
        if (_spawnIntervalSlice <= 0)
        {
            if (Physics.Raycast(player.transform.position, Vector3.down, out RaycastHit hitInfo, 5f,
                    LayerMask.GetMask("whatIsGround")))
            {
                TerrainCollider tC = hitInfo.collider as TerrainCollider;

                float alpha = Random.Range(0f, 360f);
                var spawnDir = new Vector3(Mathf.Cos(alpha), playerPosition.y, Mathf.Sin(alpha)).normalized;
                var worldPos = playerPosition + spawnRadius * (playerPosition - spawnDir);
                var instPoint = tC.gameObject.GetComponent<Terrain>().SampleHeight(worldPos);
                var instPos = new Vector3(worldPos.x, instPoint, worldPos.z);
                
                Instantiate(enemy, instPos, Quaternion.identity);
            }
            _spawnIntervalSlice = spawnInterval;
        }
    }


    IEnumerator cor()
    {
        while (true)
        {
            playerPosition = player.transform.position;
            GetChunk();
            scene = GetScene(chunk);
            if (lastScene != scene)
            {
                SceneLoader();
            }
            yield return new WaitForSeconds(1);
        }
        
    }

    private void GetChunk()
    {
        //get relative player Position
        int p_x = (int) Mathf.Ceil(playerPosition.x / 100F);
        int p_z = (int) Mathf.Ceil(playerPosition.z / 100F) - 1;

        chunk = (20 * p_z) + p_x;
        chunk = chunk <= 0 ? 1 : chunk;

        
        
    }

    private int GetScene(int currChunk)
    {
        int currScene;
        //wood camp?
        if (currChunk == 175 || currChunk == 176 || currChunk == 195 || currChunk == 196)
        {
            currScene = 175;
            
            return currScene;
        }

        //arminius camp?
        if (currChunk == 182 || currChunk == 183 || currChunk == 202 || currChunk == 203)
        {
            currScene = 181;
            
            return currScene;
        }

        //copper camp?
        if (currChunk == 312 || currChunk == 313 || currChunk == 332 || currChunk == 333)
        {
            currScene = 306;
            
            return currScene;
        }

        currScene = currChunk;
        
        //Wood
        currScene -= currChunk > 175 ? 1 : 0;
        currScene -= currChunk > 195 ? 2 : 0;
        //Arminius
        currScene -= currChunk > 182 ? 1 : 0;
        currScene -= currChunk > 202 ? 2 : 0;
        //Copper
        currScene -= currChunk > 312 ? 1 : 0;
        currScene -= currChunk > 332 ? 2 : 0;
        return currScene;
        
    }
    private void SceneLoader()
    {
        scenesToBeLoaded = new int[33];


        //Wood Camp
        if (scene == 175)
        {
            scenesToBeLoaded[0] = (173);
            scenesToBeLoaded[1] = (174);
            scenesToBeLoaded[2] = (175);
            scenesToBeLoaded[3] = (176);
            scenesToBeLoaded[4] = (177);
            scenesToBeLoaded[5] = (191);
            scenesToBeLoaded[6] = (192);
            scenesToBeLoaded[7] = (193);
            scenesToBeLoaded[8] = (194);
            scenesToBeLoaded[9] = (207);
            scenesToBeLoaded[10] = (208);
            scenesToBeLoaded[11] = (209);
            scenesToBeLoaded[12] = (210);
            scenesToBeLoaded[13] = (211);
            scenesToBeLoaded[14] = (212);
            scenesToBeLoaded[15] = (227);
            scenesToBeLoaded[16] = (228);
            scenesToBeLoaded[17] = (229);
            scenesToBeLoaded[18] = (230);
            scenesToBeLoaded[19] = (231);
            scenesToBeLoaded[20] = (232);
            scenesToBeLoaded[21] = (153);
            scenesToBeLoaded[22] = (154);
            scenesToBeLoaded[23] = (155);
            scenesToBeLoaded[24] = (156);
            scenesToBeLoaded[25] = (157);
            scenesToBeLoaded[26] = (158);
            scenesToBeLoaded[27] = (133);
            scenesToBeLoaded[28] = (134);
            scenesToBeLoaded[29] = (135);
            scenesToBeLoaded[30] = (136);
            scenesToBeLoaded[31] = (137);
            scenesToBeLoaded[32] = (138);

        }
        else if (scene == 181)
        {
            //Arminius camp
            scenesToBeLoaded[0] = (141);
            scenesToBeLoaded[1] = (142);
            scenesToBeLoaded[2] = (143);
            scenesToBeLoaded[3] = (144);
            scenesToBeLoaded[4] = (145);

            scenesToBeLoaded[5] = (161);
            scenesToBeLoaded[6] = (162);
            scenesToBeLoaded[7] = (163);
            scenesToBeLoaded[8] = (164);
            scenesToBeLoaded[9] = (165);

            scenesToBeLoaded[10] = (180);
            scenesToBeLoaded[11] = (181);
            scenesToBeLoaded[12] = (182);
            scenesToBeLoaded[13] = (183);

            scenesToBeLoaded[14] = (197);
            scenesToBeLoaded[15] = (198);
            scenesToBeLoaded[16] = (199);

            scenesToBeLoaded[17] = (215);
            scenesToBeLoaded[18] = (216);
            scenesToBeLoaded[19] = (217);
            scenesToBeLoaded[20] = (218);
            scenesToBeLoaded[21] = (219);

            scenesToBeLoaded[22] = (235);
            scenesToBeLoaded[23] = (236);
            scenesToBeLoaded[24] = (237);
            scenesToBeLoaded[25] = (238);
            scenesToBeLoaded[26] = (239);
        }
        else if (scene == 306)
        {
            scenesToBeLoaded[0] = (264);
            scenesToBeLoaded[1] = (265);
            scenesToBeLoaded[2] = (266);
            scenesToBeLoaded[3] = (267);
            scenesToBeLoaded[4] = (268);
            scenesToBeLoaded[5] = (269);

            scenesToBeLoaded[6] = (284);
            scenesToBeLoaded[7] = (285);
            scenesToBeLoaded[8] = (286);
            scenesToBeLoaded[9] = (287);
            scenesToBeLoaded[10] = (288);
            scenesToBeLoaded[11] = (289);

            scenesToBeLoaded[12] = (304);
            scenesToBeLoaded[13] = (305);
            scenesToBeLoaded[14] = (306);
            scenesToBeLoaded[15] = (307);
            scenesToBeLoaded[16] = (308);

            scenesToBeLoaded[17] = (323);
            scenesToBeLoaded[18] = (324);
            scenesToBeLoaded[19] = (325);
            scenesToBeLoaded[20] = (326);

            scenesToBeLoaded[21] = (341);
            scenesToBeLoaded[22] = (342);
            scenesToBeLoaded[23] = (343);
            scenesToBeLoaded[24] = (344);
            scenesToBeLoaded[25] = (345);
            scenesToBeLoaded[26] = (346);

            scenesToBeLoaded[27] = (361);
            scenesToBeLoaded[28] = (362);
            scenesToBeLoaded[29] = (363);
            scenesToBeLoaded[30] = (364);
            scenesToBeLoaded[31] = (365);
            scenesToBeLoaded[32] = (366);
        }
        else
        {
            scenesToBeLoaded[0] = (scene);

            int right, top, bottom, left;


            //right side
            right = GetScene(chunk + 1);


            scenesToBeLoaded[1] = (right);
            top = GetScene(chunk + 1 + 20);

            scenesToBeLoaded[2] = (top);

            bottom = GetScene(chunk + 1 - 20);

            scenesToBeLoaded[3] = (bottom);

            right = GetScene(chunk + 1 + 1);

            scenesToBeLoaded[4] = (right);
            top = GetScene(chunk + 2 + 20);
            bottom = GetScene(chunk + 2 - 20);

            scenesToBeLoaded[5] = (bottom);

            scenesToBeLoaded[6] = (top);



            //left side
            left = GetScene(chunk - 1);


            scenesToBeLoaded[7] = (left);
            top = GetScene(chunk - 1 + 20);

            scenesToBeLoaded[8] = (top);

            bottom = GetScene(chunk - 1 - 20);

            scenesToBeLoaded[9] = (bottom);

            left = GetScene(chunk - 1 - 1);

            scenesToBeLoaded[10] = (left);
            top = GetScene(chunk - 2 + 20);
            bottom = GetScene(chunk - 2 - 20);

            scenesToBeLoaded[11] = (bottom);


            scenesToBeLoaded[12] = (top);




            //top
            top = GetScene(chunk + 20);


            scenesToBeLoaded[13] = (top);
            top = GetScene(chunk + 20 + 20);

            scenesToBeLoaded[14] = (top);
            right = GetScene(chunk + 40 + 1);

            scenesToBeLoaded[15] = (right);
            right = GetScene(chunk + 40 + 1 + 1);

            scenesToBeLoaded[16] = (right);



            left = GetScene(chunk + 20 + 20 - 1);

            scenesToBeLoaded[17] = (left);
            left = GetScene(chunk + 20 + 20 - 1 - 1);

            scenesToBeLoaded[18] = (left);






            //bottom
            bottom = GetScene(chunk - 20);


            scenesToBeLoaded[19] = (bottom);
            bottom = GetScene(chunk - 20 - 20);

            scenesToBeLoaded[20] = (bottom);
            right = GetScene(chunk - 40 + 1);

            scenesToBeLoaded[21] = (right);
            right = GetScene(chunk - 40 + 1 + 1);

            scenesToBeLoaded[22] = (right);


            left = GetScene(chunk - 40 - 1);

            scenesToBeLoaded[23] = (left);
            left = GetScene(chunk - 41 - 1);

            scenesToBeLoaded[24] = (left);



        }    
            

        




        for (int i = 0; i < sceneList.Length; i++) 
        {
            if (sceneList[i]<=0 || sceneList[i] >= 392)
            {
                continue;
            }

                for (int j = 0; j < scenesToBeLoaded.Length; j++)
                {
                    if (sceneList[i] == scenesToBeLoaded[j])
                    {

                        break;
                    }else
                    {
                        if(j == scenesToBeLoaded.Length-1)
                        {


                            SceneManager.UnloadSceneAsync(sceneList[i]);
                        }

                        continue;
                    }
                }
        }

        for (int i = 0; i < scenesToBeLoaded.Length; i++)
        {
            for (int j = i + 1; j < scenesToBeLoaded.Length; j++)
            {
                if (scenesToBeLoaded[i] == scenesToBeLoaded[j])
                {
                    scenesToBeLoaded[j] = - 1;
                }
            }
        }
        


        foreach (int scenee in scenesToBeLoaded) {
            if (scenee > 0 && scenee < 392)
            {
              
                if (!SceneManager.GetSceneByBuildIndex(scenee).isLoaded)
                {
                    SceneManager.LoadSceneAsync(scenee, LoadSceneMode.Additive);
                }
            }
        }

        sceneList = scenesToBeLoaded;
        
    }

    }
