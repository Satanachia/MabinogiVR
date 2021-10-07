using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
using UnityEngine.Windows.Speech;

public class SpeechRecognition : MonoBehaviour
{
    public string[] keywords = new string[] { "up" };


    public ConfidenceLevel confidence = ConfidenceLevel.Medium;
    public float speed = 1;

    public GameObject target;
    public GameObject broom_H;
    public GameObject Letter;
    public GameObject clear;
    public bool is_perfect = false;

    protected PhraseRecognizer recognizer;
    protected string word = "empty";
    Vector3 end;


   private void Start()
    {
        end = broom_H.transform.position;
        
    }
    public void StartRec()
    {
        if (keywords != null)
        {
            recognizer = new KeywordRecognizer(keywords, confidence);
            recognizer.OnPhraseRecognized += Recognizer_OnPhraseRecognized;
            recognizer.Start();
            Debug.Log("START REC");
        }
        
 
    }

    private void Recognizer_OnPhraseRecognized(PhraseRecognizedEventArgs args)
    {
        word = args.text;
        Debug.Log(word);
        StartCoroutine(UpBroom());
    }

    
    private IEnumerator UpBroom()
    {
        switch (word)
        {
            case "up":
                while (Vector3.Distance(target.transform.position,end)>0.01)
                {
                    target.transform.position = (Vector3.Lerp(target.transform.position, end, Time.deltaTime));
                    yield return null;
                    
                }
                if (Letter.activeInHierarchy && is_perfect == false)//맨처음 성공했을때만 Perfect가 나옴.
                {
                    clear.SetActive(true);
                    is_perfect = true;
                }
                break;

        }
        

        
    }


    public void OnApplicationQuit()
    {
        if (recognizer != null && recognizer.IsRunning)
        {
            recognizer.OnPhraseRecognized -= Recognizer_OnPhraseRecognized;
            recognizer.Stop();
        }
    }
}
