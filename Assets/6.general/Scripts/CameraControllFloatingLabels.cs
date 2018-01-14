using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControllFloatingLabels : MonoBehaviour {

	public GameObject CameraContainer;
	public float Speed = 10.0f;
	public float FactorOffset = 3.0f;

	private CharacterController charControll;
	private Vector3 lastMousePosition;
	private Transform cameraTransform;
	private Camera mainCamera;
	private bool dragging = false;
	private Rigidbody draggingObject;
	private bool DRBSUseGravity;
	private bool DRBSUisKinematic;

	// Use this for initialization
	void Start () {
		this.charControll = this.gameObject.GetComponent<CharacterController> ();
		this.lastMousePosition = new Vector3 (Screen.width / 2.0f, Screen.height / 2.0f, 0.0f);
		if (this.CameraContainer != null) {
			this.cameraTransform = this.CameraContainer.transform;
			this.mainCamera = this.CameraContainer.GetComponent<Camera> ();
		} else
			Debug.Log ("no Camera");
	}

	// Update is called once per frame
	void Update () {
		if (this.charControll != null) {
			if (this.cameraTransform != null) {
				Vector3 translation = this.gameObject.transform.right * Input.GetAxis ("Horizontal") * this.Speed;
				translation += this.gameObject.transform.forward * Input.GetAxis ("Vertical") * this.Speed;
				this.charControll.SimpleMove (translation);
				Vector3 mousePosition = Input.mousePosition/4;
				Vector3 mouseMovement = this.lastMousePosition - mousePosition;

				this.gameObject.transform.RotateAround (this.gameObject.transform.position, this.gameObject.transform.up, mouseMovement.x * -1.0f);
				this.cameraTransform.transform.RotateAround (this.cameraTransform.transform.position, this.cameraTransform.transform.right, mouseMovement.y);
				this.lastMousePosition = new Vector3 (mousePosition.x, mousePosition.y, mousePosition.z);
			} else
				Debug.Log (this.gameObject.name + " has no HeadTransform");
		} else {
			Debug.Log (this.gameObject.name + " has no characterControll");
		}

		Ray ray = this.mainCamera.ScreenPointToRay(Input.mousePosition);

		RaycastHit hit;
		int layerMask = 1 << 2;
		layerMask = ~layerMask;
		if (Physics.Raycast(ray, out hit, 1000, layerMask))
		{
			if (Input.GetMouseButtonDown(0) && hit.collider.gameObject.CompareTag("VisualObject") && !this.dragging)
			{
				this.draggingObject = hit.collider.gameObject.GetComponent<Rigidbody> ();
				if (this.draggingObject != null) {
					Debug.Log ("dragging: " + hit.collider.gameObject.name);
					this.DRBSUisKinematic = this.draggingObject.isKinematic;
					this.DRBSUseGravity = this.draggingObject.useGravity;
					this.draggingObject.useGravity = false;
					this.draggingObject.isKinematic = true;
					this.dragging = true;

					// Search for textmesh in siblings
					for (int i = 0; i < this.draggingObject.transform.parent.childCount; i++) {
						Transform sibling = this.draggingObject.transform.parent.GetChild (i);
						if (sibling.gameObject.tag == "FloatingLabel") {
							print ("Changing floating label to unmergable");
							sibling.gameObject.tag = "FloatingLabelTarget";
							sibling.gameObject.GetComponent<InfoBoxPositiioning> ().isDragging = false;
						}
					}
				}
			}
		}

		if (this.draggingObject != null) {
			this.draggingObject.position = this.mainCamera.transform.position + (this.mainCamera.transform.forward * this.FactorOffset);
		}

		if (Input.GetMouseButtonUp (0) && this.dragging && this.draggingObject != null) {
			// Reactivate 
			// Search for textmesh in siblings
			for (int i = 0; i < this.draggingObject.transform.parent.childCount; i++) {
				Transform sibling = this.draggingObject.transform.parent.GetChild (i);
				if (sibling.gameObject.tag == "FloatingLabelTarget") {
					sibling.gameObject.tag = "FloatingLabel";
					sibling.gameObject.GetComponent<InfoBoxPositiioning> ().enabled = true;
					sibling.gameObject.GetComponent<InfoBoxPositiioning> ().ResetStartposition ();
				}
			}

			this.dragging = false;
			this.draggingObject.isKinematic = this.DRBSUisKinematic;
			this.draggingObject.useGravity = this.DRBSUseGravity;
			this.draggingObject = null;
		}
	}
}