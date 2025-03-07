using Mono.CSharp;
using System.Collections;
using UnityEngine;

public class EnemyInfantryScript : AbstractEnemyBehaviour
{
    
    [SerializeField] private AudioClip[] stepsGrass;
    [SerializeField] private AudioClip[] stepsWood;
    [SerializeField] private AudioClip[] clipsAttack;
    [SerializeField] private AudioClip[] clipsHit;
    [SerializeField] private AudioClip[] clipsFalling;

    [SerializeField] private Collider hitbox;
    private Coroutine _attackRoutine;

    private void Start()
    {
        StartCoroutine(EnemyMovement());
        hitbox.enabled = false;
        hitbox.isTrigger = true;
    }

    private void Update()
    {
        if (_health < 0) Die();
        if (_target == gameObject && !tutorial)
        {
            
            _target = GameObject.FindGameObjectWithTag("Player");
            if (_target == null) _target = gameObject;
        }
    }

    private IEnumerator EnemyMovement()
    {
        while (_health > 0f)
        {
            if (_target == gameObject)
            {
                _animator.SetTrigger("enemyIdle");
                Idle();
                yield return new WaitUntil(()=>_target!=gameObject);
            }
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
                Patrol();
            } else
            {
                if (distance <= _attackRange)
                {
                    if (distance <= _attackRange && _attackRoutine == null) 
                    {
                        StartCoroutine(Attack());
                        _animator.ResetTrigger("enemyRun");
                        _animator.ResetTrigger("enemyWalk");
                        yield return new WaitForSeconds(2.5f);
                    } else
                    {
                        _animator.SetTrigger("enemyRun");
                        Chasing();
                    }
                    
                } else
                {
                    if (distance > _alertRange)
                    {
                        _animator.SetTrigger("enemyIdle");
                        Idle();
                        yield return new WaitForSeconds(0.3f);  
                    }
                    else
                    {
                        _animator.SetTrigger("enemyRun");
                        Intercept();
                    }
                }
            }

            if (_behaviour != Behaviour.Chasing)
            {
                yield return new WaitForSeconds(Random.Range(0.01f, 1f));
            }else
            {
                yield return new WaitForSeconds(0.01f);
            }
        }
    }
    
    private IEnumerator Attack()
    {
        _animator.SetTrigger("enemyAttack");
        _agent.isStopped = true;
        hitbox.enabled = true;
        yield return new WaitForSeconds(0.3f);
        hitbox.enabled = false;
        yield return new WaitForSeconds(_attackCooldown);
        _attackRoutine = null;
    }

    private void Die()
    {
        if(dead) return;
        base.Die();
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
        if (!tutorial)
        {
            Destroy(gameObject, _despawnTime);
        }
    }

    //Audio Events

    public void Hit()
    {
        if (!_combatSource.isPlaying)
        {
            _combatSource.clip = clipsHit[Random.Range(0, clipsHit.Length)];
            _combatSource.Play();
        }
    }

    public void FallSmall()
    {
        _source.clip = clipsFalling[0];
        _source.Play();
    }

    public void FallBig()
    {
        if (!_combatSource.isPlaying)
        {
            _combatSource.clip = clipsFalling[1];
            _combatSource.Play();
        }
    }



    public void Step()
    {
        
        if (Physics.Raycast(gameObject.transform.position, Vector3.down,out RaycastHit hitInfo, 3 , LayerMask.GetMask("whatIsGround"))) 
        {
            if(hitInfo.collider.gameObject.CompareTag("Wood"))
            {
                _source.clip = stepsWood[Random.Range(0, stepsWood.Length)];
            }else
            {
                _source.clip = stepsGrass[Random.Range(0, stepsGrass.Length)];
            }
            _source.Play();
        }
        
    }

    public void SwordSwing()
    {
        _combatSource.clip = clipsAttack[Random.Range(0, 2)];
        _combatSource.Play();
        //if player hit play clipsAttack[2 oder 3]
    }



    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, _attackRange);
        Gizmos.color = Color.white;
        Gizmos.DrawWireSphere(transform.position, _alertRange);
        
    }

}
