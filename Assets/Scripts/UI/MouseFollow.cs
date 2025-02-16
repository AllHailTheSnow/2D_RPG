using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseFollow : MonoBehaviour
{
    private void Update()
    {
        FaceMouse();
    }

    private void FaceMouse()
    {
        // Get the mouse position
        Vector3 mousePos = Input.mousePosition;
        // Convert the mouse position to world space
        mousePos = Camera.main.ScreenToWorldPoint(mousePos);

        // Get the direction from the player to the mouse position
        Vector2 dir = transform.position - mousePos;

        // Set the rotation of the object to face the mouse position
        transform.right = -dir;
    }
}
