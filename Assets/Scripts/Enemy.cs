using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float health;
    public string enemyName;
    public float enemyCollisionDamage;
    MonoBehaviour enemyScript;
    void Start()
    {
        enemyScript = GetComponents<MonoBehaviour>()[1];
        // Get jelly 
        Collider2D col = Physics2D.OverlapCircle(transform.position, 0.1f, LayerMask.GetMask("Jelly"));
        if(col != null){
            Jelly jelly = col.gameObject.GetComponent<Jelly>();
            jelly.wakeEnemies.AddListener(Wake);
            jelly.sleepEnemies.AddListener(Sleep);
        }

    }

    void Wake(){
        enemyScript.BroadcastMessage("Wake" + enemyName);
    }

    void Sleep(){
        enemyScript.BroadcastMessage("Sleep" + enemyName);
    }

    void Update()
    {
        
    }

    void OnTriggerEnter2D(Collider2D other){
        if(other.gameObject.layer == LayerMask.NameToLayer("Player")){
            other.gameObject.GetComponent<Player>().DamagePlayer(enemyCollisionDamage);
        }
    }

    public void DamageEnemy(float damage){
        this.health -= damage;
        if(health <= 0){
            Destroy(this.gameObject);
        }
    }
}
