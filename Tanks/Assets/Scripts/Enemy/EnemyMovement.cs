using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyMovement : MonoBehaviour
{
   [HideInInspector] public bool gameEnabled = false;
    public float minimumDistance = 2f;

    int targetsIndex = 0;
    Rigidbody enemyRigidbody;
    GameObject[] target = new GameObject[5];
    NavMeshAgent navMesh;

    private void Awake()
    {
        enemyRigidbody = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        navMesh = GetComponent<NavMeshAgent>();
    }

    public void SetTargets(GameObject player)
    {
        target[targetsIndex] = player;
        targetsIndex++;
    }

    private void FixedUpdate()
    {
        int setTarget = 0;
        float preference;

        if (gameEnabled == true)
        {
            for(int i = 0; i < targetsIndex; i++)
            {
                float preferenceTest = 1 / (TargetsDistance(target[i]));
            }

            navMesh.SetDestination(this.target[setTarget].transform.position);
        }
    }

    private float TargetsDistance(GameObject targetObject)
    {
        float minimumDistantTarget = 100f;
        if (Vector3.Distance(this.transform.position, targetObject.transform.position) < minimumDistantTarget)
        {
            minimumDistantTarget = Vector3.Distance(this.transform.position, targetObject.transform.position);
        }
        
        return minimumDistantTarget;
    }

    private void TargetsHealth()
    {

    }
}
