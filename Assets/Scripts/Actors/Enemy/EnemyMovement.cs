﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyMovement : MonoBehaviour
{
    private NavMeshAgent _navMeshAgent;
    [SerializeField] private float _enemyHeight = 1.18f;
    [SerializeField] private float _idleSpeed = 5f;
    [SerializeField] private float _chaseSpeed = 8.5f;

    //private bool _isFollowing = false;
    //private Transform _followTarget;

    void OnEnable()
    {
        _navMeshAgent = gameObject.AddComponent<NavMeshAgent>();

        _navMeshAgent.stoppingDistance = 2;
        _navMeshAgent.height = _enemyHeight;
        _navMeshAgent.autoBraking = true;
        _navMeshAgent.speed = _idleSpeed;
    }

    /*void Update()
    {
        if (_isFollowing && _followTarget != null)
        {
            _navMeshAgent.SetDestination(_followTarget.position);
        }
    }*/
    
    public void MoveTo(Vector3 worldPosition, bool forceChange = true)
    {
        if (_navMeshAgent.isStopped)
        {
            _navMeshAgent.SetDestination(worldPosition);
        }
        else if (forceChange)
        {
            _navMeshAgent.SetDestination(worldPosition);
        }
    }

    /// <summary>
    /// Should call StopFollowTarget if should stop
    /// otherwise will keep follow or until reaches the destination
    /// </summary>
    /// <param name="target"></param>
    /// <param name="forceChange"></param>
    public void GoToTarget(Transform target, bool forceChange = true)
    {
        if (_navMeshAgent.isStopped)
        {
            _navMeshAgent.SetDestination(target.position);
            /*_isFollowing = true;
            _followTarget = target;*/
        }
        else if (forceChange)
        {
            _navMeshAgent.SetDestination(target.position);
            /*_isFollowing = true;
            _followTarget = target;*/
        }
    }

    public void StopMoving()
    {
        _navMeshAgent.SetDestination(transform.position);
    }

    public void ToggleChaseSpeed(bool isChasing)
    {
        _navMeshAgent.speed = isChasing ? _chaseSpeed : _idleSpeed;
    }

    public void MoveToRandomPointWithMinDistFromPlayer(Vector3 lastKnownPlayerPos ,float minDist, bool forceChange = true)
    {
        float distance = Random.Range(minDist, 30);
        
        MoveTo(RandomNavSphere(lastKnownPlayerPos, distance), forceChange);
    }
    
    public void MoveToRandomPointWithMaxDistFromPlayer(Vector3 lastKnownPlayerPos ,float maxDist, bool forceChange = true)
    {
        float distance = Random.Range(0, maxDist);
        
        MoveTo(RandomNavSphere(lastKnownPlayerPos, distance), forceChange);
    }

    public void MoveToRandomPointWithMinDist(float minDist,  bool forceChange = true)
    {
        float distance = Random.Range(minDist, 30);
        
        MoveTo(RandomNavSphere(transform.position, distance), forceChange);
    }
    
    public void MoveToRandomPointWithMaxDist(float maxDist,  bool forceChange = true)
    {
        float distance = Random.Range(0, maxDist);
        
        MoveTo(RandomNavSphere(transform.position, distance), forceChange);
    }
    
    public static Vector3 RandomNavSphere (Vector3 origin, float distance, int layermask = -1) {
        Vector3 randomDirection = Random.insideUnitSphere * distance;
           
        randomDirection += origin;
           
        NavMeshHit navHit;
           
        NavMesh.SamplePosition (randomDirection, out navHit, distance, layermask);
           
        return navHit.position;
    }

    public bool GetIsNavMeshStopped()
    {
        return Vector3.Distance(_navMeshAgent.destination,transform.position) < _navMeshAgent.stoppingDistance + 1;
    }

    /*public void StopFollowTarget()
    {
        _isFollowing = false;
        _followTarget = null;
    }*/
}