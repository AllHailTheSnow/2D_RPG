using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectiles : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 22f;
    [SerializeField] private GameObject particleVFX;

    private WeaponInfo weaponInfo;
    private Vector3 startPos;

    // Start is called before the first frame update
    void Start()
    {
        startPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        MoveProjectile();
        DetectFireDistance();
    }

    public void UpdateWeaponInfo(WeaponInfo weaponInfo)
    {
        this.weaponInfo = weaponInfo;
    }

    private void MoveProjectile()
    {
        //Quaternion rotation = Quaternion.Euler(0, 0, 45);
        //Vector3 movementDir = rotation * Vector3.right;

        transform.Translate(Vector3.right * Time.deltaTime * moveSpeed);
    }

    private void DetectFireDistance()
    {
        //if the distance between the start position and the current position is greater than the weapon range, destroy the projectile
        if (Vector3.Distance(transform.position, startPos) > weaponInfo.weaponRange)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //get the enemy health script from the enemy object
        EnemyHealth enemyHealth = collision.GetComponent<EnemyHealth>();
        //get the indestructable script from the indestructable object
        Indestructable indestructable = collision.GetComponent<Indestructable>();

        //if the projectile collides with an enemy or indestructable object, destroy the projectile and spawn the particle VFX
        if (!collision.isTrigger && (enemyHealth || indestructable))
        {
            Instantiate(particleVFX, transform.position, transform.rotation);
            Destroy(gameObject);
        }
    }
}
