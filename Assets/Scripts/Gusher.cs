using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gusher : MonoBehaviour
{
    Rigidbody2D rgbd;
    Transform target;
    Vector2 playerDirection;
    public GameObject bulletPrefab;
    public bool isFollowingPlayer;
    public float gusherSpeed;
    public float bulletStrength;
    public float bulletSpeed;
    public float rotationDegreesPerSecond;
    public float gushIntervalInSeconds;
    public float gushTimer;

    public float bulletBounciness;
    List<Transform> bulletOrigins;
    [SerializeField]
    bool isAwake;
    bool rotateRight;
    float rotationDirection;
    public float rotationInterval;
    void Start()
    {
        rgbd = GetComponent<Rigidbody2D>();
        target = GameObject.Find("Player").transform;
        bulletOrigins = new List<Transform>();
        for(int i = 0; i < transform.childCount; i++){
            bulletOrigins.Add(transform.GetChild(i));
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(isAwake){
            gushTimer+= Time.deltaTime;
            if(gushTimer >= gushIntervalInSeconds){
                gushTimer = 0f;
                Gush();
            }
            if(isFollowingPlayer){
                playerDirection = ((Vector2)target.transform.position - (Vector2)transform.position).normalized;
                rgbd.velocity = playerDirection * gusherSpeed;
            }
            else{
                rgbd.velocity = new Vector2(0, 0);
            }
        }
        else{
            gushTimer = 0f;
            rgbd.velocity = new Vector2(0, 0);
        }

        if(rotateRight){
            rotationDirection += Time.deltaTime / rotationInterval;
            if(rotationDirection > 1){
                rotationDirection = 1f;
                rotateRight = false;
            }
        }
        else{
            rotationDirection -= Time.deltaTime / rotationInterval;
            if(rotationDirection < 0){
                rotationDirection = 0f;
                rotateRight = true;
            }
        }
        float currentRotateAxis = Mathf.Lerp(-1f, 1f, EaseFunctions.easeInOutCubic(rotationDirection));

        transform.Rotate(new Vector3(0f, 0f, rotationDegreesPerSecond * Time.deltaTime * currentRotateAxis));        
    }
    void Gush(){
        foreach(Transform origin in bulletOrigins){
            GameObject bullet = GameObject.Instantiate(bulletPrefab);
            bullet.transform.position = origin.position;
            Vector2 shootDirection = ((Vector2)origin.position - (Vector2)transform.position).normalized;
            bullet.GetComponent<Bullet>().PrepareBullet(shootDirection, bulletStrength, bulletSpeed, bulletBounciness, 0, 5f);
        }
    }

    public void WakeGusher(){
        isAwake = true;
    }
    public void SleepGusher(){
        isAwake = false;
    }
}
