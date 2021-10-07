using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IcanGrab : MonoBehaviour
{
    public GameObject HoverEffect;

    private void OnTriggerEnter(Collider col)
    {
        
        if (col.gameObject.tag == "Grabable")
        {

            HoverEffect.SetActive(true);
        }
    }
    private void OnTriggerExit(Collider col)
    {

        if (col.gameObject.tag == "Grabable")
        {

            HoverEffect.SetActive(false);
        }
    }
    
}
