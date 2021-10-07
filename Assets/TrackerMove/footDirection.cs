using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class footDirection : MonoBehaviour
{
     public TrackerManager_edited trackerManager;
    //public TrackerManager trackerManager;
    // Update is called once per frame
    void Update()
    {
        transform.rotation = Quaternion.LookRotation(-trackerManager.firstMoveDir);//이거 왜 마이너스임?ㄷㄷ
    }
}
