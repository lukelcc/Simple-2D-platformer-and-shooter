using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.InputSystem;

public class GameSession : MonoBehaviour
{
    [Header("Game settings")]
    [SerializeField] int levelResetDelay = 3;
    [SerializeField] int gameOverDelay = 3;
    [SerializeField] int maxWins = 3;
    [Range(2, 4)]
    [SerializeField] int numPlayers = 2;
    [Header("Players spawn settings")]
    [Header("Player 1")]
    [SerializeField] PlayerMortality Player1Prefab;
    [SerializeField] Transform P1SpawnPoint;
    [Header("Player 2")]
    [SerializeField] PlayerMortality Player2Prefab;
    [SerializeField] Transform P2SpawnPoint;
    [Header("Player 3")]
    [SerializeField] PlayerMortality Player3Prefab;
    [SerializeField] Transform P3SpawnPoint;
    [Header("Player 4")]
    [SerializeField] PlayerMortality Player4Prefab;
    [SerializeField] Transform P4SpawnPoint;

    [Header("Countdown settings")]
    [SerializeField] int countdownStartingNumber = 3;
    [SerializeField] public float countdownDuration = 3f;
    [SerializeField] public float playerLabelDuration = 4f;
    [SerializeField] float startTextDuration = 1f;
    [SerializeField] TextMeshProUGUI countDownTimerText;

    [Header("Sudden death UI")]
    [SerializeField] TextMeshProUGUI suddenDeathTimerText;
    [SerializeField] TextMeshProUGUI suddenDeathText;
    [SerializeField] public float suddenDeathTextDuration = 2f;
    [SerializeField] public int FirstWarningToSuddenDeathTime = 5;
    [SerializeField] public int SecondWarningToSuddenDeathTime = 3;


    int numRounds = 0;//edit
    private bool firstWeaponAlreadySpawned = false;
    private bool firstItemAlreadySpawned = false;

    public event Action onLifeChange;

    IEnumerator FirstWeaponSpawnCountdownTimer(int firstSpawnCooldown)
    {
        yield return new WaitForSeconds(firstSpawnCooldown);
        firstWeaponAlreadySpawned = true;
    }

    IEnumerator FirstItemSpawnCountdownTimer(int firstSpawnCooldown)
    {
        yield return new WaitForSeconds(firstSpawnCooldown);
        firstItemAlreadySpawned = true;
    }

    public bool HasTheFirstWeaponAlreadySpawned(int firstSpawnCooldown)//has the first weapons spawn?
    {
        if (firstWeaponAlreadySpawned == false)//if the first weapons has not spawn
        {
            StartCoroutine(FirstWeaponSpawnCountdownTimer(firstSpawnCooldown));
            return false;
        }
        else //if the first weapon has spawn
            return true;
    }

    public bool HasTheFirstItemAlreadySpawned(int firstSpawnCooldown)//has the first items spawn?
    {
        if (firstItemAlreadySpawned == false)//if the first items has not spawn
        {
            StartCoroutine(FirstItemSpawnCountdownTimer(firstSpawnCooldown));
            return false;
        }
        else //if the first items has spawn
            return true;
    }

    public void initializedSuddenDeathTimer()
    {
        suddenDeathText.gameObject.SetActive(false);
        suddenDeathTimerText.text = FindObjectOfType<DeathZone>().countDownTimer.ToString();
        suddenDeathTimerText.color = Color.white;
    }

    public void updateSuddenDeathTimer(int timeLeft)
    {       
        suddenDeathTimerText.text = timeLeft.ToString();
        if (timeLeft <= FirstWarningToSuddenDeathTime && timeLeft > SecondWarningToSuddenDeathTime)
            suddenDeathTimerText.color = Color.yellow;
        else if (timeLeft <= SecondWarningToSuddenDeathTime)
            suddenDeathTimerText.color = Color.red;
    }

    public void displaySuddenDeathText()
    {       
        StartCoroutine(toggleSuddenDeathUI());
    }

    IEnumerator toggleSuddenDeathUI()
    {
        suddenDeathTimerText.text = "SUDDEN DEATH";
        suddenDeathText.gameObject.SetActive(true);
        yield return new WaitForSeconds(suddenDeathTextDuration);
        suddenDeathText.gameObject.SetActive(false);
    }

    private void Start()
    {
        initializedSuddenDeathTimer();
        StartCoroutine(StartCountdownTimer(countdownStartingNumber, countdownDuration));
    }

    IEnumerator StartCountdownTimer(int countdownStartingNumber, float countdownDuration)
    {
        Debug.Log("Start countdown:");
        //FreezeAllPlayers();
        countDownTimerText.gameObject.SetActive(true);
        float interval = countdownDuration / countdownStartingNumber;
        for(int number = countdownStartingNumber; number >= 1; number--)
        {
            countDownTimerText.text = number.ToString();
            yield return new WaitForSeconds(interval);
        }
        countDownTimerText.text = "FIGHT!";
        //UnfreezeAllPlayers();
        Debug.Log("end countdown");
        yield return new WaitForSeconds(startTextDuration);
        countDownTimerText.gameObject.SetActive(false);
        //start sudden death countdown timer
        //DestroyPlayerLabel();
    }

    //destroy player label when start
    //private void DestroyPlayerLabel()
    //{
    //    for (int playerIndex = 0; playerIndex < PlayerInput.all.Count; playerIndex++)
    //    {
    //        PlayerInput.GetPlayerByIndex(playerIndex).GetComponent<PlayerMortality>().DisablePlayerLabel();
    //    }
    //}

    

    //public void FreezeAllPlayers()
    //{
    //    for (int playerIndex = 0; playerIndex < PlayerInput.all.Count; playerIndex++)
    //    {
    //        PlayerInput.GetPlayerByIndex(playerIndex).DeactivateInput();
    //    }
    //    Debug.Log("freeze all players");
    //}

    //public void UnfreezeAllPlayers()
    //{
    //    for (int playerIndex = 0; playerIndex < PlayerInput.all.Count; playerIndex++)
    //    {
    //        PlayerInput.GetPlayerByIndex(playerIndex).ActivateInput();
    //    }
    //    Debug.Log("Unfreeze all players");
    //}

    public void ResetGameRoundSettings()
    {
        firstWeaponAlreadySpawned = false;
    }


    private void SpawnPlayers(int numPlayers)
    {
        switch(numPlayers)
        {
            case 2:
                Instantiate(Player1Prefab, P1SpawnPoint.transform.position, Quaternion.identity);
                Instantiate(Player2Prefab, P2SpawnPoint.transform.position, Quaternion.identity);
                break;
            case 3:
                Instantiate(Player1Prefab, P1SpawnPoint.transform.position, Quaternion.identity);
                Instantiate(Player2Prefab, P2SpawnPoint.transform.position, Quaternion.identity);
                Instantiate(Player3Prefab, P3SpawnPoint.transform.position, Quaternion.identity);
                break;
            case 4:
                Instantiate(Player1Prefab, P1SpawnPoint.transform.position, Quaternion.identity);
                Instantiate(Player2Prefab, P2SpawnPoint.transform.position, Quaternion.identity);
                Instantiate(Player3Prefab, P3SpawnPoint.transform.position, Quaternion.identity);
                Instantiate(Player4Prefab, P4SpawnPoint.transform.position, Quaternion.identity);
                break;
            default:
                Instantiate(Player1Prefab, P1SpawnPoint.transform.position, Quaternion.identity);
                break;
        }
    }

    private void Awake()//singleton for gamesession
    {
        SpawnPlayers(numPlayers);
        int numGameSessions = FindObjectsOfType<GameSession>().Length;
        if (numGameSessions > 1) //restart level
        {           
            Debug.Log("destroy old game session and create another");            
            Destroy(gameObject);           
        }
        else //restart game
        {
            Debug.Log("create new game session");//when 1st time startup
            DontDestroyOnLoad(gameObject);
        }       
    }


    public void PlayerDeath()//edit
    {
        //StartCoroutine(PlayerDeathCoroutine());
        //Debug.Log("Num players: " + PlayerInput.all.Count);
        if (PlayerInput.all.Count <= 1)
        {
            numRounds++;
            checkWhichPlayerWins();
        }
    }

    IEnumerator PlayerDeathCoroutine()
    {
        yield return new WaitForSeconds(levelResetDelay);
        Debug.Log("Num players: " + PlayerInput.all.Count);
        if (PlayerInput.all.Count <= 1)
        {
            numRounds++;
            checkWhichPlayerWins();           
        }
    }

    public void ResetLevel()
    {
        StartCoroutine(ResetLevelCoroutine());
    }

    public int LevelRandomizer()
    {
        int lastLevelIndex = SceneManager.sceneCountInBuildSettings;
        int nextLevelIndex;
        try
        {
            do
            {
                nextLevelIndex = UnityEngine.Random.Range(0, lastLevelIndex);
            } while (nextLevelIndex == SceneManager.GetActiveScene().buildIndex);
        }
        catch (NullReferenceException error)
        {
            Debug.Log(error.Message);
            nextLevelIndex = 0;
        }
        return nextLevelIndex;
    }

    IEnumerator LoadNextLevelCoroutine()
    {
        gameObject.transform.GetChild(0).GetChild(5).gameObject.GetComponent<TextMeshProUGUI>().text =
            FindObjectOfType<PlayerWins>().name + " wins round " + numRounds;
        gameObject.transform.GetChild(0).GetChild(5).gameObject.SetActive(true);
        Debug.Log("proceed to next level");
        //int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;


        yield return new WaitForSeconds(levelResetDelay);
        //start countdown timer for next round
        StartCoroutine(StartCountdownTimer(countdownStartingNumber, countdownDuration));
        gameObject.transform.GetChild(0).GetChild(5).gameObject.SetActive(false);
        //reset the game settings for next round
        ResetGameRoundSettings();
        SceneManager.LoadScene(1);
        //start countdown timer for next round
        //StartCoroutine(StartCountdownTimer(countdownStartingNumber, countdownDuration));
    }

    public void LoadNextLevel()
    {
        StartCoroutine(LoadNextLevelCoroutine());
    }
    IEnumerator ResetLevelCoroutine()//edit
    {
        gameObject.transform.GetChild(0).GetChild(5).gameObject.GetComponent<TextMeshProUGUI>().text = 
            FindObjectOfType<PlayerWins>().name + " wins round " + numRounds;
        gameObject.transform.GetChild(0).GetChild(5).gameObject.SetActive(true);
        //Debug.Log("Num players: " + PlayerInput.all.Count);
        Debug.Log("level resetting");
        //StartCoroutine(ResetLevelCoroutine());
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;


        yield return new WaitForSeconds(levelResetDelay);
        //start countdown timer for next round
        StartCoroutine(StartCountdownTimer(countdownStartingNumber, countdownDuration));
        gameObject.transform.GetChild(0).GetChild(5).gameObject.SetActive(false);
        //reset the game settings for next round
        ResetGameRoundSettings();
        SceneManager.LoadScene(currentSceneIndex);
        //start countdown timer for next round
        //StartCoroutine(StartCountdownTimer(countdownStartingNumber, countdownDuration));
    }

    public void ResetGame()
    {
        StartCoroutine(ResetGameCoroutine());
    }


    IEnumerator ResetGameCoroutine() //go back to level 1, reset everything
    {
        //yield return new WaitForSeconds(levelResetDelay);
        Debug.Log("game resetting");
        gameObject.transform.GetChild(0).GetChild(4).gameObject.SetActive(true);//display game over text
        gameObject.transform.GetChild(0).GetChild(5).gameObject.GetComponent<TextMeshProUGUI>().text = "Winner: " +
            FindObjectOfType<PlayerWins>().name;
        gameObject.transform.GetChild(0).GetChild(5).gameObject.SetActive(true);
        yield return new WaitForSeconds(gameOverDelay);
        //reset all collectibles
        FindObjectOfType<ScenePersist>().ResetScenePersist();
        //SceneManager.LoadScene(0);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        Destroy(gameObject);
    }

    IEnumerator TransitionToGameOverCoroutine() //game over, display stats
    {
        //yield return new WaitForSeconds(levelResetDelay);
        Debug.Log("game over");
        gameObject.transform.GetChild(0).GetChild(4).gameObject.SetActive(true);//display game over text
        gameObject.transform.GetChild(0).GetChild(5).gameObject.GetComponent<TextMeshProUGUI>().text = "Winner: " +
            FindObjectOfType<PlayerWins>().name;
        gameObject.transform.GetChild(0).GetChild(5).gameObject.SetActive(true);
        yield return new WaitForSeconds(gameOverDelay);
        //reset all collectibles
        FindObjectOfType<ScenePersist>().ResetScenePersist();
        SceneManager.LoadScene(0);
        Destroy(gameObject);
    }

    public void checkWhichPlayerWins()
    {       
        Debug.Log("Round: " + numRounds +", Winner: "+ FindObjectOfType<PlayerWins>().name);
        if (FindObjectOfType<PlayerWins>().playerWins1Round() == maxWins)
            ResetGame();
        else
            //LoadNextLevel();
            ResetLevel();
    }
}
