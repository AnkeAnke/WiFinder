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
            transform.parent = null;

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

    private void FixedUpdate()
    {
        var activeMover = _platformAttachedTo?.GetComponent<Mover>();
        if (activeMover != null)
            _rigidbody.position += activeMover.Velocity * Time.fixedDeltaTime;
        _rigidbody.velocity = new Vector2(Input.GetAxis("Horizontal") * MovementSpeed, _rigidbody.velocity.y);
        
        if (_jump)
        {
            var force = Vector2.up * JumpForce;
//            if (activeMover != null)
//                force += activeMover.Velocity * JumpForce; // uhm?
            _platformAttachedTo = null;
            _rigidbody.AddForce(force);
            _jump = false;
        }

        if (_rigidbody.velocity.x > 0.1f)
            GetComponent<SpriteRenderer>().flipX = false;
        else if (_rigidbody.velocity.x < -0.1f)
            GetComponent<SpriteRenderer>().flipX = true;
    }
}
