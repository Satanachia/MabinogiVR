using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Open : MonoBehaviour
{
    //Animation
    public Animator Grating_anim;
    public AudioSource openSound;

    
    private void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.tag == "Player")
        {

            StartCoroutine(Opening());

            //Beacon Activation Code will be here.
        }
    }
    
    IEnumerator Opening()
    {
        Grating_anim.SetBool("isOpen", true);
        openSound.Play(2000);
        yield return new WaitForSecondsRealtime(2f);
        
    }
}
