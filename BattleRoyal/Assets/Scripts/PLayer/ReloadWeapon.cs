using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReloadWeapon : MonoBehaviour
{
    public Animator rigController;
    public WeaponAnimationEvents animationEvents;
    public ActiveWeapon activeWeapon;
    public Transform leftHand;
    public AmmoWidget ammoWidget;
    GameObject magazineHand;

    // Start is called before the first frame update
    void Start()
    {
        animationEvents.weaponAnimationEvent.AddListener(OnAnimationEvent);
    }

    // Update is called once per frame
    void Update()
    {
        RayCastWeapon weapon = activeWeapon.GetActiveWeapon();
        if(weapon){
            if(Input.GetKeyDown(KeyCode.R) || weapon.ammoCount == 0){
                rigController.SetTrigger("reload_weapon");
            }
            if(weapon.isFiring){
                ammoWidget.RefreshText(weapon.ammoCount);
            }
        }
    }

    void OnAnimationEvent(string eventName) {
        switch (eventName)
        {
            case "detach_magazine":
                DetachMagazine();
                break;
            case "drop_magazine":
                DropMagazine();
                break;
            case "new_magazine":
                NewMagazine();
                break;
            case "insert_magazine":
                InsertMagazine();
                break;
        }
    }
    void DetachMagazine(){
        RayCastWeapon weapon = activeWeapon.GetActiveWeapon();
        magazineHand = Instantiate(weapon.magazine, leftHand, true);
        weapon.magazine.SetActive(false);
    }
    void DropMagazine(){
        GameObject droppedMagazine = Instantiate(magazineHand, magazineHand.transform.position, magazineHand.transform.rotation);
        droppedMagazine.AddComponent<Rigidbody>();
        droppedMagazine.AddComponent<BoxCollider>();
        magazineHand.SetActive(false);

    }
    void NewMagazine(){
        magazineHand.SetActive(true);
    }
    void InsertMagazine(){
        RayCastWeapon weapon = activeWeapon.GetActiveWeapon();
        weapon.magazine.SetActive(true);
        magazineHand.SetActive(false);
        weapon.ammoCount = weapon.clipSize;
        rigController.ResetTrigger("reload_weapon");
        ammoWidget.RefreshText(weapon.ammoCount);
    }
}
