using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelfDestruct : MonoBehaviour
{
    [SerializeField] float alivePeriod = 0.5f;

    private void Awake()
    {
        Destroy(gameObject,alivePeriod);
    }

 


}

