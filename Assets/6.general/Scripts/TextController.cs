using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextController : MonoBehaviour {

	public Text Name;
	public Text TransformInfo;

	public void setName(string name){
		if (this.Name != null)
			this.Name.text = name;
	}

	public void setTransformInfo(string transformInfo){
		if (this.TransformInfo != null)
			this.TransformInfo.text = transformInfo;
	}
}
