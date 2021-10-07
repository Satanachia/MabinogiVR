using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChickenGame_endLine : MonoBehaviour {
    private void OnTriggerEnter(Collider other)
    {
        if (!other) return;
        ChickenGame_controller controller = other.GetComponent<ChickenGame_controller>();
        if (!controller) return;


        if (controller.stopSW())
        {
            StartCoroutine(controller.foulCheck());
        }
    }
}
