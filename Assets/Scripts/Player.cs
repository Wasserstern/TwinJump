using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Player : MonoBehaviour
{

    public float twinStickMagnitude;
    public float twinStickAcceleration;
    public float platformerMagnitude;
    public float platformerAcceleration;
    public float platformerAccelerationBonus;
    public float platformerDeceleration;
    public float minPlatformerMagnitude;
    public float jumpForce;
    public float changeBoost;
    public float groundCheckDistance;
    public float jumpCooldown;
    [Range(0f, 1f)]
    public float stickDeadZone;

    [SerializeField]
    bool isPlatformer;
    [SerializeField]
    bool isHoldingChange;
    [SerializeField]
    bool pressedJump;
    [SerializeField]
    bool isGrounded;
    [SerializeField]
    float xInput;
    [SerializeField]
    float yInput;
    [SerializeField]
    int currentLayer;
    [SerializeField]
    Vector2 nextVelocity;
    [SerializeField]
    bool isJumping;

    Rigidbody2D rgbd;
    Collider2D col;
    void Start()
    {
        rgbd = GetComponent<Rigidbody2D>();
        col = GetComponent<Collider2D>();
        currentLayer = Physics2D.OverlapCircle(transform.position, col.bounds.size.x / 2).gameObject.layer;
        if(LayerMask.LayerToName(currentLayer) == "Jelly"){
            isPlatformer = false;
        }
        else{
            isPlatformer = true;
        }
    }


    void Update()
    {
        GetInput();

        if(isHoldingChange){
            Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Player"), LayerMask.NameToLayer("JellyEdge"), true);
        }
        else{
            Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Player"), LayerMask.NameToLayer("JellyEdge"), false);
        }

        if(LayerMask.LayerToName(Physics2D.OverlapCircle(transform.position, col.bounds.size.x / 3).gameObject.layer) == "Jelly"){
            // Check if player is deep inside jelly
            isPlatformer = false;
        }


        if(isPlatformer){ // Platformer mode

            // Check ground
            if(rgbd.velocity.y <= 0f){
                Vector2 rayDirection = (new Vector2(transform.position.x, transform.position.y -1) - (Vector2)transform.position).normalized;
                RaycastHit2D hit = Physics2D.Raycast((Vector2)transform.position, rayDirection, groundCheckDistance, LayerMask.GetMask("Jelly", "JellyEdge"));
                if(hit.collider != null){
                    isGrounded = true;
                }
            }
            else{
                isGrounded = false;
            }
            //Set gravity to active and apply platformer variables
            rgbd.gravityScale = 10f;
            float accelerationBonus = 0f;
            if(xInput > 0 && rgbd.velocity.x < 0){
                accelerationBonus = -platformerAccelerationBonus;
            }
            else if(xInput < 0 && rgbd.velocity.x > 0){
                accelerationBonus = platformerAccelerationBonus;
            }
            nextVelocity = new Vector2(rgbd.velocity.x + xInput * (platformerAcceleration + accelerationBonus) * Time.deltaTime, rgbd.velocity.y);
            if(Mathf.Abs(nextVelocity.x) > platformerMagnitude){
                if(nextVelocity.x > 0){
                    nextVelocity = new Vector2(platformerMagnitude, nextVelocity.y);
                }
                else{
                    nextVelocity = new Vector2(-platformerMagnitude, nextVelocity.y);
                }
            }
            if(xInput >= -stickDeadZone  && xInput <= stickDeadZone){
                // No xInput, decellerate player
                if(nextVelocity.x > 0){
                   //TODO: ADD DECELERATION
                   float xV = nextVelocity.x - platformerDeceleration * Time.deltaTime;
                   if(xV <= minPlatformerMagnitude){
                        xV = 0;
                   }
                   nextVelocity = new Vector2(xV, nextVelocity.y);
                }
                else{
                    //TODO: ADD DECELERATION
                    float xV = nextVelocity.x + platformerDeceleration * Time.deltaTime;
                    if(xV >= -minPlatformerMagnitude){
                        xV = 0;
                    }
                    nextVelocity = new Vector2(xV, nextVelocity.y);
                }
            }
            if(pressedJump){
                if(isGrounded && !isJumping){
                    nextVelocity = new Vector2(nextVelocity.x, nextVelocity.y + jumpForce );
                }
            }
        }
        else{ // Twin Stick Shooter mode
            rgbd.gravityScale = 0f;
            nextVelocity = new Vector2(rgbd.velocity.x + xInput * twinStickAcceleration * Time.deltaTime, rgbd.velocity.y + yInput * twinStickAcceleration * Time.deltaTime);

            if(nextVelocity.magnitude > twinStickMagnitude){
                nextVelocity = nextVelocity.normalized * twinStickMagnitude;
            }
            
        }
        if(!isJumping){
            rgbd.velocity = nextVelocity;
        }
    }
    
    void GetInput(){
        isHoldingChange = Input.GetKey(KeyCode.J);
        xInput = Input.GetAxisRaw("Horizontal");
        yInput = Input.GetAxisRaw("Vertical");
        pressedJump = Input.GetKeyDown(KeyCode.Space);
    }

    private void OnTriggerExit2D(Collider2D other){
        if(other.gameObject.layer == LayerMask.NameToLayer("Jelly")){
            // Push out of jelly
            isPlatformer = true;
            
        }
        else if(other.gameObject.layer == LayerMask.NameToLayer("Air")){
            // Push inside jelly
            isPlatformer = false;
        }
    }


  

}
