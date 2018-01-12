using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HoloToolkit.Unity.InputModule;


public class TapToEndEditMode : MonoBehaviour, IInputClickHandler
{
    // When Object is Clicked than is manipulating possible

    public GameObject ManipulationBar;
    public GameObject InfoLabel;
    public GameObject MainObject;

    public void OnInputClicked(InputClickedEventData eventData)
    {
        // Make manipulating object visible
        ManipulationBar.SetActive(false);
        InfoLabel.SetActive(true);
        MainObject.GetComponent<LineRenderer>().enabled = true;
    }
}
