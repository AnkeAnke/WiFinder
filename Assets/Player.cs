using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[RequireComponent(typeof(Rigidbody2D))]
public class Player : MonoBehaviour
{
    public float MovementSpeed = 1.0f;
    public float JumpForce = 10.0f;
    public Transform GroundCheck = null;

    private bool jump = false;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    void Update()
    {
        var grounded = Physics2D.Linecast(transform.position, GroundCheck.position, 
                                          1 << LayerMask.NameToLayer("Ground"));

        if (Input.GetButtonDown("Jump") && grounded)
        {
            jump = true;
        } 
    }

    private void FixedUpdate()
    {
        var rigidBody = GetComponent<Rigidbody2D>();
        rigidBody.velocity = new Vector2(Input.GetAxis("Horizontal") * MovementSpeed, rigidBody.velocity.y);
        if (jump)
        {
            rigidBody.AddForce(Vector2.up * JumpForce);
            jump = false;
        }
    }
}
