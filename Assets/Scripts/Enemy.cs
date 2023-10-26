using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using Random = UnityEngine.Random;

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
            switch(name){
                case "Splitter":{
                    Splitter splitter = GetComponent<Splitter>();
                    if(splitter.splitCount > 0){
                        Transform playerTransform = GameObject.Find("Player").transform;
                        Vector2 splitDirection = ((Vector2)transform.position) - ((Vector2)playerTransform.position).normalized;
                        for(int i = 0; i< splitter.splitAmount; i++){
                            GameObject newSplitter = GameObject.Instantiate(splitter.splitterInactivePrefab);
                            float randomAngle = Random.Range(-splitter.splitMaxAngle, splitter.splitMaxAngle);
                            Vector2 rotatedDirection = Quaternion.Euler(0, 0, randomAngle) * splitDirection;
                            newSplitter.transform.position = transform.position;
                            StartCoroutine(newSplitter.GetComponent<Splitter>().PrepareSplitter(splitter.splitCount -1, splitter.splitterScale / 1.5f, rotatedDirection));
                        }
                    }
                    break;
                }
                case "Gusher":{
                    break;
                }
                case "Swarmer":{
                    break;
                }
                case "Worm":{
                    break;
                }
            }
            Destroy(this.gameObject);
        }
    }
}
