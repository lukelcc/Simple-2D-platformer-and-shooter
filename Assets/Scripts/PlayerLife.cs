using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerLife : MonoBehaviour
{

    private int remainingLife;

    public event Action onLifeChange;


    private void Start()
    {
        setRemainingLife(FindObjectOfType<PlayerMortality>().GetStartingLife());
    }

    public void setRemainingLife(int life)
    {
        remainingLife = life;
        if (onLifeChange != null)
        {
            onLifeChange();
        }
    }
    public int getRemainingLife()
    {
        return remainingLife;
    }
    public void MinusLife()
    {       
        remainingLife--;
        if (onLifeChange != null)
        {
            onLifeChange();
        }
       // if (FindObjectsOfType<PlayerMortality>().Length)

        if (remainingLife > 0)
        {          
            Debug.Log("remaining health: " + remainingLife);          
            //Debug.Log("num remaining players: " + FindObjectsOfType<PlayerMortality>().Length);
            FindObjectOfType<GameSession>().PlayerDeath();//edit            
        }
        else
        {
            Debug.Log("game resetting, back to level 1");
            FindObjectOfType<GameSession>().ResetGame();
        }
    }
}
