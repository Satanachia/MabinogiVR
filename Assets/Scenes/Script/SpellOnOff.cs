using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellOnOff : MonoBehaviour {

    public GameObject speech;
	
	// Update is called once per frame
	public void OnSpell()
    {
        speech.SetActive(true);
    }
   public void OffSpell()
    {
        speech.SetActive(false);
    }
}
