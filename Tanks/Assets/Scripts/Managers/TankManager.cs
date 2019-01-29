using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class TankManager
{
    public Color playerColor;
    public Transform spawnPoint;
    [HideInInspector] public int playerNumber;
    [HideInInspector] public string coloredPlayerText;
    [HideInInspector] public GameObject instance;
    [HideInInspector] public int wins;

    TankMovement playerMovement;
    TankShooting playerShooting;
    GameObject canvasGameobject;

    public void Setup()
    {
        playerMovement = instance.GetComponent<TankMovement>();
        playerShooting = instance.GetComponent<TankShooting>();
        canvasGameobject = instance.GetComponentInChildren<Canvas>().gameObject;

        playerMovement.playerNumber = playerNumber;
        playerShooting.playerNumber = playerNumber;

        coloredPlayerText = "<color=#" + ColorUtility.ToHtmlStringRGB(playerColor) + ">PLAYER " + playerNumber + "</color>";

        MeshRenderer[] meshRenderers = instance.GetComponentsInChildren<MeshRenderer>();

        for(int i=0;i < meshRenderers.Length;i++)
        {
            meshRenderers[i].material.color = playerColor;
        }
    }

    public void DisableControl()
    {
        playerMovement.enabled = false;
        playerShooting.enabled = false;
        canvasGameobject.SetActive(false);
    }

    public void EnableControl()
    {
        playerMovement.enabled = true;
        playerShooting.enabled = true;
        canvasGameobject.SetActive(true);
    }

    public void Reset()
    {
        instance.transform.position = spawnPoint.position;
        instance.transform.rotation = spawnPoint.rotation;

        instance.SetActive(false);
        instance.SetActive(true);
    }
}