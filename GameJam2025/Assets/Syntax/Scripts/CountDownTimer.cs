using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CountDownTimer : MonoBehaviour
{
    [SerializeField] public float realTimeDuration = 1200;
    [SerializeField] public float timeRemaining = 1200;
    [SerializeField] public float gameTimeDuration = 21600;
    [SerializeField] public Text timeText;
    void Start()
    {
        timeRemaining = gameTimeDuration;
    }

    // Update is called once per frame
    void Update()
    {
        if (timeRemaining > 0)
        {
            timeRemaining -= Time.deltaTime * (gameTimeDuration / realTimeDuration);
            UpdateTimerDisplay(timeRemaining);
        }
        else
        {
            timeRemaining = 0; 
            Debug.Log("Chao papa");
        }
    }
    
    void UpdateTimerDisplay(float timeToDisplay)
    {
        timeToDisplay += 1; 

        int hours = Mathf.FloorToInt(timeToDisplay / 3600);
        int minutes = Mathf.FloorToInt((timeToDisplay % 3600) / 60);
        int seconds = Mathf.FloorToInt(timeToDisplay % 60);

        timeText.text = string.Format("{0:00}:{1:00}:{2:00}", hours, minutes, seconds);
    }
    
    public float GetMinutes()
    {
        return Mathf.FloorToInt(timeRemaining / 60);
    }
    
    public float GetHour()
    {
        return Mathf.FloorToInt(timeRemaining / 3600);
    }
}
