using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawLineBetween : MonoBehaviour {

	public GameObject start;
	public GameObject end;

	private LineRenderer lr;

	// Use this for initialization
	void Start () {
		lr = GetComponent<LineRenderer> ();
	}
	
	// Update is called once per frame
	void Update () {

		lr.SetPosition (0, start.transform.position);
		lr.SetPosition (1, end.transform.position);
	}
}
