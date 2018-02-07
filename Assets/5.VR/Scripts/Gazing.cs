using UnityEngine;
using System.Collections;

/**
 * This Scripts contains functions for gaze based input
 *
 * Attach on main Camera. It uses RayCast System
 * 
 * Also handling a UI Dot
 * 
 * if called from outside, put 
 * 
 * GameObject myObject = RayCast();
 * if(myObject != null) {
 * 
 * myObject.GetComponent<Script>.event();
 * 
 * }
 * 
 * in Update() method
 * 
 * myObject will be the properly tagged object you can interact with
 * 
 * 
 * */
public class Gazing : MonoBehaviour {

    [Header("RayCast")]
    public bool DebugOn;
    public float gazeRange = 5f;

    public GameObject objectInSight;


    [Header("Interactive Objects")]
    public string interactiveObjectsTag = "Grabable";

    // Use this for initialization

    private RaycastHit hit;

    void Start () {
	
	}

    bool shortActivated = false;
    // Update is called once per frame
    void Update() {
        objectInSight = gazedObject();
        if(objectInSight != null) {
            if(!shortActivated) activateShortCut(true);
        }
        else {
            if(shortActivated) activateShortCut(false);
        }
    }

    void activateShortCut(bool on) {
        //acivate shortcuts here in selectable Component of Object
    }

    private GameObject gazedObject()
    {
        Vector3 forward = transform.TransformDirection(Vector3.forward)*gazeRange;
            
		if(DebugOn) Debug.DrawRay(transform.position, forward, Color.green);

        if (Physics.Raycast(transform.position, forward, out hit, gazeRange))
        {
            
            if (hit.collider.tag.Equals(interactiveObjectsTag))
            {                   
                return hit.collider.gameObject;
            }               
        
        }
        return null;
    }

}
