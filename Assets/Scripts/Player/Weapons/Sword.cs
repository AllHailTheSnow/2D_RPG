using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword : MonoBehaviour, IWeapon
{
    [SerializeField] private GameObject slashAnimPrefab;
    [SerializeField] WeaponInfo weaponInfo;

    // The weapon collider object
    private Transform weaponCollider;

    // The spawn point for the slash animation
    private Transform slashAnimSpawnPoint;

    // The animator component
    private Animator myAnimator;

    // The slash animation object
    private GameObject slashAnim;

    private void Awake()
    {
        // Get the animator component
        myAnimator = GetComponent<Animator>();
    }

    private void Start()
    {
        weaponCollider = PlayerController.Instance.GetWeaponCollider();
        slashAnimSpawnPoint = PlayerController.Instance.GetSlashAnimSpawnPoint();
    }

    private void Update()
    {
        MouseFollowWithOffset();
    }

    public WeaponInfo GetWeaponInfo()
    {
        return weaponInfo;
    }

    public void Attack()
    {
        // Set the attack trigger in the animator
        myAnimator.SetTrigger("Attack");

        // Enable the weapon collider
        weaponCollider.gameObject.SetActive(true);

        // Instantiate the slash animation
        slashAnim = Instantiate(slashAnimPrefab, slashAnimSpawnPoint.position, Quaternion.identity);

        // Set the parent of the slash animation to the same parent as the sword
        slashAnim.transform.parent = this.transform.parent;
    }

    public void DoneAttacking()
    {
        //set weapon collider to false after the attack animation is done
        weaponCollider.gameObject.SetActive(false);
    }

    public void SwingUpFlipAnim()
    {
        // Flip the sword animation upside down
        FlipLeft_x_Angle(-180);
    }

    public void SwingDownFlipAnim()
    {
        // Flip the sword animation back to normal
        FlipLeft_x_Angle(0);
    }

    private void FlipLeft_x_Angle(int angle)
    {
        // Set the rotation of the slash animation to the angle passed in
        slashAnim.transform.rotation = Quaternion.Euler(angle, 0, 0);

        // Flip the slash animation if the player is facing left
        if (PlayerController.Instance.FacingLeft)
        {
            slashAnim.GetComponent<SpriteRenderer>().flipX = true;
        }
    }

    private void MouseFollowWithOffset()
    {
        // Get the mouse position
        Vector3 mousePos = Input.mousePosition;
        // Get the player screen point
        Vector3 playerScreenPoint = Camera.main.WorldToScreenPoint(PlayerController.Instance.transform.position);

        Vector3 playerPos = PlayerController.Instance.transform.position;

        // Get the angle between the player and the mouse position in degrees
        float angle = Mathf.Atan2(mousePos.y - playerPos.y, Mathf.Abs(mousePos.x - playerPos.x)) * Mathf.Rad2Deg;

        // If the mouse is on the left side of the player
        if (mousePos.x < playerScreenPoint.x)
        {
            // Set the rotation of the sword to the angle with an offset 
            ActiveWeapon.Instance.transform.rotation = Quaternion.Euler(0, 180, angle);
            weaponCollider.transform.rotation = Quaternion.Euler(0, 180, 0);
        }
        else
        {
            // Set the rotation of the sword to the angle with an offset the opposite way
            ActiveWeapon.Instance.transform.rotation = Quaternion.Euler(0, 0, angle);
            weaponCollider.transform.rotation = Quaternion.Euler(0, 0, 0);
        }
    }
}
