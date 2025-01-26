using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class TimeManager : MonoBehaviour
{
    private int gameTimeExpectationMinutes = 5;
    private int maxHours = 6;
    private int initialTime = 0;
    private int finalTime;
    private int counter = 0;
    private float timer = 0;
    private float currentTime;
    private float hourConversion;
    private float minuteConvertion;

    [SerializeField] private Transform hourHand;
    [SerializeField] private Transform minuteHand;
    
    private int lastGameHour;


// Start is called before the first frame update
    void Start()
    {
        int hour = System.DateTime.Now.Hour;
        initialTime = hour < 12 ? hour : hour - 12;
        finalTime = (initialTime + maxHours) < 12 ? initialTime + maxHours : initialTime - maxHours;
        currentTime = initialTime;
        lastGameHour = Mathf.FloorToInt(currentTime);
        hourConversion = gameTimeExpectationMinutes / maxHours;
        minuteConvertion = hourConversion / 3600;
        /*Debug.Log($"Initial time {initialTime}");
        Debug.Log($"finalTime {finalTime}");
        Debug.Log($"currentTime {currentTime}");
        Debug.Log($"hourConversion {hourConversion}");
        Debug.Log($"minuteConvertion {minuteConvertion}");*/
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

        UpdateClockHands();
        CheckForNewGameHour();

        /*Debug.Log($"Counter {counter}");
        Debug.Log($"Current time {currentTime}");*/
    }

    void UpdateClockHands()
    {
        float hours = currentTime % 12;
        float minutes = (currentTime * 60) % 60;
        hourHand.localRotation = Quaternion.Euler(0, 0, -hours * 30);
        minuteHand.localRotation = Quaternion.Euler(0, 0, -minutes * 6);
    }
    
    void CheckForNewGameHour()
    {
        int currentGameHour = Mathf.FloorToInt(currentTime); // Obtener la hora de juego actual completa

        // Verificar si ha pasado una nueva hora de juego
        if (currentGameHour > lastGameHour)
        {
            lastGameHour = currentGameHour; // Actualizar la última hora de juego registrada
            Debug.Log($"Ha pasado una hora de juego. Hora actual: {currentGameHour}");

            // Aquí puedes agregar lógica adicional, como mostrar un mensaje en la UI
        }
    }

    public void IncrementHour()
    {
        currentTime += 1;
    }
}