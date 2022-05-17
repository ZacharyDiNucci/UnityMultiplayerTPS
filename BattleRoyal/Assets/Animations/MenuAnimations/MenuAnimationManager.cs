using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class MenuAnimationManager : MonoBehaviour
{
    [SerializeField]
    private TMP_InputField roomNameInput;

    [SerializeField]
    private TMP_InputField playerNameInput;
    

    private Animator anim;

    void Start() {
        anim = GetComponent<Animator>();
    }

    public void SetAnimationClip(int clip)
    {
        switch (clip)
        {
            case(0):
                anim.SetInteger("AnimState", clip);
                break;
            case(1):
                 anim.SetInteger("AnimState", clip);
                break;
            case(2):
                if((roomNameInput.text.Length >= 3 && roomNameInput.text.Length <= 10)&& (playerNameInput.text.Length >= 3 && playerNameInput.text.Length <= 15))
                {
                    anim.SetInteger("AnimState", clip);
                }
                else
                {
                    Debug.Log("Test2");
                }
                break;
            case(3):
                anim.SetInteger("AnimState", clip);
                break;
        }
    }
    public void QuitButton()
    {
        Application.Quit();
    }
    public void LoadScene(string scene)
    {
        SceneManager.LoadScene(scene, LoadSceneMode.Additive);
    }
    public void LoadNetworkScene(string scene)
    {
        NetworkManager.instance.photonView.RPC("LoadScene", Photon.Pun.RpcTarget.All, scene);
    }
}
