using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HoloToolkit.Unity.InputModule;


public class FingerTapToSelect : MonoBehaviour, IInputClickHandler
{
    // When Object is clicked, then manipulating is possible
    public GameObject Tools;
    public GameObject InfoLabel;

    // Use this for initialization
    void Start ()
	{
		Tools.SetActive(false);
    }
		
    public void OnInputClicked(InputClickedEventData eventData)
    {
        // Make tools visible
		Tools.SetActive(true);

		// Set active Object in Tools
		ToolParent tp = Tools.GetComponent<ToolParent>();
		if (tp)
			tp.setManipulatingObject(gameObject);

		// Activate info label
		if (InfoLabel)
		{ // Ich habe das hier erstmal fuer mein Level ohne Info Labels abefangen
			// Kann in Zukunft vielleicht wieder entfernt werden
			InfoLabel.SetActive(false);
			this.GetComponent<LineRenderer>().enabled = false;
		}
    }
}
