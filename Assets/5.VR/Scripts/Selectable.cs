using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SAP.VR.General;

namespace SAP.VR.Interaction
{
    public class Selectable : MonoBehaviour
    {
		//public Material outline;
        // Use this for initialization

		public enum SelectableType { selectable, uiButton }

		public Material label;
		public Material labelSelected;

		public SelectableType type;

		public bool selected;

		public GameObject _toolbelt;
		private VR_System vrSys;
        void Start()
        {
				vrSys = FindObjectOfType<VR_System>();
        }
		
        // Update is called once per frame
        void Update()
        {

        }
		public GameObject outline;
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
					if(type == SelectableType.selectable) outline.SetActive(true);
					else if(type == SelectableType.uiButton) {
						
					}
					toolbelt(true);
				}
				else {
					selected = false;
					if(type == SelectableType.selectable)  outline.SetActive(false);
					else if(type == SelectableType.uiButton) {

					}
					toolbelt(false);
				}
			}
		}

		public void toolbelt(bool on) {
				if(on) _toolbelt.transform.position = this.transform.position;
				_toolbelt.SetActive(on);
		}
    }
}