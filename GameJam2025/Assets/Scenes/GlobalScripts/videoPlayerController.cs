using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class videoPlayerController : MonoBehaviour
{
    [SerializeField] private CanvasGroup backgroundCanvas;
    private VideoPlayer videoPlayer;
    private float duration;
    private float counter = 0;
    private float transitionCounter = 0;
    private readonly float transitionDuration = 2;

    void Start()
    {
        backgroundCanvas.alpha = 0;
        videoPlayer = GetComponent<VideoPlayer>();
        duration = (float)videoPlayer.length;
    }

    void Update()
    {
        CheckVideoDuration();
        CheckTransition();
    }

    void CheckVideoDuration()
    {
        counter += Time.deltaTime;
        
        if (counter >= duration)
        {
            InitialVideoController.Instance.SetHasVideoRun(true);
            SceneManager.LoadScene("FinalFinal");
        }

        if (transitionCounter == 0 && (duration - counter) < transitionDuration)
        {
            transitionCounter = transitionDuration;
        }
    }

    void CheckTransition()
    {
        if (transitionCounter > 0)
        {
            transitionCounter -= Time.deltaTime;
            backgroundCanvas.alpha = Mathf.Clamp01((transitionDuration - transitionCounter) / transitionDuration);

        }
    }
}
