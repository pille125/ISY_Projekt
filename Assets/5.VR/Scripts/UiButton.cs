using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SAP.VR.General;
public class UiButton : MonoBehaviour {

			//public Material outline;
        // Use this for initialization

		public Material label;
		public Material labelSelected;


		public bool selected;

		public GameObject _UI;
		private VR_System vrSys;
        void Start()
        {
				vrSys = FindObjectOfType<VR_System>();
        }
		
        // Update is called once per frame
        void Update()
        {

        }
		public int outlineArrayPosition;
		void OnTriggerEnter(Collider collider) {
			if(collider.tag.Equals("LeftIndex") || collider.tag.Equals("RightIndex" )) {
				if(collider.tag.Equals("RightIndex" )) {
					if(vrSys.rightHandState != VR_System.HandState.pointing) {
						return;
					}
				}
				if(collider.tag.Equals("LeftIndex" )) {
					if(vrSys.leftHandState != VR_System.HandState.pointing) {
						return;
					}
				}

				if(!selected) {
					selected = true;
					GetComponent<MeshRenderer>().material = labelSelected; 					
					UIOn(true);
				}
				else {
					selected = false;
					GetComponent<MeshRenderer>().material = label; 
					UIOn(false);
				}
			}
		}

		public void UIOn(bool on) {
				//if(on) _UI.transform.position = this.transform.position;
				_UI.SetActive(on);
		}
}
