using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimTransition : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject walkObject;
    public GameObject idleObject;
    void WalkStart()
    {
        walkObject.SetActive(true);
        idleObject.SetActive(false);
        Debug.Log("Start Walking!");
    }
}
