using UnityEngine;
using System.Collections;
using Prime31;
using System;

public class DemoScene : MonoBehaviour
{

    public PlayerStats playerStats;

    // movement config
    public float gravity = -25f;
    public float walkSpeed;
    public float runSpeed;
    public static int player_damage;


    public float groundDamping = 20f; // how fast do we change direction? higher means faster
    public float inAirDamping = 5f;
    public float jumpHeight = 3f;

    [HideInInspector]
    private float normalizedHorizontalSpeed = 0;

    private CharacterController2D _controller;
    private Animator _animator;
    private RaycastHit2D _lastControllerColliderHit;
    private Vector3 _velocity;

    private bool isRunning = false;
    private bool iscombat;
    private bool isTransitioning = false;

    private void Start()
    {
        playerStats = GetComponent<PlayerStats>();
    }
    void Awake()
    {
        
        _animator = GetComponent<Animator>();
        _controller = GetComponent<CharacterController2D>();

        // listen to some events for illustration purposes
        _controller.onControllerCollidedEvent += onControllerCollider;
        _controller.onTriggerEnterEvent += onTriggerEnterEvent;
        _controller.onTriggerExitEvent += onTriggerExitEvent;
    }

    #region Event Listeners

    void onControllerCollider(RaycastHit2D hit)
    {
        // bail out on plain old ground hits cause they aren't very interesting
        if (hit.normal.y == 1f)
            return;
        // logs any collider hits if uncommented. It gets noisy, so it is commented out for the demo
        // Debug.Log("flags: " + _controller.collisionState + ", hit.normal: " + hit.normal);
    }

    void onTriggerEnterEvent(Collider2D col)
    {
        //Debug.Log("onTriggerEnterEvent: " + col.gameObject.name);
    }

    void onTriggerExitEvent(Collider2D col)
    {
        //Debug.Log("onTriggerExitEvent: " + col.gameObject.name);
    }

    #endregion

    // the Update loop contains a very simple example of moving the character around and controlling the animation
    void Update()
    {
        walkSpeed = (float)playerStats.playerSpeed;
        runSpeed = 2f * walkSpeed;
        player_damage = (int)playerStats.playerDamage;
        Vector3 mousePosition = Input.mousePosition;
        Vector3 worldMousePosition = Camera.main.ScreenToWorldPoint(mousePosition);
        worldMousePosition.z = 0f; // Make sure the z-coordinate is appropriate for your scene

        

        if (worldMousePosition.x < transform.position.x)
        {

            //Debug.Log("mouse look left");
            if (transform.localScale.x > 0f)
                 transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);

        }
        else if (worldMousePosition.x > transform.position.x)
        {
            // Mouse is on the right, face right
           //Debug.Log("mouse look right");
            
            if (transform.localScale.x < 0f)
                 transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);


        }


        if (_controller.isGrounded)
            _velocity.y = 0;

        bool isWalking = false;

        if (Input.GetMouseButtonDown(0) && iscombat == true)
        {
            
            
            //_animator.Play(Animator.StringToHash("Body_attack1"));
            _animator.SetTrigger("Body_attack1");

        }



        if (Input.GetKey(KeyCode.A) && !isTransitioning)
        {

            if (Input.GetKey(KeyCode.LeftShift))
            {
                isRunning = true;
                isWalking = true;

            }
        }

        if (Input.GetKey(KeyCode.D) && !isTransitioning)
        {

            if (Input.GetKey(KeyCode.LeftShift))
            {
                isRunning = true;
                isWalking = true;

            }
        }

        if (Input.GetKeyDown(KeyCode.Q) && !iscombat && !isTransitioning)
        {
            combat();
        }
        if (Input.GetKeyDown(KeyCode.Q) && iscombat && !isTransitioning)
        {
            combat_off();
        }




        if (!isWalking)
        {
            isRunning = false;
        }

        if (Input.GetKey(KeyCode.D) && !isTransitioning)
        {
            normalizedHorizontalSpeed = 1;
            //if (transform.localScale.x < 0f)
            //    transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);

            if (_controller.isGrounded && !isTransitioning )
            {
                if (iscombat == true)
                {
                    _animator.Play(Animator.StringToHash(isRunning ? "combat_run" : "combat_walk"));

                }
                if (iscombat == false)
                {
                    _animator.Play(Animator.StringToHash(isRunning ? "run" : "walk"));

                }

            }
        }
        else if (Input.GetKey(KeyCode.A) && !isTransitioning)
        {
            normalizedHorizontalSpeed = -1;
            //if (transform.localScale.x > 0f)
            //    transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);

            if (_controller.isGrounded && !isTransitioning)
            {

                if (iscombat == true)
                {
                    _animator.Play(Animator.StringToHash(isRunning ? "combat_run" : "combat_walk"));

                }
                if (iscombat == false)
                {
                    _animator.Play(Animator.StringToHash(isRunning ? "run" : "walk"));

                }

            }
        }
        else
        {
            normalizedHorizontalSpeed = 0;

            if (_controller.isGrounded && !iscombat && !isTransitioning)
            {
                _animator.Play(Animator.StringToHash("idle"));  
            }
            if (_controller.isGrounded && iscombat == true && !isTransitioning)
            {
                _animator.Play(Animator.StringToHash("combat_idle"));
            }
            if (_controller.isGrounded && isTransitioning == true)
            {
                _animator.Play(Animator.StringToHash("combat"));
            }
        }

        // we can only jump while grounded
        if (_controller.isGrounded && Input.GetKeyDown(KeyCode.Space))
        {
            if (iscombat == true)
            {
                _animator.Play(Animator.StringToHash(isRunning ? "combat_jump_run" : "combat_jump"));

            }
            else
            {
               _animator.Play(Animator.StringToHash(isRunning ? "jump_run" : "jump"));

            }

            
            //_animator.speed = 0.05f; // Adjust this speed as needed
            _velocity.y = Mathf.Sqrt(2f * jumpHeight * -gravity);
            
        }



        // apply horizontal speed smoothing it. Don't really do this with Lerp. Use SmoothDamp or something that provides more control
        var smoothedMovementFactor = _controller.isGrounded ? groundDamping : inAirDamping; // how fast do we change direction?
        float currentSpeed = isRunning ? runSpeed : walkSpeed;
        _velocity.x = Mathf.Lerp(_velocity.x, normalizedHorizontalSpeed * currentSpeed, Time.deltaTime * smoothedMovementFactor);

        // apply gravity before moving
        _velocity.y += gravity * Time.deltaTime;

        // if holding down bump up our movement amount and turn off one-way platform detection for a frame.
        // This lets us jump down through one-way platforms
        if (_controller.isGrounded && Input.GetKey(KeyCode.S))
        {
            _velocity.y *= 3f;
            _controller.ignoreOneWayPlatformsThisFrame = true;
        }

        _controller.move(_velocity * Time.deltaTime);

        // grab our current _velocity to use as a base for all calculations
        _velocity = _controller.velocity;
    }

    private void combat()
    {
        Debug.Log("combat transition start");

        isTransitioning = true; // Set the flag to prevent further transitions       
        StartCoroutine(ResetCombatTransition());

    }

    private void combat_off()
    {
        iscombat = false;




    }

    IEnumerator ResetCombatTransition()
    {
        yield return new WaitForSeconds(1.5f); // Wait for 2 seconds
        Debug.Log("combat transition off");

        isTransitioning = false; // Reset the transition flag
        iscombat = true;

    }

}
