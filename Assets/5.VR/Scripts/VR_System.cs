using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VR;

namespace SAP.VR.General {

    public class VR_System : MonoBehaviour {

        public bool disableGrab = false;
        public bool objectGrabbed = false;
        public GameObject grabbedObject;
        public enum HandState { idle, pointing, fist }

        public HandState leftHandState;
        public HandState rightHandState; 

        public GameObject head;
        public Transform leftHand, rightHand;

        [Header("Button Presses")]
        public bool leftIndex;
		public bool rightIndex, leftThumb, rightThumb;

        // Use this for initialization
        void Start() {
            //set to Roomscale
            UnityEngine.XR.XRDevice.SetTrackingSpaceType(UnityEngine.XR.TrackingSpaceType.RoomScale);
        }

        // Update is called once per frame
        void Update() {

            trackController();
			buttononInputs ();
        }

        void trackController()
        {
            //update HandPositions Left and Right Hand
            if (leftHand != null || rightHand != null)
            {

                leftHand.position = UnityEngine.XR.InputTracking.GetLocalPosition(UnityEngine.XR.XRNode.LeftHand);
                leftHand.rotation = UnityEngine.XR.InputTracking.GetLocalRotation(UnityEngine.XR.XRNode.LeftHand);

                rightHand.position = UnityEngine.XR.InputTracking.GetLocalPosition(UnityEngine.XR.XRNode.RightHand);
                rightHand.rotation = UnityEngine.XR.InputTracking.GetLocalRotation(UnityEngine.XR.XRNode.RightHand);
            }
        }

        void buttononInputs()
        {
            //index buttons
            leftIndex = Input.GetButton("VR_LeftIndex");
            rightIndex = Input.GetButton("VR_RightIndex");

            leftThumb = Input.GetButton("VR_LeftThumb");
            rightThumb = Input.GetButton("VR_RightThumb");
            
            //leftHandStates
            if(leftIndex && (leftThumb || Input.GetButton("VR_ButtonLeft"))) {
                leftHandState = HandState.fist;
            }
            else if(!leftIndex && (leftThumb || Input.GetButton("VR_ButtonLeft"))) {
                leftHandState = HandState.pointing;
            } 
            else {
                leftHandState = HandState.idle;
            } 

            if(rightIndex  && (rightThumb || Input.GetButton("VR_ButtonRight")) ) {
                rightHandState = HandState.fist;
            }
            else if(!rightIndex && (rightThumb || Input.GetButton("VR_ButtonRight"))) {
                rightHandState = HandState.pointing;
            } 
            else {
                rightHandState = HandState.idle;
            } 

        }
    }
}
