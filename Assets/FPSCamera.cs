using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FPSCamera : MonoBehaviour
{
    [SerializeField] Transform playerCamera = null;
    [SerializeField] float mouseSensitivity = 3.5f;
    [SerializeField] float walkSpeed = 6.0f;
    [SerializeField] float gravity = -13.0f;
    [SerializeField] float jumpHeight = 3f;
    [SerializeField] bool lookCursor = true;

    private float cameraPitch = 0.0f;
    private CharacterController controller = null;

    Vector3 velocity;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        if (lookCursor)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }

    void Update()
    {
        updateMouseLook();
        updateMovement();
    }

    void updateMouseLook()
    {
        Vector2 currentMouseDelta = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));

        cameraPitch -= currentMouseDelta.y * mouseSensitivity;
        cameraPitch = Mathf.Clamp(cameraPitch, -90f, 90f);

        playerCamera.localEulerAngles = Vector3.right * cameraPitch;
        transform.Rotate(Vector3.up * currentMouseDelta.x * mouseSensitivity);
    }

    void updateMovement()
    {
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical"); // Corrected from 'y' to 'z'

        if (controller.isGrounded)
            velocity.y = 0f;

        velocity.y += gravity * Time.deltaTime;

        if (controller.isGrounded && Input.GetButton("Jump"))
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }

        Vector3 move = (transform.right * x) + (transform.forward * z) + (Vector3.up * velocity.y);

        controller.Move(move * Time.deltaTime * walkSpeed);
    }
}
