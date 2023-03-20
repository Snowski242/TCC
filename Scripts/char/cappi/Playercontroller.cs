using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Playercontroller : MonoBehaviour
{ 
    public Transform trans;
    public Rigidbody2D body;
    public Animator animator;
    private BoxCollider2D bb;
    public ParticleSystem dustSmall;
    public ParticleSystem dustBig;

    public AudioClip jumpeffect;

    [SerializeField] public float speed; 
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

    bool CanWallJump => Physics2D.Raycast(transform.position, isLeft ? Vector2.right : Vector2.left, 0.55f);
    Vector2 WallJumpForce => (isLeft ? -5 : 5) * transform.right + transform.up * walljumpForce;

    private float jumpBufferTime = 0.5f;
    private float jumpBufferCounter;

    //coyote
    private float coyoteTime = 0.35f;
    private float coyoteTimeCounter;

//fast falling
    public float fastFallSpeed;
    bool isfastFalling;

    //wall sliding
    [HideInInspector]
    public static bool IsWallSliding;
    private float wallSlidingSpeed = 1.5f;

    //wall jumping (courtesy of bendux on yt)
    [HideInInspector]
    public static bool isWallJumping;
    [HideInInspector]
    public bool isLeft;
    private float wallJumpingDirection;
    private float wallJumpingTime = 0.2f;
    private float WallJumpingCounter;
    private float WallJumpingDuration = 0.4f;
    private Vector2 wallJumpingPower = new Vector2(8f, 16f);

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

    // Update is called once per frame
    void Update()
    {
        walk();
        stance();
        jumpingAnim();

        if(isGrounded)
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




        WallSlide();
        WallJump();




    }
    void FixedUpdate() 
    {
        if(jumpInput && isGrounded && coyoteTimeCounter > 0f && jumpBufferCounter > 0f)
        {
            jump();
            coyoteTimeCounter = 0f;
            jumpBufferCounter = 0f;
        }

        SlopeCheck();

        if (body.velocity.y < 0)
        {
            body.gravityScale = body.gravityScale * 1.12f;
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
        if (Input.GetKey(KeyCode.RightArrow) && !currentlyDashing && !isAttacking)
        {
            if (!pauseMenu.isPaused)
            {

                trans.position += transform.right * Time.deltaTime * speed;
                

                trans.rotation = Quaternion.Euler(0, 0, 0);

                animator.SetFloat("speed", Mathf.Abs(2));
                isLeft = false;
                CreateDust();
            }

        }
        if (Input.GetKey(KeyCode.LeftArrow) && !currentlyDashing && !isAttacking)
        {
            if (!pauseMenu.isPaused)
            {

                trans.position += transform.right * Time.deltaTime * speed;
                trans.rotation = Quaternion.Euler(0, 180, 0); //change y rotation to 180
                animator.SetFloat("speed", Mathf.Abs(2));
                isLeft = true;
                CreateDust();
            }

        }

        if (Input.GetKey(KeyCode.RightArrow) && currentlyDashing && !isWallJumping && !IsWallSliding && !isAttacking)
        {
            if (!pauseMenu.isPaused)
            {
                trans.position += transform.right * Time.deltaTime * speed * dashForce;
                trans.rotation = Quaternion.Euler(0, 0, 0);

                animator.SetFloat("speed", Mathf.Abs(4));
                isLeft = false;
                CreateBigDust();
            }


        }
        if (Input.GetKey(KeyCode.LeftArrow) && currentlyDashing && !isWallJumping && !IsWallSliding && !isAttacking)
        {
            if (!pauseMenu.isPaused)
            {
                trans.position += transform.right * Time.deltaTime * speed * dashForce;
                trans.rotation = Quaternion.Euler(0, 180, 0);

                animator.SetFloat("speed", Mathf.Abs(4));
                isLeft = true;
                CreateBigDust();
            }


        }

        if (Input.GetKey(KeyCode.RightArrow) && currentlyDashing && isAttacking && !isWallJumping && !IsWallSliding)
        {
            if (!pauseMenu.isPaused)
            {
                trans.position += transform.right * Time.deltaTime * speed * 3;
                trans.rotation = Quaternion.Euler(0, 0, 0);

                animator.SetFloat("speed", Mathf.Abs(4));
                animator.SetBool("IsDashAttacking", true);
                StartCoroutine(DashAttack());
                isLeft = false;
                CreateBigDust();
            }
        }

        if (Input.GetKey(KeyCode.LeftArrow) && currentlyDashing && isAttacking && !isWallJumping && !IsWallSliding)
        {
            if (!pauseMenu.isPaused)
            {
                trans.position += transform.right * Time.deltaTime * speed * 3;
                trans.rotation = Quaternion.Euler(0, 180, 0);

                animator.SetFloat("speed", Mathf.Abs(4));
                animator.SetBool("IsDashAttacking", true);
                StartCoroutine(DashAttack());
                isLeft = true;
                CreateBigDust();
            }
        }

        IEnumerator DashAttack()
        {
            if (isAttacking)
            {

                animator.SetBool("IsDashAttacking", true);
                animator.SetFloat("speed", Mathf.Abs(4));

            }
            yield return new WaitForSeconds(.5f);

            animator.SetBool("IsDashAttacking", false);
            isAttacking = false;

        }


        //dash

        if (Input.GetKey(KeyCode.RightArrow) && Input.GetKey(KeyCode.A))
        {
            currentlyDashing = true;
        }
        if (Input.GetKey(KeyCode.LeftArrow) && Input.GetKey(KeyCode.A))
        {
            currentlyDashing = true;
        }
    }
    //animation stuff start
    void stance()
    {
        if (!Input.GetKey(KeyCode.RightArrow) && (!Input.GetKey(KeyCode.LeftArrow)) && !isAttacking)
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


    void walljumpforce()
    {
        if (!CanWallJump)
            return;

        body.AddForce(WallJumpForce, ForceMode2D.Impulse);
        CreateDust();
        Flip();


    }


    void Flip()
    {
        trans.rotation = isLeft ? Quaternion.identity : Quaternion.Euler(0, 180, 0);

        isLeft = !isLeft;
    }



    private bool IsWalled()
    {
        return Physics2D.OverlapCircle(wallCheck.position, 0.2f, wallLayer);
    }

    private void WallSlide()
    {
        if (IsWalled() && !isGrounded)
        {
            IsWallSliding = true;
            body.velocity = new Vector2(body.velocity.x, Mathf.Clamp(body.velocity.y, -wallSlidingSpeed, float.MaxValue));

        }
        else
        {
            IsWallSliding = false;
        }
    }

    //wall jumping

    private void WallJump()
    {
        if(IsWallSliding)
        {
            isWallJumping = false;
            wallJumpingDirection = -trans.localScale.x;
            WallJumpingCounter = wallJumpingTime;

            CancelInvoke(nameof(StopWallJumping));
        }
        else
        {
            WallJumpingCounter -= Time.deltaTime;
        }

        if (Input.GetKeyDown(KeyCode.Z) && WallJumpingCounter > 0f && !isAttacking)
        {
            isWallJumping = true;
            WallJumpingCounter = 0f;
            walljumpforce();
        }

        if (transform.localScale.x != wallJumpingDirection)
        {

        }

        Invoke(nameof(StopWallJumping), WallJumpingDuration);
    }

    private void StopWallJumping()
    {
        isWallJumping = false;
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
