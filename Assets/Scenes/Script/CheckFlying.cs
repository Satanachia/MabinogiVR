using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckFlying : MonoBehaviour
{
    public bool isflying=false;
    public LayerMask layermask;//Ground

    // Update is called once per frame
    void Update()
    {
        
            RaycastHit hit;
            if (!Physics.Raycast(transform.position, -transform.up, out hit, 0.5f, layermask))
            {
                isflying = true;
                
            }
            else
                isflying = false;
        Debug.DrawRay(transform.position, -transform.up, Color.red, 1f);

    }
}
