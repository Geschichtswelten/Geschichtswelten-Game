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
    [SerializeField] private Terrain terrain;

    private Vector3 playerPosition;
    private Scene activeScene;
    private List<Scene> sceneList;
    List<Scene> scenesToBeLoaded;

    private void Awake()
    {
        sceneList = new List<Scene>();
        scenesToBeLoaded = new List<Scene>();

    }



    private void Update()
    {
        playerPosition = player.transform.position;
        GetChunk();
        GetScene();
        SceneLoader();
    }

    private void GetChunk()
    {
        //get relative player Position
        int p_x = (int) Mathf.Ceil(playerPosition.x / 100F);
        int p_z = (int) Mathf.Ceil(playerPosition.z / 100F);

        chunk = (20 * p_z) + p_x;
        chunk = chunk == 0 ? 1 : chunk;

        Collider[] colliders = Physics.OverlapSphere(playerPosition, 5, 6, QueryTriggerInteraction.UseGlobal);

        if (colliders.Length > 0)
        {
            terrain = colliders[0].gameObject.GetComponent<Terrain>();
        }
        
        
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
        
        //Wood
        scene -= chunk > 175 ? 1 : 0;
        scene -= chunk > 195 ? 2 : 0;
        //Arminius
        scene -= chunk > 182 ? 1 : 0;
        scene -= chunk > 202 ? 2 : 0;
        //Copper
        scene -= chunk > 312 ? 1 : 0;
        scene -= chunk > 332 ? 2 : 0;

        activeScene = SceneManager.GetSceneAt(chunk);
        sceneName = activeScene.name;
    }
    private void SceneLoader()
    {
        scenesToBeLoaded.Clear();


        //Wood Camp
        if (scene == 175)
        {
            scenesToBeLoaded.Add(SceneManager.GetSceneByBuildIndex(173));
            scenesToBeLoaded.Add(SceneManager.GetSceneByBuildIndex(174));
            scenesToBeLoaded.Add(SceneManager.GetSceneByBuildIndex(175));
            scenesToBeLoaded.Add(SceneManager.GetSceneByBuildIndex(176));
            scenesToBeLoaded.Add(SceneManager.GetSceneByBuildIndex(177));
            scenesToBeLoaded.Add(SceneManager.GetSceneByBuildIndex(191));
            scenesToBeLoaded.Add(SceneManager.GetSceneByBuildIndex(192));
            scenesToBeLoaded.Add(SceneManager.GetSceneByBuildIndex(193));
            scenesToBeLoaded.Add(SceneManager.GetSceneByBuildIndex(194));
            scenesToBeLoaded.Add(SceneManager.GetSceneByBuildIndex(207));
            scenesToBeLoaded.Add(SceneManager.GetSceneByBuildIndex(208));
            scenesToBeLoaded.Add(SceneManager.GetSceneByBuildIndex(209));
            scenesToBeLoaded.Add(SceneManager.GetSceneByBuildIndex(210));
            scenesToBeLoaded.Add(SceneManager.GetSceneByBuildIndex(211));
            scenesToBeLoaded.Add(SceneManager.GetSceneByBuildIndex(212));
            scenesToBeLoaded.Add(SceneManager.GetSceneByBuildIndex(227));
            scenesToBeLoaded.Add(SceneManager.GetSceneByBuildIndex(228));
            scenesToBeLoaded.Add(SceneManager.GetSceneByBuildIndex(229));
            scenesToBeLoaded.Add(SceneManager.GetSceneByBuildIndex(230));
            scenesToBeLoaded.Add(SceneManager.GetSceneByBuildIndex(231));
            scenesToBeLoaded.Add(SceneManager.GetSceneByBuildIndex(232));
            scenesToBeLoaded.Add(SceneManager.GetSceneByBuildIndex(153));
            scenesToBeLoaded.Add(SceneManager.GetSceneByBuildIndex(154));
            scenesToBeLoaded.Add(SceneManager.GetSceneByBuildIndex(155));
            scenesToBeLoaded.Add(SceneManager.GetSceneByBuildIndex(156));
            scenesToBeLoaded.Add(SceneManager.GetSceneByBuildIndex(157));
            scenesToBeLoaded.Add(SceneManager.GetSceneByBuildIndex(158));
            scenesToBeLoaded.Add(SceneManager.GetSceneByBuildIndex(133));
            scenesToBeLoaded.Add(SceneManager.GetSceneByBuildIndex(134));
            scenesToBeLoaded.Add(SceneManager.GetSceneByBuildIndex(135));
            scenesToBeLoaded.Add(SceneManager.GetSceneByBuildIndex(136));
            scenesToBeLoaded.Add(SceneManager.GetSceneByBuildIndex(137));
            scenesToBeLoaded.Add(SceneManager.GetSceneByBuildIndex(138));

        }
        else if (scene == 181)
        {
            //Arminius camp
            scenesToBeLoaded.Add(SceneManager.GetSceneByBuildIndex(141));
            scenesToBeLoaded.Add(SceneManager.GetSceneByBuildIndex(142));
            scenesToBeLoaded.Add(SceneManager.GetSceneByBuildIndex(143));
            scenesToBeLoaded.Add(SceneManager.GetSceneByBuildIndex(144));
            scenesToBeLoaded.Add(SceneManager.GetSceneByBuildIndex(145));

            scenesToBeLoaded.Add(SceneManager.GetSceneByBuildIndex(161));
            scenesToBeLoaded.Add(SceneManager.GetSceneByBuildIndex(162));
            scenesToBeLoaded.Add(SceneManager.GetSceneByBuildIndex(163));
            scenesToBeLoaded.Add(SceneManager.GetSceneByBuildIndex(164));
            scenesToBeLoaded.Add(SceneManager.GetSceneByBuildIndex(165));

            scenesToBeLoaded.Add(SceneManager.GetSceneByBuildIndex(180));
            scenesToBeLoaded.Add(SceneManager.GetSceneByBuildIndex(181));
            scenesToBeLoaded.Add(SceneManager.GetSceneByBuildIndex(182));
            scenesToBeLoaded.Add(SceneManager.GetSceneByBuildIndex(183));

            scenesToBeLoaded.Add(SceneManager.GetSceneByBuildIndex(197));
            scenesToBeLoaded.Add(SceneManager.GetSceneByBuildIndex(198));
            scenesToBeLoaded.Add(SceneManager.GetSceneByBuildIndex(199));

            scenesToBeLoaded.Add(SceneManager.GetSceneByBuildIndex(215));
            scenesToBeLoaded.Add(SceneManager.GetSceneByBuildIndex(216));
            scenesToBeLoaded.Add(SceneManager.GetSceneByBuildIndex(217));
            scenesToBeLoaded.Add(SceneManager.GetSceneByBuildIndex(218));
            scenesToBeLoaded.Add(SceneManager.GetSceneByBuildIndex(219));

            scenesToBeLoaded.Add(SceneManager.GetSceneByBuildIndex(235));
            scenesToBeLoaded.Add(SceneManager.GetSceneByBuildIndex(236));
            scenesToBeLoaded.Add(SceneManager.GetSceneByBuildIndex(237));
            scenesToBeLoaded.Add(SceneManager.GetSceneByBuildIndex(238));
            scenesToBeLoaded.Add(SceneManager.GetSceneByBuildIndex(239));
        }
        else if (scene == 306)
        {
            scenesToBeLoaded.Add(SceneManager.GetSceneByBuildIndex(264));
            scenesToBeLoaded.Add(SceneManager.GetSceneByBuildIndex(265));
            scenesToBeLoaded.Add(SceneManager.GetSceneByBuildIndex(266));
            scenesToBeLoaded.Add(SceneManager.GetSceneByBuildIndex(267));
            scenesToBeLoaded.Add(SceneManager.GetSceneByBuildIndex(268));
            scenesToBeLoaded.Add(SceneManager.GetSceneByBuildIndex(269));

            scenesToBeLoaded.Add(SceneManager.GetSceneByBuildIndex(284));
            scenesToBeLoaded.Add(SceneManager.GetSceneByBuildIndex(285));
            scenesToBeLoaded.Add(SceneManager.GetSceneByBuildIndex(286));
            scenesToBeLoaded.Add(SceneManager.GetSceneByBuildIndex(287));
            scenesToBeLoaded.Add(SceneManager.GetSceneByBuildIndex(288));
            scenesToBeLoaded.Add(SceneManager.GetSceneByBuildIndex(289));

            scenesToBeLoaded.Add(SceneManager.GetSceneByBuildIndex(304));
            scenesToBeLoaded.Add(SceneManager.GetSceneByBuildIndex(305));
            scenesToBeLoaded.Add(SceneManager.GetSceneByBuildIndex(306));
            scenesToBeLoaded.Add(SceneManager.GetSceneByBuildIndex(307));
            scenesToBeLoaded.Add(SceneManager.GetSceneByBuildIndex(308));

            scenesToBeLoaded.Add(SceneManager.GetSceneByBuildIndex(323));
            scenesToBeLoaded.Add(SceneManager.GetSceneByBuildIndex(324));
            scenesToBeLoaded.Add(SceneManager.GetSceneByBuildIndex(325));
            scenesToBeLoaded.Add(SceneManager.GetSceneByBuildIndex(326));

            scenesToBeLoaded.Add(SceneManager.GetSceneByBuildIndex(341));
            scenesToBeLoaded.Add(SceneManager.GetSceneByBuildIndex(342));
            scenesToBeLoaded.Add(SceneManager.GetSceneByBuildIndex(343));
            scenesToBeLoaded.Add(SceneManager.GetSceneByBuildIndex(344));
            scenesToBeLoaded.Add(SceneManager.GetSceneByBuildIndex(345));
            scenesToBeLoaded.Add(SceneManager.GetSceneByBuildIndex(346));

            scenesToBeLoaded.Add(SceneManager.GetSceneByBuildIndex(361));
            scenesToBeLoaded.Add(SceneManager.GetSceneByBuildIndex(362));
            scenesToBeLoaded.Add(SceneManager.GetSceneByBuildIndex(363));
            scenesToBeLoaded.Add(SceneManager.GetSceneByBuildIndex(364));
            scenesToBeLoaded.Add(SceneManager.GetSceneByBuildIndex(365));
            scenesToBeLoaded.Add(SceneManager.GetSceneByBuildIndex(366));
        }
        else
        {
            scenesToBeLoaded.Add(SceneManager.GetSceneByBuildIndex(scene));

            Terrain right, top, bottom, left;


            //right side
            right = terrain.rightNeighbor;

            if (right != null)
            {
                scenesToBeLoaded.Add(right.gameObject.scene);
                top = right.topNeighbor;
                if (top != null)
                {
                    scenesToBeLoaded.Add(top.gameObject.scene);
                }
                bottom = right.bottomNeighbor;
                if (bottom != null)
                {
                    scenesToBeLoaded.Add(bottom.gameObject.scene);
                }
                right = right.rightNeighbor;
                if (right != null)
                {
                    scenesToBeLoaded.Add(right.gameObject.scene);
                    top = right.topNeighbor;
                    bottom = right.bottomNeighbor;
                    if (bottom != null)
                    {
                        scenesToBeLoaded.Add(bottom.gameObject.scene);
                    }
                    if (top != null)
                    {
                        scenesToBeLoaded.Add(top.gameObject.scene);
                    }
                }
            }
            //left side
            left = terrain.leftNeighbor;

            if (left != null)
            {
                scenesToBeLoaded.Add(left.gameObject.scene);
                top = left.topNeighbor;
                if (top != null)
                {
                    scenesToBeLoaded.Add(top.gameObject.scene);
                }
                bottom = left.bottomNeighbor;
                if (bottom != null)
                {
                    scenesToBeLoaded.Add(bottom.gameObject.scene);
                }
                left = left.leftNeighbor;
                if (left != null)
                {
                    scenesToBeLoaded.Add(left.gameObject.scene);
                    top = left.topNeighbor;
                    bottom = left.bottomNeighbor;
                    if (bottom != null)
                    {
                        scenesToBeLoaded.Add(bottom.gameObject.scene);
                    }
                    if (top != null)
                    {
                        scenesToBeLoaded.Add(top.gameObject.scene);
                    }
                }
            }

            //top
            top = terrain.topNeighbor;

            if (top != null)
            {
                scenesToBeLoaded.Add(top.gameObject.scene);
                top = top.topNeighbor;
                if (top != null)
                {
                    scenesToBeLoaded.Add(top.gameObject.scene);
                    right = top.rightNeighbor;
                    if (right != null)
                    {
                        scenesToBeLoaded.Add(right.gameObject.scene);
                        right = right.rightNeighbor;
                        if (right != null)
                        {
                            scenesToBeLoaded.Add(right.gameObject.scene);
                        }
                    }

                    left = top.leftNeighbor;
                    if (left != null)
                    {
                        scenesToBeLoaded.Add(left.gameObject.scene);
                        left = left.leftNeighbor;
                        if (left != null)
                        {
                            scenesToBeLoaded.Add(left.gameObject.scene);
                        }
                    }

                }
            }

            //bottom
            bottom = terrain.bottomNeighbor;

            if (bottom != null)
            {
                scenesToBeLoaded.Add(bottom.gameObject.scene);
                bottom = bottom.bottomNeighbor;
                if (bottom != null)
                {
                    scenesToBeLoaded.Add(bottom.gameObject.scene);
                    right = bottom.rightNeighbor;
                    if (right != null)
                    {
                        scenesToBeLoaded.Add(right.gameObject.scene);
                        right = right.rightNeighbor;
                        if (right != null)
                        {
                            scenesToBeLoaded.Add(right.gameObject.scene);
                        }
                    }

                    left = bottom.leftNeighbor;
                    if (left != null)
                    {
                        scenesToBeLoaded.Add(left.gameObject.scene);
                        left = left.leftNeighbor;
                        if (left != null)
                        {
                            scenesToBeLoaded.Add(left.gameObject.scene);
                        }
                    }

                }
            }

        }

        List<Scene> tempList = new List<Scene>();

        scenesToBeLoaded.ForEach(scene =>
        {
            if(sceneList.Contains(scene))
            {
                sceneList.Remove(scene);
                tempList.Add(scene);
                scenesToBeLoaded.Remove(scene);
            }

        });

        sceneList.ForEach(scene =>
        {
            SceneManager.UnloadSceneAsync(scene, UnloadSceneOptions.None);
        });

        scenesToBeLoaded.ForEach(scene =>
        {
            sceneList.Add(scene);
            SceneManager.LoadSceneAsync(scene.buildIndex, LoadSceneMode.Additive);
        });

        scenesToBeLoaded.AddRange(tempList);

    }

    }
