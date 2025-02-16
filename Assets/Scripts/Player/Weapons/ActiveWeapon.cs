using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActiveWeapon : Singleton<ActiveWeapon>
{
    public MonoBehaviour CurrentActiveWeapon { get; private set; }

    private PlayerControls playerControls;

    private bool attackButtonDown, isAttacking = false;

    private float timeBetweenAttacks;

    protected override void Awake()
    {
        base.Awake();

        // Get the player controls object
        playerControls = new PlayerControls();
    }

    private void OnEnable()
    {
        // Enable the player controls
        playerControls.Enable();
    }

    private void Start()
    {
        // Subscribe to the attack input
        playerControls.Combat.Attack.started += _ => StartAttcking();

        // Subscribe to the attack input cancel
        playerControls.Combat.Attack.canceled += _ => StopAttacking();

        AttackCooldown();
    }

    public void Update()
    {
        //call the attack function
        Attack();
    }

    public void NewWeapon(MonoBehaviour newWeapon)
    {
        //set the current active weapon to the new weapon
        CurrentActiveWeapon = newWeapon;
        //call the attack cooldown function
        AttackCooldown();
        //set the time between attacks to the weapon cooldown
        timeBetweenAttacks = (CurrentActiveWeapon as IWeapon).GetWeaponInfo().weaponCooldown;
    }

    public void WeaponNull()
    {
        //set the current active weapon to null
        CurrentActiveWeapon = null;
    }

    private void AttackCooldown()
    {
        //set the is attacking bool to true
        isAttacking = true;
        //stop all coroutines
        StopAllCoroutines();
        //start the time between attacks routine
        StartCoroutine(TimeBetweenAttacksRoutine());
    }

    private IEnumerator TimeBetweenAttacksRoutine()
    {
        //wait for the time between attacks
        yield return new WaitForSeconds(timeBetweenAttacks);
        //set the is attacking bool to false
        isAttacking = false;
    }

    private void StartAttcking()
    {
        //set the attack button down to true
        attackButtonDown = true;
    }

    private void StopAttacking()
    {
        //set the attack button down to false
        attackButtonDown = false;
    }

    private void Attack()
    {
        //if the attack button is down and the player is not attacking
        if (attackButtonDown && !isAttacking && CurrentActiveWeapon)
        {
            //call the attack function on the current active weapon
            AttackCooldown();
            (CurrentActiveWeapon as IWeapon).Attack();
        }
    }
}

