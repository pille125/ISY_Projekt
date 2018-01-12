using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HoloToolkit.Unity;
using HoloToolkit.Unity.InputModule;

public class LabelCameraInteraction : MonoBehaviour {

    private Vector3 camPosition;
    private GameObject Label;

    // Use this for initialization
    void Start ()
	{
        Label = this.gameObject;
    }
	
	// Update is called once per frame
	void FixedUpdate () {
        camPosition = Camera.main.transform.position;
        Label.transform.LookAt(camPosition);
    }
}
