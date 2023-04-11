using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Playercontroller : MonoBehaviour
{
    public Transform trans;
    public Rigidbody2D body;
    public Animator animator;

    public ParticleSystem dustSmall;
    public ParticleSystem dustBig;

    public InputAction playerControl;

    public AudioClip jumpeffect;

    [Header("Jumping/Dashing Settings")]
    [SerializeField] protected float jumpForce;
    [SerializeField] public float dashForce;
    [SerializeField] float walljumpForce;


    //main movements
    protected bool jumpInput;
    public static bool isGrounded;
    [HideInInspector]
   public static bool currentlyDashing;
    [HideInInspector]
    public static bool isAttacking;

    protected Vector3 vertVelocity;
    Vector2 horizInput;
    private float horizontal;

    [Header("Buffers")]
    protected float jumpBufferTime = 0.5f;
    protected float jumpBufferCounter;
    protected float coyoteTime = 0.38f;
    protected float coyoteTimeCounter;

    [Header("Fastfall")]
//fast falling
    public float fastFallSpeed;
    bool isfastFalling;

    //wall sliding

    [Header("Speed Settings")]
    [SerializeField] public float speed;
    const float MaxSpeed = 18;
    const float AccelSpeed = 1;
    const float accelRate = 0.14f;





    public bool isFacingRight;

    [Header("Wall Settings")]
    [SerializeField] private Transform wallCheck;
    [SerializeField] private LayerMask wallLayer;
    [SerializeField] private LayerMask groundLayer;


    // Start is called before the first frame update
    void Start()
    {
        trans = GetComponent<Transform>(); // takes the game object , get specific component and applies it to variable; 
        body = GetComponent<Rigidbody2D>();

    }

    private void OnEnable()
    {
        playerControl.Enable();
    }

    private void OnDisable()
    {
        playerControl.Disable();
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
            animator.SetBool("Fastfalling", false);
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
        if (Input.GetKeyUp(KeyCode.Z))
        {
            jumpInput = false;
            jumpBufferCounter -= Time.deltaTime;
        }

        if (Input.GetKeyDown(KeyCode.X) && currentlyDashing)
        {
            isAttacking = true;

        }

        if (Input.GetKey(KeyCode.DownArrow) && !isGrounded && !isfastFalling)
        {
            if (!pauseMenu.isPaused)
            {
                isfastFalling = true;
                fastFall();
            }


        }









    }
    void FixedUpdate() 
    {
        if (!pauseMenu.isPaused)
        {
   
            IncreaseSpeed();

            if (body.velocity.x == 0 && horizontal == 0)
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
            speed += AccelSpeed * accelRate;

            if (speed > MaxSpeed)
            {
                animator.speed = 1;
                speed = MaxSpeed;
            }
            if (speed > 5)
            {
                animator.speed = 1.25f;
            }
            if (speed > 10)
            {
                animator.speed = 1.75f;
            }

        }









        if (body.velocity.y < 0)
        {
            body.gravityScale = body.gravityScale * 1.075f;
        }
        else
        {
            body.gravityScale = 3f;
        }

        if (jumpInput && isGrounded && coyoteTimeCounter > 0f && jumpBufferCounter > 0f)
        {
            Jump();
            coyoteTimeCounter = 0f;
            jumpBufferCounter = 0f;
        }



    }



    void fastFall()
    {

        if(isfastFalling)
        {
            body.AddForce(-transform.up * fastFallSpeed, ForceMode2D.Impulse);
            animator.SetBool("Fastfalling", true);
            CreateBigDust();


        }
        else if (!isfastFalling)
        {

            animator.SetBool("Fastfalling", false);
            stance();
        }



    }
    void walk()
    {
        if (!pauseMenu.isPaused)
        {
            if (speed > 0 && !isAttacking)
            {


                animator.SetFloat("speed", Mathf.Abs(2));
                CreateDust();


            }

            if (speed == MaxSpeed && !isAttacking)
            {


                animator.SetFloat("speed", Mathf.Abs(4));
                CreateBigDust();


            }
        }



        //IEnumerator DashAttack()
        //{
        //    if (isAttacking)
        //    {

        //        animator.SetBool("IsDashAttacking", true);
        //        animator.SetFloat("speed", Mathf.Abs(4));

        //    }
        //    yield return new WaitForSeconds(.5f);

        //    animator.SetBool("IsDashAttacking", false);
        //    isAttacking = false;

        //}


        //dash

    }
    //animation stuff start
    void stance()
    {
        if (Input.GetAxis("Horizontal") == 0 && !isAttacking)
        {
            if (!pauseMenu.isPaused)
            {

                animator.speed = 1;

                animator.SetFloat("speed", Mathf.Abs(0));
            currentlyDashing = false;
                isfastFalling = false;

            }

        }
    }

    void jumpingAnim()
    {
        if (isGrounded)
        {
            animator.SetBool("IsJumping", false);

        }
        else if (!isGrounded)
        {
            animator.SetBool("IsJumping", true);

        }

    }
    //animation stuff end

    void Jump()
    {

        AudioSource.PlayClipAtPoint(jumpeffect, transform.position);

        //body.AddForce(transform.up * jumpForce, ForceMode2D.Impulse);
        trans.position += transform.up * Time.deltaTime * jumpForce;

        isGrounded = false;
        CreateDust();

    }




    void Flip()
    {
        if(isFacingRight && horizontal < 0f || !isFacingRight && horizontal > 0f)
        {
            isFacingRight = !isFacingRight;
            Vector3 localScale = trans.localScale;
            localScale.x *= -1f;
            transform.localScale = localScale;
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

   public virtual void CreateDust()
    {
        dustSmall.Play();
    }

    public virtual void CreateBigDust()
    {
        dustBig.Play();
    }

    
}
