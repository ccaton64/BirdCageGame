using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public CharacterController chrCont;

    public float speed = 2f;
    public float gravity = -9.81f;
    public float jumpHeight = 3;
    
    Vector3 velocity;

    [SerializeField]
    bool isGrounded;
    bool isJumping;
    bool isMoving;
    bool isSprinting;

    public float turnSmoothTime = 0.1f;
    float turnSmoothVelocity;

    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask groundLayerMask;

    Animation anim;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        anim = GetComponent<Animation>();
        anim.Play("Idle");
    }

    void Update()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundLayerMask);

        if(isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }


        float horizonatal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        Vector3 direction = new Vector3(horizonatal, 0, vertical).normalized;

        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2 * gravity);
            anim.Play("Jumping");
        }

        velocity.y += gravity * Time.deltaTime;
        chrCont.Move(velocity * Time.deltaTime);

        if (direction.magnitude >= 0.1f)
        {
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);

            chrCont.Move(direction * speed * Time.deltaTime);
        }

        if (anim.isPlaying == false)
        {
            anim.Play("Idle");
        }


    }
}
