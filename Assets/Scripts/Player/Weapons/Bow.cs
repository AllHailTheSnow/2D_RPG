using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bow : MonoBehaviour, IWeapon
{
    //private SpriteRenderer spriteRenderer;
    [SerializeField] private WeaponInfo weaponInfo;
    [SerializeField] private GameObject arrowPrefab;
    [SerializeField] private Transform arrowSpawnPoint;

    private Animator myAnimator;

    private void Awake()
    {
        //get animator component
        myAnimator = GetComponent<Animator>();
    }

    public void Attack()
    {
        //play attack animation
        myAnimator.SetTrigger("Fire");
        //create arrow projectile at spawn point position and active weapon rotation
        GameObject newArrow = Instantiate(arrowPrefab, arrowSpawnPoint.position, ActiveWeapon.Instance.transform.rotation);
        //update weapon info for the arrow
        newArrow.GetComponent<Projectiles>().UpdateWeaponInfo(weaponInfo);
    }

    public WeaponInfo GetWeaponInfo()
    {
        return weaponInfo;
    }
}
