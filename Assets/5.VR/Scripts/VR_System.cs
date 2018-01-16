using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VR;

namespace SAP.VR.General {

    public class VR_System : MonoBehaviour {

        public bool disableGrab = false;
        public bool objectGrabbed = false;
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
            VRDevice.SetTrackingSpaceType(TrackingSpaceType.RoomScale);
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

                leftHand.position = InputTracking.GetLocalPosition(VRNode.LeftHand);
                leftHand.rotation = InputTracking.GetLocalRotation(VRNode.LeftHand);

                rightHand.position = InputTracking.GetLocalPosition(VRNode.RightHand);
                rightHand.rotation = InputTracking.GetLocalRotation(VRNode.RightHand);
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
