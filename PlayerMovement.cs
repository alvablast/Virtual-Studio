using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 10f;

    private Transform cTf;
    private float collisionOffset = 0.15f; 

    public Rigidbody player;
    // Start is called before the first frame update
    void Start()
    {
        cTf = transform;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (Input.GetButton("Horizontal") || Input.GetButton("Vertical"))
        {
            
            Vector3 direction= Input.GetAxis("Horizontal") * speed * Time.fixedDeltaTime * transform.right +
                               Input.GetAxis("Vertical") * speed * Time.fixedDeltaTime * transform.forward;
            MovePlayer(direction);
        }

        if (Input.GetKeyDown("escape"))
        {
            Application.Quit();
        }
    }
    
    public void MovePlayer(Vector3 direction)
    {
        // Check for potential collisions
        RaycastHit hit;
        bool collide = player.SweepTest(
            direction, out hit // X and Y values between -1 and 1 that represent the direction from the body to look for collisions
            , // List of collisions to store the found collisions into after the Cast is finished
            collisionOffset); // The amount to cast equal to the movement plus an offset
        if (!collide)
        {
            player.position += (direction);
        }
    }
}
