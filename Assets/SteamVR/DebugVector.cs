using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugVector : MonoBehaviour
{
    private LineRenderer lineRenderer;
    public TrackerManager_edited TrackerManager;
    private Vector3 start;
    private Vector3 end;
    // Start is called before the first frame update
    void Start()
    {
        //LineRenderer Setting
        lineRenderer = GetComponent<LineRenderer>();

    }

    // Update is called once per frame
    void Update()
    {
        start = TrackerManager.hmdPos.transform.position;
        end = TrackerManager.origin.transform.position;
        lineRenderer.SetPosition(0, start);
        lineRenderer.SetPosition(1, end);
    }
}
