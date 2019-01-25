using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineConnector : MonoBehaviour {

    public Transform end1;
    public Transform end2;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		// Get centre positions of the objects at each end, and
        // set 

        if (end1 != null)
        {
            LineRenderer lr = GetComponent<LineRenderer>();
            if (lr != null)
            {
                lr.SetPosition(0, end1.position);
            }
            else
            {
                Debug.LogError("Null line renderer!!");
            }
        }
        if (end2 != null)
        {
            LineRenderer lr = GetComponent<LineRenderer>();
            if (lr != null)
            {
                lr.SetPosition(1, end2.position);
            }
            else
            {
                Debug.LogError("Null line renderer!!");
            }
        }
	}
}
