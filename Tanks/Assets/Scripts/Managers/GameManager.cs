using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public int numberOfRoundsToWin = 3;
    public float startDelay = 3f;
    public float endDelay = 3f;
    public CameraController cameraController;
    public Text messageText;
    public GameObject tankPrefab;
    public TankManager[] tankManager;

    int roundNumber;
    WaitForSeconds startWait;
    WaitForSeconds endWait;
    TankManager roundWinner;
    TankManager gameWinner;

    private void Start()
    {
        startWait = new WaitForSeconds(startDelay);
        endWait = new WaitForSeconds(endDelay);

        SpawnAllTanks();
        SetCameraTargets();

        StartCoroutine(GameLoop()); 
    }

    private void SpawnAllTanks()
    {
        for(int i = 0; i < tankManager.Length;i++)
        {
            tankManager[i].instance = Instantiate(tankPrefab, tankManager[i].spawnPoint.position, tankManager[i].spawnPoint.rotation) as GameObject;
            tankManager[i].playerNumber = i + 1;
            tankManager[i].Setup();
        }
    }

    private void SetCameraTargets()
    {
        Transform[] targets = new Transform[tankManager.Length];    
        for(int i = 0;i < targets.Length;i++)
        {
            targets[i] = tankManager[i].instance.transform;
        }

        cameraController.targets = targets;
    }

    private IEnumerator GameLoop()
    {
        yield return StartCoroutine(RoundStarting());
        yield return StartCoroutine(RoundPlaying());
        yield return StartCoroutine(RoundEnding());

        if (gameWinner != null)
        {
            SceneManager.LoadScene("Main");
        }
        else
        {
            StartCoroutine(GameLoop());
        }
    }

    private IEnumerator RoundStarting()
    {
        ResetAllTanks();
        DisableTankControl();
        cameraController.SetStartPositionAndSize();

        roundNumber++;

        messageText.text = "ROUND " + roundNumber;

        yield return startWait;
    }

    private IEnumerator RoundPlaying()
    {
        EnableTankControl();
        messageText.text = string.Empty;

        while(!OneTankLeft())
        {
            yield return null;  
        }
    }

    private IEnumerator RoundEnding()
    {
        DisableTankControl();

        roundWinner = null;
        roundWinner = GetRoundWinner();
        if(roundWinner != null)
        {
            roundWinner.wins++;
        }

        gameWinner = GetGameWinner();

        string message = EndMessage();
        messageText.text = message;

        yield return endWait;   
    }

    private TankManager GetRoundWinner()
    {
        for (int i = 0; i < tankManager.Length; i++)
        {
            if(tankManager[i].instance.activeSelf)
            {
                return tankManager[i];
            }
        }
        return null;
    }

    private TankManager GetGameWinner()
    {
        for (int i = 0;i < tankManager.Length;i++)
        {
            if(tankManager[i].wins == numberOfRoundsToWin)
            {
                return tankManager[i];
            }
        }
        return null;
    }

    private string EndMessage()
    {
        string endMessage = "Draw!";
        
        if(roundWinner!=null)
        {
            endMessage = roundWinner.coloredPlayerText + " WINS THE ROUND!";
        }

        endMessage += "\n\n\n\n";

        for( int i = 0;i < tankManager.Length;i++)
        {
            endMessage += tankManager[i].coloredPlayerText + ": " + tankManager[i].wins + " WINS\n";
        }

        if(gameWinner!=null)
        {
            endMessage = gameWinner.coloredPlayerText + "WINS THE GAME!";
        }

        return endMessage;
    }

    private void ResetAllTanks()
    {
        for(int i = 0;i<tankManager.Length;i++)
        {
            tankManager[i].Reset();
        }
    }

    private void EnableTankControl()
    {
        for(int i = 0;i < tankManager.Length;i++)
        {
            tankManager[i].EnableControl();
        }
    }

    private void DisableTankControl()
    {
        for(int i = 0;i<tankManager.Length;i++)
        {
            tankManager[i].DisableControl();
        }
    }

    private bool OneTankLeft()
    {
        int numberOfTanksLeft = 0;

        for(int i = 0;i < tankManager.Length;i++)
        {
            if(tankManager[i].instance.activeSelf)
            {
                numberOfTanksLeft++;
            }
        }
        return numberOfTanksLeft <= 1;
    }
 }