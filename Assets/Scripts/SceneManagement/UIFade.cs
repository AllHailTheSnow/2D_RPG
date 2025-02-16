using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIFade : Singleton<UIFade>
{
    [SerializeField] private Image fadeScreen;
    [SerializeField] private float fadeSpeed = 1f;

    private IEnumerator fadeRoutine;

    public void FadeToBlack()
    {
        // If the fade routine is not null, stop the coroutine
        if (fadeRoutine != null)
        {
            StopCoroutine(fadeRoutine);
        }

        // Set the fade routine to the FadeRoutine with a target alpha of 1
        fadeRoutine = FadeRoutine(1);
        // Start the coroutine with the fade routine
        StartCoroutine(fadeRoutine);
    }

    public void FadeToClear()
    {
        // If the fade routine is not null, stop the coroutine
        if (fadeRoutine != null)
        {
            StopCoroutine(fadeRoutine);
        }

        // Set the fade routine to the FadeRoutine with a target alpha of 0
        fadeRoutine = FadeRoutine(0);
        // Start the coroutine with the fade routine
        StartCoroutine(fadeRoutine);
    }

    private IEnumerator FadeRoutine(float targetAplha)
    {
        // While the fade screen alpha is not approximately equal to the target alpha
        while (!Mathf.Approximately(fadeScreen.color.a, targetAplha))
        {
            // Move the alpha of the fade screen towards the target alpha
            float alpha = Mathf.MoveTowards(fadeScreen.color.a, targetAplha, fadeSpeed * Time.deltaTime);
            // Set the alpha of the fade screen to the new alpha
            fadeScreen.color = new Color(fadeScreen.color.r, fadeScreen.color.g, fadeScreen.color.b, alpha);
            yield return null;
        }
    }
}
