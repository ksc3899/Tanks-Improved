using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TankShooting : MonoBehaviour
{
    public int playerNumber = 1;
    public GameObject shell;
    public Transform fireTransform;
    public Slider aimSlider;
    public AudioSource firingAudio;
    public AudioClip shotAudio;
    public AudioClip chargingAudio;
    public float minimumLaunchForce = 15f;
    public float maximumLaunchForce = 30f;
    public float maximumChargeTime = 0.75f;

    string shootButton;
    float currentLaunchForce;
    float chargeSpeed;
    bool fired;

    private void OnEnable()
    {
        currentLaunchForce = minimumLaunchForce;
        aimSlider.value = minimumLaunchForce;
    }

    private void Start()
    {
        shootButton = "Fire" + playerNumber;
        chargeSpeed = (maximumLaunchForce - minimumLaunchForce) / maximumChargeTime;
    }

    private void Update()
    {
        aimSlider.value = minimumLaunchForce;

        if (currentLaunchForce >= maximumLaunchForce && !fired)
        {
            currentLaunchForce = maximumLaunchForce;
            Fire();
        }
        else if (Input.GetButtonDown(shootButton))
        {
            fired = false;

            currentLaunchForce = minimumLaunchForce;
            firingAudio.clip = chargingAudio;
            firingAudio.Play();
        }
        else if(Input.GetButton(shootButton) && !fired)
        {
            currentLaunchForce += (chargeSpeed * Time.deltaTime);
            aimSlider.value = currentLaunchForce;
        }
        else if(Input.GetButtonUp(shootButton) && !fired)
        {
            Fire();
        }
    }

    private void Fire()
    {
        fired = true;
        GameObject shellInstance = Instantiate(shell, fireTransform.position, fireTransform.rotation) as GameObject;

        shellInstance.GetComponent<Rigidbody>().velocity = currentLaunchForce * fireTransform.forward;

        firingAudio.clip = shotAudio;
        firingAudio.Play();

        currentLaunchForce = minimumLaunchForce;
    }
}




