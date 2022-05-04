using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class AmmoWidget : MonoBehaviour
{
    public Text ammotext;
    public void RefreshText(int ammoCount){
        ammotext.text = ammoCount.ToString();
    }
}
