using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LauncherProjectile : MonoBehaviour
{
    [SerializeField] private float duration = 1f;
    [SerializeField] private AnimationCurve animCurve;
    [SerializeField] private float heightY = 3f;
    [SerializeField] private GameObject launcherProjectileShadow;
    [SerializeField] private GameObject remainsPrefab;

    private void Start()
    {
        //Creates a projectile shadow object at the position of the launcher projectile
        GameObject projectileShadow = Instantiate(launcherProjectileShadow, transform.position + new Vector3(0, -0.3f, 0), Quaternion.identity);

        //Creates a player position object for the player
        Vector3 playerPos = PlayerController.Instance.transform.position;
        //Creates a launcher shadow start position object for the projectile shadow
        Vector3 launcherShadowStartPos = projectileShadow.transform.position;

        //Starts the launcher curve routine
        StartCoroutine(LauncherCurveRoutine(transform.position, playerPos));
        //Starts the move shadow routine
        StartCoroutine(MoveShadowRoutine(projectileShadow, launcherShadowStartPos, playerPos));
    }

    private IEnumerator LauncherCurveRoutine(Vector3 startPos, Vector3 endPos)
    {
        //Sets the time passed to 0
        float timePassed = 0f;

        //While the time passed is less than the duration
        while (timePassed < duration)
        {
            //Adds the time since the last frame to the time passed
            timePassed += Time.deltaTime;
            //Sets the linear t to the time passed divided by the duration
            float linearT = timePassed / duration;
            //Sets the height t to the evaluation of the animation curve
            float heightT = animCurve.Evaluate(linearT);
            //Sets the height to the lerp of 0, height y, and the height t
            float height = Mathf.Lerp(0f, heightY, heightT);

            //Sets the position of the launcher projectile to the lerp of the start position, the end position, and the linear t plus the height
            transform.position = Vector2.Lerp(startPos, endPos, linearT) + new Vector2(0f, height);

            yield return null;
        }

        //Creates a remains prefab object at the position of the launcher projectile
        Instantiate(remainsPrefab, transform.position, Quaternion.identity);
        //Destroys the game object
        Destroy(gameObject);
    }

    private IEnumerator MoveShadowRoutine(GameObject launcherShadow, Vector3 startPos, Vector3 endPos)
    {
        //Sets the time passed to 0
        float timePassed = 0f;

        //While the time passed is less than the duration
        while (timePassed < duration)
        {
            //Adds the time since the last frame to the time passed
            timePassed += Time.deltaTime;
            //Sets the linear t to the time passed divided by the duration
            float linearT = timePassed / duration;
            //Sets the position of the launcher shadow to the lerp of the start position and the end position
            launcherShadow.transform.position = Vector2.Lerp(startPos,endPos, linearT);
            yield return null;
        }

        //Destroys the game object
        Destroy(launcherShadow);
    }
}
