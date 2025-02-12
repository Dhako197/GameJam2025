using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InitialVideoController : MonoBehaviour
{
    public static InitialVideoController Instance { get; private set; }

    private bool hasVideoRun = false;
    
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }

        DontDestroyOnLoad(this.gameObject);
    }

    public bool GetHasVideoRun()
    {
        return hasVideoRun;
    }

    public void SetHasVideoRun(bool run)
    {
        hasVideoRun = run;
    }
}
