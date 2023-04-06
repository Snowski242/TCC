using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Playercontroller : MonoBehaviour
{ 
    public Transform trans;
    public Rigidbody2D body;
    public Animator animator;
    private BoxCollider2D bb;
    public ParticleSystem dustSmall;
    public ParticleSystem dustBig;

    public InputAction playerControl;

    public AudioClip jumpeffect;

    [Header("Jumping/Dashing Settings")]
    [SerializeField] float jumpForce;
    [SerializeField] public float dashForce;
    [SerializeField] float walljumpForce;

    private Vector2 colliderSize;
    [SerializeField] private float slopeCheckDistance;
    private float slopeDownAngle;
    private Vector2 slopeNormalPerp;
    private bool isOnSlope;
    private float slopeDownAngleOld;

    //main movements
    bool jumpInput;
    public static bool isGrounded;
    bool isMoving;
    [HideInInspector]
   public static bool currentlyDashing;
    [HideInInspector]
    public static bool isAttacking;

    Vector2 horizInput;
    private float horizontal;

    [Header("Buffers")]
    private float jumpBufferTime = 0.5f;
    private float jumpBufferCounter;
    private float coyoteTime = 0.38f;
    private float coyoteTimeCounter;

    [Header("Fastfall")]
//fast falling
    public float fastFallSpeed;
    bool isfastFalling;

    //wall sliding

    [Header("Speed Settings")]
    [SerializeField] public float speed;
    const float MaxSpeed = 18;
    const float AccelSpeed = 1;
    const float accelRate = 0.19f;




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
        bb = GetComponent<BoxCollider2D>();

        colliderSize = bb.size;
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
            body.velocity = new Vector2(horizontal * speed, body.velocity.y * Time.deltaTime);
            IncreaseSpeed();

            if (body.velocity.x == 0 && horizontal == 0)
                speed = 0;

            if (isFacingRight && speed > 0 && Input.GetAxis("Horizontal") < 0)
            {
                speed = 0;
            }

            if (!isFacingRight && speed > 0 && Input.GetAxis("Horizontal") > 0)
            {
                speed = 0;
            }
        }

        void IncreaseSpeed()
        {
            speed += AccelSpeed * accelRate;

            if (speed > MaxSpeed)
            {
                speed = MaxSpeed;
            }

        }
        if (jumpInput && isGrounded && coyoteTimeCounter > 0f && jumpBufferCounter > 0f)
        {
            jump();
            coyoteTimeCounter = 0f;
            jumpBufferCounter = 0f;
        }






        SlopeCheck();

        if (body.velocity.y < 0)
        {
            body.gravityScale = body.gravityScale * 1.075f;
        }
        else
        {
            body.gravityScale = 2.5f;
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
            if (speed > 0 && !currentlyDashing && !isAttacking)
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

        if (Input.GetAxis("Horizontal") > 0 && Input.GetKey(KeyCode.LeftShift))
        {
            currentlyDashing = true;
        }
        if (Input.GetAxis("Horizontal") < 0 && Input.GetKey(KeyCode.LeftShift))
        {
            currentlyDashing = true;
        }
    }
    //animation stuff start
    void stance()
    {
        if (Input.GetAxis("Horizontal") == 0 && !isAttacking)
        {
            if (!pauseMenu.isPaused)
            {

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
    void jump()
    {

            AudioSource.PlayClipAtPoint(jumpeffect, transform.position);

        body.AddForce(transform.up * jumpForce, ForceMode2D.Impulse);
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

    void CreateDust()
    {
        dustSmall.Play();
    }

    void CreateBigDust()
    {
        dustBig.Play();
    }

    private void SlopeCheck()
    {
        Vector2 checkPos = transform.position - new Vector3(0.0f, colliderSize.y / 2);

        SlopeCheckVertical(checkPos);
    }

    private void SlopeCheckerHorizontal(Vector2 checkPos)
    {

    }

    private void SlopeCheckVertical(Vector2 checkPos)
    {
        RaycastHit2D hit = Physics2D.Raycast(checkPos, Vector2.down, slopeCheckDistance, groundLayer);

        if (hit)
        {
            slopeNormalPerp = Vector2.Perpendicular(hit.normal).normalized;

            slopeDownAngle = Vector2.Angle(hit.normal, Vector2.up);

            if (slopeDownAngle != slopeDownAngleOld)
            {
                isOnSlope = true;
            }
            slopeDownAngleOld = slopeDownAngle;
            Debug.DrawRay(hit.point, slopeNormalPerp, Color.red);
            Debug.DrawRay(hit.point, hit.normal, Color.blue);
        }
    }
}
