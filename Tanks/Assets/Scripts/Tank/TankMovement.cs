using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankMovement : MonoBehaviour
{
    [HideInInspector] public int playerNumber;
    public AudioSource tankAudio;
    public AudioClip tankIdle;
    public AudioClip tankMoving;
    public float movementSpeed;
    public float turnSpeed;

    Rigidbody playerRigidbody;
    string movementAxisName;
    string turnAxisName;
    float movementInputValue;
    float turnInputValue;
    float originalPitch;
    float pitchRange = 0.1f;

    private void Awake()
    {
        playerRigidbody = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        movementAxisName = "Vertical" + playerNumber;
        turnAxisName = "Horizontal" + playerNumber;

        originalPitch = tankAudio.pitch;
    }

    private void OnEnable()
    {
        playerRigidbody.isKinematic = false;
        movementInputValue = 0f;
        turnInputValue = 0f;
    }

    private void OnDisable()
    {
        playerRigidbody.isKinematic = true;
    }

    private void Update()
    {
        if (Mathf.Abs(turnInputValue) > 0.2f && Mathf.Abs(movementInputValue) > 0.2f)
        {
            if (tankAudio.clip == tankIdle)
            {
                tankAudio.clip = tankMoving;
                tankAudio.pitch = Random.Range(originalPitch - pitchRange, originalPitch + pitchRange);
                tankAudio.Play();
            }
        }
        else
        {
            if (tankAudio.clip == tankMoving)
            {
                tankAudio.clip = tankIdle;
                tankAudio.pitch = Random.Range(originalPitch - pitchRange, originalPitch + pitchRange);
                tankAudio.Play();
            }
        }
    }

    private void FixedUpdate()
    {
        movementInputValue = Input.GetAxis(movementAxisName);
        turnInputValue = Input.GetAxis(turnAxisName);

        Move();
        Turn();
    }

    private void Move()
    {
        Vector3 moveBy = this.transform.forward * movementSpeed * movementInputValue * Time.deltaTime;
        playerRigidbody.MovePosition(this.transform.position + moveBy);
    }

    private void Turn()
    {
        float turning = turnInputValue * turnSpeed * Time.deltaTime;
        Quaternion turnBy = Quaternion.Euler(0f, turning, 0f);
        playerRigidbody.MoveRotation(this.transform.rotation * turnBy);
    }
}
