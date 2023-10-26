using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Swarmer : MonoBehaviour
{
    Transform target;
    Vector2 playerDirection;
    Player playerScript;
    bool isAwake;
    Rigidbody2D rgbd;
    public float swarmerSpeed;
    public int splitCount;
    void Start()
    {
        target = GameObject.Find("Player").transform;
        playerScript = target.GetComponent<Player>();
        rgbd = GetComponent<Rigidbody2D>();
        if(splitCount > 0){
            this.gameObject.layer = LayerMask.NameToLayer("Splitter");
        }
    }

    void Update()
    {
        if(isAwake){
            playerDirection = ((Vector2)target.transform.position - (Vector2)transform.position).normalized;
            rgbd.velocity = playerDirection * swarmerSpeed;
        }
        else{
            rgbd.velocity = new Vector2(0, 0);
        }
    }

    public void WakeSwarmer(){
        isAwake = true;
    }

    public void SleepSwarmer(){
        isAwake = false;
    }

    private void OnTriggerEnter2D(Collider2D other){
        if(other.gameObject.layer == LayerMask.NameToLayer("Bullet")){
            splitCount--;
            if(splitCount <= 0){
                
            }
        }
    }

    
}
