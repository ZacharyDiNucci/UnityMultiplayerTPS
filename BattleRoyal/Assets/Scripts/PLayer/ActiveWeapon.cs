using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.Animations;
using Cinemachine;

public class ActiveWeapon : MonoBehaviour
{
    public enum WeaponSlot {
        Primary = 0,
        Secondary  = 1
    }
    public Transform crossHairTarget;
    public UnityEngine.Animations.Rigging.Rig handIk;
    public Transform[] weaponSlots;
    public CinemachineFreeLook playerCamera;
    
    public Transform weaponLeftGrip;
    public Transform weaponRightGrip;
    public Animator rigController;
    public AmmoWidget ammoWidget;

    RayCastWeapon[] equipped_weapons = new RayCastWeapon[2];
    int activeWeaponIndex;
    bool isHolstered = false;


    // Start is called before the first frame update
    void Start()
    {
        RayCastWeapon existingWeapon = GetComponentInChildren<RayCastWeapon>();
        if(existingWeapon){
            Equip(existingWeapon);
        }
    }

    public RayCastWeapon GetActiveWeapon(){
        return GetWeapon(activeWeaponIndex);
    }

    RayCastWeapon GetWeapon(int index){
        if(index < 0 || index >= equipped_weapons.Length){
            return null;
        }
        return equipped_weapons[index];
    }
    // Update is called once per frame
    void Update()
    {
        var weapon = GetWeapon(activeWeaponIndex);
        if(weapon && !isHolstered){
            weapon.UpdateWeapon(Time.deltaTime);
        }
        if(Input.GetKeyDown(KeyCode.X)){
            ToggleActiveWeapon();
        }
        if(Input.GetKeyDown(KeyCode.Alpha1)){
            SetActiveWeapon(WeaponSlot.Primary);
        }
        if(Input.GetKeyDown(KeyCode.Alpha2)){
            SetActiveWeapon(WeaponSlot.Secondary);
        }
    }

    public void Equip(RayCastWeapon newWeapon){
        int weaponSlotIndex = (int)newWeapon.weaponSlot;
        var weapon = GetWeapon(weaponSlotIndex);
        if(weapon)
        {
            Destroy(weapon.gameObject);
        }
        weapon = newWeapon;
        weapon.raycastTarget = crossHairTarget;
        weapon.weaponRecoil.playerCamera = playerCamera;
        weapon.weaponRecoil.rigController = rigController;
        weapon.transform.SetParent(weaponSlots[weaponSlotIndex], false);
        equipped_weapons[weaponSlotIndex] = weapon;

        SetActiveWeapon(newWeapon.weaponSlot);

        ammoWidget.RefreshText(weapon.ammoCount);
    }

    void ToggleActiveWeapon(){
        bool isHolstered = rigController.GetBool("holster_weapon");
        if(isHolstered){
            StartCoroutine(ActivateWeapon(activeWeaponIndex));
        } else{
            StartCoroutine(HolsterWeapon(activeWeaponIndex));
        }
        rigController.SetBool("holster_weapon", !isHolstered);
    }

    void SetActiveWeapon(WeaponSlot weaponSlot){
        int holsterIndex = activeWeaponIndex;
        int activateIndex = (int)weaponSlot;
        if(holsterIndex == activateIndex){
            holsterIndex = -1;
        }
        StartCoroutine(switchWeapon(holsterIndex, activateIndex));
    }
    IEnumerator switchWeapon(int holsterIndex, int activeIndex)
    {
        yield return StartCoroutine(HolsterWeapon(holsterIndex));
        yield return StartCoroutine(ActivateWeapon(activeIndex));
        activeWeaponIndex = activeIndex;

    }
    IEnumerator HolsterWeapon(int Index){
        isHolstered = true;
        var weapon = GetWeapon(Index);
        if(weapon){
            rigController.SetBool("holster_weapon", true);
            do{
                yield return new WaitForEndOfFrame();
            } while(rigController.GetCurrentAnimatorStateInfo(0).normalizedTime < 1.0f);
        }
    }
    IEnumerator ActivateWeapon(int Index){
        
        var weapon = GetWeapon(Index);
        if(weapon){
            rigController.SetBool("holster_weapon", false);
            rigController.Play("equip_" + weapon.weaponName);
            do{
                yield return new WaitForEndOfFrame();
            } while(rigController.GetCurrentAnimatorStateInfo(0).normalizedTime < 1.0f);
            isHolstered = false;
        }
    }

}
