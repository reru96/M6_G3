using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Counter : MonoBehaviour
{
    
    void Start()
    {
        Debug.Log(GameManager.Instance.coins);
    }

}
