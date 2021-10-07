using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class SendToGoogle : MonoBehaviour
{
    public int Result1;
    public int Result2;
    public int Result3;
    public int Result4;
    public int Result5;
    public int Result6;
    public float Result7;

    

    private int Question_1;
    private int Question_2;
    private int Question_3;
    private int Question_4;
    private int Question_5;
    private int Question_6;
    private float Question_7;
    [SerializeField]
    private string BASE_URL = "https://docs.google.com/forms/u/1/d/e/1FAIpQLSc0OgwW4u7D_yEU8P7CribZrQLulRb5EpcrvRieCLeEUYPZ0w/formResponse";
    IEnumerator Post(int Question_1, int Question_2, int Question_3, int Question_4, int Question_5, int Question_6,float Question_7)
    {
        WWWForm form = new WWWForm();
        form.AddField("entry.1710351577", Question_1);
        form.AddField("entry.1493584159", Question_2);
        form.AddField("entry.53453002", Question_3);
        form.AddField("entry.176499490", Question_4);
        form.AddField("entry.528613236", Question_5);
        form.AddField("entry.177929947", Question_6);
        form.AddField("entry.957971386", Question_7.ToString());

        byte[] rawData = form.data;
        WWW www = new WWW(BASE_URL, rawData);
       
        yield return www;
    }
    public void Send()
    {
        Question_1 = Result1;
        Question_2 = Result2;
        Question_3 = Result3;
        Question_4 = Result4;
        Question_5 = Result5;
        Question_6 = Result6;
        Question_7 = Result7;

        StartCoroutine(Post(Question_1, Question_2, Question_3, Question_4, Question_5, Question_6,Question_7));
    }
}
