using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameSession : MonoBehaviour
{
    [SerializeField] int levelResetDelay = 2;

    private int remainingHp;

    public event Action onHpChange;


    private void Awake()//singleton for gamesession
    {
        int numGameSessions = FindObjectsOfType<GameSession>().Length;
        if (numGameSessions > 1)
        {            
            Debug.Log("destroy old game session and create another");            
            Destroy(gameObject);
        }
        else
        {
            remainingHp = FindObjectOfType<PlayerMortality>().GetStartingHealth();
            Debug.Log("reset health to: " + remainingHp);
            Debug.Log("create new game session");//when 1st time startup
            DontDestroyOnLoad(gameObject);
        }
    }

    public int getRemainingHp()
    {
        return remainingHp;
    }
    public void MinusHp()
    {
        remainingHp--;
        if (onHpChange != null)
        {
            onHpChange();
        }
        if (remainingHp > 0) 
        {
            Debug.Log("level resetting");
            Debug.Log("remaining health: " + remainingHp);
            StartCoroutine(ResetLevelCoroutine());
        }
        else 
        {
            Debug.Log("game resetting, back to level 1");
            StartCoroutine(ResetGameCoroutine());
        }
    }

    IEnumerator ResetLevelCoroutine()
    {
        yield return new WaitForSeconds(levelResetDelay);
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;       
        SceneManager.LoadScene(currentSceneIndex);
    }

    IEnumerator ResetGameCoroutine() //go back to level 1, reset everything
    {
        yield return new WaitForSeconds(levelResetDelay);
        //reset all collectibles
        FindObjectOfType<ScenePersist>().ResetScenePersist();
        SceneManager.LoadScene(0);      
        Destroy(gameObject);
    }
}
