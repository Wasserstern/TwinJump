using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    string enemyName;
    MonoBehaviour enemyScript;
    void Start()
    {
        // Get jelly 
        Collider2D col = Physics2D.OverlapCircle(transform.position, 0.1f, LayerMask.NameToLayer("Jelly"));
        if(col != null){
            Jelly jelly = col.gameObject.GetComponent<Jelly>();
            jelly.wakeEnemies.AddListener(Wake);
            jelly.sleepEnemies.AddListener(Sleep);
        }
    }

    void Wake(){
        enemyScript.BroadcastMessage(enemyName + "Wake");
    }

    void Sleep(){
        enemyScript.BroadcastMessage(enemyName + "Sleep");
    }

    void Update()
    {
        
    }
}
