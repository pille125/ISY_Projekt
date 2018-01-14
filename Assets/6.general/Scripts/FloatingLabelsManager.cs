using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatingLabelsManager : MonoBehaviour {

	public GameObject Camera;
	public GameObject InfoBubble;

	private List<GameObject> mergedObjects = new List<GameObject> ();

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

		foreach (Transform child in transform) {
			foreach (Transform grandchild in child) {
				GameObject go = grandchild.gameObject;
				if (go.tag == "FloatingLabel") {
					TestForMerge (go);
				}
			}
		}

		foreach (GameObject bubble in mergedObjects) {
			bool isDestroyed = TestForUnmerge (bubble);
			if (isDestroyed) {
				return;
			}
		}
	}

	private bool TestForUnmerge(GameObject bubble) {
		float distanceToCamera = Vector3.Distance (bubble.gameObject.transform.position, Camera.transform.position);
		if (distanceToCamera <= 15.0f) {
			bubble.GetComponent<JointGetter> ().Unmerge ();
			mergedObjects.Remove (bubble);
			Destroy (bubble);
			return true; // Destroyed object
		}
		return false; // Didn't destroy
	}

	private void TestForMerge(GameObject textmesh) {
		if (!textmesh.activeSelf) {
			return;
		}

		float distanceToCamera = Vector3.Distance (Camera.transform.position, textmesh.transform.position);
		if (distanceToCamera > 25.0f && textmesh.activeSelf) {
			// Collisions of this object
			List<GameObject> currentCollisions = textmesh.GetComponent<InfoBoxPositiioning> ().GetCurrentCollisions ();
			GameObject visualObj1 = textmesh.GetComponent<InfoBoxPositiioning> ().VisualObject;

			// Create bubble for this merge
			GameObject bubble = Instantiate (InfoBubble);
			Vector3 position = (visualObj1.transform.position);
			bubble.transform.position = position;

			// If there are no merges, the bubble needs to be destroyed... ugliest code ever
			bool merged = false;

			foreach (GameObject go in currentCollisions) {
				if (go != null) {
					if (go.tag == "InfoBubble") {
						textmesh.SetActive (false);
						textmesh.GetComponentInParent<LineRenderer> ().enabled = false;
						go.GetComponent<JointGetter> ().AddTextmesh (textmesh);
						Destroy (bubble);
						return;
					} else if(go.tag == "FloatingLabel") {
						if (go.activeSelf) {
							// Disable game object and add to bubble
							go.SetActive (false);
							go.GetComponentInParent<LineRenderer> ().enabled = false;
							bubble.GetComponent<JointGetter> ().AddTextmesh (go);
							merged = true;
						}
					}
				}
			}

			// Now for the origin game object of this whole mess
			if (merged) {
				textmesh.SetActive (false);
				textmesh.GetComponentInParent<LineRenderer> ().enabled = false;
				bubble.GetComponent<JointGetter> ().AddTextmesh (textmesh);
				mergedObjects.Add (bubble);
			} else {
				Destroy (bubble);
			}
		}
	}
}
