using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class Jelly : MonoBehaviour
{
    public bool playerInside;
    public UnityEvent wakeEnemies;
    public UnityEvent sleepEnemies;
    public float wakeIntervalTime;
    float lastWakeTime;
    void Start()
    {
        lastWakeTime = Time.time;
    }

    void Update()
    {
        if(playerInside){
            if(Time.time - lastWakeTime >= wakeIntervalTime){
                wakeEnemies.Invoke();
                lastWakeTime = Time.time;
            } 
        }
    }

    private void OnTriggerEnter2D(Collider2D other){
        if(other.gameObject.layer == LayerMask.NameToLayer("Player")){
            lastWakeTime = Time.time;
            playerInside = true;
        }
    }
    private void OnTriggerExit2D(Collider2D other){
        if(other.gameObject.layer == LayerMask.NameToLayer("Player")){
            playerInside = false;
            sleepEnemies.Invoke();
        }
    }
}
