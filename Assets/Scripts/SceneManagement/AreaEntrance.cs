using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaEntrance : MonoBehaviour
{
    // The name of the scene transition that the player is coming from
    [SerializeField] private string transitionName;

    private void Start()
    {
        // If the scene transition name is the same as the scene transition name in the SceneManagement script
        if (transitionName == SceneManagement.Instance.SceneTransitionName)
        {
            // Set the player's position to the position of the area entrance
            PlayerController.Instance.transform.position = transform.position;
            // Set the camera to follow the player
            CameraController.Instance.SetPlayerFollowCamera();
            // Fade the screen to clear
            UIFade.Instance.FadeToClear();
        }
    }
}
