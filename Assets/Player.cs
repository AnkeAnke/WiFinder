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

    private bool _jump = false;
    private bool _isGrounded = false;
    private Vector3 _spawnPosition;
    private Transform _platformAttachedTo;

    private Rigidbody2D _rigidbody;
    
    // Start is called before the first frame update
    void Start()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _spawnPosition = transform.position;
    }

    void Update()
    {
        var groundLayerMask = 1 << LayerMask.NameToLayer("Ground");
        var cast1 = Physics2D.Linecast(transform.position, GroundCheck1.position, groundLayerMask);
        var cast2 = Physics2D.Linecast(transform.position, GroundCheck2.position, groundLayerMask);

        if (cast1 || cast2)
        {
            _isGrounded = true;
            if (Input.GetButtonDown("Jump"))
            {
                _jump = true;
            }
            else
            {
                _platformAttachedTo = (cast1 ? cast1 : cast2).transform;
            }
        }
        else
            _isGrounded = false;

        if (Input.GetButtonDown("Use"))
        {
            var filter = new ContactFilter2D();
            filter.layerMask = 1 << LayerMask.NameToLayer("Router");
            filter.useLayerMask = true;
            filter.useTriggers = true;

            var overlappingColliders = new Collider2D[1];
            if (_rigidbody.OverlapCollider(filter, overlappingColliders) > 0)
            {
                var routerObject = overlappingColliders[0].gameObject;
                var controller = routerObject.GetComponent<WifiController>();
                controller.ToggleRouterActive();
            }
        }
    }

    public void Reset()
    {
        transform.position = _spawnPosition;
    }

    private void FixedUpdate()
    {
        var activeMover = _platformAttachedTo?.GetComponent<Mover>();
        if (activeMover != null)
            _rigidbody.position += activeMover.Velocity * Time.fixedDeltaTime;
        _rigidbody.velocity = new Vector2(Input.GetAxis("Horizontal") * MovementSpeed, _rigidbody.velocity.y);

        var animator = GetComponent<Animator>();
        animator.SetBool("walk", Input.GetAxis("Horizontal") != 0.0f);
        if (_jump)
        {
            animator.SetTrigger("jump");
            var force = Vector2.up * JumpForce;
//            if (activeMover != null)
//                force += activeMover.Velocity * JumpForce; // uhm?
            _platformAttachedTo = null;
            _rigidbody.AddForce(force);
            _jump = false;
        }

        if (_rigidbody.velocity.x > 0.1f)
            transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        else if (_rigidbody.velocity.x < -0.1f)
            transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
    }
}
