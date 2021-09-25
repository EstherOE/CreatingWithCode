using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallMovement : MonoBehaviour
{
    private Rigidbody body;
    public float speed;
    public int minSwipe = 500; // which swipe diretion should it be seen

    private Vector3 travelDirectiom;
    private Vector3 nextCollision;

    private bool isTravel;
    
    public Vector2 swipeLastFrame;
    private Vector2 swipeCurrentFrane;
    public Vector2 currentSwipe;
    private Color swipeColor;
    // Start is called before the first frame update
    void Start()
    {
        body = GetComponent<Rigidbody>();
        swipeColor = Random.ColorHSV(0.5f, 1f);
       GetComponent<MeshRenderer>().material.color = swipeColor;
    }
    public bool gameOver = false;

    private void FixedUpdate()
    {

        if (isTravel)
        {


            body.velocity = speed * travelDirectiom;
        }
        
        Collider[] hitCollider = Physics.OverlapSphere(transform.position - (Vector3.up / 2), .05f);
        int i = 0;
        while (i < hitCollider.Length)
        {
            GrounndPiece piece = hitCollider[i].GetComponent<GrounndPiece>();

            if (piece && !piece.iscolor)
            {
                piece.ChangeColored(swipeColor);
            }
            i++;

        }


        // swipe direction

        if(nextCollision != Vector3.zero)
        {
            /* if the distance of the postion of the ball and compare with  
             * the next collison which is the  wall
             */
            if (Vector3.Distance(transform.position, nextCollision)<1)
            

            {
                isTravel = false;
                travelDirectiom = Vector3.zero;
                nextCollision = Vector3.zero;// cos we dont have the next position
            }
        }
        /* is travell get get of the fixed update */
        if (isTravel)
            return;
        //if the Input is a touch screen or mouse
        if(Input.GetMouseButton(0))
        {
            swipeCurrentFrane = new Vector2(Input.mousePosition.x, Input.mousePosition.y);

            if(swipeLastFrame != Vector2.zero)
            {
                currentSwipe = swipeCurrentFrane - swipeLastFrame;
                if(currentSwipe.sqrMagnitude <minSwipe)
                {
                    return;
                }
                currentSwipe.Normalize(); // get the direction not the distance


                //UP  or down
                if(currentSwipe.x >-0.5f && currentSwipe.x < 0.5f) // is it in top section
                {
                    //Go Up/down
                    setDirection(currentSwipe.y > 0 ? Vector3.forward : Vector3.back);
                }

                //Left or right
                if (currentSwipe.y > -0.5f && currentSwipe.y < 0.5f) // is it in top section
                {
                    //Go  left or right
                    setDirection(currentSwipe.x > 0 ? Vector3.right : Vector3.left);
                }



            }


            swipeLastFrame = swipeCurrentFrane; // current of the finger  is used  , next frame will be the last frame
        }
       

        // release the screen
        if (Input.GetMouseButtonUp(0))
            
        {
            swipeLastFrame = Vector2.zero;
            currentSwipe = Vector2.zero;
        }
    }
   

    void setDirection(Vector3 direction)
    {
        travelDirectiom = direction;


        RaycastHit hit; // check which obj is collider
        if(Physics.Raycast(transform.position, direction, out hit, 100f))
        {
            nextCollision = hit.point; // direction which we hit
        }
        isTravel = true;

    }
    

}
