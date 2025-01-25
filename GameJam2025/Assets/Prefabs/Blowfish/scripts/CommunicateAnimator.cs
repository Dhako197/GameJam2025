using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommunicateAnimator : MonoBehaviour
{
    private SleepingBlowfish parent;

    // Start is called before the first frame update
    void Start()
    {
        parent = gameObject.GetComponentInParent<SleepingBlowfish>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ResetSleeping()
    {
        parent.ResetSleep();
    }

    public void RandomizeChecking()
    {
        parent.SetIsChecking(UnityEngine.Random.Range(0,1) > 0.5);
    }

    public void SetChecking(float checking)
    {
        parent.SetIsChecking(checking == 1.0);
    }
}
