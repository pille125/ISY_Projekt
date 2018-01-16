using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SAP.VR.General;

namespace SAP.VR.Interaction
{
    public class Selectable : MonoBehaviour
    {
		public Material outline;
        // Use this for initialization

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
					GetComponent<Renderer>().materials[1].SetFloat("_Outline", 0.07f );
					toolbelt(true);
				}
				else {
					selected = false;
					GetComponent<Renderer>().materials[1].SetFloat("_Outline", 0 );
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