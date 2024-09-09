using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMovement3D : MonoBehaviour
{
    [SerializeField] CharacterController controller;
    public float speed = 12f;
    public float gravity = -10f;
    public float jumpHeight = 3f;

    [SerializeField] Transform groundCheck;
    [SerializeField] float groundDistace = 0.4f;
    [SerializeField] LayerMask groundMask;

    Vector3 velocity;
    bool grounded;
    // Update is called once per frame
    void Update()
    {
        grounded = Physics.CheckSphere(groundCheck.position, groundDistace, groundMask);

        if (grounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 move = transform.right * x + transform.forward * z;

        controller.Move(move * speed * Time.deltaTime);

        if(Input.GetKeyDown("space") && grounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }

        velocity.y += gravity * Time.deltaTime;

        controller.Move(velocity * Time.deltaTime);
    }
}
