using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destructible : MonoBehaviour
{
    [SerializeField] private GameObject destroyVFX;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //if object has a damage source component, destroy this object and instantiate the destroy VFX
        if (collision.gameObject.GetComponent<DamageSource>() || collision.gameObject.GetComponent<Projectiles>())
        {
            GetComponent<PickupSpawner>().DropItems();
            Instantiate(destroyVFX, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }
}
