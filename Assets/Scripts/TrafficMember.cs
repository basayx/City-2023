using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class TrafficMember : MonoBehaviour
{
    public NavMeshAgent Agent;
    public Vector3 Destination;

    private void Start()
    {
        Initialize();
        return;
        if (Random.Range(0,10) == 0)
        {
            Initialize();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void Initialize()
    {
        transform.parent = null;
        FindNewDestination();
    }

    public void MoveToDestination(Vector3 destination)
    {
        Destination = destination;
        Agent.SetDestination(Destination);
        StartCoroutine(Moving());
        IEnumerator Moving()
        {
            while(Vector3.Distance(transform.position, Destination) > 0.1f)
            {
                yield return new WaitForEndOfFrame();
            }
        }
    }

    public void FindNewDestination()
    {
        if (RandomPoint(transform.position, 100f, out Vector3 r))
        {
            MoveToDestination(r);
        }
        else
        {
            //StopAllCoroutines();
            //Destroy(gameObject);
        }
    }

    bool RandomPoint(Vector3 center, float range, out Vector3 result)
    {
        for (int i = 0; i < 30; i++)
        {
            Vector3 randomPoint = center + Random.insideUnitSphere * range;
            NavMeshHit hit;
            if (NavMesh.SamplePosition(randomPoint, out hit, 1.0f, NavMesh.AllAreas))
            {
                result = hit.position;
                return true;
            }
        }
        result = Vector3.zero;
        return false;
    }
}
