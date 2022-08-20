using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class PlayerScorePresenter : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI playerScoreText;

    // Start is called before the first frame update
    void Start()
    {
        GetComponent<PlayerScore>().onScoreChange += UpdateUI;
        UpdateUI();
    }

    void UpdateUI()
    {
        playerScoreText.text = GetComponent<PlayerScore>().GetScore().ToString();
    }
}
