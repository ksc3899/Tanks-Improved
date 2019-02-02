    using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShooting : MonoBehaviour
{
    public GameObject shell;
    public Transform fireTransform;
    public AudioSource firingAudio;
    public float minimumLaunchForce = 15f;
    public float maximumLaunchForce = 30f;
    public float maximumChargeTime = 0.75f;
    public bool fired = false;
    public float shotDelay = 0.5f;

    float requiredLaunchForce;
    float currentLaunchForce;
    float chargeSpeed;

    private void OnEnable()
    {
        currentLaunchForce = minimumLaunchForce;
        requiredLaunchForce = currentLaunchForce;
        fired = false;
    }

    private void Start()
    {
        chargeSpeed = (maximumLaunchForce - minimumLaunchForce) / maximumChargeTime;
    }

    public IEnumerator CheckObstruction()
    {
        fired = true;

        RaycastHit hitPoint;
        LayerMask playersLayer = LayerMask.GetMask("Players");

        if(Physics.Raycast(transform.position, transform.forward, out hitPoint, 100f, playersLayer))
        {
            requiredLaunchForce = CalculateLaunchForce(hitPoint.collider.gameObject);
            for (currentLaunchForce = minimumLaunchForce; currentLaunchForce <= requiredLaunchForce; )
            {
                currentLaunchForce += (chargeSpeed * Time.deltaTime);
            }
            Fire();
        }
        
        yield return new WaitForSeconds(shotDelay);

        fired = false;
    }

    private float CalculateLaunchForce(GameObject targetObject)
    {
        return Vector3.Distance(this.transform.position, targetObject.transform.position) * 2;
    }

    private void Fire()
    {
        fired = true;
        GameObject shellInstance = Instantiate(shell, fireTransform.position, fireTransform.rotation) as GameObject;
        
        shellInstance.GetComponent<Rigidbody>().velocity = currentLaunchForce * fireTransform.forward;

        firingAudio.Play();

        currentLaunchForce = minimumLaunchForce;
    }
}
