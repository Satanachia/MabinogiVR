using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR.InteractionSystem;
using UnityEngine.UI;

//SteamVR_TrackedObject_Tracker(15개) gameobject type 수정하기
//UIManager 도 gameobject type 수정하기

static class Flags
{
    public const int LEFT = 1;      //-z
    public const int RIGHT = 0;     //+z
    public const int FORWARD = 2;   //+x
    public const int BACKWARD = 3;  //-x
    public const int UP = 4;        //+y
    public const int DOWN = 5;      //-y
    public const int HALT = -1;     //Don't move
    public const int MOVE = 6;
}

public class TrackerManager_edited : MonoBehaviour
{
    //Debug
    public GameObject vecStart;//
    public GameObject hmdPos;
    public Vector3 leanDir;
    public float leanMult = 2.0f;
    public float angle;

    public GameObject origin;
    //public Vector3 height = new Vector3(0,0,0);

    //sound
    public AudioSource footstep_normal;
    public AudioSource footstep_grass;


    public List<GameObject> TrackerList = new List<GameObject>();
    public GameObject Hmd;
 
    public GameObject[] footDir = new GameObject[2];
    public Vector3 firstMoveDir;
    private bool makeDir = false;

    public GameObject TrackerL;
    public GameObject TrackerR;

    

    public BodyCollider bodycollider;
    public IEnumerator moveIEnumerator;

    public float[] threshold = {1, 1};

    private int walkLeftFlag;
    private int walkRightFlag;
    
    private bool isLean = false;//
    private bool isTrackerSet = false;
    private bool initFootDirFlag = false;

    public float moveSpeed = 3.0f;


    //public bool isLeftFootMove;
    //public bool isRightFootMove;
    //public GameObject LeftFoot;
    //public GameObject RightFoot;
    //public Vector3 maxDis = new Vector3(0, 0, 0); //특정 발 하나가 움직일 때 움직이지 않는 다른 발과의 거리의 최댓값


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

                
                isTrackerSet = !isTrackerSet;
            }
            yield return null;
        }
    }

    public void setThreshold()
    {
        threshold[Flags.RIGHT] = TrackerR.transform.localPosition.y * 1.05f;
        threshold[Flags.LEFT] = TrackerL.transform.localPosition.y * 1.05f;

    }

    void Update()
    {

        if (isTrackerSet && initFootDirFlag)//front눌러야 됨.&& !bodycollider.isflying
        {
            if (checkLeanState() == false)
            {
                legacy_checkWalkForward();

            }
            else//isLean == true
            {
                //origin.transform.position = ((TrackerL.transform.position + TrackerR.transform.position) / 2) + height;
                origin.transform.position = ((TrackerL.transform.position + TrackerR.transform.position) / 2);
                legacy_checkWalkSide();

            }


        }
        
    }



    void getFootDir()//Step1
    {
        Vector3 trackersVec = TrackerList[Flags.LEFT].transform.position - TrackerList[Flags.RIGHT].transform.position;
        trackersVec.y = 0;
        
        Quaternion Y_Rot_90 = Quaternion.Euler(0f, 90f, 0f);

        firstMoveDir = Y_Rot_90 * trackersVec;
        Vector3 hmdVector = Hmd.transform.forward;
        hmdVector.y = 0;

        if (Mathf.Abs(GetAngle(firstMoveDir, hmdVector)) > 90)//controller로 바꾸기
            firstMoveDir = -firstMoveDir;

        Debug.Log(firstMoveDir);
        //firstMoveDir.Normalize();
    }

    public void GetleanDir()
    {
        //Vector3 hmdvector = Hmd.transform.localPosition;
        Vector3 hmdvector = Hmd.transform.position;
        
        hmdPos.transform.position = hmdvector;//Debug hmdvector

        //Vector3 playerOrigin = Vector3.zero;//bodycollider.transform.localPosition; 
        Vector3 playerOrigin = origin.transform.position;

        leanDir = (hmdvector - playerOrigin);

        leanDir.y = 0;
        


        //////////Debug///////////
        vecStart.transform.rotation = Quaternion.LookRotation(-leanDir);
        //Debug.DrawRay(vecStart.transform.position, leanDir, Color.red, 20, false);
        //moveDir은 leanDir을 정사영시킨거임 x,z만 남기고 y는 없애부러
    }

    public bool checkLeanState()
    {
        GetleanDir();//얘는 항상 쳌크하면서 lean dir업뎃해야됨.
        return true;//Debug
       // return isLean;//기울였는지 안 기울였는지 체크 bool isLean
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
        footDir[Flags.LEFT].transform.localPosition = new Vector3(0, 0, 0);//여기에서 footDir의 local position을 계속 부모인 tracker의 원점에 있게 만들어버림.
        footDir[Flags.LEFT].transform.rotation = Quaternion.LookRotation(firstMoveDir);//footDir은 tracker의 자식이니까 tracker에 상대적인 좌표계(tracker의 로컬좌표계)인 셈이고 rotation값이 부모의 rotation값에 영원히 상대적이기때문에 부모의 rotation이 바뀔때마다 firstMoveDir만큼 바뀔것이다. 그소리는 굳이 update에서 계속 앞방향 벡터를 정해주지 않아도 알아서 계속 앞방향만 가리키는 벡터가 된다는 소리;
 
        footDir[Flags.RIGHT].transform.localPosition = new Vector3(0, 0, 0);
        footDir[Flags.RIGHT].transform.rotation = Quaternion.LookRotation(firstMoveDir);

        initFootDirFlag = true;
    }

    public void reverseDir()
    {
        firstMoveDir = -firstMoveDir;
        
    }

   
    //public void initFootPosition()
    //{
    //    //Init Foot collider's position
    //    LeftFoot.transform.position = TrackerL.transform.position;
    //    RightFoot.transform.position = TrackerR.transform.position;

    //    maxDis =  new Vector3((LeftFoot.transform.localPosition.x + RightFoot.transform.localPosition.x) / 2, (LeftFoot.transform.localPosition.y + RightFoot.transform.localPosition.y) / 2, (LeftFoot.transform.localPosition.z + RightFoot.transform.localPosition.z) / 2);

    //    TrackerL.transform.name = "TrackerL";
    //    TrackerR.transform.name = "TrackerR";

    //}
    


    /*
     * Code from TrackerManager.cs
    */
    void legacy_checkWalkForward()
    {
        if (walkLeftFlag + walkRightFlag == 0)           ///두 발 다 바닥에 닿았을 때
        {
            if (TrackerL.transform.localPosition.y < threshold[Flags.LEFT] && TrackerR.transform.localPosition.y > threshold[Flags.RIGHT])
            {
                moveIEnumerator = translateMove(1);
                StartCoroutine(moveIEnumerator);
                walkRightFlag = 1;
            }
            else if (TrackerL.transform.localPosition.y > threshold[Flags.LEFT] && TrackerR.transform.localPosition.y < threshold[Flags.RIGHT])
            {
                moveIEnumerator = translateMove(0);
                StartCoroutine(moveIEnumerator);
                walkLeftFlag = 1;
            }
        }
        else if (walkLeftFlag + walkRightFlag == 1)       ///첫 발 떼었을 때
        {
            if (TrackerL.transform.localPosition.y < threshold[Flags.LEFT] && TrackerR.transform.localPosition.y < threshold[Flags.RIGHT])
            {
                if (walkLeftFlag == 1)
                {
                    walkLeftFlag = 2;
                }
                else
                {
                    walkRightFlag = 2;
                }
                StopCoroutine(moveIEnumerator);

            }
        }
        else if (walkLeftFlag + walkRightFlag == 2)      ///첫 발 내려놨을 때
        {
            if (walkLeftFlag == 2)
            {
                if (TrackerL.transform.localPosition.y < threshold[Flags.LEFT] && TrackerR.transform.localPosition.y > threshold[Flags.RIGHT])
                {
                    moveIEnumerator = translateMove(1);
                    StartCoroutine(moveIEnumerator);
                    walkRightFlag = 1;
                }
            }
            else
            {
                if (TrackerL.transform.localPosition.y > threshold[Flags.LEFT] && TrackerR.transform.localPosition.y < threshold[Flags.RIGHT])
                {
                    moveIEnumerator = translateMove(0);
                    StartCoroutine(moveIEnumerator);
                    walkLeftFlag = 1;
                }
            }
        }
        else if (walkLeftFlag + walkRightFlag == 3)      ///두번째 발 떼었을 때
        {
            if (TrackerL.transform.localPosition.y < threshold[Flags.LEFT] && TrackerR.transform.localPosition.y < threshold[Flags.RIGHT])
            {
                if (walkLeftFlag == 1)
                {
                    walkLeftFlag = 2;
                }
                else
                {
                    walkRightFlag = 2;
                }
                StopCoroutine(moveIEnumerator);
            }
        }
        else if (walkLeftFlag + walkRightFlag == 4)
        {
            walkLeftFlag = 0;
            walkRightFlag = 0;
            
        }
    }

    void legacy_checkWalkSide()
    {
        if (walkLeftFlag + walkRightFlag == 0)           ///두 발 다 바닥에 닿았을 때
        {
            if (TrackerL.transform.localPosition.y < threshold[Flags.LEFT] && TrackerR.transform.localPosition.y > threshold[Flags.RIGHT])
            {
                moveIEnumerator = translateSideMove(leanDir); 
                StartCoroutine(moveIEnumerator);
                walkRightFlag = 1;
            }
            else if (TrackerL.transform.localPosition.y > threshold[Flags.LEFT] && TrackerR.transform.localPosition.y < threshold[Flags.RIGHT])
            {
                moveIEnumerator = translateSideMove(leanDir);
                StartCoroutine(moveIEnumerator);
                walkLeftFlag = 1;
            }
        }
        else if (walkLeftFlag + walkRightFlag == 1)       ///첫 발 떼었을 때
        {
            if (TrackerL.transform.localPosition.y < threshold[Flags.LEFT] && TrackerR.transform.localPosition.y < threshold[Flags.RIGHT])
            {
                if (walkLeftFlag == 1)
                {
                    walkLeftFlag = 2;
                }
                else
                {
                    walkRightFlag = 2;
                }
                StopCoroutine(moveIEnumerator);

            }
        }
        else if (walkLeftFlag + walkRightFlag == 2)      ///첫 발 내려놨을 때
        {
            if (walkLeftFlag == 2)
            {
                if (TrackerL.transform.localPosition.y < threshold[Flags.LEFT] && TrackerR.transform.localPosition.y > threshold[Flags.RIGHT])
                {
                    moveIEnumerator = translateSideMove(leanDir);
                    StartCoroutine(moveIEnumerator);
                    walkRightFlag = 1;
                }
            }
            else
            {
                if (TrackerL.transform.localPosition.y > threshold[Flags.LEFT] && TrackerR.transform.localPosition.y < threshold[Flags.RIGHT])
                {
                    moveIEnumerator = translateSideMove(leanDir);
                    StartCoroutine(moveIEnumerator);
                    walkLeftFlag = 1;
                }
            }
        }
        else if (walkLeftFlag + walkRightFlag == 3)      ///두번째 발 떼었을 때
        {
            if (TrackerL.transform.localPosition.y < threshold[Flags.LEFT] && TrackerR.transform.localPosition.y < threshold[Flags.RIGHT])
            {
                if (walkLeftFlag == 1)
                {
                    walkLeftFlag = 2;
                }
                else
                {
                    walkRightFlag = 2;
                }
                StopCoroutine(moveIEnumerator);
            }
        }
        else if (walkLeftFlag + walkRightFlag == 4)
        {
            walkLeftFlag = 0;
            walkRightFlag = 0;

        }
    }
    /*
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
     */
    IEnumerator translateMove(int i)
    {
        footstep_normal.Play(); //이거 대신 발자국사운드 넣으면 됨여.

        Vector3 dest = footDir[i].transform.forward - new Vector3(0, footDir[i].transform.forward.y, 0);//움직이는 방향
        
        float moveTime = 0;
        while (moveTime < 1f)
        {
            transform.Translate(dest * moveSpeed * Time.deltaTime);
            moveTime += Time.deltaTime;
            yield return null;
        }
        
    }

    IEnumerator translateSideMove(Vector3 dest)
    {
        //Sound play
        footstep_normal.Play(); Debug.Log("Play");//이거 대신 발자국사운드 넣으면 됨여.

        //Vector3 dest = 움직이는 방향
        dest.Normalize();
        float moveTime = 0;
        while (moveTime < 1f)
        {
            transform.Translate(dest * moveSpeed * Time.deltaTime);
            moveTime += Time.deltaTime;
            yield return null;
        }

    }

    public void SpeedUp()
    {

        moveSpeed = moveSpeed + 0.1f;
    }
    public void SpeedDown()
    {

        moveSpeed = moveSpeed - 0.1f;
    }

}
