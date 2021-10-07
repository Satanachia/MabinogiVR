using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR.InteractionSystem;


public class TrackerManager_test : MonoBehaviour
{
    public GameObject Hmd;
    public List<GameObject> TrackerList = new List<GameObject>();
 
    public GameObject[] footDir = new GameObject[2];
    public Vector3 firstMoveDir;
    public Vector3 maxDis = new Vector3(0, 0, 0); //특정 발 하나가 움직일 때 움직이지 않는 다른 발과의 거리의 최댓값
    private bool makeDir = false;

    public GameObject TrackerL;
    public GameObject TrackerR;

    public GameObject LeftFoot;
    public GameObject RightFoot;

    public BodyCollider bodycollider;
    public IEnumerator moveIEnumerator;

    public float threshold = 0.04f;
    public float test_threshold = 0.8f;

    private int walkLeftFlag;
    private int walkRightFlag;
    public bool isLeftFootMove;
    public bool isRightFootMove;
    private bool isTrackerSet = false;
    private bool initFootDirFlag = false;

    public float moveSpeed = 2f;

    // Use this for initialization
    void Awake()
    {
        StartCoroutine(getTrackerObject());

    }

    IEnumerator getTrackerObject()
    {
        while (!isTrackerSet)
        {
            if (TrackerList.Count == 2)
            {
                /*
                 * 월드 좌표계에서 y방향(위쪽) 이 Up이고, 플레이어의 발 방향이 firstmoveDir이며, 플레이어와 각 트래커 간에 생기는 벡터를 V라 할때,
                 * Up 과 (firstmoveDir x V) 의 내적 > 0 이면 오른쪽, < 0 이면 왼쪽(x는 외적).
                 * 외적으로 생긴 벡터의 방향이 Up과 이루는 각을 세타라고 할때, 그 내적은 세타가 예각이면 cos(세타) > 0인 것을 이용!
                 */
                getFootDir();

                if (Vector3.Dot(Vector3.up, Vector3.Cross(TrackerList[Flags.LEFT].transform.position, firstMoveDir)) < 0)
                {
                    TrackerL = TrackerList[Flags.LEFT];
                    TrackerR = TrackerList[Flags.RIGHT];
                }
                else
                {
                    TrackerR = TrackerList[Flags.LEFT];
                    TrackerL = TrackerList[Flags.RIGHT];
                }

               // TrackerL = TrackerList[Flags.LEFT];
               // TrackerR = TrackerList[Flags.RIGHT];
                isTrackerSet = !isTrackerSet;
            }
            yield return null;
        }
    }

    void Update()
    {
        if (Input.GetKeyDown("space"))
        {
            initFootDir();
        }

        if (isTrackerSet && initFootDirFlag)
        {
            legacy_checkWalkForward();
        }
    }

    ////////////// Check Walk State ////////////////////

    void getFootDir()//Step1
    {
        Vector3 trackersVec = TrackerList[Flags.LEFT].transform.position - TrackerList[Flags.RIGHT].transform.position;
        trackersVec.y = 0;
        
        Quaternion Y_Rot_90 = Quaternion.Euler(0f, 90f, 0f);

        firstMoveDir = Y_Rot_90 * trackersVec;
        Vector3 hmdVector = Hmd.transform.forward;
        hmdVector.y = 0;

        if (Mathf.Abs(GetAngle(firstMoveDir, hmdVector)) < 90)
            firstMoveDir = -firstMoveDir;

        //firstMoveDir.Normalize();
    }

    public static float GetAngle(Vector3 vStart, Vector3 vEnd)
    {
        Vector3 v = vEnd - vStart;

        return Mathf.Atan2(v.y, v.x) * Mathf.Rad2Deg;
    }


    public void initFootDir()
    {
        getFootDir();

        if (makeDir == false)
        {
            footDir[Flags.LEFT] = Instantiate(new GameObject());
            footDir[Flags.LEFT].transform.parent = TrackerList[Flags.LEFT].transform;

            footDir[Flags.RIGHT] = Instantiate(new GameObject());
            footDir[Flags.RIGHT].transform.parent = TrackerList[Flags.RIGHT].transform;

            makeDir = true;
        }
        footDir[Flags.LEFT].transform.localPosition = new Vector3(0, 0, 0);
        footDir[Flags.LEFT].transform.rotation = Quaternion.LookRotation(firstMoveDir);

        footDir[Flags.RIGHT].transform.localPosition = new Vector3(0, 0, 0);
        footDir[Flags.RIGHT].transform.rotation = Quaternion.LookRotation(firstMoveDir);

        initFootDirFlag = true;
    }

    public void reverseDir()
    {
        firstMoveDir = -firstMoveDir;
        
    }

    public void updateThreshold()
    {
        float height;
        if (TrackerL.transform.localPosition.y > TrackerR.transform.localPosition.y)
        {
            height = TrackerL.transform.localPosition.y;
            threshold = (height + TrackerR.transform.localPosition.y) / 2;
        }
        else
        {
            height = TrackerR.transform.localPosition.y;
            threshold = (height + TrackerL.transform.localPosition.y) / 2;
        }
    }

    /*
     * Code from TrackerManager.cs
    */
    void legacy_checkWalkForward()
    {
        if (walkLeftFlag + walkRightFlag == 0)           ///두 발 다 바닥에 닿았을 때
        {
            if (TrackerL.transform.localPosition.y < threshold && TrackerR.transform.localPosition.y > threshold)
            {
                walkRightFlag = 1;
            }
            else if (TrackerL.transform.localPosition.y > threshold && TrackerR.transform.localPosition.y < threshold)
            {
                walkLeftFlag = 1;
            }
        }
        else if (walkLeftFlag + walkRightFlag == 1)       ///첫 발 떼었을 때
        {
            if (TrackerL.transform.localPosition.y < threshold && TrackerR.transform.localPosition.y < threshold)
            {
                if (walkLeftFlag == 1)
                {
                    moveIEnumerator = translateMove(0);
                    StartCoroutine(moveIEnumerator);
                    walkLeftFlag = 2;
                }
                else
                {
                    moveIEnumerator = translateMove(1);
                    StartCoroutine(moveIEnumerator);
                    walkRightFlag = 2;
                }
            }
        }
        else if (walkLeftFlag + walkRightFlag == 2)      ///첫 발 내려놨을 때
        {
            if (walkLeftFlag == 2)
            {
                if (TrackerL.transform.localPosition.y < threshold && TrackerR.transform.localPosition.y > threshold)
                {
                    walkRightFlag = 1;
                }
            }
            else
            {
                if (TrackerL.transform.localPosition.y > threshold && TrackerR.transform.localPosition.y < threshold)
                {
                    walkLeftFlag = 1;
                }
            }
            StopCoroutine(moveIEnumerator);
        }
        else if (walkLeftFlag + walkRightFlag == 3)      ///두번째 발 떼었을 때
        {
            if (TrackerL.transform.localPosition.y < threshold && TrackerR.transform.localPosition.y < threshold)
            {
                if (walkLeftFlag == 1)
                {
                    moveIEnumerator = translateMove(0);
                    StartCoroutine(moveIEnumerator);
                    walkLeftFlag = 2;
                }
                else
                {
                    moveIEnumerator = translateMove(1);
                    StartCoroutine(moveIEnumerator);
                    walkRightFlag = 2;
                }
            }
        }
        else if (walkLeftFlag + walkRightFlag == 4)
        {
            walkLeftFlag = 0;
            walkRightFlag = 0;
            StopCoroutine(moveIEnumerator);
        }
    }

    IEnumerator MoveForward(int i)
    {
        Vector3 dest = transform.position + footDir[i].transform.forward - new Vector3(0, footDir[i].transform.forward.y, 0);
        //발걸음 소리 넣는곳

        while (Vector3.Distance(transform.position, dest) > 0.1)
        {
            transform.position = Vector3.MoveTowards(transform.position, dest, 3 * Time.deltaTime);
            yield return null;
        }
    }

    IEnumerator translateMove(int i)
    {
        Vector3 dest = footDir[i].transform.forward - new Vector3(0, footDir[i].transform.forward.y, 0);

        float moveTime = 0;
        while (moveTime < 1.5f)
        {
            transform.Translate(dest * moveSpeed * Time.deltaTime);
            moveTime += Time.deltaTime;
            yield return null;
        }
    }

}
