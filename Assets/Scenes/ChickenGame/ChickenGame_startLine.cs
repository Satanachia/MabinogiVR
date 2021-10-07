using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChickenGame_startLine : MonoBehaviour {

    private int lastPlayerNum = 0;
    
    private void OnTriggerExit(Collider other)
    {
        if (!other) return;
        ChickenGame_controller controller = other.GetComponent<ChickenGame_controller>();
        if (!controller) return;
        
        if (controller.PlayerNum == 0) controller.PlayerNum = ++lastPlayerNum;
        controller.startSW();
    }
}
