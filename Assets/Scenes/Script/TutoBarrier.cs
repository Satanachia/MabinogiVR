using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutoBarrier : MonoBehaviour {

    MeshRenderer b_mesh;
    
    public float time = 5.0f;
    public GameObject barrierImage;
   
    // Use this for initialization
    void Start()
    {
        
        //upper = barrierImage.transform.Find("upper_border").gameObject.GetComponent<MeshRenderer>();  
        b_mesh = barrierImage.GetComponent<MeshRenderer>();
       
    }
    IEnumerator FadeOut()
    {
        for (float f = 1f; f > -0.05f; f -= 0.05f)
        {
            Color c = b_mesh.material.color;
            
            c.a = f;
            
            b_mesh.material.color = c;
            
            yield return new WaitForSeconds(0.08f);
        }
    }


    IEnumerator BarrierOn()
    {
        barrierImage.SetActive(true);
        yield return new WaitForSeconds(time);
        StartCoroutine("FadeOut");//fade로 천천히 사라지게 만들어야겠다. 투명도 높이고 ㅇㅇ
    }

   void OnCollisionEnter()//속도를 반대로하거나 아니면 아예 없애거나 조금 튕기거나? 할지말지 고민중
    {
        Debug.Log("collision enter");
        barrierImage.SetActive(true);
        //StartCoroutine (BarrierOn());
        StartCoroutine("FadeOut");

    }

    // Update is called once per frame
    void Update () {
        
	}
}
