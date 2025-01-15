using Mono.CSharp;
using System.Collections;
using UnityEngine;

public class EnemyArcher : AbstractEnemyBehaviour
{

    [SerializeField] private AudioClip[] stepsGrass;
    [SerializeField] private AudioClip[] stepsWood;
    [SerializeField] private AudioClip[] clipsAttack;
    [SerializeField] private AudioClip[] clipsHit;
    [SerializeField] private AudioClip[] clipsFalling;


    [Header("ArcherSpecific")]
    [SerializeField] private Transform arrowRoot;
    [SerializeField] private GameObject arrowPrefab;
    [SerializeField] private float fleeRange;
    public float arrowSpeed;

    private GameObject arrow;
    private Vector3 targetPosition; //for arrow


    private float cd;
    private bool fleeing;
    private void Start()
    {
        fleeing = false;
        StartCoroutine(EnemyMovement());
        cd = _attackCooldown;
        targetPosition = _target.transform.position;
    }

    private void Update()
    {
        cd -= Time.deltaTime;
        if (cd < 0) cd = 0;
        if (_health < 0) Die();
        if (_behaviour == Behaviour.Idle && Vector3.Distance(_target.transform.position, transform.position) > fleeRange)
        {
            transform.LookAt(_target.transform.position);
        }
        lastPos = _target.transform.position;
    }

    private IEnumerator EnemyMovement()
    {

            while (_health > 0f)
            {
            if (fleeing)
            {
                _animator.SetTrigger("enemyRun");
                Flee();
                yield return new WaitForSeconds(0.5f);
            }
            else
            {
                float distance = Vector3.Distance(_target.transform.position, transform.position);
                if (distance >= _alertRange && _patrollingTargets.Length > 1)
                {
                    if (reachedPoint)
                    {
                        _animator.SetTrigger("enemyIdle");
                        Idle();
                        reachedPoint = false;
                        yield return new WaitForSeconds(Random.Range(2f, 6f));
                    }
                    _animator.SetTrigger("enemyWalk");
                    Patroll();
                }
                else
                {
                    if (distance <= _attackRange)
                    {

                        if (Vector3.Distance(_target.transform.position, transform.position) < fleeRange)
                        {
                            _animator.SetTrigger("enemyRun");
                            Flee();

                        }else if (distance <= _attackRange && cd <= 0 && _behaviour != Behaviour.Flee)
                        {
                            cd = _attackCooldown;
                            Attack();
                            _animator.ResetTrigger("enemyRun");
                            _animator.ResetTrigger("enemyWalk");
                            yield return new WaitForSeconds(2.5f);
                        }
                        else
                        {
                            _animator.SetTrigger("enemyIdle");
                            
                            Idle();
                        }

                    }
                    else
                    {
                        _animator.SetTrigger("enemyRun");
                        Intercept();
                    }
                }

                if (_behaviour != Behaviour.Chasing)
                {
                    yield return new WaitForSeconds(Random.Range(0.01f, 1f));
                }
                else
                {
                    yield return new WaitForSeconds(0.01f);
                }
            }
        }
        Die();

    }


    private void Flee()
    {
        _behaviour = Behaviour.Flee;
        _agent.isStopped = false;
        _agent.speed = _runSpeed;
        Vector3 fleeDirection = (transform.position - _target.transform.position).normalized;
        Vector3 newPosition = _target.transform.position + _attackRange * fleeDirection;
        _agent.SetDestination(newPosition);

    }

    private void Attack()
    {

        _animator.SetTrigger("enemyAttack");
        _agent.isStopped = true;
        targetPosition = _target.transform.position +  (_target.transform.position - lastPos).normalized;   
        transform.LookAt(targetPosition);
        lastPos = _target.transform.position;
    }

    public void SetArrow()
    {
        arrow = Instantiate(arrowPrefab, 
            new Vector3(arrowRoot.transform.position.x, arrowRoot.transform.position.y, arrowRoot.transform.position.z),
            arrowRoot.rotation);
        arrow.GetComponent<ArrowScript>().archer = this;
        
    }

    public void FireArrow()
    {
        arrow.transform.position = arrowRoot.transform.position;
        arrow.transform.rotation = arrowRoot.rotation;
        arrow.transform.Rotate(new Vector3(-2.5f, 0, 0));
        targetPosition = _target.transform.position + 4 *(_target.transform.position - lastPos).normalized;
        arrow.GetComponent<ArrowScript>().ShootArrow(targetPosition);
    }


    private void Die()
    {

        StopAllCoroutines();
        _agent.isStopped = true;
        if (Random.Range(0, 2) == 0)
        {
            _animator.SetTrigger("enemyDeath");
        }
        else
        {
            _animator.SetTrigger("enemyDeath2");
        }
        Destroy(gameObject, _despawnTime);
    }

    //Audio Events

    public void Hit()
    {
        _combatSource.clip = clipsHit[Random.Range(0, clipsHit.Length)];
        _combatSource.Play();
    }

    public void FallSmall()
    {
        _source.clip = clipsFalling[0];
        _source.Play();
    }

    public void FallBig()
    {
        _combatSource.clip = clipsFalling[1];
        _combatSource.Play();
    }



    public void Step()
    {

        if (Physics.Raycast(transform.position, Vector3.down, out RaycastHit hitInfo, 3))
        {
            if (hitInfo.collider.gameObject.CompareTag("Wood"))
            {
                _source.clip = stepsWood[Random.Range(0, stepsWood.Length)];
            }
            else
            {
                _source.clip = stepsWood[Random.Range(0, stepsWood.Length)];
            }
            _source.Play();
        }

    }




    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, _attackRange);
        Gizmos.color = Color.white;
        Gizmos.DrawWireSphere(transform.position, _alertRange);
        Gizmos.color = Color.black;
        Gizmos.DrawWireSphere(transform.position, fleeRange);

    }

}
