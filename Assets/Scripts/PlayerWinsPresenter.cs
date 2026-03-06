using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerWinsPresenter : MonoBehaviour
{
    [Header("Player Wins UI text")]
    [SerializeField] TextMeshProUGUI PlayerWinsText;
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<PlayerWins>().onWinCountChange += UpdateUI;
        //gameSession.onHpChange += UpdateUI;
        UpdateUI();
    }

    // Update is called once per frame
    void UpdateUI()
    {
        PlayerWinsText.text = GetComponent<PlayerWins>().getWinCount().ToString();
        //Debug.Log("update win");
        //PlayerHpText.text = gameSession.getRemainingHp().ToString();
    }
}
