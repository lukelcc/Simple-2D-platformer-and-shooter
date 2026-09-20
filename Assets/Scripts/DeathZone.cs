using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathZone : MonoBehaviour
{
    //[SerializeField] private float deathZoneMovingSpeed=0.001f;
    [SerializeField] private float distancePerInterval = 0.1f;
    [SerializeField] private float moveInterval = 1f;
    [SerializeField] public int damage = 1;
    [SerializeField] public float damageInterval = 1f;
    [SerializeField] public int countDownTimer = 10;
    [SerializeField] public float countDownTimerInterval = 1f;

    Coroutine damagePlayerCoroutine;

    public enum gameObjectTag
    {
        Player
    }

    // Start is called before the first frame update
    void Start()
    {
        GetComponent<SpriteRenderer>().enabled = false;
        StartCoroutine(pauseThenStartSuddenDeathTimer(FindObjectOfType<GameSession>().countdownDuration));
    }

    IEnumerator pauseThenStartSuddenDeathTimer(float pausetime)
    {
        Debug.Log("Pause sudden death count down timer");       
        yield return new WaitForSeconds(pausetime);      
        Debug.Log("Start sudden death count down timer");
        StartCoroutine(startSuddenDeathTimer(countDownTimer));
    }

    IEnumerator startSuddenDeathTimer(int countDownTimer)//start sudden death countdown
    {
        for(int timeLeft = countDownTimer; timeLeft > 0; timeLeft --)
        {
            FindObjectOfType<GameSession>().updateSuddenDeathTimer(timeLeft);
            yield return new WaitForSeconds(countDownTimerInterval);           
        }      
        FindObjectOfType<GameSession>().displaySuddenDeathText();
        StartCoroutine(ActivateDeathZone());//times up, start sudden death
    }


    IEnumerator ActivateDeathZone()
    {
        GetComponent<SpriteRenderer>().enabled = true;
        while (true)
        {           
            transform.position = new Vector2(transform.position.x + distancePerInterval, transform.position.y);
            yield return new WaitForSeconds(moveInterval);
        }
    }

}
