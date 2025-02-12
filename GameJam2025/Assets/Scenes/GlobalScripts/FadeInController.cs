using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeInController : MonoBehaviour
{
    [SerializeField] private CanvasGroup blackout;
    [SerializeField] private float fadeInTime = 1f;

    private float counter = 0;
    private bool isDone = false;

    private void Start()
    {
        blackout.alpha = 1;
        isDone = false;
        counter = 0;
    }

    void Update()
    {
        if (isDone) return;

        counter += Time.deltaTime;

        blackout.alpha = Mathf.Clamp01(fadeInTime - counter);

        if (counter > fadeInTime)
        {
            blackout.gameObject.SetActive(false);
            isDone = true;
        }
    }
}
