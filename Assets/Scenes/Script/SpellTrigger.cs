using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
using Valve.VR.InteractionSystem;

public class SpellTrigger : MonoBehaviour {

    public GameObject speech;


    // Update is called once per frame
    public void OnSpell() {
        speech.SetActive(true);
	}
    public void OffSpell()
    {
        speech.SetActive(false);
    }
}
