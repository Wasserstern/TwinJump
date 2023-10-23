using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Worm : MonoBehaviour
{
    Rigidbody2D rgbd;
    Transform target;
    public Transform wormDirector;
    Vector2 playerDirection;
    Vector2 currentDirection;
    public float wormSpeed;
    public int wormBodySegments;
    public bool isFollowingPlayer;

    [SerializeField]
    bool isAwake;
    void Start()
    {
        target = GameObject.Find("Player").transform;
        rgbd = GetComponent<Rigidbody2D>();
        currentDirection = ((Vector2)wormDirector.position - (Vector2)transform.position).normalized;
    }

    // Update is called once per frame
    void Update()
    {
        if(isAwake){
            if(isFollowingPlayer)
            {
                // Simply move towards player
                playerDirection = ((Vector2)target.transform.position - (Vector2)transform.position).normalized;
                rgbd.velocity = playerDirection * wormSpeed;
            }
            else
            {
                // Move to currentDirection
                rgbd.velocity = currentDirection * wormSpeed;
            }
        }
        else{
            if(isFollowingPlayer){
                // Move around in circle
            }
            else{
                // Keep moving
            }
        }
        
    }
    public void OnCollisionEnter2D(Collision2D other){
        if(other.collider.gameObject.layer == LayerMask.NameToLayer("JellyEdge")){
            ContactPoint2D contactPoint = other.contacts[0];
            Vector2 flippedDirection = currentDirection * -1;
            float angleBetween = Vector2.SignedAngle(flippedDirection, contactPoint.normal);
            Vector2 newDirection = Quaternion.AngleAxis(angleBetween * 2, Vector3.forward) * flippedDirection;
            currentDirection = newDirection;
        }
    }

    public void WakeWorm(){
        isAwake = true;
    }
    public void SleepWorm(){
        isAwake = false;
    }
}
