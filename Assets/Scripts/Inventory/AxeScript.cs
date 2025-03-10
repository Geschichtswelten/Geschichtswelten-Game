using DefaultNamespace;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Tree;
using UnityEngine;

public class AxeScript : ItemBehaviour
{
    private Coroutine attackRoutine;
    [SerializeField] private Collider hitbox;
    [SerializeField] private float attackCooldown = 1.1f;
    [SerializeField] private float timberDistance;
    [SerializeField] private float maxTimberAngle;
    [SerializeField] private GameObject treePrefab;

    private GameObject terrain;
    private List<TreeInstance> treeArray;
    private TreePrototype[] prototypes;
    private TerrainData terrainData;
    private bool isBlocking = false;
    
    [SerializeField] private GameObject logPrefab;
    private enum animationIds
    {
        attack1,
        block1
    }
    
    private void OnDisable()
    {
        if (!isBlocking) 
            return;
        var player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerBehaviour>();
        if (!player)
            return;
        player.RemoveArmor(id);
        isBlocking = false;
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

    public override void Action1() //attack
    {
        isBlocking = false;
        if (attackRoutine == null)
            attackRoutine = StartCoroutine(attack());
    }
    
    public override void Action2()
    {

        var player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerBehaviour>();

        if (!player)
            return;
        if (isBlocking)
        {
            player.RemoveArmor(id);
            isBlocking = false;
            animationHandler.playAnimation((int)animationIds.block1);
            return;
        }
        if (attackRoutine != null) 
            return;
        var a = new PlayerBehaviour.Armor()
        {
            ItemId = id,
            Multiplier = 0.4f
        };
        player.RegisterArmor(a);
        isBlocking = true;
        animationHandler.playAnimation((int)animationIds.block1);
    }

    private IEnumerator attack()
    {
        hitbox.enabled = true;
        harvestTree();
        animationHandler.playAnimation((int)animationIds.attack1);
        itemSfxHandler.PlayAction1();
        yield return new WaitForSeconds(attackCooldown);
        hitbox.enabled = false;
        attackRoutine = null;
    }

    private void harvestTree()
    {
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
                    var heights = terrainData.GetHeights(0, 0, 0, 0);
                    terrainData.SetHeights(0, 0, heights);
                    //terrain.GetComponent<TerrainCollider>().terrainData = terrainData; 
                    //var terrH = terrain.GetComponent<TerrainCollider>().terrainData.GetHeights(0, 0, 0, 0);
                    //terrain.GetComponent<TerrainCollider>().terrainData.SetHeights(0, 0, terrH);
                    
                    var dropItem = Instantiate(treePrefab/*logPrefab*/, worldPosition/* + 3.5f * Vector3.up*/, tempRot/*Quaternion.Euler(90, 0, 0)*/);
                    dropItem.transform.localScale = new Vector3(treeArray[i].heightScale, treeArray[i].heightScale, treeArray[i].heightScale);
                    if (!dropItem.TryGetComponent<Rigidbody>(out var rb))
                    {
                        rb = dropItem.AddComponent<Rigidbody>();
                    }
                    rb.useGravity = true;
                    rb.isKinematic = false;
                    //rb.mass = 10;
                    rb.detectCollisions = true;
                    rb.excludeLayers = LayerMask.GetMask("Player");
                    rb.excludeLayers = LayerMask.GetMask("Tree");
                
                    /*if (dropItem.TryGetComponent<Collider>(out var coll))
                    {
                        coll.isTrigger = false;
                        coll.enabled = true;
                    }
                    else 
                        dropItem.AddComponent<SphereCollider>();*/
                    break;
                }
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.gameObject.CompareTag("Enemy")) return;
        if (other.TryGetComponent<AbstractEnemyBehaviour>(out var enemy))
        {
            enemy.AttackEnemy(damage);
        }
    }
}
