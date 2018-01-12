using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToolBelt : MonoBehaviour {

    public enum State
    {
        INIT,
        TRANSLATE,
        ROTATE,
        SCALE
    }

    private State state;
    private List<GameObject> tools;

    public void addTool(GameObject tool)
    {
        this.tools.Add(tool);
    }

    public State getState()
    {
        return this.state;
    }

    public void setState(State state)
    {
        this.state = state;
        foreach(GameObject tool in tools)
        {
            SelectTool selectTool = tool.GetComponent(typeof(SelectTool)) as SelectTool;
            if(selectTool != null)
            {
                if (selectTool.associatedState != this.state)
                    selectTool.deselect();
                else
                    selectTool.select();
            }
        }
    }

	// Use this for initialization
	void Start ()
	{
        this.state = State.INIT;
        this.tools = new List<GameObject>();
	}
}
