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
    public Vector3 enemyStartPosition;
    [HideInInspector]public EnemyMovement enemyMovement;
    public GameObject enemyTankPrefab;

    GameObject enemyTank;
    int roundNumber;
    WaitForSeconds startWait;
    WaitForSeconds endWait;
    TankManager roundWinner;
    TankManager gameWinner;

    private void Awake()
    {
        enemyTank = GameObject.FindGameObjectWithTag("Enemy");
    }

    private void Start()
    {
        startWait = new WaitForSeconds(startDelay);
        endWait = new WaitForSeconds(endDelay);

        SpawnAllTanks();
        SetCameraTargets();

        StartCoroutine(GameLoop());

        enemyMovement = enemyTank.GetComponent<EnemyMovement>();
    }

    private void SpawnAllTanks()
    {
        for(int i = 0; i < tankManager.Length; i++)
        {
            tankManager[i].instance = Instantiate(tankPrefab, tankManager[i].spawnPoint.position, tankManager[i].spawnPoint.rotation) as GameObject;
            enemyMovement.SetTargets(tankManager[i].instance);
            tankManager[i].playerNumber = i + 1;
            tankManager[i].Setup();
        }
    }

    private void SetCameraTargets()
    {
        Transform[] targets = new Transform[tankManager.Length + 1];
        for(int i = 0;i < tankManager.Length + 1; i++)
        {
            if(i == tankManager.Length)
            {
                targets[i] = enemyTank.transform;
                continue; 
            }
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
        enemyMovement.gameEnabled = false;

        roundNumber++;

        messageText.text = "ROUND " + roundNumber;

        if (enemyTank != null)
        {
            enemyTank.transform.position = enemyStartPosition;
        }
        else
        {
            enemyTank = Instantiate(enemyTankPrefab, enemyStartPosition, Quaternion.identity) as GameObject;
        }

        yield return startWait;

        enemyMovement.gameEnabled = true;
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

        enemyMovement.gameEnabled = false;

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

        if(enemyTank != null)
        {
            numberOfTanksLeft++;
        }

        return numberOfTanksLeft <= 1;
    }
 }