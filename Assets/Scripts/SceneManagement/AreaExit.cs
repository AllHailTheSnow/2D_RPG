using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AreaExit : MonoBehaviour
{
    //Creates a string for the scene to load
    [SerializeField] private string sceneToLoad;
    [SerializeField] private string sceneTransitionName;

    private float waitToLoad = 1f;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //If the collision object has a player controller component
        if (collision.gameObject.GetComponent<PlayerController>())
        {
            SceneManagement.Instance.SetSceneTransitionName(sceneTransitionName);
            UIFade.Instance.FadeToBlack();
            StartCoroutine(LoadSceneRoutine());
        }
    }

    private IEnumerator LoadSceneRoutine()
    {
        while(waitToLoad >= 0)
        {
            waitToLoad -= Time.deltaTime;
            yield return null;
        }

        SceneManager.LoadScene(sceneToLoad);
    }
}
