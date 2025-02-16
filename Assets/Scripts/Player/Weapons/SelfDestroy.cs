using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelfDestroy : MonoBehaviour
{
    private ParticleSystem particleSys;

    private void Awake()
    {
        // Get the particle system component
        particleSys = GetComponent<ParticleSystem>();
    }

    private void Update()
    {
        // Check if the particle system is alive
        if (particleSys && !particleSys.IsAlive())
        {
            // Destroy the object
            DestroySelf();
        }
    }

    public void DestroySelf()
    {
        // Destroy the object
        Destroy(gameObject);
    }
}
