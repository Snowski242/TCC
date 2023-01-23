using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Playercontroller : MonoBehaviour
{ 
    Transform trans;
    Rigidbody2D body;

    [SerializeField] float speed; //we want to add thi in the spector in the unity
    [SerializeField] float jumpForce;
    [SerializeField] float dashForce;

    bool jumpInput;
    bool isGrounded;
    bool canDash;
    bool rollInput;

    float timeToNextShot = 0;



    // Start is called before the first frame update
    void Start()
    {
        trans = GetComponent<Transform>(); // takes the game object , get specific component and applies it to variable; 
        body = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        walk();
        if (Input.GetKeyDown(KeyCode.Z))
        {
            jumpInput = true;
        }
        if (Input.GetKeyUp(KeyCode.Z))
        {
            jumpInput = false;
        }
        if (Input.GetKeyDown(KeyCode.A))
        {
            rollInput = true;
        }
        if (Input.GetKeyUp(KeyCode.A))
        {
            rollInput = false;
        }




    }
    void FixedUpdate() // it's meant for any physics updates
    {
        if(jumpInput && isGrounded)
        {
            jump();
        }
       // jump();
       if(rollInput && canDash)
        {
            roll();
        }
    }
    void walk()
    {
       if(Input.GetKey(KeyCode.RightArrow)) // detect while walking is the player input
       {
            trans.position += transform.right * Time.deltaTime * speed; // Time.deltaTime, it does not depend on the performance of your computer
            trans.rotation = Quaternion.Euler(0, 0, 0); // set the rotation of game object
       }
        if (Input.GetKey(KeyCode.LeftArrow)) 
        {
            trans.position += transform.right * Time.deltaTime * speed;
            trans.rotation = Quaternion.Euler(0, 180, 0); //change y rotation to 180
        }

    }
    void jump()
    {
        /* if (Input.GetKey(KeyCode.W))
         {
             body.AddForce(transform.up * jumpForce, ForceMode2D.Impulse);
         }
         it's always jumped
        */
        
        body.AddForce(transform.up * jumpForce, ForceMode2D.Impulse);
        isGrounded = false;
        
    }

    void roll()
    {
        body.AddForce(transform.right * dashForce, ForceMode2D.Impulse);
        canDash = false;
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
                    canDash = true;
                }
            }
        }
    }
}
