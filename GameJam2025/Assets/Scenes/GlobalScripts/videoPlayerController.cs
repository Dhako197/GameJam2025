using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class videoPlayerController : MonoBehaviour
{
    private VideoPlayer videoPlayer;
    private float duration;
    private float counter = 0;

    void Start()
    {
        videoPlayer = GetComponent<VideoPlayer>();
        duration = (float)videoPlayer.length;
    }

    // Update is called once per frame
    void Update()
    {
        counter += Time.deltaTime;

        if (counter >= duration)
        {
            InitialVideoController.Instance.SetHasVideoRun(true);
            SceneManager.LoadScene("FinalFinal");
        }
    }
}
