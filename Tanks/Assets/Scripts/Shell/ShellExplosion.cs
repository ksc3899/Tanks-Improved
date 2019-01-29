using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShellExplosion  : MonoBehaviour
{
    public LayerMask tanksLayer;
    public ParticleSystem explosionParticles;
    public AudioSource explosionAudio;
    public float maximumDamage = 100f;
    public float explosionForce = 1000f;
    public float maximumLifeTime = 5f;
    public float explosionRadius = 5f;

    private void Awake()
    {
        Destroy(gameObject, maximumLifeTime);
        //tanksLayer = LayerMask.GetMask("Players");
             
    }

    private void OnTriggerEnter(Collider other)
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, explosionRadius, tanksLayer);

        for (int i = 0; colliders.Length > i; i++)
        {
            Rigidbody targetRigidbody = colliders[i].GetComponent<Rigidbody>();
            if (!targetRigidbody)
                continue;

            targetRigidbody.AddExplosionForce(explosionForce, transform.position, explosionRadius);

            TankHealth targetHealth = targetRigidbody.GetComponent<TankHealth>();

            if (!targetHealth)
                continue;

            float damage = CalculateDamage(targetRigidbody.position);
            targetHealth.TakeDamage(damage);
        }
        explosionParticles.transform.parent = null;
        explosionParticles.Play();
        explosionAudio.Play();
        Destroy(explosionParticles, explosionParticles.main.duration);
        Destroy(this.gameObject);
    }

    private float CalculateDamage(Vector3 targetPosition)
    {
        Vector3 explosionToTarget = targetPosition - this.gameObject.transform.position;
        float explosionDistance = explosionToTarget.magnitude;
        float relativeDistance = (explosionRadius - explosionDistance) / explosionRadius;

        float damage = relativeDistance * maximumDamage;
        damage = Mathf.Max(0f, damage);

        return damage;
    }
}
