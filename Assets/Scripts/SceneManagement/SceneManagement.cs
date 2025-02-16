using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneManagement : Singleton<SceneManagement>
{
    // The name of the scene transition that the player is coming from
    public string SceneTransitionName { get; private set; }

    // Set the scene transition name
    public void SetSceneTransitionName(string sceneTransitionName)
    {
        // Set the scene transition name to the given scene transition name
        this.SceneTransitionName = sceneTransitionName;
    }
}
