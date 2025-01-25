using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MesaInteractable : MonoBehaviour, IInteractable
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public string GetAction()
    {
        string action = "Inspect";
        return action;
    }

    public GameObject GetObject()
    {
        return gameObject;
    }
}
