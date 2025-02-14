using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class TutorialController : MonoBehaviour
{
    public static TutorialController Instance { get; private set; }

    [SerializeField] private RectTransform TutorialUI;
    [SerializeField] private RectTransform TimeTutorial;
    private bool _uiTutorialIsOpen = false;
    private bool _timerTutorialIsOpen = false;


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
    }

    // Start is called before the first frame update
    void Start()
    {

        TutorialUI.DOAnchorPos(new Vector2(0, -1953), 1f, true).SetEase(Ease.OutBack)
            .OnComplete(PauseTime);
        _uiTutorialIsOpen = true;


    }

    private void PauseTime()
    {
        Time.timeScale = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Pass"))
        {
            Time.timeScale = 1;

            if (_uiTutorialIsOpen)
            {
                
                TutorialUI.DOAnchorPos(new Vector2(0, 0), 0.5f, true).SetEase(Ease.InBack);
                _uiTutorialIsOpen = false;
            }

            if (_timerTutorialIsOpen )
            {
                Debug.Log("Adios tutorial Timer");
                TimeTutorial.DOAnchorPos(new Vector2(0, 0), 0.5f, true).SetEase(Ease.InBack);
                _timerTutorialIsOpen = false;
            }
            
            

        }
    }

    public void TimeTutorialActivate()
    {
        TimeTutorial.DOAnchorPos(new Vector2(0, -1953), 1f, true).SetEase(Ease.OutBack)
            .OnComplete(PauseTime);
        _timerTutorialIsOpen = true;
    }
}
