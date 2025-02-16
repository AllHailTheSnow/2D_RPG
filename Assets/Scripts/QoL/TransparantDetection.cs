using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TransparantDetection : MonoBehaviour
{
    //Sets range for transparancy amount
    [Range(0, 1)]
    [SerializeField] private float transparancyAmount = 0.8f;
    //Sets fade time
    [SerializeField] private float fadeTime = 0.4f;

    // private SpriteRenderer variable
    private SpriteRenderer spriteRenderer;
    // private Tilemap variable
    private Tilemap tilemap;

    private void Awake()
    {
        // sets spriteRenderer to the SpriteRenderer component of the object
        spriteRenderer = GetComponent<SpriteRenderer>();
        // sets tilemap to the Tilemap component of the object
        tilemap = GetComponent<Tilemap>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // checks if the object that collided with the object has a PlayerController component
        if (collision.GetComponent < PlayerController>())
        {
            // checks if spriteRenderer is not null
            if (gameObject.activeInHierarchy && spriteRenderer && spriteRenderer.enabled)
            {
                // starts the FadeRoutine with the spriteRenderer, fadeTime, the alpha value of the sprite
                StartCoroutine(FadeRoutine(spriteRenderer, fadeTime, spriteRenderer.color.a, transparancyAmount));
            }
            // checks if tilemap is not null
            else if(gameObject.activeInHierarchy && tilemap && tilemap.isActiveAndEnabled)
            {
                // starts the FadeRoutine with the tilemap, fadeTime, the alpha value of the tilemap
                StartCoroutine(FadeRoutine(tilemap, fadeTime, tilemap.color.a, transparancyAmount));
                //Debug.Log("Tilemap");
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        // checks if the object that collided with the object has a PlayerController component
        if (collision.GetComponent<PlayerController>())
        {
            // checks if spriteRenderer is not null
            if (gameObject.activeInHierarchy && spriteRenderer && spriteRenderer.enabled)
            {
                // starts the FadeRoutine with the spriteRenderer, fadeTime, the alpha value of the sprite
                StartCoroutine(FadeRoutine(spriteRenderer, fadeTime, spriteRenderer.color.a, 1f));
            }
            // checks if tilemap is not null
            else if (gameObject.activeInHierarchy && tilemap && tilemap.isActiveAndEnabled)
            {
                // starts the FadeRoutine with the tilemap, fadeTime, the alpha value of the tilemap
                StartCoroutine(FadeRoutine(tilemap, fadeTime, tilemap.color.a, 1f));
            }
        }
    }

    private IEnumerator FadeRoutine(SpriteRenderer spriteRenderer, float fadeTime, float startValue, float transparancyTarget)
    {
        // sets elapsedTime to 0
        float elapsedTime = 0;
        // while elapsedTime is less than fadeTime
        while (elapsedTime < fadeTime)
        {
            // adds Time.deltaTime to elapsedTime
            elapsedTime += Time.deltaTime;
            // sets newAlpha to the lerp between startValue and transparancyTarget with elapsedTime / fadeTime
            float newAlpha = Mathf.Lerp(startValue, transparancyTarget, elapsedTime / fadeTime);
            // sets the alpha value of the spriteRenderer to newAlpha
            spriteRenderer.color = new Color(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, newAlpha);
            yield return null;
        }
    }

    private IEnumerator FadeRoutine(Tilemap tilemap, float fadeTime, float startValue, float transparancyTarget)
    {
        // sets elapsedTime to 0
        float elapsedTime = 0;
        // while elapsedTime is less than fadeTime
        while (elapsedTime < fadeTime)
        {
            // adds Time.deltaTime to elapsedTime
            elapsedTime += Time.deltaTime;
            // sets newAlpha to the lerp between startValue and transparancyTarget with elapsedTime / fadeTime
            float newAlpha = Mathf.Lerp(startValue, transparancyTarget, elapsedTime / fadeTime);
            // sets the alpha value of the tilemap to newAlpha
            tilemap.color = new Color(tilemap.color.r, tilemap.color.g, tilemap.color.b, newAlpha);
            yield return null;
        }
    }
}
