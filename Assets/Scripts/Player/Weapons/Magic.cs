using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Magic : MonoBehaviour
{
    [SerializeField] private float magicSpreadTime = 2f;

    private bool isGrowing = true;
    private float magicRange;
    private SpriteRenderer spriteRenderer;
    private CapsuleCollider2D capsuleCollider2D;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        capsuleCollider2D = GetComponent<CapsuleCollider2D>();
    }

    private void Start()
    {
        FaceMouse();
    }

    public void UpdateMagicRange(float magicRange)
    {
        this.magicRange = magicRange;
        StartCoroutine(IncreaseMagicLengthRoutine());
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //if the magic hits an indestructable object and is not a trigger
        if (collision.gameObject.GetComponent<Indestructable>() && !collision.isTrigger)
        {
            //stop the magic from growing
            isGrowing = false;
        }
    }

    private void FaceMouse()
    {
        Vector3 mousePos = Input.mousePosition;
        mousePos = Camera.main.ScreenToWorldPoint(mousePos);
        Vector2 dir = transform.position - mousePos;
        transform.right = -dir;
    }

    private IEnumerator IncreaseMagicLengthRoutine()
    {
        float timePassed = 0f;

        while(spriteRenderer.size.x < magicRange && isGrowing)
        {
            //add time to timePassed every frame
            timePassed += Time.deltaTime;
            //calculate the linear time value by getting timePassed and dividing it by magicSpreadTime
            float linearT = timePassed / magicSpreadTime;

            //set the size of the sprite to lerp starting at 1 and ending at magic range over linear time
            spriteRenderer.size = new Vector2(Mathf.Lerp(1f, magicRange, linearT), 1f);

            //set the size of the capsule collider to lerp starting at 1 and ending at magic range over linear time
            capsuleCollider2D.size = new Vector2(Mathf.Lerp(1f, magicRange, linearT), capsuleCollider2D.size.y);
            //set the offset of the capsule collider to lerp starting at 1 and ending at magic range over linear time
            capsuleCollider2D.offset = new Vector2((Mathf.Lerp(1f, magicRange, linearT)) / 2, capsuleCollider2D.offset.y);

            yield return null;
        }

        StartCoroutine(GetComponent<SpriteFade>().SlowFadeRoutine());
    }
}
