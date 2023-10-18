using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Swarmer : MonoBehaviour
{
    Transform target;
    Vector2 playerDirection;
    Player playerScript;
    bool isAwake;
    Rigidbody2D rgbd;
    public float swarmerSpeed;
    void Start()
    {
        target = GameObject.Find("Player").transform;
        playerScript = target.GetComponent<Player>();
        rgbd = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if(isAwake){
            playerDirection = ((Vector2)target.transform.position - (Vector2)transform.position).normalized;
            Debug.Log(playerDirection);
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
}
