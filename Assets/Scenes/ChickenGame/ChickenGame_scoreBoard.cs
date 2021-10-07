using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChickenGame_scoreBoard : MonoBehaviour {

    public TextMesh text;
    public scoreInfo[] highScore;
    private const int HIGHSCOREMAXSIZE = 100;
    private int scoreBoardIdx = 0;
    public void newRecord(scoreInfo newScore)
    {
        Debug.Log("newRecord");


        scoreInfo temp1 = newScore, temp2;
        int i; for (i = 0; i < scoreBoardIdx; i++)
        {
            if (highScore[i].score <= newScore.score) continue;
            if (highScore[i].id == newScore.id)
            {
                highScore[i] = temp1;
                break;
            }
            temp2 = highScore[i];
            highScore[i] = temp1;
            temp1 = temp2;
        }
        if (i == scoreBoardIdx)
        {
            highScore[i] = temp1;
            scoreBoardIdx++;
        }
        text.text = "Scoreboard\n";
        for (i = 0; i < 5; i++)
        {
            if (highScore[i].id == 0) break;
            text.text += highScore[i].id.ToString() + " : " + highScore[i].score.ToString() + "\n";
        }
    }
	// Use this for initialization
	void Start () {
        highScore = new scoreInfo[HIGHSCOREMAXSIZE];
    }
	/*
	// Update is called once per frame
	void Update () {
		
	}*/
}
