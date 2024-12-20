using Sirenix.Config;
using System.Collections.Generic;
using System.Collections;
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

    private int lastScene;
    private Vector3 playerPosition;
    private Scene activeScene;
    private int[] sceneList;
    int[] scenesToBeLoaded;

    private void Start()
    {
        sceneList = new int[32];
        scenesToBeLoaded = new int[25];
        lastScene = 0;
        StartCoroutine(nameof(cor));
    }



    IEnumerator cor()
    {
        while (true)
        {
            playerPosition = player.transform.position;
            GetChunk();
            GetScene();
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

    private void GetScene()
    {
        //wood camp?
        if (chunk == 175 || chunk == 176 || chunk == 195 || chunk == 196)
        {
            scene = 175;
            activeScene = SceneManager.GetSceneByBuildIndex(chunk);
            sceneName = activeScene.name;
            return;
        }

        //arminius camp?
        if (chunk == 182 || chunk == 183 || chunk == 202 || chunk == 203)
        {
            scene = 181;
            activeScene = SceneManager.GetSceneByBuildIndex(chunk);
            sceneName = activeScene.name;
            return;
        }

        //copper camp?
        if (chunk == 312 || chunk == 313 || chunk == 332 || chunk == 333)
        {
            scene = 306;
            activeScene = SceneManager.GetSceneByBuildIndex(chunk);
            sceneName = activeScene.name;
            return;
        }

        scene = chunk;
        
        //Wood
        scene -= chunk > 175 ? 1 : 0;
        scene -= chunk > 195 ? 2 : 0;
        //Arminius
        scene -= chunk > 182 ? 1 : 0;
        scene -= chunk > 202 ? 2 : 0;
        //Copper
        scene -= chunk > 312 ? 1 : 0;
        scene -= chunk > 332 ? 2 : 0;

        activeScene = SceneManager.GetSceneByBuildIndex(chunk);
        sceneName = activeScene.name;
    }
    private void SceneLoader()
    {
        scenesToBeLoaded = new int[32];


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
            right = scene +1;

            if (right % 20 >1)
            {
                scenesToBeLoaded[1] = (right);
                top = right+20;
                if (top < 392)
                {
                    scenesToBeLoaded[2] = (top);
                }
                bottom = right-20;
                if (bottom > 0)
                {
                    scenesToBeLoaded[3] = (bottom);
                }
                right = right + 1;
                if (right % 20 > 1 && right <392)
                {
                    scenesToBeLoaded[4] = (right);
                    top = right + 20;
                    bottom = right - 20;
                    if (bottom > 0)
                    {
                        scenesToBeLoaded[5] = (bottom);
                    }
                    if (top < 392)
                    {
                        scenesToBeLoaded[6] = (top);
                    }
                }
            }
            //left side
            left = scene - 1;

            if (left % 20 < 19)
            {
                scenesToBeLoaded[7] = (left);
                top = left + 20;
                if (top < 392)
                {
                    scenesToBeLoaded[8] = (top);
                }
                bottom = left - 20;
                if (bottom > 0)
                {
                    scenesToBeLoaded[9] = (bottom);
                }
                left = left-1;
                if (left % 20 < 19 && left > 0)
                {
                    scenesToBeLoaded[10] = (left);
                    top = left + 20;
                    bottom = left-20;
                    if (bottom >0)
                    {
                        scenesToBeLoaded[11] = (bottom);
                    }
                    if (top < 392)
                    {
                        scenesToBeLoaded[12] = (top);
                    }
                }
            }

            //top
            top = scene + 20;

            if (top < 392)
            {
                scenesToBeLoaded[13] = (top);
                top = top + 20;
                if (top < 392)
                {
                    scenesToBeLoaded[14] = (top);
                    right = top + 1;
                    if (right % 20 > 1)
                    {
                        scenesToBeLoaded[15] = (right);
                        right = right + 1;
                        if (right % 20 > 1)
                        {
                            scenesToBeLoaded[16] = (right);
                        }
                    }

                    left = top - 1;
                    if (left % 20 < 19)
                    {
                        scenesToBeLoaded[17] = (left);
                        left = left - 1;
                        if (left % 20 < 19)
                        {
                            scenesToBeLoaded[18] = (left);
                        }
                    }

                }
            }

            //bottom
            bottom = scene - 20;

            if (bottom >0)
            {
                scenesToBeLoaded[19] = (bottom);
                bottom = bottom - 20;
                if (bottom > 0)
                {
                    scenesToBeLoaded[20] = (bottom);
                    right = bottom + 1;
                    if (right % 20 > 1)
                    {
                        scenesToBeLoaded[21] = (right);
                        right = right + 1;
                        if (right % 20 > 1)
                        {
                            scenesToBeLoaded[22] = (right);
                        }
                    }

                    left = bottom - 1;
                    if (left % 20 < 19)
                    {
                        scenesToBeLoaded[23] = (left);
                        left = left - 1;
                        if (left % 20 < 19)
                        {
                            scenesToBeLoaded[24] = (left);
                        }
                    }

                }
            }

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




        foreach (int scenee in scenesToBeLoaded) {
            if (scenee > 0)
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
