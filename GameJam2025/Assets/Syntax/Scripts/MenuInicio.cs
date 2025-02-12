using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class ShowImageWithSmoothTransition : MonoBehaviour
{
    public CanvasGroup imageCanvasGroup;
    public CanvasGroup menuCanvasGroup;
    public float displayTime = 3f;
    public float transitionDuration = 1f;

    private void Start()
    {
        bool initialImage = !InitialVideoController.Instance.GetHasVideoRun();

        if (imageCanvasGroup != null)
        {
            imageCanvasGroup.alpha = initialImage ? 1 : 0;
            imageCanvasGroup.interactable = initialImage;
            imageCanvasGroup.blocksRaycasts = initialImage;
        }

        StartCoroutine(ShowImageThenMenuWithSmoothTransition());
    }

    private System.Collections.IEnumerator ShowImageThenMenuWithSmoothTransition()
    {
        yield return new WaitForSeconds(displayTime);

        float elapsedTime = 0f;

        while (elapsedTime < transitionDuration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / transitionDuration;

            if (imageCanvasGroup != null)
                imageCanvasGroup.alpha = Mathf.Clamp01(1 - t);

            yield return null;
        }

        if (imageCanvasGroup != null)
        {
            imageCanvasGroup.alpha = 0;
            imageCanvasGroup.interactable = false;
            imageCanvasGroup.blocksRaycasts = false;
        }
    }

    public void StartGame()
    {
        string nextScene = InitialVideoController.Instance.GetHasVideoRun() ? "FinalFinal" : "VideoLore";
        SceneManager.LoadScene(nextScene);
    }
    
    public void ExitGame()
    {
        Application.Quit();
    }
}