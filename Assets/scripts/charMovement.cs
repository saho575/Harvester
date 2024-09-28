using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using Unity.Netcode;
using UnityEngine;

public class charMovement : NetworkBehaviour
{
    // Start is called before the first frame update
    #region MOVEMENT ÝÇÝN
    private CharacterController controller;
    public float speed = 12f;
    private float gravity = -9.81f;
    Vector3 velocity;
    private float jumpHeigt = 3f;
    public Transform groundCheck;
    private float groundDistance = 0.4f;
    public LayerMask groundLayerMask;
    public bool isgrounded;
    #endregion

    public Camera cam;
    private float mouseSensivity = 150;
    float xRotation = 0;
    void Start()
    {
        
        Cursor.lockState = CursorLockMode.Locked;
        controller =GetComponent<CharacterController>();
        if (IsOwner)
        {
            cam.gameObject.SetActive(true);

        }
    }

    private void FixedUpdate()
    {
        if (!IsOwner) return;
        move();
        look();
    }
    

    private void move()
    {
        isgrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundLayerMask);

        if (isgrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 Move = transform.right * x + transform.forward * z;

        controller.Move(Move * speed * Time.deltaTime);

        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);

        if (Input.GetKeyDown("space") && isgrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeigt * -2f * gravity);
        }
        if (Input.GetKey(KeyCode.LeftShift) && isgrounded)
        {
            speed = 20;
        }
        else
        {
            speed = 12f;
        }
    }

    private void look()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensivity * Time.deltaTime;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90, 90);

        cam.transform.localRotation = Quaternion.Euler(xRotation, 0, 0);
        transform.Rotate(Vector3.up * mouseX);
    }
}
