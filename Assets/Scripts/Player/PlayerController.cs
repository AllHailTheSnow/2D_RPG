using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : Singleton<PlayerController>
{
    [SerializeField] private float moveSpeed = 1f;
    [SerializeField] private float dashSpeed = 4f;
    [SerializeField] private TrailRenderer dashTrail;
    [SerializeField] private Transform weaponCollider;
    [SerializeField] private Transform slashAnimSpawnPoint;

    //Creates a property for the facing left bool
    public bool FacingLeft { get { return facingLeft; } }

    //Creates a player controls object
    private PlayerControls playerControls;

    //Creates a vector2 for movement
    private Vector2 movement;

    //Creates a rigidbody2d for the player
    //private Rigidbody2D rb;

    //Creates an animator for the player
    private Animator myAnimator;

    //Creates a sprite renderer for the player
    private SpriteRenderer mySpriteRenderer;

    //Creates a knockback for the player
    private Knockback knockback;

    //Creates a bool for facing left and dashing
    private bool facingLeft, isDashing = false;

    //Creates a float for the starting move speed
    private float startingMoveSpeed;

    //Creates a matrix4x4 for the transformation matrix
    private Matrix4x4 transformationMatrix;

    protected override void Awake()
    {
        //Calls the base awake function
        base.Awake();

        //Sets the player controls to a new player controls object
        playerControls = new PlayerControls();

        //Sets the rigidbody to the rigidbody on the object
        //rb = GetComponent<Rigidbody2D>();

        //Sets the animator to the animator on the object
        myAnimator = GetComponent<Animator>();

        //Sets the sprite renderer to the sprite renderer on the object
        mySpriteRenderer = GetComponent<SpriteRenderer>();

        //Sets the knockback to the knockback on the object
        knockback = GetComponent<Knockback>();
    }

    private void Start()
    {
        playerControls.Combat.Dash.performed += _ => Dash();

        startingMoveSpeed = moveSpeed;
    }

    private void OnEnable()
    {
        //Enables the player controls
        playerControls.Enable();
    }

    private void Update()
    {
        //Calls the player input function
        PlayerInput();
    }

    private void FixedUpdate()
    {
        //Calls the move function
        Move();

        //Calls the player direction function
        PlayerDirection();
    }

    public Transform GetWeaponCollider()
    {
        //Returns the weapon collider
        return weaponCollider;
    }

    public Transform GetSlashAnimSpawnPoint()
    {
        //Returns the slash animation spawn point
        return slashAnimSpawnPoint;
    }

    private void PlayerInput()
    {
        //Sets the movement vector to the player input
        movement = playerControls.Movement.Move.ReadValue<Vector2>();

        myAnimator.SetFloat("MoveX", movement.x);
        myAnimator.SetFloat("MoveY", movement.y);
    }

    private void Move()
    {
        //Moves the player based on the movement vector
        //rb.MovePosition(rb.position + movement * (moveSpeed * Time.fixedDeltaTime));

        if(knockback.GetingKnockedback) { return; }

        //Sets the transformation matrix to a translation matrix based on the movement vector
        transformationMatrix = Matrix4x4.Translate(new Vector3(movement.x * moveSpeed * Time.fixedDeltaTime, movement.y * moveSpeed * Time.fixedDeltaTime, 0));

        //Sets the position of the player to the transformed position
        transform.position = transformationMatrix.MultiplyPoint3x4(transform.position);
    }

    private void PlayerDirection()
    {
        //Sets the mouse position to the input mouse position
        Vector3 mousePos = Input.mousePosition;
        //Sets the player screen point to the world to screen point of the player
        Vector3 playerScreenPoint = Camera.main.WorldToScreenPoint(transform.position);

        //If the mouse position is less than the player screen point, flip the sprite
        if (mousePos.x < playerScreenPoint.x)
        {
            mySpriteRenderer.flipX = true;
            facingLeft = true;
        }
        //Else, do not flip the sprite
        else
        {
            mySpriteRenderer.flipX = false;
            facingLeft = false;
        }
    }

    private void Dash()
    {
        //If the player is not dashing, dash
        if (!isDashing)
        {
            //Sets the player to dashing
            isDashing = true;
            //Sets the move speed to the dash speed
            moveSpeed *= dashSpeed;
            //Sets the dash trail to emitting
            dashTrail.emitting = true;
            //Starts the end dash routine
            StartCoroutine(EndDashRoutine());
        }
    }

    private IEnumerator EndDashRoutine()
    {
        //Sets the dash time and cooldown
        float dashTime = 0.2f;
        float dashCD = .25f;
        //Waits for the dash time
        yield return new WaitForSeconds(dashTime);
        //Sets the move speed back to the original speed
        moveSpeed = startingMoveSpeed;
        //Sets the dash trail to not emitting
        dashTrail.emitting = false;
        dashTrail.Clear();
        //Waits for the dash cooldown
        yield return new WaitForSeconds(dashCD);
        //Sets the player to not dashing
        isDashing = false;
    }
}

