using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Worm : MonoBehaviour
{
    public GameObject wormSegmentPrefab;
    Rigidbody2D rgbd;
    Transform target;
    public Transform wormDirector;
    Vector2 playerDirection;
    Vector2 currentDirection;
    public float wormSpeed;
    public int wormSegmentCount;
    public bool isFollowingPlayer;

    Transform[] wormSegments;
    [SerializeField]
    Vector2[] segmentTargetPositions;
    Vector2[] targetDirections;
    public float segmentSampleRate;
    public float currentSampleTime;
    public float segmentDistance;

    [SerializeField]
    bool isAwake;
    void Start()
    {
        target = GameObject.Find("Player").transform;
        rgbd = GetComponent<Rigidbody2D>();
        currentDirection = ((Vector2)wormDirector.position - (Vector2)transform.position).normalized;
        wormSegments = new Transform[wormSegmentCount];
        segmentTargetPositions = new Vector2[wormSegmentCount];
        targetDirections = new Vector2[wormSegmentCount];

        // Initialize segments
        for(int i = 0; i < wormSegmentCount; i++){
            Vector2 initialSegmentPosition = (Vector2)transform.position -currentDirection * segmentDistance * (i+1);
            GameObject newSegment = GameObject.Instantiate(wormSegmentPrefab);
            newSegment.transform.position = initialSegmentPosition;
            wormSegments[i] = newSegment.transform;
            if(i == 0){
                segmentTargetPositions[i] = (Vector2)transform.position;
            }
            else{
                segmentTargetPositions[i] = wormSegments[i -1].position;
            }
            
        }
        Array.Fill(targetDirections, currentDirection);
    }

    // Update is called once per frame
    void Update()
    {
        if(isAwake){
            if(isFollowingPlayer)
            {
                // Simply move towards player
                playerDirection = ((Vector2)target.transform.position - (Vector2)transform.position).normalized;
                rgbd.velocity = playerDirection * wormSpeed;
            }
            else
            {
                // Move to currentDirection
                rgbd.velocity = currentDirection * wormSpeed;

                // Move segments

                for(int i = 0; i < wormSegmentCount; i++){
                    Transform segment = wormSegments[i];
                    segment.position = Vector2.MoveTowards(segment.position, segmentTargetPositions[i], wormSpeed * Time.deltaTime);
                }
                // Update if sample timer hits sample rate
                if(currentSampleTime >= segmentSampleRate){
                    currentSampleTime -= segmentSampleRate;
                    for(int i = 0; i < wormSegmentCount; i++){
                        if(i == 0){
                            segmentTargetPositions[i] = (Vector2)transform.position;
                        }
                        else{
                            segmentTargetPositions[i] = wormSegments[i -1].transform.position;
                        }
                    }
                }
                currentSampleTime += Time.deltaTime;
            }
        }
        else{
            if(isFollowingPlayer){
                // Move around in circle
            }
            else{
                // Keep moving
            }
        }
        
    }
    public void OnCollisionEnter2D(Collision2D other){
        if(other.collider.gameObject.layer == LayerMask.NameToLayer("JellyEdge")){
            ContactPoint2D contactPoint = other.contacts[0];
            Vector2 flippedDirection = currentDirection * -1;
            float angleBetween = Vector2.SignedAngle(flippedDirection, contactPoint.normal);
            Vector2 newDirection = Quaternion.AngleAxis(angleBetween * 2, Vector3.forward) * flippedDirection;
            currentDirection = newDirection;
        }
    }

    public void WakeWorm(){
        isAwake = true;
    }
    public void SleepWorm(){
        isAwake = false;
    }
}
