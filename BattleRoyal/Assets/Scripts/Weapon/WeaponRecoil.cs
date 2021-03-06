using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class WeaponRecoil : MonoBehaviour
{
    [HideInInspector] public CinemachineFreeLook playerCamera;
    [HideInInspector] public CinemachineImpulseSource cameraShake;
    [HideInInspector] public Animator rigController;
    

    public Vector2[] recoilPattern;
    float verticalRecoil;
    float horizontalRecoil;
    public float duration;

    float time;
    int index = 0;
    private void Awake() {
        cameraShake = GetComponent<CinemachineImpulseSource>();
    }
    public void Reset(){
        index = 0;
    }
    int NextIndex(int index){
        return (index + 1) % recoilPattern.Length; 
    }

    public void GenerateRecoil(string weaponName){
        time = duration;
        
        cameraShake.GenerateImpulse(Camera.main.transform.forward);
        
        horizontalRecoil = recoilPattern[index].x;
        verticalRecoil = recoilPattern[index].y;

        index = NextIndex(index);
        rigController.Play("weapon_recoil_" + weaponName, 1, 0.0f);
    }
    void Update()
    {
        if(time > 0){
            playerCamera.m_YAxis.Value -= (verticalRecoil/1000 * Time.deltaTime) / duration;
            playerCamera.m_XAxis.Value -= (horizontalRecoil/10 * Time.deltaTime) / duration;
            time -= Time.deltaTime;
        }
    }
}
