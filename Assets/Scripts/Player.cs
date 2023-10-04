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
    public float platformerFallAcceleration;

    [SerializeField]
    bool isPlatformer;
    [SerializeField]
    bool isHoldingChange;
    [SerializeField]
    float xInput;
    [SerializeField]
    float yInput;
    [SerializeField]
    int currentLayer;
    [SerializeField]
    Vector2 nextVelocity;

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
        if(isPlatformer){ // Platformer mode
            rgbd.gravityScale = 10f;
            nextVelocity = new Vector2(rgbd.velocity.x + xInput * twinStickAcceleration * Time.deltaTime, rgbd.velocity.y);
            if(Mathf.Abs(nextVelocity.x) > platformerMagnitude){
                if(nextVelocity.x > 0){
                    nextVelocity = new Vector2(platformerMagnitude, nextVelocity.y);
                }
                else{
                    nextVelocity = new Vector2(-platformerMagnitude, nextVelocity.y);
                }
            }
        }
        else{ // Twin Stick Shooter mode
            rgbd.gravityScale = 0f;
            nextVelocity = new Vector2(rgbd.velocity.x + xInput * twinStickAcceleration * Time.deltaTime, rgbd.velocity.y + (yInput * twinStickAcceleration) * Time.deltaTime);

            if(nextVelocity.magnitude > twinStickMagnitude){
                nextVelocity = nextVelocity.normalized * twinStickMagnitude;
            }
            
        }
        rgbd.velocity = nextVelocity;
    }
    
    void GetInput(){
        isHoldingChange = Input.GetKey(KeyCode.J);
        xInput = Input.GetAxis("Horizontal");
        yInput = Input.GetAxis("Vertical");
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
    private void OnTriggerEnter2D(Collider2D other){
        if(other.gameObject.layer == LayerMask.NameToLayer("Jelly")){
            isPlatformer = false;
        }
        else if(other.gameObject.layer == LayerMask.NameToLayer("Air")){
           
        }
    }

}
