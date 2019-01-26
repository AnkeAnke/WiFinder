using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[RequireComponent(typeof(Rigidbody2D))]
public class Player : MonoBehaviour
{
    public float MovementSpeed = 1.0f;
    public float JumpForce = 10.0f;
    public Transform GroundCheck1 = null;
    public Transform GroundCheck2 = null;

    private bool jump = false;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    void Update()
    {
        var groundLayerMask = 1 << LayerMask.NameToLayer("Ground");
        var grounded = Physics2D.Linecast(transform.position, GroundCheck1.position, groundLayerMask) ||
                       Physics2D.Linecast(transform.position, GroundCheck2.position, groundLayerMask);

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

        if (rigidBody.velocity.x > 0.1f)
            GetComponent<SpriteRenderer>().flipX = false;
        else if (rigidBody.velocity.x < -0.1f)
            GetComponent<SpriteRenderer>().flipX = true;
    }
}
