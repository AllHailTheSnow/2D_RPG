using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallax : MonoBehaviour
{
    [SerializeField] private float parallaxOffset = -0.15f;

    private Camera cam;
    private Vector2 startPos;
    private Vector2 travel => (Vector2)cam.transform.position - startPos;

    private void Awake()
    {
        // sets cam to the main camera
        cam = Camera.main;
    }

    private void Start()
    {
        // sets startPos to the current position of the object
        startPos = transform.position;
    }

    private void FixedUpdate()
    {
        // sets the position of the object to the startPos + the travel * the parallaxOffset
        transform.position = startPos + travel * parallaxOffset;
    }
}
