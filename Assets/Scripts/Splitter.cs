using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Splitter : MonoBehaviour
{
    // Start is called before the first frame Update
    public GameObject splitterInactivePrefab;
    Transform target;
    Rigidbody2D rgbd;
    Collider2D col;
    Vector2 targetDirection;
    public float splitterSpeed;
    public float splitterScale;
    public float splitMaxAngle;
    public int splitCount;
    public int splitAmount;
    public bool isPrepared;
    public bool isAwake;
    
    public float activationTime;
    public float splitForce;
    void Awake(){
        rgbd = GetComponent<Rigidbody2D>();
    }
    void Start()
    {
        rgbd = GetComponent<Rigidbody2D>();
        col = GetComponent<Collider2D>();
        target = GameObject.Find("Player").transform;
    }

    // Update is called once per frame
    void Update()
    {
        if(isAwake){
            if(isPrepared){
                gameObject.layer = LayerMask.NameToLayer("Enemy");
                targetDirection = ((Vector2)target.position - (Vector2)transform.position).normalized;
                rgbd.velocity = targetDirection * splitterSpeed;
            }

        }
        else{
            rgbd.velocity = new Vector2(0, 0);
        }
        
        
    }

    void Death(){

    }

    public IEnumerator PrepareSplitter(int splitCount, float splitterScale, Vector2 splitDirection){
        transform.localScale = new Vector3(splitterScale, splitterScale, splitterScale);
        this.splitCount = splitCount;
        Debug.Log(rgbd);
        this.rgbd.AddForce(splitDirection * splitForce, ForceMode2D.Impulse);
        yield return new WaitForSeconds(activationTime);
        isPrepared = true;
    }
  

    public void WakeSplitter(){
        isAwake = true;
    }
    public void SleepSplitter(){
        isAwake = false;
    }
}
