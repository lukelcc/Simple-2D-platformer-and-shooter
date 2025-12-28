using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.InputSystem;

public class GameSession : MonoBehaviour
{
    [SerializeField] int levelResetDelay = 2;

    //private int remainingLife;

    public event Action onLifeChange;


    private void Awake()//singleton for gamesession
    {
        
        int numGameSessions = FindObjectsOfType<GameSession>().Length;
        if (numGameSessions > 1) //restart level
        {            
            Debug.Log("destroy old game session and create another");            
            Destroy(gameObject);
        }
        else //restart game
        {
            //remainingLife = FindObjectOfType<PlayerMortality>().GetStartingLife();
            //Debug.Log("reset health to: " + remainingLife);
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
        StartCoroutine(PlayerDeathCoroutine());
    }

    IEnumerator PlayerDeathCoroutine()
    {
        yield return new WaitForSeconds(levelResetDelay);
        Debug.Log("Num players: " + PlayerInput.all.Count);
        if(PlayerInput.all.Count <= 1)
        {
            ResetLevel();
        }
    }

    public void ResetLevel()
    {
        //Debug.Log("Num players: " + PlayerInput.all.Count);
        Debug.Log("level resetting");
        //StartCoroutine(ResetLevelCoroutine());
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;

        //remove all player UI hud. //P1,P2,P3,P4
        gameObject.transform.GetChild(0).GetChild(0).gameObject.SetActive(false);
        gameObject.transform.GetChild(0).GetChild(1).gameObject.SetActive(false);
        gameObject.transform.GetChild(0).GetChild(2).gameObject.SetActive(false);
        gameObject.transform.GetChild(0).GetChild(3).gameObject.SetActive(false);

        SceneManager.LoadScene(currentSceneIndex);
    }

    
    public void ResetGame()
    {
        Debug.Log("game resetting");
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
        yield return new WaitForSeconds(levelResetDelay);
        //reset all collectibles
        FindObjectOfType<ScenePersist>().ResetScenePersist();
        SceneManager.LoadScene(0);      
        Destroy(gameObject);
    }
}
