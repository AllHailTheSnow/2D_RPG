using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteFade : MonoBehaviour
{
    [SerializeField] private float fadeTime = .4f;

    private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        //Sets the sprite renderer to the sprite renderer component on the object
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public IEnumerator SlowFadeRoutine()
    {
        //Sets the elapsed time to 0
        float elapsedTime = 0f;
        //Sets the start value to the alpha of the sprite renderer
        float startValue = spriteRenderer.color.a;

        //While the elapsed time is less than the fade time
        while (elapsedTime < fadeTime)
        {
            //Adds the time since the last frame to the elapsed time
            elapsedTime += Time.deltaTime;
            //Sets the new alpha to the lerp of the start value, 0, and the elapsed time divided by the fade time
            float newAlpha = Mathf.Lerp(startValue, 0f, elapsedTime / fadeTime);
            //Sets the alpha of the sprite renderer to the new alpha
            spriteRenderer.color = new Color(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, newAlpha);
            yield return null;
        }

        //Destroys the game object
        Destroy(gameObject);
    }
}
