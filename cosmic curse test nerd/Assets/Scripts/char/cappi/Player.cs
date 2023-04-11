using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{ 
    public Transform transs;
    public static Rigidbody2D bodys;
    public Animator animators;

    public ParticleSystem dustSmall;
    public ParticleSystem dustBig;

    public InputAction playerControl;

    public AudioClip jumpeffect;

    [Header("Jumping/Dashing Settings")]
    [SerializeField] protected float jumpForce;
    [SerializeField] public float dashForce;
    [SerializeField] float walljumpForce;

    bool canMove = true;

    //main movements
    protected bool jumpInput;
    public static bool isGrounded;
    [HideInInspector]
    public static bool currentlyDashing;
    [HideInInspector]
    public static bool isAttacking;

    protected Vector3 rbVelocity;
    private float vectorVar;

    Vector2 horizInput;
    private float horizontal;

    public float fastFallSpeed;
    bool isfastFalling;


    public bool isFacingRight;

    [Header("Buffers")]
    protected float jumpBufferTime = 0.9f;
    protected float jumpBufferCounter;
    protected float coyoteTime = 0.38f;
    protected float coyoteTimeCounter;

    [Header("Speed Settings")]
    [SerializeField] public float speed;
    const float MaxSpeed = 19;
    const float AccelSpeed = 1;
    const float accelRate = 0.13f;

    private void OnEnable()
    {
        playerControl.Enable();
    }

    private void OnDisable()
    {
        playerControl.Disable();
    }

    // Start is called before the first frame update
    void Start()
    {
        transs = GetComponent<Transform>(); // takes the game object , get specific component and applies it to variable; 
        bodys = GetComponent<Rigidbody2D>();
        vectorVar = rbVelocity.x;
    }

    // Update is called once per frame
    void Update()
    {
        walk();
        Flip();

        stance();
        jumpingAnim();

        horizInput = playerControl.ReadValue<Vector2>();
        horizontal = horizInput.x;

        if (isGrounded)
        {
            isfastFalling = false;
            animators.SetBool("Fastfalling", false);
            coyoteTimeCounter = coyoteTime;

        }
        else if (!isGrounded)
        {
            coyoteTimeCounter -= Time.deltaTime;
        }
        if (Input.GetKeyDown(KeyCode.Z))
        {
            jumpInput = true;
            jumpBufferCounter = jumpBufferTime;
        }
        else { jumpBufferCounter -= Time.deltaTime; }

        if (Input.GetKeyUp(KeyCode.Z))
        {
            jumpInput = false;

        }


        if (Input.GetKeyDown(KeyCode.X) && currentlyDashing)
        {
            isAttacking = true;
            StartCoroutine(DashAttack());

        }

        if (Input.GetKey(KeyCode.DownArrow) && !isGrounded && !isfastFalling)
        {
            if (!pauseMenu.isPaused)
            {
                isfastFalling = true;
                fastFall();
                canMove = false;
            }


        }







    }
    void FixedUpdate()
    {
        Vector3 clampedVelocity = bodys.velocity;
        clampedVelocity.x = Mathf.Clamp(clampedVelocity.x, -MaxSpeed, MaxSpeed);
        bodys.velocity = clampedVelocity;

        if (isGrounded)
        {
            canMove = true;
        }

        if (canMove)
        {
            bodys.AddForce(transform.right * horizontal, ForceMode2D.Impulse);
        }
        else if (canMove && !isGrounded && speed < MaxSpeed)
        {
            bodys.AddForce(transform.right * horizontal * 0.4f, ForceMode2D.Impulse);
            bodys.velocity = Vector3.ClampMagnitude(bodys.velocity, speed);
        }
 

        if (jumpInput && isGrounded && coyoteTimeCounter > 0f && jumpBufferCounter > 0f && canMove)
        {
            Jump();
            coyoteTimeCounter = 0f;
            jumpBufferCounter = 0f;
        }
        if (!pauseMenu.isPaused)
        {
 
            IncreaseSpeed();

            if (horizontal == 0)
                while (speed > 1)
                {
                    speed -= AccelSpeed * accelRate;
                }




            if (isFacingRight && speed > 0 && horizontal < 0)
            {
                speed -= AccelSpeed * accelRate;
            }

            if (!isFacingRight && speed > 0 && horizontal > 0)
            {
                speed -= AccelSpeed * accelRate;
            }
        }

        void IncreaseSpeed()
        { 
            if (isGrounded && canMove)
            speed += AccelSpeed * accelRate;

            if (speed > MaxSpeed)
            {
                animators.speed = 1;
                speed = MaxSpeed;
                currentlyDashing = true;
            }
            if (speed > 5)
            {
                animators.speed = 1.25f;
            }
            if (speed > 10)
            {
                animators.speed = 1.75f;
            }


            if (bodys.velocity.y < 0)
            {
                bodys.gravityScale = bodys.gravityScale * 1.075f;
            }
            else
            {
                bodys.gravityScale = 3f;
            }


        }
    }
    void walk()
    {
        if (!pauseMenu.isPaused)
        {
            if (speed > 0 && !isAttacking && canMove)
            {


                animators.SetFloat("speed", Mathf.Abs(2));
                CreateDust();


            }

            if (speed == MaxSpeed && !isAttacking && canMove)
            {


                animators.SetFloat("speed", Mathf.Abs(4));
                CreateBigDust();


            }
        }
    }
    //animation stuff start
    void stance()
    {
        if (Input.GetAxis("Horizontal") == 0 && !isAttacking)
        {
            if (!pauseMenu.isPaused)
            {

                animators.speed = 1;

                animators.SetFloat("speed", Mathf.Abs(0));
                currentlyDashing = false;
                isfastFalling = false;

            }

        }
    }

    IEnumerator DashAttack()
    {
        if (isAttacking)
        {

            animators.SetBool("IsDashAttacking", true);
            animators.SetFloat("speed", Mathf.Abs(4));

        }
        yield return new WaitForSeconds(.5f);

        animators.SetBool("IsDashAttacking", false);
        isAttacking = false;

    }
    void jumpingAnim()
    {
        if (isGrounded)
        {
            animators.SetBool("IsJumping", false);

        }
        else if (!isGrounded)
        {
            animators.SetBool("IsJumping", true);

        }

    }
    //animation stuff end
    void Jump()
    {

        AudioSource.PlayClipAtPoint(jumpeffect, transform.position);

        bodys.AddForce(transform.up * jumpForce, ForceMode2D.Impulse);
 

        isGrounded = false;
        CreateDust();



    }

    public virtual void CreateDust()
    {
        dustSmall.Play();
    }

    public virtual void CreateBigDust()
    {
        dustBig.Play();
    }


    void Flip()
    {
        if (isFacingRight && horizontal < 0f || !isFacingRight && horizontal > 0f)
        {
            isFacingRight = !isFacingRight;
            Vector3 localScale = transs.localScale;
            localScale.x *= -1f;
            transform.localScale = localScale;
        }
    }

    void fastFall()
    {

        if (isfastFalling)
        {
            bodys.AddForce(-transform.up * fastFallSpeed, ForceMode2D.Impulse);
            animators.SetBool("Fastfalling", true);
            CreateBigDust();


        }
        else if (!isfastFalling)
        {

            animators.SetBool("Fastfalling", false);
            stance();
        }



    }


    private void OnCollisionEnter2D(Collision2D collision) // detects when the object collides with another object
    {
        if(collision.collider.tag == "Ground") // saying if the thing you're colliding with has the ground tag on it
        {
            for(int i=0; i < collision.contacts.Length; i++) // if any one of the collisions is on the ground
            {
                if (collision.contacts[i].normal.y > 0.5) 
                {
                    isGrounded = true;

                }
            }
        }
    }
}
