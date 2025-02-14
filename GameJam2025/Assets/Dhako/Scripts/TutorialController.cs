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
    private bool _alreadyUseUi = false;
    private bool _alreadyUseTimerUi = false;


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

            if (!_alreadyUseUi)
            {
                Time.timeScale = 1;
                TutorialUI.DOAnchorPos(new Vector2(0, 0), 0.5f, true).SetEase(Ease.InBack);
                _alreadyUseUi = true;
            }

            if (!_alreadyUseTimerUi)
            {
                Time.timeScale = 1;
                TimeTutorial.DOAnchorPos(new Vector2(0, 0), 0.5f, true).SetEase(Ease.InBack);
                _alreadyUseTimerUi = true;
            }
            
            

        }
    }

    public void TimeTutorialActivate()
    {
        TimeTutorial.DOAnchorPos(new Vector2(0, -1953), 1f, true).SetEase(Ease.OutBack)
            .OnComplete(PauseTime);
    }
}
