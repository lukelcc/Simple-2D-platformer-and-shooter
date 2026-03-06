using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


public class PlayerWins : MonoBehaviour
{
    private int winCount = 0;
    //[SerializeField] private int maxWin = 3;
    public event Action onWinCountChange;

    // Start is called before the first frame update
    void Start()
    {
        //initializeWinCountTo0();
    }

    //public void setWinCount(int winCount)
    //{
    //    this.winCount = winCount;
    //    if (onWinCountChange != null)
    //    {
    //        onWinCountChange();
    //    }
    //}
    public void initializeWinCountTo0 () //initialize win = 0
    {
        winCount = 0;
        if (onWinCountChange != null)
        {
            onWinCountChange();
        }
    }

    public int getWinCount()
    {
        return winCount;        
    }

    public int playerWins1Round()
    {
        winCount++;
        //Debug.Log("Win count: "+winCount);
        if (onWinCountChange != null)
        {
            onWinCountChange();
        }
        return winCount;

        //if(winCount == maxWin)
        //{
        //    Debug.Log("Reached max win.");
        //    FindObjectOfType<GameSession>().isGameOver = true;
        //}
        //else
        //    FindObjectOfType<GameSession>().isGameOver = false;
    }


}
