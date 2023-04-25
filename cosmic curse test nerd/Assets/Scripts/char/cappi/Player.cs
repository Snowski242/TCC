using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.InputSystem;



public class Player : MonoBehaviour
{
    public Transform transs;
    public static Rigidbody2D bodys;
    public Animator animators;
    public SpriteRenderer spriteRend;

    public CapsuleCollider2D standingCollider;
    public CapsuleCollider2D crouchCollider;

    public ParticleSystem dustSmall;
    public ParticleSystem dustBig;

    public InputAction playerControl;

    public AudioClip jumpeffect;

    [Header("Jumping/Dashing Settings")]
    [SerializeField] protected float jumpForce;
    [SerializeField] public float dashForce;
    [SerializeField] float walljumpForce;

    bool canMove = true;
    bool isCrouching;
    bool isBonking;



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




    public bool isFacingRight;

    [Header("Buffers")]
    protected float jumpBufferTime = 0.9f;
    protected float jumpBufferCounter;
    protected float coyoteTime = 0.38f;
    protected float coyoteTimeCounter;

    [Header("Speed Settings")]
    [SerializeField] public float speed;
    const float MaxSpeed = 24;
    const float AccelSpeed = 1;
    const float accelRate = 0.24f;

    [Header("Boost Settings")]
    public float boostForce;
    public float boostSpeed = 1;
    public const float boostRate = 0.75f;
    public static float boostGauge = 2;
    public static float boostGaugeMax = 10;

    private bool isOnSlope;
    private float slopeDownAngle;
    private float slopeSideAngle;
    private float lastSlopeAngle;
    private Vector2 capsuleColliderSize;

    private Vector2 slopeNormalPerp;
    [SerializeField]
    private float slopeCheckDistance;
    [SerializeField]
    private float maxSlopeAngle;
    private bool canWalkOnSlope;
    private Vector2 newVelocity;

    [SerializeField]
    private float groundCheckRadius;

    [SerializeField]
    private LayerMask whatIsGround;

    [Header("KnockBack Settings")]
    public float KBForce;
    public float KBCounter;
    public float KBTotaltime;
    public bool KnockFromRight;
    public int numberOfFlashes;

    private float invulnTime = 3f;
    [HideInInspector]
    public bool canBeHit = true;

    public static event Action TimeIncrease;

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
        spriteRend = GetComponent<SpriteRenderer>();

        vectorVar = rbVelocity.x;
        CineMachineShake.Instance.ShakeCamera(0f, .1f);

        boostGauge = 5f;

        crouchCollider.enabled = false;
        standingCollider.enabled = true;
        isGrounded = false;

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

        if (Input.GetKeyDown(KeyCode.A) && boostGauge > 0)
        {
            StartCoroutine(Boost());
            isAttacking = false;
            animators.SetFloat("speed", Mathf.Abs(0));
        }



        if(Input.GetKeyDown(KeyCode.DownArrow) && isGrounded && !jumpInput)
        {
            standingCollider.enabled = false;
            crouchCollider.enabled = true;
            isCrouching = true;
            animators.SetBool("isCrouching", true);
        } else if (Input.GetKeyUp(KeyCode.DownArrow))
        {
            standingCollider.enabled = true;
            crouchCollider.enabled = false;
            isCrouching = false;
            animators.SetBool("isCrouching", false);
        }







    }
    void FixedUpdate()
    {
        Vector3 clampedVelocity = bodys.velocity;
        clampedVelocity.x = Mathf.Clamp(clampedVelocity.x, -MaxSpeed, MaxSpeed);
        bodys.velocity = clampedVelocity;

        SlopeCheck();

        if (isGrounded && !isBonking)
        {
            canMove = true;
        }

        if (canMove && !isCrouching)
        {
            bodys.AddForce(transform.right * horizontal, ForceMode2D.Impulse);

            if (isGrounded && isOnSlope && canWalkOnSlope && !jumpInput) //If on slope
            {
                newVelocity.Set(speed * slopeNormalPerp.x * -horizontal, speed * slopeNormalPerp.y * -horizontal);
                bodys.velocity = newVelocity;
            }
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
                   speed = Mathf.MoveTowards(speed, 0f, AccelSpeed * Time.deltaTime);
                }




            if(speed > 0.1f && speed < 8)
            {
                animators.SetBool("isWalking", true);
            }
            else
            {
                animators.SetBool("isWalking", false);
            }


            if (isFacingRight && speed > 0 && horizontal < 0)
            {
                speed = Mathf.MoveTowards(speed, 0f, AccelSpeed * Time.deltaTime);
            }

            if (!isFacingRight && speed > 0 && horizontal > 0)
            {
                speed = Mathf.MoveTowards(speed, 0f, AccelSpeed * Time.deltaTime);
            }
        }

        void IncreaseSpeed()
        {
            if (isGrounded && canMove && !isCrouching)
                speed += AccelSpeed * accelRate;

            if (speed > MaxSpeed)
            {
                animators.speed = 1;
                speed = MaxSpeed;
                currentlyDashing = true;

            }
            if (speed > 1)
            {
                animators.speed = 0.75f;
            }
            if (speed > 5)
            {
                animators.speed = 1.25f;
            }
            if (speed > 12)
            {
                animators.speed = 1.55f;
            }
            if (speed > 18)
            {
                animators.speed = 1.75f;
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

                crouchCollider.enabled = false;
                standingCollider.enabled = true;

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
        else if (!isGrounded && jumpInput)
        {
            animators.SetBool("IsJumping", true);
            animators.SetBool("isCrouching", false);

        }

    }
    //animation stuff end
    void Jump()
    {

        AudioSource.PlayClipAtPoint(jumpeffect, transform.position);

        newVelocity.Set(0.0f, 0.0f);

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
        if (canMove)
        {
            if (isFacingRight && horizontal < 0f || !isFacingRight && horizontal > 0f)
            {
                isFacingRight = !isFacingRight;
                Vector3 localScale = transs.localScale;
                localScale.x *= -1f;
                transform.localScale = localScale;
            }
        }

    }



    IEnumerator Boost()
    {
        if (canMove)
        {
            while (boostGauge > 0f && horizontal > 0|| boostGauge > 0f && horizontal < 0)
            {
                isAttacking = true;

                animators.SetFloat("speed", Mathf.Abs(4));
                speed = MaxSpeed;
                bodys.AddForce(transform.right * boostForce * horizontal, ForceMode2D.Impulse);
                boostGauge = Mathf.MoveTowards(boostGauge, 0f, boostRate * Time.deltaTime);
                CineMachineShake.Instance.ShakeCamera(1.36f, .1f);

                yield return null;
            }

            isAttacking = false;
            animators.SetFloat("speed", Mathf.Abs(0));

        }
    }


    //private Vector3 AdjustVelocityToSlope(Vector3 velocity)
    //{
    //    var ray = new Ray(transform.position, Vector3.down);

    //    if (Physics.Raycast(ray, out RaycastHit hitInfo, 0.2f))
    //    {
    //        var slopeRotation = Quaternion.FromToRotation(Vector3.up, hitInfo.normal);
    //        var adjustedVelocity = slopeRotation * velocity;

    //        if (adjustedVelocity.y < 0)
    //        {
    //            return adjustedVelocity;
    //        }

    //    }

    //    return velocity;
    //}

    

    private void OnCollisionEnter2D(Collision2D collision) // detects when the object collides with another object
    {
        if (collision.collider.tag == "Ground") // saying if the thing you're colliding with has the ground tag on it
        {
            for (int i = 0; i < collision.contacts.Length; i++) // if any one of the collisions is on the ground
            {
                if (collision.contacts[i].normal.y > 0.2)
                {
                    isGrounded = true;

                }
            }
        }

        if (collision.collider.tag == "SlopeUp")
        {
            if (canMove && horizontal > 0f)
            {
                bodys.AddForce(transform.right * 2, ForceMode2D.Impulse);
                bodys.AddForce(transform.up * 3, ForceMode2D.Impulse);
            }
        }

        if (collision.collider.tag == "Enemy" && canBeHit)
        {
            StartCoroutine(Hurt());
        }

        if (collision.collider.tag == "Wall" && speed > 18)
        {
            canMove = false;
            speed = 0;
            StartCoroutine(Bonked());

        }
    }

  
    

    IEnumerator Hurt()
    {
        canBeHit = false;
        TimeIncrease?.Invoke();
        Physics2D.IgnoreLayerCollision(6, 7, true);
        for (int i = 0; i < numberOfFlashes; i++)
        {
            spriteRend.color = new Color(1, 0, 0, 0.5f);
            yield return new WaitForSeconds(0.4f);
        }
        yield return new WaitForSeconds(invulnTime);
        spriteRend.color = Color.white;
        canBeHit = true;
        Physics2D.IgnoreLayerCollision(6, 7, false);
        Debug.Log("invuln works");

    }

    IEnumerator Bonked() 
    {
        canMove = false;
        isBonking = true;

        bodys.AddForce(-transform.right * 12, ForceMode2D.Impulse);
        bodys.AddForce(transform.up * 3, ForceMode2D.Impulse);
        speed = 0;
        horizontal = speed;
        animators.SetTrigger("Bonking 0");
        yield return new WaitForSeconds(0.95f);

        isBonking = true;
        canMove = true;
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if(collision.collider.tag == "Ground")
        {
            isGrounded = false;
        }
    }
    private void SlopeCheck()
    {
        Vector2 checkPos = transform.position - (Vector3)(new Vector2(0.0f, capsuleColliderSize.y / 2));

        SlopeCheckHorizontal(checkPos);
        SlopeCheckVertical(checkPos);
    }

    private void SlopeCheckHorizontal(Vector2 checkPos)
    {
        RaycastHit2D slopeHitFront = Physics2D.Raycast(checkPos, transform.right, slopeCheckDistance, whatIsGround);
        RaycastHit2D slopeHitBack = Physics2D.Raycast(checkPos, -transform.right, slopeCheckDistance, whatIsGround);

        if (slopeHitFront)
        {
            isOnSlope = true;

            slopeSideAngle = Vector2.Angle(slopeHitFront.normal, Vector2.up);

        }
        else if (slopeHitBack)
        {
            isOnSlope = true;

            slopeSideAngle = Vector2.Angle(slopeHitBack.normal, Vector2.up);
        }
        else
        {
            slopeSideAngle = 0.0f;
            isOnSlope = false;
        }

    }

    private void SlopeCheckVertical(Vector2 checkPos)
    {
        RaycastHit2D hit = Physics2D.Raycast(checkPos, Vector2.down, slopeCheckDistance, whatIsGround);

        if (hit)
        {

            slopeNormalPerp = Vector2.Perpendicular(hit.normal).normalized;

            slopeDownAngle = Vector2.Angle(hit.normal, Vector2.up);

            if (slopeDownAngle != lastSlopeAngle)
            {
                isOnSlope = true;
            }

            lastSlopeAngle = slopeDownAngle;

            Debug.DrawRay(hit.point, slopeNormalPerp, Color.blue);
            Debug.DrawRay(hit.point, hit.normal, Color.green);

        }

        if (slopeDownAngle > maxSlopeAngle || slopeSideAngle > maxSlopeAngle)
        {
            canWalkOnSlope = false;
        }
        else
        {
            canWalkOnSlope = true;
        }
    }
}
