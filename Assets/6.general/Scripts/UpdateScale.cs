using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpdateScale : MonoBehaviour {

	private Camera camera;

	// Use this for initialization
	void Start () {
		this.camera = GameObject.Find ("Main Camera").GetComponent<Camera> ();

	}
	
	// Update is called once per frame
	void Update () {
		this.transform.localScale = new Vector3 (this.camera.pixelRect.width, 1000.0f, this.camera.pixelRect.height);
	}
}
