using DefaultNamespace;
using System.Collections;
using UnityEngine;

public class AxeScript : ItemBehaviour
{

    private Coroutine attackRoutine;
    [SerializeField] private Collider hitbox;
    [SerializeField] private float damage;
    [SerializeField] private float timberDistance;
    [SerializeField] private float maxTimberAngle;
    [SerializeField] private GameObject treePrefab;

    private Terrain actTerrain;
    private enum animationIds
    {
        attack1,
        block1
    }

    private void Awake()
    {
        id = 2; //may have to change that
        type = itemType.weapon;
        name = "Axe";
    }


    private void Start()
    {
        StartCoroutine(terrainRoutine());
    }

    private IEnumerator terrainRoutine()
    {
        while (true)
        {
            if (Physics.Raycast(gameObject.transform.position, Vector3.down, out RaycastHit hitInfo, 3, 6))
            {
                if (hitInfo.collider.gameObject.TryGetComponent<Terrain>(out Terrain terr))
                {
                    actTerrain = terr;
                }
            }
            yield return new WaitForSeconds(7);
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
        var trees = actTerrain.terrainData.treeInstances;
        for (int i = 0; i < trees.Length; i++)
        {
            if (Vector3.Distance(trees[i].position, gameObject.transform.position) <= timberDistance)
            {
                if(Vector3.Angle(Camera.main.transform.forward, gameObject.transform.position + (trees[i].position - gameObject.transform.position).normalized) < maxTimberAngle)
                {
                    TreeInstance[] instances = new TreeInstance[trees.Length - 1];
                    int c = 0;
                    for (int j = 0; j < instances.Length; j++)
                    {
                        if(j != i)
                        {
                            instances[j] = trees[j + c];
                        }else
                        {
                            c++;
                        }
                    }
                    Instantiate(treePrefab, trees[i].position, Quaternion.identity);
                    actTerrain.terrainData.treeInstances = instances;
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
