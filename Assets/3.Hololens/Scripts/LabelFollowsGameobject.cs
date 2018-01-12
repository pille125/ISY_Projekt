using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LabelFollowsGameobject : MonoBehaviour {

    public GameObject label;
    public int SlowDownForce = 2;
    public float minDistanceFromGoToLabel = 2f;
	public int UpdateSec = 2;

    private int frameCount;
    private bool downSpeed;

    private float targetXpos;
    private float targetYpos;
    private float targetZpos;

    private Rigidbody labelRb;

    // Use this for initialization
    void Start () {
        downSpeed = false;
        frameCount = 0;
        labelRb = label.GetComponent<Rigidbody>();
    }
	
	// Update is called once per frame
	void Update () {

        UpdatePosEveryXSeconds(UpdateSec);

        if(downSpeed)
        {
            labelRb.velocity = labelRb.velocity / 2;
            if(labelRb.velocity.magnitude <= 0.1f)
            {
                labelRb.velocity = Vector3.zero;
                downSpeed = false;
            }
        }
    }


    void UpdatePosEveryXSeconds(int sec)
    {
		if (frameCount <= sec * 100) // 100 erstmal geraten für anzahl der frames pro sec
        {
            frameCount += 1;
        }
        else // Update Zielposition
        {
            targetXpos = 0.7f * (float)(this.gameObject.transform.position.x - label.transform.position.x);
            targetYpos = 0.7f * (float)(this.gameObject.transform.position.y - label.transform.position.y);
            targetZpos = 0.7f * (float)(this.gameObject.transform.position.z - label.transform.position.z);

            float dist = Vector3.Distance(this.gameObject.transform.position, label.transform.position);
            if (dist <= minDistanceFromGoToLabel)
            {
                downSpeed = true;
            }
            else
            {
                labelRb.AddForce((new Vector3(targetXpos, targetYpos, targetZpos)) / SlowDownForce, ForceMode.Impulse);
            }

            frameCount = 0;
        }
    }
}
