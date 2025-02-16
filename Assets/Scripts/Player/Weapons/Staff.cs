using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class Staff : MonoBehaviour, IWeapon
{
    [SerializeField] private WeaponInfo weaponInfo;
    [SerializeField] private GameObject magicPrefab;
    [SerializeField] private Transform magicSpawnPoint;

    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        // Call the MouseFollowWithOffset method
        MouseFollowWithOffset();
    }

    public void Attack()
    {
        //Debug.Log("Staff Attack");
        //ActiveWeapon.Instance.ToggleIsAttacking(false);

        animator.SetTrigger("Fire");
    }

    public void SpawnMagic()
    {
        GameObject newMagic = Instantiate(magicPrefab, magicSpawnPoint.position, quaternion.identity);
        newMagic.GetComponent<Magic>().UpdateMagicRange(weaponInfo.weaponRange);
    }

    public WeaponInfo GetWeaponInfo()
    {
        return weaponInfo;
    }

    private void MouseFollowWithOffset()
    {
        // Get the mouse position and the player screen point
        Vector3 mousePos = Input.mousePosition;
        Vector3 playerScreenPoint = Camera.main.WorldToScreenPoint(PlayerController.Instance.transform.position);

        // Get the angle between the player and the mouse position in degrees
        float angle = Mathf.Atan2(mousePos.y, mousePos.x) * Mathf.Rad2Deg;

        // If the mouse is on the left side of the player
        if (mousePos.x < playerScreenPoint.x)
        {
            // Set the rotation of the staff to the angle with an offset
            ActiveWeapon.Instance.transform.rotation = Quaternion.Euler(0, -180, angle);
        }
        // If the mouse is on the right side of the player
        else
        {
            // Set the rotation of the staff to the angle with an offset the opposite way
            ActiveWeapon.Instance.transform.rotation = Quaternion.Euler(0, 0, angle);
        }
    }
}
