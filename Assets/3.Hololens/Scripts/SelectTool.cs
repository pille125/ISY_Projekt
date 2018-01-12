using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HoloToolkit.Unity.InputModule;

public class SelectTool : MonoBehaviour, IInputClickHandler {

    public Color selectionColor;
    public ToolBelt.State associatedState;
    public string nameOfToolBelt = "ToolBelt";

    private List<MeshRenderer> meshes;
    private List<Color> defaultColors;
    private bool isSelected;
    private ToolBelt toolBelt;

	// Use this for initialization
	void Start () {
        meshes = new List<MeshRenderer>();
        defaultColors = new List<Color>();
        addMesh(this.gameObject);
        this.findToolBelt();
	}

    private void addMesh(GameObject gameObject)
    {
        MeshRenderer mr = gameObject.GetComponent(typeof(MeshRenderer)) as MeshRenderer;
        if(mr != null)
        {
            Material mat = mr.material;
            if(mat != null)
            {
                Color defaultColor = new Color(mat.color.r, mat.color.g, mat.color.b, mat.color.a);
                this.meshes.Add(mr);
                this.defaultColors.Add(defaultColor);
            }
        }
        for (int i = 0; i < gameObject.transform.childCount; i++)
            this.addMesh(gameObject.transform.GetChild(i).gameObject);
    }

    private void findToolBelt()
    {
        GameObject toolBeltGameObject = GameObject.Find(nameOfToolBelt);
        if (toolBeltGameObject != null)
            this.toolBelt = toolBeltGameObject.GetComponent(typeof(ToolBelt)) as ToolBelt;
    }

    public void select()
    {
        if (!this.isSelected)
        {
            this.isSelected = true;
            foreach (MeshRenderer mr in this.meshes)
            {
                if (mr.material != null)
                    mr.material.color = this.selectionColor;
            }
        }
    }

    public void deselect()
    {
        if (this.isSelected)
        {
            this.isSelected = false;
            for (int i = 0; i < meshes.Count; i++)
            {
                if (meshes[i] != null && meshes[i].material != null && defaultColors[i] != null)
                    meshes[i].material.color = new Color(defaultColors[i].r, defaultColors[i].g, defaultColors[i].b, defaultColors[i].a);
            }
        }
        
    }

    public void OnInputClicked(InputClickedEventData eventData)
    {
        if (this.isSelected)
        {
            if (this.toolBelt == null)
                this.findToolBelt();
            this.toolBelt.setState(ToolBelt.State.INIT);
        }
        else
        {
            if (this.toolBelt == null)
                this.findToolBelt();
            this.toolBelt.setState(this.associatedState);
        }
    }
}
