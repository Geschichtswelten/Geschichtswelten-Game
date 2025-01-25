using System;
using Mono.CSharp;
using System.Collections;
using UnityEditor;
using UnityEngine;
using Random = UnityEngine.Random;

public class ArminiusBehaviour : AbstractEnemyBehaviour
{

    [SerializeField] private AudioClip[] stepsGrass;
    [SerializeField] private AudioClip[] stepsWood;
    [SerializeField] private AudioClip[] clipsAttack;
    [SerializeField] private AudioClip[] clipsHit;
    [SerializeField] private AudioClip[] clipsFalling;

    [Header("Animators")]
    [SerializeField] private Animator[] animators;
    [Header("BossBehaviour")]
    [SerializeField] private float chargeCooldown;
    [SerializeField] private float chargeDuration;
    [SerializeField] private float chargeRange;
    private float cd, cA;
    private bool charge;
    [SerializeField] private Collider attackHitbox;
    private MainGameLoop mainGameLoop;
    
    [SerializeField] private bool isInFinalLevel = true;
    private void Start()
    {
        charge = false;
        cd = _attackCooldown;
        cA = chargeCooldown;
        mainGameLoop = FindAnyObjectByType<MainGameLoop>();
        StartCoroutine(arminiusLoop());
    }

    private void Update()
    {
        cd -= Time.deltaTime;
        cA -= Time.deltaTime;
        if (cd < 0) cd = 0;
        if (cA < 0) cA = 0;
        if (_health < 0) Die();
        if (charge) _agent.SetDestination(_target.transform.position);
    }

    private IEnumerator arminiusLoop()
    {
        while (_health > 0)
        {
            float distance = Vector3.Distance(_target.transform.position, transform.position);
            if (distance > _alertRange)
            {
                trigger("enemyBlock");
                _animator.SetTrigger("enemyBlockIdle");
                Block();
            }
            else if (distance > chargeRange)
            {
                block = false;
                trigger("enemyRun");
                _animator.SetTrigger("enemyRun");
                Intercept();
             
            }
            else if (distance > _attackRange && cA == 0)
            {
                block = false;
                foreach (Animator anim in animators)
                {
                    anim.ResetTrigger("enemyRun");
                }
                _animator.ResetTrigger("enemyRun");
                Charge();
                yield return new WaitForSeconds(chargeDuration);
                charge = false;
            }
            else if (distance <= _attackRange && cd == 0)
            {
                block = false;
                foreach (Animator anim in animators)
                {
                    anim.ResetTrigger("enemyRun");
                }
                _animator.ResetTrigger("enemyRun");
                cd = _attackCooldown;
                // attack timen, maybe mit animation keyframes oder so. TODO
                Attack();
                yield return new WaitForSeconds(2f);
                if (attackHitbox) attackHitbox.enabled = false;
            }
            else
            {
                if (distance > _attackRange)
                {
                    block = false;
                    trigger("enemyRun");
                    _animator.SetTrigger("enemyRun");
                    Intercept();
                }
            }
            yield return null;
        }
        yield return null;
    }

    private void Block()
    {
        block = true;
        //_agent.isStopped = true;
        if (_target != gameObject)
        {
            transform.LookAt(new Vector3(_target.transform.position.x, transform.position.y,
                _target.transform.position.z));
        }

    }


    private void Attack()
    {
        trigger("enemyAttack");
        _animator.SetTrigger("enemyAttack");
        //_agent.isStopped = true;
        if (attackHitbox) attackHitbox.enabled = true;
    }

    private void Charge()
    {
        charge = true;
        _agent.isStopped = false;
        _agent.speed = _runSpeed;
        //evtl battlecry
        trigger("enemyCharge");
        _animator.SetTrigger("enemyCharge");
        //gameObject.tag = "Charge"; different attack      
        //tag zur�cksetzen
        cA = chargeCooldown;
    }


    private void Die()
    {
        base.Die();
        StopAllCoroutines();
        _agent.isStopped = true;
        if (isInFinalLevel)
            mainGameLoop.EndingSequence();
        Destroy(gameObject, _despawnTime);
    }

    //Audio Events

    public void Hit()
    {
        if (!_combatSource.isPlaying)
        {
            trigger("enemyHit");
            _combatSource.clip = clipsHit[Random.Range(0, clipsHit.Length)];
            _combatSource.Play();
        }

    }

    public void FallSmall()
    {
        if (!_source.isPlaying)
        {
            _source.clip = clipsFalling[0];
            _source.Play();
        }
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
        if (!_source.isPlaying)
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

    }

    public void SwordSwing()
    {
        if (!_combatSource.isPlaying)
        {
            _combatSource.clip = clipsAttack[Random.Range(0, 2)];
            _combatSource.Play();
            //if player hit play clipsAttack[2 oder 3]
        }
    }



    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, _attackRange);
        Gizmos.color = Color.white;
        Gizmos.DrawWireSphere(transform.position, _alertRange);
        Gizmos.color = Color.white;
        Gizmos.DrawWireSphere(transform.position, chargeRange);

    }

    private void trigger(string trigger)
    {
        foreach (Animator anim in animators)
        {
            anim.SetTrigger(trigger);
        }
    }
}

