using UnityEngine;
using System;

public class TimerManager : MonoBehaviour
{
    [SerializeField] private Transform hourHand;
    [SerializeField] private Transform minuteHand;

    public float gameTimeDuration = 20f * 60f;
    private DateTime startTime;
    private float elapsedTime = 0f;
    private float gameSecondsPerRealSecond;
    private int lastLoggedHour = -1;

    private DateTime simulatedTime;

    private void Start()
    {
        startTime = DateTime.Now;
        gameSecondsPerRealSecond = (6f * 60f * 60f) / gameTimeDuration;
    }

    private void Update()
    {
        elapsedTime += Time.deltaTime;
        float gameTimeElapsed = elapsedTime * gameSecondsPerRealSecond;
        simulatedTime = startTime.AddSeconds(gameTimeElapsed);
        UpdateClock(simulatedTime);
        LogHourChange(simulatedTime);
    }

    private void UpdateClock(DateTime simulatedTime)
    {
        float hourAngle = (simulatedTime.Hour % 12) * 30f + simulatedTime.Minute * 0.5f;
        float minuteAngle = simulatedTime.Minute * 6f;

        if (hourHand != null)
            hourHand.localRotation = Quaternion.Euler(0f, 0f, -hourAngle);

        if (minuteHand != null)
            minuteHand.localRotation = Quaternion.Euler(0f, 0f, -minuteAngle);
    }

    private void LogHourChange(DateTime simulatedTime)
    {
        if (simulatedTime.Hour != lastLoggedHour)
        {
            lastLoggedHour = simulatedTime.Hour;
            Debug.Log($"Nueva hora en el reloj: {simulatedTime:HH:mm}");
        }
    }

    public void Increase()
    {
        simulatedTime = simulatedTime.AddHours(1);
        elapsedTime = (float)(simulatedTime - startTime).TotalSeconds / gameSecondsPerRealSecond;
        UpdateClock(simulatedTime);
        Debug.Log($"Se ha agregado una hora. Nueva hora del juego: {simulatedTime:HH:mm}");
    }
}