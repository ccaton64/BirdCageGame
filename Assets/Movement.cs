using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    // Animation set on player game object
    Animation anim;

    // Globals for move direction and velocity
    Vector3 direction;
    float fVel = 1.2f;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animation>();
        anim.Play("Idle");
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.W))
        {
            direction = transform.forward * (fVel * Time.deltaTime);
            anim.Play("Crouched Walking");
        }

        if (Input.GetKeyUp(KeyCode.W))
        {
            anim.Play("Idle");
        }

        if (Input.GetKey(KeyCode.Space))
        {
            anim.Play("Jumping");
        }

        if (anim.isPlaying == false)
        {
            anim.Play("Idle");
        }
    }
}
