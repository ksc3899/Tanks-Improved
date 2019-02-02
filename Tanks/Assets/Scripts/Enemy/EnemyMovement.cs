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
        float preference = 0f;

        if (gameEnabled == true)
        {
            for(int i = 0; i < targetsIndex; i++)
            {
                if (target[i].GetComponent<TankHealth>().currentHealth <= 0)
                    continue;

                float preferenceTest = 1 / (TargetsDistance(target[i]) * TargetsHealth(target[i]) * TargetsAmmoLeft(target[i], i));
                if (preferenceTest > preference)
                {
                    setTarget = i;
                    preference = preferenceTest;
                }
            }

            navMesh.SetDestination(this.target[setTarget].transform.position);
        }
    }

    private float TargetsDistance(GameObject targetObject)
    {
        return Vector3.Distance(this.transform.position, targetObject.transform.position);
    }

    private float TargetsHealth(GameObject targetObject)
    {
        return targetObject.GetComponent<TankHealth>().currentHealth / 10f;
    }

    private float TargetsAmmoLeft(GameObject targetObject, int i)
    {
        if (i == 0)
            return targetObject.GetComponent<TankShooting>().ammoLeftTank1;
        else
            return targetObject.GetComponent<TankShooting>().ammoLeftTank2;
    }
}
