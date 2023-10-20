using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using Unity.VisualScripting;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    Rigidbody2D rgbd;
    Collider2D col;

    Vector2 direction;
    float strength;
    float speed;
    float bounciness;
    int pierceCount;

    bool isPrepared;
    bool hasBeenShot;

    public void PrepareBullet(Vector2 direction,float strength, float speed, float bounciness, int pierceCount){
        this.direction = direction;
        this.strength = strength;
        this.speed = speed;
        this.bounciness = bounciness;
        this.pierceCount = pierceCount;

        //rgbd.sharedMaterial.bounciness = this.bounciness;

        isPrepared = true;
    }
    
    void Start()
    {
        rgbd = GetComponent<Rigidbody2D>();
        col = GetComponent<Collider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if(isPrepared){
            if(!hasBeenShot){
                rgbd.AddForce(direction * speed, ForceMode2D.Impulse);
                hasBeenShot = true;
            }
            if(rgbd.velocity.magnitude < speed / 10){
                ShatterBullet();
            }
        }


    }

    private void OnTriggerEnter2D(Collider2D other){
        if(other.gameObject.layer == LayerMask.NameToLayer("Enemy")){
            other.gameObject.GetComponent<Enemy>().DamageEnemy(strength);
            
            pierceCount--;
            if(pierceCount < 0){
                ShatterBullet();
            }
        }
    }

    private void ShatterBullet(){
        // TODO: Spawn bullet shatter effects
        Destroy(this.gameObject);
    }
}
