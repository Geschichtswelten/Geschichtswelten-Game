using DefaultNamespace;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Tree;
using UnityEngine;

public class AxeScript : ItemBehaviour
{

    private Coroutine attackRoutine;
    [SerializeField] private Collider hitbox;
    [SerializeField] private float damage;
    [SerializeField] private float timberDistance;
    [SerializeField] private float maxTimberAngle;
    [SerializeField] private GameObject treePrefab;

    private GameObject terrain;
    private List<TreeInstance> treeArray;
    private TreePrototype[] prototypes;
    private TerrainData terrainData;
    private enum animationIds
    {
        attack1,
        block1
    }

    private void Awake()
    {
        treeArray = new List<TreeInstance>();
        id = 2; //may have to change that
        type = itemType.weapon;
        name = "Axe";
    }

    private void Start()
    {
        StartCoroutine(getTerrain());
    }

    private IEnumerator getTerrain()
    {
        while (true)
        {
            if (Physics.Raycast(gameObject.transform.position, Vector3.down, out RaycastHit hitInfo, 4, LayerMask.GetMask("whatIsGround")))
            {
                if (hitInfo.collider.gameObject == terrain)
                {
                    yield return new WaitForSeconds(2);
                    continue;
                }
                if (hitInfo.collider.gameObject.TryGetComponent<Terrain>(out Terrain terr))
                {
                    terrain = terr.gameObject;
                    terrainData = terr.terrainData;
                    treeArray.Clear();
                    treeArray.AddRange(terrainData.treeInstances);
                    prototypes = terrainData.treePrototypes;
                }
            }
            yield return new WaitForSeconds(2);
        }
    }

    public override void action1() //attack
    {
        if (attackRoutine == null)
            attackRoutine = StartCoroutine(attack());
    }

    private IEnumerator attack()
    {
        Debug.Log("Attacking maybe");
        hitbox.enabled = true;
        for (int i = 0; i < treeArray.Count; i++)
        {
            if (prototypes[treeArray[i].prototypeIndex].prefab.CompareTag("Tree"))
            {
                Vector3 worldPosition = Vector3.Scale(treeArray[i].position, terrainData.size) + terrain.transform.position;

                if (Vector3.Distance(worldPosition, gameObject.transform.position) <= timberDistance && Vector3.Angle(worldPosition, gameObject.transform.position) <= maxTimberAngle)
                {
                    Quaternion tempRot = Quaternion.AngleAxis(treeArray[i].rotation * Mathf.Rad2Deg, Vector3.up);
                    treePrefab = terrainData.treePrototypes[treeArray[i].prototypeIndex].prefab;
                    treePrefab.transform.localScale = new Vector3(treeArray[i].heightScale, treeArray[i].heightScale, treeArray[i].heightScale);
                    treeArray.RemoveAt(i);
                    terrainData.treeInstances = treeArray.ToArray();
                    terrain.GetComponent<TerrainCollider>().terrainData = terrainData;
                    var heights = terrainData.GetHeights(0, 0, 0, 0);
                    terrainData.SetHeights(0, 0, heights);
                    var temp = Instantiate(treePrefab, worldPosition, tempRot);
                    temp.AddComponent<Rigidbody>();
                    break;
                    
                }

            }
        }
        animationHandler.playAnimation((int)animationIds.attack1);
        itemSfxHandler.PlayAction1();
        yield return new WaitForSeconds(0.5f);
        hitbox.enabled = false;
        attackRoutine = null;
    }

    public override void action2()
    {
        Debug.Log("Blocking or smth");
    }


    private void OnCollisionEnter(Collision other)
    {
        if (!other.gameObject.CompareTag("Enemy")) return;
        Debug.Log("Hit an Enemy");
        //other.gameObject.GetComponent<AbstractEnemyBehaviour>().AttackEnemy(damage, null);
    }

}
