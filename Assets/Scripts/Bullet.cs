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
    float lifeTime;

    bool isPrepared;
    bool hasBeenShot;

    float lifeTimeTimer;

    public void PrepareBullet(Vector2 direction,float strength, float speed, float bounciness, int pierceCount, float lifeTime = 5f){
        this.direction = direction;
        this.strength = strength;
        this.speed = speed;
        this.bounciness = bounciness;
        this.pierceCount = pierceCount;
        this.lifeTime = lifeTime;

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
            lifeTimeTimer+= Time.deltaTime;
            if(lifeTimeTimer > lifeTime){
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
        else if(other.gameObject.layer == LayerMask.NameToLayer("Player")){
            other.gameObject.GetComponent<Player>().DamagePlayer(strength);

            pierceCount--;
            if(pierceCount < 0){
                ShatterBullet();
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D other){
        if(other.gameObject.layer == LayerMask.NameToLayer("JellyEdge")){
            if(bounciness <= 0){
                pierceCount--;
                if(pierceCount < 0){
                    ShatterBullet();
                }
            }
        }
    }

    private void ShatterBullet(){
        // TODO: Spawn bullet shatter effects
        Destroy(this.gameObject);
    }
}
