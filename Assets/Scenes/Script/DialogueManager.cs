using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    public TextMesh dialogueText;
    public Renderer rend;
    public GameObject letter;
    //public GameObject snitch;
    public TrackerManager trackerManager;
    private Queue<string> sentences;

    public GameObject frontUI;
    public GameObject reverseUI;
    public GameObject setUI;
    public GameObject RestartUI;

    public GameObject Calimenu;
    public GameObject grabcol;

    public float Istimer = 0;
    public float timer = 0;

   




    void Start()
    {
        sentences = new Queue<string>();
        rend.material.shader = Shader.Find("Custom/DissolveFont");
        


    }
    public void StartDialogue(Dialogue dialogue)
    {
        timer = 0;
        sentences.Clear();

        foreach (string sentence in dialogue.sentences)
        {
            sentences.Enqueue(sentence);

        }
        trackerManager.enabled = true;
        DisplayNextSentence();

    }

    public void DisplayNextSentence()
    {
        Istimer = 0;
        Debug.Log("DisplayNextSentence started. Istimer = 0");
       /* if (snitch.activeInHierarchy)
        {
            snitch.SetActive(false);
        }*/
        if (sentences.Count == 0)
        {
            
            EndDialogue();
            Istimer = 0;
            return;
        }
        

        string sentence = sentences.Dequeue();

        dialogueText.text = sentence;
        StartCoroutine(TimerOn());

        if (sentences.Count == 4)
        {
            frontUI.SetActive(true);
        }
        if (sentences.Count == 3)
        {
            frontUI.SetActive(false);
            reverseUI.SetActive(true);
        }
        if (sentences.Count == 2)
        {
            reverseUI.SetActive(false);
            setUI.SetActive(true);
        }
        if (sentences.Count == 1)
        {
        
            setUI.SetActive(false);
         
        }
        if (sentences.Count == 0)
        {
            RestartUI.SetActive(true);
            
            
        }

    }
   
    IEnumerator TimerOn()
    {
        timer = 0;
        Istimer = 1;//timer on
        Debug.Log("Istimer : " + Istimer.ToString());
        yield return null;// new WaitForSecondsRealtime(2f);
        //Istimer = 0;//timer off
        //Debug.Log("Istimer : " + Istimer.ToString());
        //Debug.Log(sentences.Count);
    }

    void EndDialogue()
    {
        Debug.Log("End of conversation.");
        letter.SetActive(false);
        grabcol.SetActive(false);
        RestartUI.SetActive(false);

    }

    void Update()
    {
        if (Istimer == 1 && timer<2.0f)
        {
            timer += Time.deltaTime;
            rend.material.SetFloat("_UnityTime", timer);
            //Debug.Log(timer);
        }

    }
}

