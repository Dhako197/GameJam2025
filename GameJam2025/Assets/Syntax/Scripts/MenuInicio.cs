using UnityEngine;

public class ShowImageWithSmoothTransition : MonoBehaviour
{
    public CanvasGroup imageCanvasGroup;
    public CanvasGroup menuCanvasGroup;
    public float displayTime = 3f;
    public float transitionDuration = 1f;

    private void Start()
    {
        if (imageCanvasGroup != null)
            imageCanvasGroup.alpha = 1;

        if (menuCanvasGroup != null)
            menuCanvasGroup.alpha = 0;

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
            imageCanvasGroup.alpha = 0;

        if (menuCanvasGroup != null)
            menuCanvasGroup.alpha = 1;
    }
}