using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackerManager : MonoBehaviour{

    public List<GameObject> TrackerList = new List<GameObject>();
    public GameObject Hmd;


    private GameObject[] footDir = new GameObject[2];
    public Vector3 firstMoveDir;
    private bool makeDir = false;

    private GameObject TrackerL;
    private GameObject TrackerR;

    public float threshold = 0.07f;

    private int walkLeftFlag;
    private int walkRightFlag;

    private bool isListed = false;

    //sound
    public AudioSource footstep_normal;
    public AudioSource footstep_grass;

    // Use this for initialization
    void Start () {
        StartCoroutine(getTrackerObject());
    }

    IEnumerator getTrackerObject()
    {
        while (!isListed)
        {
            if (TrackerList.Count == 2)
            {
                TrackerL = TrackerList[0];
                TrackerR = TrackerList[1];

                isListed = !isListed;
            }
            yield return null;
        }
    }

    void Update()
    {
        // 여기에 코드 작성하시면 됩니다 기현이형
        if (isListed)
        {
            checkWalkForward();
        }
    }


    void getFootDir()//Step1
    {
        Vector3 trackersVec = TrackerList[0].transform.position - TrackerList[1].transform.position;
        trackersVec.y = 0;

        Quaternion Y_Rot_90 = Quaternion.Euler(0f, 90f, 0f);

        firstMoveDir = Y_Rot_90 * trackersVec;
        Vector3 hmdVector = Hmd.transform.forward;
        hmdVector.y = 0;

        if (Mathf.Abs(GetAngle(trackersVec, hmdVector)) < 90)
            firstMoveDir = -firstMoveDir;

        // firstMoveDir.Normalize();
        
    }

    public static float GetAngle(Vector3 vStart, Vector3 vEnd)
    {
        Vector3 v = vEnd - vStart;

        return Mathf.Atan2(v.y, v.x) * Mathf.Rad2Deg;
    }

    public void initFootDir()
    {
        getFootDir();
        Debug.Log(firstMoveDir);


        if (makeDir == false)
        {
            footDir[0] = Instantiate(new GameObject());
            footDir[0].transform.parent = TrackerList[0].transform;

            footDir[1] = Instantiate(new GameObject());
            footDir[1].transform.parent = TrackerList[1].transform;

            makeDir = true;
        }
        footDir[0].transform.localPosition = new Vector3(0, 0, 0);
        footDir[0].transform.rotation = Quaternion.LookRotation(firstMoveDir);

        footDir[1].transform.localPosition = new Vector3(0, 0, 0);
        footDir[1].transform.rotation = Quaternion.LookRotation(firstMoveDir);
    }

    public void reverseDir()
    {
        firstMoveDir = -firstMoveDir;
        
    }

    public void updateThreshold()
    {
        Debug.Log(TrackerL.transform.position.y);
        Debug.Log(TrackerR.transform.position.y);
        Debug.Log((TrackerL.transform.position.y + TrackerR.transform.position.y) / 2);
        //threshold = (TrackerL.transform.position.y + TrackerR.transform.position.y)/2;
        float height;
        if (TrackerL.transform.position.y > TrackerR.transform.position.y)
        {
            height = TrackerL.transform.position.y;
            threshold = (height + TrackerR.transform.position.y) / 2;
        }
        else
        {
            height = TrackerR.transform.position.y;
            threshold = (height + TrackerL.transform.position.y) / 2;
        }
    }


    ////////////// Check Walk State ////////////////////
    void checkWalkForward()
    {
        if (walkLeftFlag + walkRightFlag == 0)           ///두 발 다 바닥에 닿았을 때
        {
            if (TrackerL.transform.position.y < threshold && TrackerR.transform.position.y > threshold)
            {
                walkRightFlag = 1;
            }
            else if (TrackerL.transform.position.y > threshold && TrackerR.transform.position.y < threshold)
            {
                walkLeftFlag = 1;
            }
        }
        else if (walkLeftFlag + walkRightFlag == 1)       ///첫 발 떼었을 때
        {
            if (TrackerL.transform.position.y < threshold && TrackerR.transform.position.y < threshold)
            {
                if (walkLeftFlag == 1)
                {
                    StartCoroutine(MoveForward(0));
                    walkLeftFlag = 2;
                }
                else
                {
                    StartCoroutine(MoveForward(1));
                    walkRightFlag = 2;
                }
            }
        }
        else if (walkLeftFlag + walkRightFlag == 2)      ///첫 발 내려놨을 때
        {
            if (walkLeftFlag == 2)
            {
                if (TrackerL.transform.position.y < threshold && TrackerR.transform.position.y > threshold)
                {
                    walkRightFlag = 1;
                }
            }
            else
            {
                if (TrackerL.transform.position.y > threshold && TrackerR.transform.position.y < threshold)
                {
                    walkLeftFlag = 1;
                }
            }
        }
        else if (walkLeftFlag + walkRightFlag == 3)      ///두번째 발 떼었을 때
        {
            if (TrackerL.transform.position.y < threshold && TrackerR.transform.position.y < threshold)
            {
                if (walkLeftFlag == 1)
                {
                    StartCoroutine(MoveForward(0));
                    walkLeftFlag = 2;
                }
                else
                {
                    StartCoroutine(MoveForward(1));
                    walkRightFlag = 2;
                }
            }
        }
        else if (walkLeftFlag + walkRightFlag == 4)
        {
            walkLeftFlag = 0;
            walkRightFlag = 0;
        }
    }

    IEnumerator MoveForward(int i)
    {
        
        Vector3 dest = transform.position + footDir[i].transform.forward - new Vector3(0, footDir[i].transform.forward.y, 0);
        //발걸음 소리 넣는곳
        footstep_normal.Play();

        while (Vector3.Distance(transform.position, dest) > 0.1)
        {
            transform.position = Vector3.MoveTowards(transform.position, dest, 3 * Time.deltaTime);
            yield return null;
        }
    }
}
