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


    private Rigidbody2D rigidbody;
    
    // Start is called before the first frame update
    void Start()
    {
        rigidbody = GetComponent<Rigidbody2D>();
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

        if (Input.GetButtonDown("Use"))
        {
            var filter = new ContactFilter2D();
            filter.layerMask = 1 << LayerMask.NameToLayer("Router");
            filter.useLayerMask = true;
            filter.useTriggers = true;

            var overlappingColliders = new Collider2D[1];
            if (rigidbody.OverlapCollider(filter, overlappingColliders) > 0)
            {
                var routerObject = overlappingColliders[0].gameObject;
                var controller = routerObject.GetComponent<WifiController>();
                controller.ToggleRouterActive();
            }
        }
    }

    private void FixedUpdate()
    {
        rigidbody.velocity = new Vector2(Input.GetAxis("Horizontal") * MovementSpeed, rigidbody.velocity.y);
        if (jump)
        {
            rigidbody.AddForce(Vector2.up * JumpForce);
            jump = false;
        }

        if (rigidbody.velocity.x > 0.1f)
            GetComponent<SpriteRenderer>().flipX = false;
        else if (rigidbody.velocity.x < -0.1f)
            GetComponent<SpriteRenderer>().flipX = true;
    }
}
