using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LabelToObjectLineRenderer : MonoBehaviour {

    public GameObject startPosObj;
    public GameObject endPosLabel;

    private LineRenderer lr;

    // Use this for initialization
    void Start()
    {
        lr = GetComponent<LineRenderer>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        lr.SetPosition(0, startPosObj.transform.position);
        lr.SetPosition(1, GetNearestLabeledgeToLineStart());
    }


    Vector3 GetNearestLabeledgeToLineStart()
    {
        Mesh cubeMesh = endPosLabel.GetComponent<MeshFilter>().mesh;
        Vector3[] vertices = cubeMesh.vertices;
        float shortestVerticeDist = 100.0f;
        Vector3 nearestEdge = endPosLabel.transform.TransformPoint(vertices[0]);

        foreach (Vector3 vert in vertices)
        {
            // get vertex in world coordinates from label edge
            Vector3 worldVertice = endPosLabel.transform.TransformPoint(vert);
            float DistToVertice = Vector3.Distance(worldVertice, startPosObj.transform.position);
            // convert to camera's local coordinates:
            if (DistToVertice < shortestVerticeDist)
            {
                shortestVerticeDist = DistToVertice;
                nearestEdge = worldVertice;
            }
        }

        return nearestEdge;
    }
}
