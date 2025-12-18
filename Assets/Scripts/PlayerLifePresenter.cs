using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class PlayerLifePresenter : MonoBehaviour
{
    //the object that's managing the player hp
    //[Header("The game session object")]
    //[SerializeField] GameSession gameSession;
    //UI text
    [Header("Player Life UI text")]
    [SerializeField] TextMeshProUGUI PlayerLifeText;

    // Start is called before the first frame update
    void Start()
    {
        GetComponent<PlayerLife>().onLifeChange += UpdateUI;
        //gameSession.onHpChange += UpdateUI;
        UpdateUI();
    }

    // Update is called once per frame
    void UpdateUI()
    {
        PlayerLifeText.text = GetComponent<PlayerLife>().getRemainingLife().ToString();
        //PlayerHpText.text = gameSession.getRemainingHp().ToString();
    }
}
