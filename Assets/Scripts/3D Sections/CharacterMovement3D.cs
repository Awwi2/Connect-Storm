using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMovement3D : MonoBehaviour
{
    [SerializeField] CharacterController controller;
    public float speed = 12f;
    public float gravity = -10f;
    public float jumpHeight = 3f;
    public bool lockMovement = false;

    [SerializeField] Transform groundCheck;
    [SerializeField] float groundDistace = 0.4f;
    [SerializeField] LayerMask groundMask;
    [SerializeField] LayerMask rayMask;

    Vector3 velocity;
    bool grounded;
    // Update is called once per frame
    void Update()
    {
        if (!lockMovement)
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

            if (Input.GetKeyDown("space") && grounded)
            {
                velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
            }

            velocity.y += gravity * Time.deltaTime;

            controller.Move(velocity * Time.deltaTime);

            if (Input.GetKeyDown("e"))
            {
                if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.TransformDirection(Vector3.forward), out RaycastHit hit, 3f, rayMask))
                {
                    if (hit.transform.gameObject.layer == 6)
                    {
                        hit.transform.gameObject.GetComponent<CharacterInteraction>().PlayDialogue();
                    }
                }
            }
        }
    }
}
