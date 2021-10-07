using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UImanager : MonoBehaviour
{
    //public TrackerManager trackerManager;
    public TrackerManager_edited trackerManager;
    public GameObject calimenu;
    public GameObject letter;
    //private SendToGoogle sendTogoogle;
    public int value =0;
    public float thres =0;
    public bool isOpen = false;

    //Effect
    public GameObject Front_VFX;
    //public GameObject Reverse_VFX;
    public GameObject Set_VFX;

    // Start is called before the first frame update
    public void Front()
    {
        trackerManager.initFootDir();
        //trackerManager.initFootPosition();
        SetThres();
        Front_VFX.SetActive(false);

    }
    //public void Reverse()
    //{
    //    trackerManager.reverseDir();
    //    Reverse_VFX.SetActive(true);
    //}
    public void SetThres()
    {
       trackerManager.setThreshold();
       Set_VFX.SetActive(true);
    }
    
    //public void GetThres()
    //{
    //    thres = trackerManager.threshold;
    //}
    //public void One()
    //{
    //    value= 1;
    //}
    //public void Two()
    //{
    //    value = 2;
    //}
    //public void Three()
    //{
    //    value = 3;
    //}
    //public void Four()
    //{
    //    value = 4;
    //}
    //public void Five()
    //{
    //    value = 5;
    //}
    //public void Restart()
    //{
        
    //    Front_VFX.SetActive(false);
    //    Reverse_VFX.SetActive(false);
    //    Set_VFX.SetActive(false);

    //    calimenu.SetActive(true);
    //    letter.SetActive(false);
    //}
    //public void Done()
    //{
    //    calimenu.SetActive(false);
    //    letter.SetActive(false);
    //}
    public void Calimenu()
    {
        //Front_VFX.SetActive(false);
        //Reverse_VFX.SetActive(false);
        //Set_VFX.SetActive(false);

        if(isOpen == true)
        {
            calimenu.SetActive(false);
            isOpen = false;
        }
        else
        {
            calimenu.SetActive(true);
            isOpen = true;
        }
        
        //letter.SetActive(false);

    }
    //public void Result1()
    //{
    //    //클릭하면 UI매니저에서 값을 받아와서 센드투구글에 result1에 넣어주기
    //    sendTogoogle.Result1 = value;
    //    value = 0;//초기화,다음 질문에 값이 넘어가면 안됨.
    //}
    //public void Result2()
    //{
    //    sendTogoogle.Result2 = value;
    //    value = 0;
    //}
    //public void Result3()
    //{
    //    sendTogoogle.Result3 = value;
    //    value = 0;
    //}
    //public void Result4()
    //{
    //    sendTogoogle.Result4 = value;
    //    value = 0;
    //}
    //public void Result5()
    //{
    //    sendTogoogle.Result5 = value;
    //    value = 0;
    //}
    //public void Result6()
    //{
    //    sendTogoogle.Result6 = value;
    //    value = 0;
    //}
}
