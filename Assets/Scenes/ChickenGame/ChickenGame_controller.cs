using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChickenGame_controller : MonoBehaviour
{
    private int playerNum = 0;
    private System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();
    private long record = long.MaxValue;
    private Canvas controllerCanvas;
    private GameObject controllerTextObj;
    public UnityEngine.UI.Text controllerText;
    public GameObject controllerTextPrefab;
    /*
    private GameObject recordDisplay;
    public TextMesh RDTM; //record display text mesh
    */
    private bool isFoulChecking = false;

    public ChickenGame_scoreBoard ScoreBoard;
    public Material recordDisplayMaterial;
    public Font recordDisplayFont;
    public long lastScore;
    public int PlayerNum
    {
        get { return playerNum; }
        set
        {
            if (value < 0) return;
            playerNum = value;
        }
    }
    public long Record { get { return record; } }
    public bool isFoul;


    // Use this for initialization
    void Start()
    {
        controllerCanvas = transform.GetChild(0).GetComponent<Canvas>();
    }

    public void startSW()
    {
        Debug.Log("startSW-----------------------------------");
        Debug.Log("sw.IsRunning : " + sw.IsRunning.ToString());
        Debug.Log("------------------------------------------");
        if (isFoulChecking) return;
        if (sw.IsRunning) sw.Stop();
        sw.Reset();
        sw.Start();
        isFoul = false;
    }
    public bool stopSW()
    {
        Debug.Log("stopSW------------------------------------");
        Debug.Log("sw.IsRunning : "+sw.IsRunning.ToString());
        Debug.Log("sw.ElapsedMilliseconds : " + sw.ElapsedMilliseconds);
        Debug.Log("------------------------------------------");

        if (!sw.IsRunning) return false;
        sw.Stop();
        lastScore = sw.ElapsedMilliseconds;

        Debug.Log(controllerTextObj);
        if (controllerTextObj)
        {
            Debug.Log("trying to destroy controllerText");
            Destroy(controllerTextObj);
        }
        controllerTextObj = Instantiate(controllerTextPrefab, controllerCanvas.transform.position, new Quaternion(), controllerCanvas.transform);
        
        controllerText = controllerTextObj.GetComponent<UnityEngine.UI.Text>();
        /*
        RectTransform controllerTextRT = controllerTextObj.GetComponent<RectTransform>();
        controllerTextRT.sizeDelta = new Vector2(4000f, 300f);
        controllerTextRT.anchorMin = new Vector2(0.5f, 0.5f);
        controllerTextRT.anchorMax = new Vector2(0.5f, 0.5f);
        controllerTextRT.pivot = new Vector2(0.5f, 0.5f);
        controllerTextRT.localScale = new Vector3(0.001f, 0.001f, 1);
        UnityEngine.UI.Text controllerText = controllerTextObj.AddComponent<UnityEngine.UI.Text>();
        controllerText.fontSize = 140;
        UnityEngine.UI.Outline controllerTextOL = controllerTextObj.AddComponent<UnityEngine.UI.Outline>();
        controllerTextOL.effectColor = new Color(0f, 0f, 0f, 255f);
        controllerTextOL.effectDistance = new Vector2(4f, -4f);
        controllerText.text = */

        /*
        RDTM = recordDisplay.AddComponent<TextMesh>();
        RDTM.transform.localScale = new Vector3(0.01f, 0.01f, 0.05f);
        RDTM.fontSize = 200;
        RDTM.anchor = TextAnchor.MiddleCenter;
        RDTM.alignment = TextAlignment.Center;
        RDTM.gameObject.GetComponent<MeshRenderer>().materials[0] = recordDisplayMaterial;
        RDTM.gameObject.GetComponent<Renderer>().material = recordDisplayMaterial;
        RDTM.font = recordDisplayFont;
        Destroy(recordDisplay, 7);
        */

        //Destroy(controllerTextObj, 7);
        if (lastScore < record)
        {
            controllerText.text = "Best record : " + lastScore + "\n" +
                        "Current record : " + lastScore + "\n"+
                        "foul checking...";
            isFoulChecking = true;
            return true;
        }
        controllerText.text = "Best record : " + record + "\n" +
                    "Current record : " + lastScore ;
        return false;
    }
    //private bool isFoulChecking = false; already initialized 
    private bool FCMEO = false;//foulCheck multy execution obstructor
    public IEnumerator foulCheck()
    {
        Debug.Log("foulCheck---");
        if (isFoulChecking && !FCMEO)
        {
            FCMEO = true;
            yield return new WaitForSeconds(3);
            Debug.Log("afterWaitforseconds");

            if (!isFoul)
            {
                record = lastScore;
                controllerText.text = "Best record : " + record + "\n" +
                            "Current record : " + lastScore;

                scoreInfo newScore = new scoreInfo();
                newScore.id = PlayerNum;
                newScore.score = Record;
                ScoreBoard.newRecord(newScore);
            }
            isFoulChecking = false;
            FCMEO = false;
        }
        else yield return null;
    }
    /*
	
	// Update is called once per frame
	void Update () {
		
	}*/
}
