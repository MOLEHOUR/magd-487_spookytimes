using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class enemyNavAgent : MonoBehaviour
{
    [SerializeField] public Transform movePostionTransform;
    public NavMeshAgent navMeshAgent;
    [SerializeField] List<Transform> wayPoint;
    public int currentWayPointIndex = 0;
    public bool pursue = false;
    //public float attackCooldown = 5f;
    //public float attackTimer;

    //when setting up the enemy movement, set the enemy capsule colldier to is trigger
    //and the player parent object shouldn't have a collider, but make a capsule as a child
    //and give that a collider

    

    private void Awake()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
    }

    public void Update()
    {
        float distanceToPlayer = Vector3.Distance(movePostionTransform.position, this.gameObject.transform.position);

        if (pursue)
        {
            navMeshAgent.destination = movePostionTransform.position;
            if (distanceToPlayer >= 60)
            {
                pursue = false;
            }
        }
        else
        {
            patrol();
        }



    }
    public void patrol()
    {
        //int random = Random.Range(0, wayPoint.Count);
        if (wayPoint.Count == 0)
        {
            return;
        }

        float distanceToWayPoint = Vector3.Distance(wayPoint[currentWayPointIndex].position, this.gameObject.transform.position);
        if (distanceToWayPoint <= 5)
        {
            //currentWayPointIndex = (random) % wayPoint.Count;
            currentWayPointIndex = (currentWayPointIndex+1) % wayPoint.Count;
        }
        navMeshAgent.SetDestination(wayPoint[currentWayPointIndex].position);
    }
}
