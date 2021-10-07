/*
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandAnimation : MonoBehaviour
{
    private Valve.VR.EVRButtonId gripButton = Valve.VR.EVRButtonId.k_EButton_Grip;
    //private Valve.VR.EVRButtonId triggerButton = Valve.VR.EVRButtonId.k_EButton_SteamVR_Trigger;


    private SteamVR_Controller.Device Controller { get { return SteamVR_Controller.Input((int)trackedObj.index); } }
    private SteamVR_TrackedObject trackedObj;
    private Animator _anim;



    // Use this for initialization
    void Start()
    {
        trackedObj = GetComponent<SteamVR_TrackedObject>();
        _anim = GetComponentInChildren<Animator>();
    }


    // Update is called once per frame
    void Update()
    {
        if (Controller == null)
        {
            Debug.Log("Controller not initialized");
            return;
        }

        if (Controller.GetPressDown(gripButton))
        {
            Debug.Log("Set Grab");
            _anim.SetBool("IsGrabbing", true);
        }
        if (Controller.GetPressUp(gripButton))
        {
            Debug.Log("Idle");
            _anim.SetBool("IsGrabbing", false);

        }

    }
    
    private void OnTriggerEnter(Collider collider)
    {
        
    }
    private void OnTriggerExit(Collider collider)
    {
        
    }
}
*/
