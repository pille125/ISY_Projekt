using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCamera : MonoBehaviour {

    public String nameOfToolBelt = "ToolBelt";
    public String mainCamName;
    public List<String> toolNames;
    public float maxAngle = 20.0f;
    public float offsetDown = -1.0f;
    public float angleBetweenTools = 2.0f;
    public float distanceFromViewer = 1.0f;

    private GameObject mainCam;
    private Vector3 toolRelevantForward;
    private List<GameObject> tools;
    private List<Vector3> offsetsFromCamera;
    private ToolBelt toolBelt;

    private void Start()
    {
        if (nameOfToolBelt != null)
            this.toolBelt = GameObject.Find(nameOfToolBelt).GetComponent(typeof(ToolBelt)) as ToolBelt;
        if (this.mainCamName != null)
        {
            this.mainCam = GameObject.Find(mainCamName);
            if (mainCam != null)
                this.toolRelevantForward = new Vector3(mainCam.transform.forward.x, 0.0f, mainCam.transform.forward.z);
        }
        else
            Debug.Log("you forgot to give me the name of the maincamera, so i cannot find and follow her.");
        this.tools = new List<GameObject>();
        if(toolNames != null)
        {
            foreach (String name in toolNames)
            {
                GameObject tool = GameObject.Find(name);
                if (tool != null)
                {
                    tools.Add(tool);
                    Debug.Log("FOUND TOOL " + name);
                    if(this.toolBelt != null)
                    {
                        this.toolBelt.addTool(tool);
                    }
                }
                else
                    Debug.Log("TOOL " + name + " DOES NOT EXIST IN THAT SCENE");
            }
        }
    }

    // Update is called once per frame
    void Update () {
        try
        {
            if (mainCam != null)
            {
                Vector3 lookDir = new Vector3(mainCam.transform.forward.x, 0.0f, mainCam.transform.forward.z);
                float angle = Vector3.Angle(toolRelevantForward, lookDir);
                if (angle > maxAngle)
                {
                    float diff = angle - maxAngle;
                    Vector3 up = Vector3.Cross(toolRelevantForward.normalized, lookDir.normalized);
                    Quaternion newRotation = Quaternion.AngleAxis(diff, up);
                    toolRelevantForward = newRotation * toolRelevantForward;
                }
                this.moveToolBelt();
                calculateOffsetsFromCamera();
            }
            else if (mainCam == null)
                Debug.Log("the maincamera is not given");
        }
        catch(Exception e)
        {
            Debug.LogError(e.Message);
        }
        
	}

    private void moveToolBelt()
    {
        this.transform.position = mainCam.transform.position;
        Quaternion rotation = Quaternion.LookRotation(toolRelevantForward);
        this.transform.rotation = rotation;
    }

    private void calculateOffsetsFromCamera()
    {
        int count = tools.Count - 1;
        if (count < 0) count = 0;
        float totalAngle = count * angleBetweenTools;
        Quaternion startRotation = Quaternion.AngleAxis(totalAngle * -0.5f, this.transform.up);
        Vector3 startVec = startRotation * toolRelevantForward;
        Quaternion stepRotation = Quaternion.AngleAxis(angleBetweenTools, this.transform.up);
        for (int i = 0; i < tools.Count; i++)
        {
            Vector3 offset = new Vector3(startVec.x, startVec.y, startVec.z).normalized * distanceFromViewer;
            tools[i].transform.position = this.transform.position + offset + this.transform.up.normalized * offsetDown;
            tools[i].transform.rotation = Quaternion.LookRotation(offset);
            startVec = stepRotation * startVec;
        }
    }
}
