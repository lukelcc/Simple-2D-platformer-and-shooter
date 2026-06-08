using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;

public class PlayerLabel : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI PlayerNumLabel;
    // Start is called before the first frame update
    void Start()
    {
        //PlayerNumLabel.transform.position = PlayerInput.GetPlayerByIndex(0).gameObject.transform.localPosition;
        //for (int playerIndex = 0; playerIndex < PlayerInput.all.Count; playerIndex++)
        //{
            
        //    PlayerInput.GetPlayerByIndex(playerIndex).DeactivateInput();
        //}
        //PlayerNumLabel.transform.position = FindObjectOfType<PlayerMovement>().
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
