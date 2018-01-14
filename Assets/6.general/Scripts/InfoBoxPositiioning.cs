using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InfoBoxPositiioning : MonoBehaviour {

	public GameObject VisualObject;
	public float MinScale = 0.1f;
	public float MaxScale = 1.0f;
	public float MinDistance = 1.0f;
	public float MaxDistance = 20.0f;
	public GameObject distanceText;
	public Vector3 StartPosition;
	public bool isDragging = false;

	private Rigidbody rb;
	private Collider cl;
	private List<GameObject> currentCollisions = new List<GameObject>();

	// Use this for initialization
	void Start () {

		StartPosition = VisualObject.transform.position + new Vector3 (0.0f, 1.0f, 0.0f);
		transform.position = StartPosition;

		rb = GetComponent<Rigidbody> ();
		cl = GetComponent<Collider> ();
	}
	
	// Update is called once per frame
	void Update () {
		UpdateText ();

		//transform.LookAt (Player.transform.position);
		transform.rotation = Camera.main.transform.rotation;
		rb.AddForce (0.0f, 20.0f, 0.0f);

		// Resize according to distance to camera
		float distanceToCamera = Vector3.Distance(Camera.main.transform.position, transform.position);
		float scale = Mathf.Clamp(distanceToCamera / 20.0f, 0.0f, 1.0f);
		transform.localScale = new Vector3 (scale, scale, scale);
	}

	private void UpdateText () {
		float distance = Vector3.Distance (VisualObject.transform.position, Camera.main.transform.position);
		TextMesh tm = distanceText.GetComponent<TextMesh> ();
		if (tm != null) {
			tm.text = "Dist.: " + distance.ToString("0.0");
		} else {
			print ("No text mesh in info box positioning script");
		}
	}

	void OnCollisionEnter (Collision col) {
		if (!isDragging) {
			// Only collide with floating labels
			if (col.gameObject.tag != "FloatingLabel" && col.gameObject.tag != "InfoBubble" && col.gameObject.tag != "FloatingLabelTarget") {
				Physics.IgnoreCollision (cl, col.collider);
			} else if (col.gameObject.tag == "FloatingLabel" || col.gameObject.tag == "InfoBubble") {
				currentCollisions.Add (col.gameObject);
			} else if (col.gameObject.tag == "FloatingLabelTarget") {
				if (currentCollisions.Contains (col.gameObject)) {
					currentCollisions.Remove (col.gameObject);
				}
			}
		}
	}

	void OnCollisionExit (Collision col) {
		if (col.gameObject.tag == "FloatingLabel" || col.gameObject.tag == "InfoBubble" || col.gameObject.tag == "FloatingLabelTarget") {
			currentCollisions.Remove (col.gameObject);
		}
	}

	public List<GameObject> GetCurrentCollisions() {
		return currentCollisions;
	}

	public void ResetStartposition() {
		StartPosition = VisualObject.transform.position + new Vector3 (0.0f, 1.0f, 0.0f);
		isDragging = false;
	}
}
