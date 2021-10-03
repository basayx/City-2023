using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuCameraMovement : MonoBehaviour
{
    public DynamicJoystick Joystick;
    public float Sensitivity = 1f;
    public float MovementSpeed = 1f;
    Vector3 direction;
    public Vector2 XandZLimits = new Vector2(50f, 50f);

    private void Update()
    {
        if (GameManager.Instance.GameState)
            return;

        if (Input.GetMouseButton(0))
        {
            direction = new Vector3(Joystick.Direction.x, 0, Joystick.Direction.y) * Sensitivity;
            if (direction.magnitude > 0.05f)
            {
                transform.position = Vector3.MoveTowards(transform.position, transform.position + direction, MovementSpeed * Time.deltaTime);
                transform.position = new Vector3(Mathf.Clamp(transform.position.x, -XandZLimits.x, XandZLimits.x), transform.position.y, Mathf.Clamp(transform.position.z, -XandZLimits.y, XandZLimits.y));
            }
        }
    }
}
