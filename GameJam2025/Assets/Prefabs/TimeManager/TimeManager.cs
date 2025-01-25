using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class TimeManager : MonoBehaviour
{
    private int gameTimeExpectationMinutes = 20;
    private int maxHours = 6;
    private int initialTime = 0;
    private int finalTime;
    private int counter = 0;
    private float timer = 0;
    private float currentTime;
    private float hourConversion;
    private float minuteConvertion;


    // Start is called before the first frame update
    void Start()
    {
        int hour = System.DateTime.Now.Hour;
        initialTime = hour < 12 ? hour : hour - 12;
        finalTime = (initialTime + maxHours) < 12 ? initialTime + maxHours : initialTime - maxHours;
        currentTime = initialTime;

        hourConversion = gameTimeExpectationMinutes / maxHours;
        minuteConvertion = hourConversion / 60;
        Debug.Log(initialTime);
        Debug.Log(finalTime);
        Debug.Log(currentTime);
        Debug.Log(hourConversion);
        Debug.Log(minuteConvertion);
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if (counter < (int)(timer % 60))
        {
            counter++;
            currentTime += minuteConvertion;
        }
        
        Debug.Log(counter);
        Debug.Log(currentTime);
    }

    public void IncrementHour()
    {
        currentTime += 1;
    }
}
