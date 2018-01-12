using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HoloToolkit.Unity.InputModule;


public class FingerTapToSelect : MonoBehaviour, IInputClickHandler
{
    // When Object is clicked, then manipulating is possible
    public GameObject ManipulationCube;
    public GameObject InfoLabel;

    // Use this for initialization
    void Start ()
	{
        ManipulationCube.SetActive(false);
    }
		
    public void OnInputClicked(InputClickedEventData eventData)
    {
        // Make manipulating object visible
        ManipulationCube.SetActive(true);
        InfoLabel.SetActive(false);
        this.GetComponent<LineRenderer>().enabled = false;
    }
}
