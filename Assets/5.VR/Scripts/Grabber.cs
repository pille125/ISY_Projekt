using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VR;
using SAP.VR.General;

namespace SAP.VR.Interaction
{
    public class Grabber : MonoBehaviour
    {
        public enum ControllerSide { left, right }

        public ControllerSide _controllerSide;

        private VR_System vrSys;
        public GameObject collidingObj;
        public GameObject heldObj;

        [Tooltip("If grabed Object should keep their selectionState (toolbelt will still be disabled during Grabbing")]
        public bool keepSelectionSate;

        private Animator handAnim;

        public bool grabbed = false;


        // Use this for initialization
        void Start()
        {
            vrSys = FindObjectOfType<VR_System>();
            //if (this.gameObject.name.Contains("Left")) _controllerSide = ControllerSide.left;
            //else if(this.gameObject.name.Contains("Right")) _controllerSide = ControllerSide.right;
            handAnim = this.gameObject.GetComponent<Animator>();
        }
    
        public VR_System.HandState lastRightHandState;
        public VR_System.HandState lastLeftHandState;

        public GameObject grabbedObject;

        // Update is called once per frame
        void Update()
        {

            //HandAnimation links
            
            //if leftHand
            if (_controllerSide == ControllerSide.left)
            {
                if(vrSys.leftHandState != lastLeftHandState) {
                    handAnim.SetTrigger(vrSys.leftHandState.ToString());
                    lastLeftHandState = vrSys.leftHandState;
                }
                if(vrSys.disableGrab) return;
                if (vrSys.leftHandState == VR_System.HandState.fist)
                {
                    grabObject();
                }
                else if (vrSys.leftHandState != VR_System.HandState.fist)
                {
                    releaseObj();
                }
            }
            //if rightHand
            if (_controllerSide == ControllerSide.right)
            {
                if(vrSys.rightHandState != lastRightHandState) {
                    handAnim.SetTrigger(vrSys.rightHandState.ToString());
                    lastRightHandState = vrSys.rightHandState;
                }
                if(vrSys.disableGrab) return;
                if (vrSys.rightHandState == VR_System.HandState.fist)
                {
                    grabObject();
                }
                else if (vrSys.rightHandState != VR_System.HandState.fist)
                {
                    releaseObj();
                }
            }
            if(vrSys.disableGrab) return;
            if (vrSys.objectGrabbed)
            {
                if (vrSys.leftHandState == VR_System.HandState.fist && vrSys.leftHandState == VR_System.HandState.fist) { 
                    scale();                   
                }
                else {
                    if(!firstScale) {
                        firstScale = true;
                        
                    }
                }
            }
            else {
                    if(!firstScale) firstScale = true;
            }
            
        }

        private void OnTriggerEnter(Collider other)
        {
            if(vrSys.disableGrab) return;
            //other.GetComponent<Rigidbody>().useGravity = false;
            //obj has to have a rigidbody and only colliding object
            if (collidingObj != null || !other.GetComponent<Rigidbody>()|| !other.GetComponent<Rigidbody>().useGravity)
            {
                return;
            }
            
            collidingObj = other.gameObject;
            

        }

        private void OnTriggerStay(Collider other)
        {
             if(vrSys.disableGrab) return;
            //obj has to have a rigidbody and only colliding object
            if (collidingObj != null || !other.GetComponent<Rigidbody>() || !other.GetComponent<Rigidbody>().useGravity)
            {
                return;
            }
            collidingObj = other.gameObject;
        }

        private void OnTriggerExit(Collider other)
        {
            if(vrSys.disableGrab) return;
            if (collidingObj) collidingObj = null;
        }
        public string _tag;
        void grabObject()
        {
            if (grabbed || collidingObj == null) return;

            heldObj = collidingObj;
            //disableToolbelt
            if(heldObj.GetComponent<Selectable>() && heldObj.GetComponent<Selectable>().selected) {
                heldObj.GetComponent<Selectable>().toolbelt(false);
                if(!keepSelectionSate) heldObj.GetComponent<Selectable>().selected = false;
            }
            //Debug.Log ("buh");
            //FixedJoint fixedJoint = this.gameObject.AddComponent<FixedJoint>();
            //when to break joint
            //fixedJoint.breakForce = 2000;
            //fixedJoint.breakTorque = 2000;      
            //fixedJoint.connectedBody = heldObj.GetComponent<Rigidbody>();
            //heldObj.GetComponent<Rigidbody> ()
            //if(heldObj.tag.Equals("Physics")) heldObj.GetComponent<BoxCollider> ().isTrigger = true;
            if (heldObj.tag.Equals(_tag)) heldObj.GetComponent<Rigidbody>().isKinematic = true;
            if (heldObj.tag.Equals(_tag)) heldObj.GetComponent<Rigidbody>().useGravity = false;
            if (heldObj.tag.Equals(_tag)) heldObj.GetComponent<Rigidbody>().freezeRotation = true;
            heldObj.transform.parent = this.transform;
            grabbed = true;
            vrSys.objectGrabbed = true;
            vrSys.grabbedObject = heldObj;
        }


        void releaseObj()
        {
            if (!grabbed) return;

            if(keepSelectionSate && heldObj.GetComponent<Selectable>() && heldObj.GetComponent<Selectable>().selected) {
                heldObj.GetComponent<Selectable>().toolbelt(true);
            }
            //Debug.Log ("hallo");	
            if (heldObj.tag.Equals(_tag)) heldObj.GetComponent<Rigidbody>().useGravity = true;
            if (heldObj.tag.Equals(_tag)) heldObj.GetComponent<Rigidbody>().freezeRotation = false;

            //thats stupid   
            if (heldObj.tag.Equals(_tag)) heldObj.GetComponent<Rigidbody>().isKinematic = false;

            //for Oculus Controller only
            //if(heldObj.tag.Equals("Physics")) heldObj.GetComponent<Rigidbody>().velocity = OVRInput.GetLocalControllerVelocity(OVRInput.Controller.RTouch);
            //if(heldObj.tag.Equals("Physics")) heldObj.GetComponent<Rigidbody>().angularVelocity = OVRInput.GetLocalControllerAngularVelocity(OVRInput.Controller.RTouch);

            //for Vive Controller only
            heldObj.transform.parent = null;
            heldObj = null;
            grabbed = false;
            vrSys.objectGrabbed = false;
            vrSys.grabbedObject = null;
        }

        bool firstScale = true;
        float lastDistance;
        //rotate/scale ? - copy to pointer
        void scale()
        {
            if(firstScale) {
                lastDistance = Vector3.Distance(vrSys.leftHand.position, vrSys.rightHand.position);
                firstScale = false;
            }
            float newDistance = Vector3.Distance(vrSys.leftHand.position, vrSys.rightHand.position);
            float scaleFactor =  newDistance - lastDistance; 
            scaleFactor *= 10;
            vrSys.grabbedObject.transform.localScale = new Vector3(vrSys.grabbedObject.transform.localScale.x + scaleFactor , vrSys.grabbedObject.transform.localScale.y+ scaleFactor, vrSys.grabbedObject.transform.localScale.z+ scaleFactor);
            lastDistance = newDistance;
        }
    }
}
