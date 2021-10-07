using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OriginPos : MonoBehaviour
{
    public TrackerManager_edited TrackerManager;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        this.transform.position = (TrackerManager.TrackerL.transform.position + TrackerManager.TrackerR.transform.position) / 2;
    }
}
