using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{

    public GameObject bulletPrefab;
    public enum BulletType{
        modify, standard, shotgun, sniper, laser, absorber, lasso
    }
    Camera mainCamera;
    Transform parent;
    float xMouseInput;
    float yMouseInput;
    Vector2 currentLocalPosition;

    public BulletType bulletType;
    public float bulletsPerSecond;
    float bulletIntervalInSeconds;
    float timeSinceLastBullet;
    [SerializeField]
    bool isHoldingShoot;
    void Start()
    {
        mainCamera = GameObject.Find("Main Camera").GetComponent<Camera>();
        currentLocalPosition = new Vector2(0, 0);
        parent = transform.parent;
        bulletIntervalInSeconds = 1 / bulletsPerSecond;
    }


    void Update()
    {
        GetInput();
        
        // Get direction to last mouse delta
        currentLocalPosition = ((Vector2)mainCamera.ScreenToWorldPoint(Input.mousePosition) - (Vector2)parent.position).normalized;
        if(xMouseInput != 0 && yMouseInput != 0){
            transform.localPosition = currentLocalPosition;
        }

        if(isHoldingShoot){
            if(timeSinceLastBullet >= bulletIntervalInSeconds){
                timeSinceLastBullet = 0f;
                // Shoot Bullet
                Shoot();
            }
        }
        timeSinceLastBullet += Time.deltaTime;

    }

    void Shoot(){
        GameObject newBullet = GameObject.Instantiate(bulletPrefab);
        newBullet.transform.position = transform.position;
        Vector2 shootDirection = ((Vector2)transform.position - (Vector2)parent.position).normalized;
        
        switch(bulletType){
            case BulletType.standard:{
                newBullet.GetComponent<Bullet>().PrepareBullet(shootDirection, 1f, 20f, 0.3f, 6);
                break;
            }
        }
    }

    void GetInput(){
        xMouseInput = Input.GetAxis("Mouse X");
        yMouseInput = Input.GetAxis("Mouse Y");

        isHoldingShoot = Input.GetKey(KeyCode.Mouse0);
    }
}
