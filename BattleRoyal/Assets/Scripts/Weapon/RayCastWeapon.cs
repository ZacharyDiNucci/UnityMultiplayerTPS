using System;
using System.Collections.Generic;
using UnityEngine;

public class RayCastWeapon : MonoBehaviour
{
    class Bullet{
        public float time;
        public Vector3 initialPosition;
        public Vector3 initialVelocity;
        public TrailRenderer tracer;
    }
    public ActiveWeapon.WeaponSlot weaponSlot;
    public bool isFiring = false;
    public int fireRate = 10;
    public float bulletSpeed = 1000.0f;
    public float bulletDrop = 0.0f;
    public ParticleSystem[] muzzleFlash;
    public ParticleSystem hitEffect;
    public TrailRenderer tracerEffect;
    public string weaponName;
    public Transform raycastOrigin;
    public Transform raycastTarget;
    public bool isAuto;
    bool alreadyFired;
    public WeaponRecoil weaponRecoil;
    public GameObject magazine;

    public int ammoCount;
    public int clipSize;

    Ray ray;
    RaycastHit hitInfo;
    float accumulatedTime;
    float maxLifetime = 3.0f;
    List<Bullet> bullets = new List<Bullet>();

    private void Awake() {
        weaponRecoil = GetComponent<WeaponRecoil>();
    }
    Vector3 GetPosition(Bullet bullet){
        Vector3 gravity = Vector3.down * bulletDrop;
        return (bullet.initialPosition) + (bullet.initialVelocity * bullet.time) + (0.5f * gravity * bullet.time * bullet.time);
    }

    Bullet CreateBullet(Vector3 position, Vector3 velocity){
        Bullet bullet = new Bullet();
        bullet.initialPosition = position;
        bullet.initialVelocity = velocity;
        bullet.time = 0.0f;
        bullet.tracer = Instantiate(tracerEffect, position, Quaternion.identity);
        bullet.tracer.AddPosition(position);
        return bullet;
    }

    public void StartFiring()
    {
        isFiring = true;
        accumulatedTime = 0.0f;
        weaponRecoil.Reset();
    }

    public void UpdateWeapon(float deltaTime){
        if(Input.GetButtonDown("Fire1")){
            StartFiring();
        }
        if(isFiring){
            UpdateFiring(deltaTime);
        }
        UpdateBullets(deltaTime);
        if(Input.GetButtonUp("Fire1")){
            StopFiring();
            alreadyFired = false;
        }
    }

    public void UpdateFiring(float deltaTime){
        accumulatedTime += deltaTime;
        float fireInterval = 1.0f / fireRate;
        while (accumulatedTime >= 0.0f){
            if(!isAuto && !alreadyFired){
                FireBullet();
                alreadyFired = true;
            } else if (isAuto){
                FireBullet();
            }
            accumulatedTime -= fireInterval;
        }
    }

    public void UpdateBullets(float deltaTime)
    {
        SimulateBullets(deltaTime);
        DestroyBullets();
    }

    void SimulateBullets(float deltaTime)
    {
        bullets.ForEach(bullet => {
            Vector3 p0 = GetPosition(bullet);
            bullet.time += deltaTime;
            Vector3 p1 = GetPosition(bullet);
            RaycastSegment(p0, p1, bullet);
        });
    }
    void DestroyBullets()
    {
        bullets.RemoveAll(bullet => bullet.time >= maxLifetime);
    }
    void RaycastSegment(Vector3 start, Vector3 end, Bullet bullet)
    {
        Vector3 direction = end - start;
        float distance = direction.magnitude;
        ray.origin = start;
        ray.direction = direction;
        if (Physics.Raycast(ray, out hitInfo, distance))
        {
            hitEffect.transform.position = hitInfo.point;
            hitEffect.transform.forward = hitInfo.normal;
            hitEffect.Emit(15);

            bullet.tracer.transform.position = hitInfo.point;
            bullet.time = maxLifetime;
        }
        else
        {
            bullet.tracer.transform.position = end;
        }
    }

    private void FireBullet()
    {
        if(ammoCount <= 0){
            return;
        }
        ammoCount--;
        foreach (var particle in muzzleFlash)
        {
            particle.Emit(10);
        }
        Vector3 velocity = (raycastTarget.position - raycastOrigin.position).normalized * bulletSpeed;
        var bullet = CreateBullet(raycastOrigin.position, velocity);
        bullets.Add(bullet);
        
        weaponRecoil.GenerateRecoil(weaponName);
    }

    public void StopFiring(){
        isFiring = false;
    }
}
