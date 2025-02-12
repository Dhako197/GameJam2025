using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class ShowImageWithSmoothTransition : MonoBehaviour
{
    private InitialVideoController initialVideoController;
    public CanvasGroup imageCanvasGroup;
    public CanvasGroup menuCanvasGroup;
    public float displayTime = 3f;
    public float transitionDuration = 1f;

    private void Start()
    {
        initialVideoController = GetComponent<InitialVideoController>();
        bool initialImage = true;

        if (imageCanvasGroup != null)
        {
            imageCanvasGroup.alpha = 1;
            imageCanvasGroup.interactable = initialImage;
            imageCanvasGroup.blocksRaycasts = initialImage;
        }

        if (menuCanvasGroup != null)
        {
            menuCanvasGroup.alpha = 0;
            menuCanvasGroup.interactable = !initialImage;
            menuCanvasGroup.blocksRaycasts = !initialImage;
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

            if (menuCanvasGroup != null)
                menuCanvasGroup.alpha = Mathf.Clamp01(t);

            yield return null;
        }

        if (imageCanvasGroup != null)
        {
            imageCanvasGroup.alpha = 0;
            imageCanvasGroup.interactable = false;
            imageCanvasGroup.blocksRaycasts = false;
        }


        if (menuCanvasGroup != null)
        {
            menuCanvasGroup.alpha = 1;
            menuCanvasGroup.interactable = true;
            menuCanvasGroup.blocksRaycasts = true;
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