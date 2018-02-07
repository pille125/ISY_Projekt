using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineGenerator : MonoBehaviour {


    //public GameObject InfoLabel;
    public GameObject ObjectWithInfo;

    LineRenderer connectionLine;

    Vector3 startposition;
    Vector3 endPosition;

	// Use this for initialization
	void Start () {

        startposition = ObjectWithInfo.transform.position;
        endPosition = this.transform.position;

        connectionLine = this.GetComponent<LineRenderer>();
        connectionLine.SetPosition(0, startposition);
        connectionLine.SetPosition(1, Camera.main.ScreenToWorldPoint(endPosition));

	}
	
	// Update is called once per frame
	void Update () {

        connectionLine.SetPosition(0, startposition);
      //  connectionLine.SetPosition(1, Camera.main.ScreenToWorldPoint(endPosition));
	}
}
