using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Beacon_Script : MonoBehaviour
{
    public Renderer rend_circle;
    public Renderer rend_wing;
    public float Istimer = 0;
    public float timer = 0;
    public GameObject letterSystem;

    //teleport mode
    public bool teleport_mode = false;
    public GameObject player;
    public Transform Spawnpoint;
    public Animator open_anim;

    //BGM
    public AudioSource bgm_readyroom;
    public AudioSource bgm_ground;

    // Start is called before the first frame update
    void Start()
    {
        rend_circle.material.shader = Shader.Find("Custom/BeaconCircle");
        rend_wing.material.shader = Shader.Find("Custom/BeaconWing");
    }

    // Update is called once per frame
    void Update()
    {
        if (Istimer == 1)
        {
            timer += Time.deltaTime;
            rend_circle.material.SetFloat("_UnityTime", timer);
            rend_wing.material.SetFloat("_UnityTime", timer);
            //Debug.Log(timer);
        }

    }
    private void OnTriggerEnter(Collider col)
    {
        Debug.Log(col.gameObject.name);
        if(col.gameObject.tag == "Player")
        {
            if (teleport_mode == true)
            {
                StartCoroutine(TimerOn());
                StartCoroutine( teleportTopoint());
                return;
                
            }
            StartCoroutine(TimerOn());
            Debug.Log(col.gameObject.tag);
            
            //Beacon Activation Code will be here.
        }
    }
    private void OnTriggerExit(Collider col)
    {
        
        if (col.gameObject.tag == "Player")
        {
            Istimer = 0;
        }
    }
  
    IEnumerator TimerOn()
    {
        if (teleport_mode == true)
        {
            timer = 0;
            yield return new WaitForSecondsRealtime(1f);
            Istimer = 1;//timer on
            yield return new WaitForSecondsRealtime(2f);
            Istimer = 0;//timer off
        }
        timer = 0;
        yield return new WaitForSecondsRealtime(2f);
        Istimer = 1;//timer on
        yield return new WaitForSecondsRealtime(2f);
        Istimer = 0;//timer off
       letterSystem.SetActive(true);
    }
    IEnumerator teleportTopoint()
    {
        yield return new WaitForSecondsRealtime(5f);
        player.transform.position = Spawnpoint.position;

        Debug.Log("Teleport!");
        bgm_readyroom.Stop();
        bgm_ground.Play();
        open_anim.enabled = false;
        
    }

}
