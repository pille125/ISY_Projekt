using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SAP.VR.General;

public class Rotater : MonoBehaviour {


	public VR_System vrSys;
	// Use this for initialization
	void Start () {
		_rotateHelper = new GameObject().transform;
		_rotateHelper.parent = this.transform;
	}
	public Transform grabableObject;

	public Transform rotatorGraphic;
	// Update is called once per frame
	void Update () {
		if(vrSys.objectGrabbed) return;

		if(startRotate) {
			if(left) {
				if(vrSys.leftHandState == VR_System.HandState.fist) {
					_rotateHelper.position = vrSys.leftHand.position;
					Rotate();
				}
				else {
					vrSys.disableGrab = false;
					startRotate = false;
					initRotate = true;
					
					handInTool = false;					
				}				
			}
			else {
				if(vrSys.rightHandState == VR_System.HandState.fist) {
					_rotateHelper.position = vrSys.rightHand.position;
					Rotate(); 
				}
				else {
					vrSys.disableGrab = false;
					startRotate = false;
					initRotate = true;
					
					handInTool = false;
				}
			}			
		}
	}

	bool left;
	bool startRotate;
	//determine localPos from hand to extract X Variable
	Transform _rotateHelper;

	bool initRotate = true;
	float lastX;
	float newX;
	float nextX; 

	public float rotateFactor = 50;
	void Rotate() {

		if(initRotate) {
			lastX = _rotateHelper.localPosition.z;
			initRotate = false;
		}

		newX = _rotateHelper.localPosition.z;
		nextX = newX - lastX;
		grabableObject.transform.localEulerAngles = new Vector3( grabableObject.transform.eulerAngles.x, grabableObject.transform.eulerAngles.y, grabableObject.transform.eulerAngles.z  + nextX * rotateFactor); 
		rotatorGraphic.transform.localEulerAngles = new Vector3(0, rotatorGraphic.transform.eulerAngles.y  + nextX * rotateFactor, 0);
		lastX = newX;
	}

	bool handInTool;
	void OnTriggerEnter(Collider col) {
		if(vrSys.objectGrabbed) return;
		if(startRotate) return;

		if(col.tag.Equals("LeftHand") || col.tag.Equals("RightHand")) {

			if(col.tag.Equals("LeftHand") && vrSys.leftHandState == VR_System.HandState.idle) {
				left = true;
				handInTool = true;
				vrSys.disableGrab = true;
			}
			else if(col.tag.Equals("RightHand")&& vrSys.rightHandState == VR_System.HandState.idle) {
				left = false;
				handInTool = true;
				vrSys.disableGrab = true;
			}
		}
	}

	void OnTriggerStay(Collider col) {
		if(vrSys.objectGrabbed) return;
		if(startRotate && !handInTool) return;

		if(col.tag.Equals("LeftHand") || col.tag.Equals("RightHand")) {

			if(left && vrSys.leftHandState == VR_System.HandState.fist) {
				//left = true;
				startRotate = true;
			}
			else if(!left && vrSys.rightHandState == VR_System.HandState.fist) {
				//left = false;
				startRotate = true;
			}
		}
	}
	void OnTriggerExit(Collider col) {
		if(vrSys.objectGrabbed) return;
		if(startRotate) return;

		if(col.tag.Equals("LeftHand") || col.tag.Equals("RightHand")) {

			handInTool = false;
			vrSys.disableGrab = false;

		}
	}
}
