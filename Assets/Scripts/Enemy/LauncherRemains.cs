using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LauncherRemains : MonoBehaviour
{
    private SpriteFade spriteFade;

    private void Awake()
    {
        //Sets the sprite fade to the sprite fade component on the object
        spriteFade = GetComponent<SpriteFade>();
    }

    private void Start()
    {
        //Starts the slow fade routine
        StartCoroutine(spriteFade.SlowFadeRoutine());

        //Disables the collider after 0.2 seconds
        Invoke("DisableCollider", 0.2f);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Creates a player health object for the player
        PlayerHealth playerHealth = collision.GetComponent<PlayerHealth>();

        //If the player health exists
        if (playerHealth != null)
        {
            //Calls the take damage function
            playerHealth.TakeDamage(1, transform);
        } 
    }

    private void DisableCollider()
    {
        //Disables the collider
        GetComponent<CapsuleCollider2D>().enabled = false;
    }
}
