using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JointGetter : MonoBehaviour {

	public GameObject label_1;
	public GameObject label_2;

	public Material LineMaterial;

	private Rigidbody rb;
	private Collider cl;

	private List<SpringJoint> joints = new List<SpringJoint> ();
	private List<LineRenderer> lines = new List<LineRenderer> ();
	private List<GameObject> textmeshes = new List<GameObject> ();

	void Start() {
		rb = GetComponent<Rigidbody> ();
		cl = GetComponent<Collider> ();
	}

	void Update() {
		// Render line
		for (int i = 0; i < lines.Count; i++) {
			LineRenderer lr = lines [i];
			SpringJoint sj = joints [i]; // Always have the same amount of joints and lines OKAY??!!
			lr.SetPosition (0, gameObject.transform.position);
			lr.SetPosition (1, sj.connectedBody.transform.position);
		}
			
		// Look at
		gameObject.transform.rotation = Camera.main.transform.rotation;

		// Force up
		rb.AddForce (0.0f, 100.0f, 0.0f);
	}

	void OnCollisionEnter (Collision col) {
		// Only collide with floating labels
		if (col.gameObject.tag != "FloatingLabel" && col.gameObject.tag != "InfoBubble") {
			Physics.IgnoreCollision (cl, col.collider);
		}
	}

	public void AddTextmesh (GameObject textmesh) {
		GameObject visualObj = textmesh.GetComponent<InfoBoxPositiioning> ().VisualObject;

		// Add a new line renderer
		LineRenderer lr = new GameObject().AddComponent<LineRenderer> ();
		lr.startWidth = 0.1f;
		lr.endWidth = 0.1f;
		lr.material = LineMaterial;
		lines.Add (lr);

		// Add a new joint
		SpringJoint sj = gameObject.AddComponent<SpringJoint> ();
		sj.connectedBody = visualObj.GetComponent<Rigidbody> ();
		joints.Add (sj);

		textmeshes.Add (textmesh);
	}

	public void Unmerge () {
		// Activate text meshes
		for (int i = 0; i < textmeshes.Count; i++) {
			GameObject textmesh = textmeshes [i];
			textmesh.SetActive (true);
			textmesh.GetComponentInParent<LineRenderer> ().enabled = true;
			textmesh.transform.position = textmesh.GetComponent<InfoBoxPositiioning>().StartPosition;
		}

		for (int i = 0; i < lines.Count; i++) {
			LineRenderer lr = lines [i];
			Destroy (lr);
		}
		lines.Clear ();

		for (int i = 0; i < joints.Count; i++) {
			SpringJoint sj = joints [i];
			Destroy (sj);
		}
		joints.Clear ();
	}
}
