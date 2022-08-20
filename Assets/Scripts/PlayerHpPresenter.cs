using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerHpPresenter : MonoBehaviour
{
    //the object that's managing the player hp
    //[Header("The game session object")]
    //[SerializeField] GameSession gameSession;
    //UI text
    [Header("Player Hp UI text")]
    [SerializeField] TextMeshProUGUI PlayerHpText;

    // Start is called before the first frame update
    void Start()
    {
        GetComponent<GameSession>().onHpChange += UpdateUI;
        //gameSession.onHpChange += UpdateUI;
        UpdateUI();
    }

    // Update is called once per frame
    void UpdateUI()
    {
        PlayerHpText.text = GetComponent<GameSession>().getRemainingHp().ToString();
        //PlayerHpText.text = gameSession.getRemainingHp().ToString();
    }
}
