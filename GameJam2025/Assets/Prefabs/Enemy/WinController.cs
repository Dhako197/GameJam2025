using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinController : MonoBehaviour
{
    private BubblerScript playerController;
    private float trapped = 0;

    void Start()
    {
        playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<BubblerScript>();
    }

    void Update()
    {
        if (trapped == 3) {
            playerController.EndRun("Credits");
        }
    }


    public void AddTrapped()
    {
        trapped += 1;
    }
}
