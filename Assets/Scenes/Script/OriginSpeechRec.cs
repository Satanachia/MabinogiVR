using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Windows.Speech;

public class OriginSpeechRec : MonoBehaviour
{
    public string[] keywords = new string[] { "up" };


    public ConfidenceLevel confidence = ConfidenceLevel.Medium;
    public float speed = 1;

    public GameObject target;
    public GameObject broom_H;

    protected PhraseRecognizer recognizer;
    protected string word = "empty";
    Vector3 end;

    private void Start()
    {
        end = broom_H.transform.position;
        if (keywords != null)
        {
            recognizer = new KeywordRecognizer(keywords, confidence);
            recognizer.OnPhraseRecognized += Recognizer_OnPhraseRecognized;
            recognizer.Start();
        }
    }

    private void Recognizer_OnPhraseRecognized(PhraseRecognizedEventArgs args)
    {
        word = args.text;
        Debug.Log(word);
    }

    private void Update()
    {


        switch (word)
        {
            case "up":
                target.transform.position = Vector3.Lerp(target.transform.position, end, Time.deltaTime);
                break;
        }

        
    }

    private void OnApplicationQuit()
    {
        if (recognizer != null && recognizer.IsRunning)
        {
            recognizer.OnPhraseRecognized -= Recognizer_OnPhraseRecognized;
            recognizer.Stop();
        }
    }
}
