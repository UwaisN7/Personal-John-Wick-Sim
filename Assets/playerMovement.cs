using UnityEngine;

public class SimpleMovement3D : MonoBehaviour
{
    // Movement speed multiplier editable in the Inspector
    public float speed = 5.0f;

    void Update()
    {
        // Get input from keyboard (Values range from -1 to 1)
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        // Calculate direction based on player input
        Vector3 movementVector = new Vector3(horizontalInput, 0, verticalInput);

        // Move the object relative to world space smoothly over time
        transform.Translate(movementVector * speed * Time.deltaTime, Space.World);
    }
}