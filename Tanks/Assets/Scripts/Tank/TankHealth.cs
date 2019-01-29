using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class TankHealth : MonoBehaviour
{
    public float startingHealth = 100f;
    public Slider healthSlider;
    public Image fillImage;
    public Color fullHealthColor = Color.green;
    public Color nillHealthColor = Color.red;
    public GameObject explosionPrefab;

    AudioSource explosionAudio;
    ParticleSystem explosionParticles;
    float currentHealth;
    bool dead;

    private void Awake()
    {
        explosionParticles = Instantiate(explosionPrefab).GetComponent<ParticleSystem>();
        explosionAudio = explosionParticles.GetComponent<AudioSource>();
        explosionParticles.gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        currentHealth = startingHealth;
        dead = false;

        SetHealthUI();
    }

    public void TakeDamage(float amount)
    {
        currentHealth -= amount;

        SetHealthUI();

        if (currentHealth <= 0f && !dead)
            OnDeath();
    }

    private void SetHealthUI()
    {
        healthSlider.value = currentHealth;
        fillImage.color = Color.Lerp(nillHealthColor, fullHealthColor, currentHealth / startingHealth);
    }    

    private void OnDeath()
    {
        dead = true;

        explosionParticles.gameObject.transform.position = this.gameObject.transform.position;
        explosionAudio.gameObject.SetActive(true);
        explosionParticles.Play();
        explosionAudio.Play();

        this.gameObject.SetActive(false);
    }
}
