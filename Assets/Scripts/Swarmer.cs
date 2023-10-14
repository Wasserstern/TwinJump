using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Swarmer : MonoBehaviour
{
    GameObject target;
    Player playerScript;
    bool isAwake;
    void Start()
    {
        target = GameObject.Find("Player");
        playerScript = target.GetComponent<Player>();
    }

    void Update()
    {
        
    }

    void WakeSwarmer(){
        isAwake = true;
    }

    void SleepSwarmer(){
        isAwake = false;
    }
}
