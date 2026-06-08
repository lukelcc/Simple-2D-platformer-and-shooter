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

    [Header("Countdown settings")]
    [SerializeField] int countdownStartingNumber = 3;
    [SerializeField] float countdownDuration = 3f;    
    [SerializeField] TextMeshProUGUI countdownTimerText;
    
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



    IEnumerator StartCountdownTimer(int countdownStartingNumber, float countdownDuration)
    {
        Debug.Log("Start countdown:");
        FreezeAllPlayers();
        countdownTimerText.gameObject.SetActive(true);
        float interval = countdownDuration / countdownStartingNumber;
        for(int number = countdownStartingNumber; number >= 1; number--)
        {
            countdownTimerText.text = number.ToString();
            yield return new WaitForSeconds(interval);
        }
        countdownTimerText.text = "FIGHT!";
        UnfreezeAllPlayers();
        Debug.Log("end countdown");
        yield return new WaitForSeconds(1f);
        countdownTimerText.gameObject.SetActive(false);
        DestroyPlayerLabel();
    }

    //destroy player label when start
    private void DestroyPlayerLabel()
    {
        for (int playerIndex = 0; playerIndex < PlayerInput.all.Count; playerIndex++)
        {
            PlayerInput.GetPlayerByIndex(playerIndex).GetComponent<PlayerMortality>().DisablePlayerLabel();
        }
    }

    

    public void FreezeAllPlayers()
    {
        for (int playerIndex = 0; playerIndex < PlayerInput.all.Count; playerIndex++)
        {
            PlayerInput.GetPlayerByIndex(playerIndex).DeactivateInput();
        }
        Debug.Log("freeze all players");
    }

    public void UnfreezeAllPlayers()
    {
        for (int playerIndex = 0; playerIndex < PlayerInput.all.Count; playerIndex++)
        {
            PlayerInput.GetPlayerByIndex(playerIndex).ActivateInput();
        }
        Debug.Log("Unfreeze all players");
    }

    public void ResetGameRoundSettings()
    {
        firstWeaponAlreadySpawned = false;
    }


    private void Awake()//singleton for gamesession
    {
        StartCoroutine(StartCountdownTimer(countdownStartingNumber, countdownDuration));
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

    
    //public int getRemainingLife()
    //{
    //    return remainingLife;
    //}
    //public void MinusLife()
    //{
    //    remainingLife--;
    //    if (onLifeChange != null)
    //    {
    //        onLifeChange();
    //    }
    //    if (remainingLife > 0) 
    //    {
    //        Debug.Log("level resetting");
    //        Debug.Log("remaining health: " + remainingLife);
    //        StartCoroutine(ResetLevelCoroutine());
    //    }
    //    else 
    //    {
    //        Debug.Log("game resetting, back to level 1");
    //        StartCoroutine(ResetGameCoroutine());
    //    }
    //}

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
        //Debug.Log("Num players: " + PlayerInput.all.Count);
        //Debug.Log("level resetting");
        ////StartCoroutine(ResetLevelCoroutine());
        //int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;

        ////remove all player UI hud. //P1,P2,P3,P4
        //gameObject.transform.GetChild(0).GetChild(0).gameObject.SetActive(false);
        //gameObject.transform.GetChild(0).GetChild(1).gameObject.SetActive(false);
        //gameObject.transform.GetChild(0).GetChild(2).gameObject.SetActive(false);
        //gameObject.transform.GetChild(0).GetChild(3).gameObject.SetActive(false);

        //SceneManager.LoadScene(currentSceneIndex);
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

        //remove all player UI hud. //P1,P2,P3,P4
        //gameObject.transform.GetChild(0).GetChild(0).gameObject.SetActive(false);
        //gameObject.transform.GetChild(0).GetChild(1).gameObject.SetActive(false);
        //gameObject.transform.GetChild(0).GetChild(2).gameObject.SetActive(false);
        //gameObject.transform.GetChild(0).GetChild(3).gameObject.SetActive(false);
        yield return new WaitForSeconds(levelResetDelay);
        //start countdown timer for next round
        StartCoroutine(StartCountdownTimer(countdownStartingNumber, countdownDuration));
        gameObject.transform.GetChild(0).GetChild(5).gameObject.SetActive(false);
        //reset the game settings for next round
        ResetGameRoundSettings();
        //StartCoroutine(StartCountdownTimer(countdownStartingNumber, countdownDuration));
        SceneManager.LoadScene(currentSceneIndex);
    }

    public void ResetGame()
    {
        StartCoroutine(ResetGameCoroutine());
    }

    //IEnumerator ResetLevelCoroutine()
    //{      
    //    yield return new WaitForSeconds(levelResetDelay);
    //    //Debug.Log("Num players: " + PlayerInput.all.Count);

    //    int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;

    //    //remove all player UI hud. //P1,P2,P3,P4
    //    gameObject.transform.GetChild(0).GetChild(0).gameObject.SetActive(false);
    //    gameObject.transform.GetChild(0).GetChild(1).gameObject.SetActive(false);
    //    gameObject.transform.GetChild(0).GetChild(2).gameObject.SetActive(false);
    //    gameObject.transform.GetChild(0).GetChild(3).gameObject.SetActive(false);

    //    SceneManager.LoadScene(currentSceneIndex);
    //}

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
        SceneManager.LoadScene(0);
        Destroy(gameObject);
    }

    public void checkWhichPlayerWins()
    {       
        Debug.Log("Round: " + numRounds +", Winner: "+ FindObjectOfType<PlayerWins>().name);
        if (FindObjectOfType<PlayerWins>().playerWins1Round() == maxWins)
            ResetGame();
        else
            ResetLevel();
    }
}
