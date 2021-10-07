using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChickenGame_Foul : MonoBehaviour {

    private void OnTriggerEnter(Collider other)
    {
        if (!other) return;
        ChickenGame_controller controller = other.GetComponent<ChickenGame_controller>();
        if (!controller) return;
        controller.isFoul = true;
        if(controller.controllerText) controller.controllerText.text = "FOUL";
        StartCoroutine(controller.foulCheck());
    }
}
