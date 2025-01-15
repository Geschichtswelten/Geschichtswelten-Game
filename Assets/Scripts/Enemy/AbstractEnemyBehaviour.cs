using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent (typeof(NavMeshAgent), typeof(AudioSource), typeof(Animator))]
public abstract class AbstractEnemyBehaviour : MonoBehaviour
{
    [Header("Behaviour")]
    [SerializeField] protected Behaviour _behaviour;
    [SerializeField] public GameObject _target;
    [SerializeField] protected GameObject[] _patrollingTargets;
    [SerializeField] private float movementPredictionTime;
    [SerializeField] protected float movementPredictionThreshold;
    [Space]
    [Header("Animation")]
    protected Animator _animator;
    [Space]
    [Header("Stats")]
    [SerializeField] protected float _health;
    [SerializeField] public int _damage;
    [SerializeField] protected float _walkSpeed;
    [SerializeField] protected float _runSpeed;
    [SerializeField] protected float _armor;
    [SerializeField] protected int _attackRange;
    [SerializeField] protected int _alertRange;
    [SerializeField] protected int _attackCooldown;
    [SerializeField] protected float _despawnTime;
    [Space]
    [Header("Audio")]
    [SerializeField] protected AudioSource _source;
    [SerializeField] protected AudioSource _combatSource;

    protected NavMeshAgent _agent;
    private int currPatrTarget;
    private bool reverse = false;
    protected bool reachedPoint = false;
    protected Vector3 lastPos;
    protected float timeSlice;
    protected bool block = false;
    
    
    

    private void Awake()
    {
        
        //_source.volume = ButtonHandler.settings.masterVolume;
        //_combatSource.volume = ButtonHandler.settings.masterVolume;
        _animator = GetComponent<Animator>();
        _agent = GetComponent<NavMeshAgent>();
        _agent.speed = _walkSpeed;
        _target = GameObject.FindGameObjectWithTag("Player");
        if (_target == null)
        {
            Debug.Log("No player found, deleting Enemy");
            Destroy(gameObject);
        }
        lastPos = _target.transform.position;
        
    }
   

    protected void Intercept()
    {
        _agent.speed = _runSpeed;
        _behaviour = Behaviour.Intercept;
        _agent.isStopped = false;

        float timeToPlayer = Vector3.Distance(_target.transform.position, transform.position) / _agent.speed;

        if(timeToPlayer > movementPredictionTime)
        {
            timeToPlayer = movementPredictionTime;
        }

        Vector3 targetPosition = _target.transform.position +  timeToPlayer * (_target.transform.position - lastPos);
        Vector3 directionToTarget = (targetPosition - transform.position).normalized;
        Vector3 directionToPlayer = (_target.transform.position - transform.position).normalized;

        float dot = Vector3.Dot(directionToPlayer, directionToTarget);
        if(dot < movementPredictionThreshold)
        {
            targetPosition = _target.transform.position;
        }
        _agent.SetDestination(targetPosition);
        lastPos = _target.transform.position;
    }
    protected void Patroll()
    {
        _agent.speed = _walkSpeed;
        _behaviour = Behaviour.Patrolling;
        _agent.isStopped = false;

        if (Vector3.Distance(_patrollingTargets[currPatrTarget].transform.position, transform.position) <= 1f)
        {
            if (reverse) currPatrTarget--;
            else currPatrTarget++;
        }

        if (currPatrTarget >= _patrollingTargets.Length)
        {
            currPatrTarget -= 2;
            reverse = true;
        }
        if (currPatrTarget < 0)
        {
            currPatrTarget = 0;
            reverse = false;
        }
        _agent.SetDestination(_patrollingTargets[currPatrTarget].transform.position);
    }
    protected void Idle()
    {
        _behaviour = Behaviour.Idle;
        _agent.isStopped = true;
    }
    protected void Chasing()
    {
        _agent.speed = _runSpeed;
        _behaviour = Behaviour.Chasing;
        _agent.isStopped = false;
        _agent.SetDestination(_target.transform.position);
    }


    
    public void AttackEnemy(float damage)
    {
        if (block)
        {
            _health -= (damage / _armor) / 2;
        }else
        {
            _health -= (damage / _armor);
        }
        _animator.SetTrigger("enemyHit");
    }



}



public enum Behaviour
{
    Idle, Patrolling, Chasing, Intercept, Flee
}
