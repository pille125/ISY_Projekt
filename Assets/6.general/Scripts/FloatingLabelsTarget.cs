using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatingLabelsTarget : MonoBehaviour {

	public GameObject VisualObject;
	public GameObject Camera;
	public GameObject distanceText;

	private Rigidbody rb;
	private Collider cl;

	// Use this for initialization
	void Start () {
		transform.position = VisualObject.transform.position + new Vector3 (0.0f, 1.0f, 0.0f);

		rb = GetComponent<Rigidbody> ();
		cl = GetComponent<Collider> ();
	}

	// Update is called once per frame
	void Update () {
		UpdateText ();

		//transform.LookAt (Player.transform.position);
		transform.rotation = Camera.transform.rotation;
		rb.AddForce (0.0f, 20.0f, 0.0f);

		// Resize according to distance to camera
		float distanceToCamera = Vector3.Distance(Camera.transform.position, transform.position);
		float scale = Mathf.Clamp(distanceToCamera / 20.0f, 0.0f, 1.0f);
		transform.localScale = new Vector3 (scale, scale, scale);
	}

	private void UpdateText () {
		float distance = Vector3.Distance (VisualObject.transform.position, Camera.transform.position);
		TextMesh tm = distanceText.GetComponent<TextMesh> ();
		if (tm != null) {
			tm.text = "Dist: " + distance.ToString("0.0");
		} else {
			print ("No text mesh in info box positioning script");
		}
	}

	void OnCollisionEnter (Collision col) {
		// Only collide with floating labels
		if (col.gameObject.tag != "FloatingLabel" && col.gameObject.tag != "InfoBubble" && col.gameObject.tag != "FloatingLabelTarget") {
			Physics.IgnoreCollision (cl, col.collider);
		}
	}
}
