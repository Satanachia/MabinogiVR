using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowSpeed : MonoBehaviour
{
    public TextMesh speedPanel;
    public TrackerManager_edited player;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        speedPanel.text = "speed"+ "\n"+ "▲" + "\n" + player.moveSpeed.ToString()  + "\n" +"▼";
    }
}
