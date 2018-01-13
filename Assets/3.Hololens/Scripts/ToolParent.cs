using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToolParent : MonoBehaviour {

	public GameObject manipulatingObject;
	
	// Update is called once per frame
	void Update ()
	{
		// Update position and rotation if an object is selected
		if (manipulatingObject)
		{
			transform.position = manipulatingObject.transform.position;
			transform.LookAt (Camera.main.transform);
		}
	}

	public void setManipulatingObject (GameObject mo)
	{
		manipulatingObject = mo;
	}
}
