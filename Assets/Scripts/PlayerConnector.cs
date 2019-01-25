using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerConnector : NetworkBehaviour {

    public GameObject lineConnectorPrefab;
    public Transform lineTransform;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Objects collided!!");
        Debug.Log(other.name);

        if (Equals(other.name, "Arena"))
        {
            Debug.Log("Arena hit");
            return;
        }

        // Instantiate a line connector

        if (lineConnectorPrefab != null)
        {
            var l = (GameObject)Instantiate(lineConnectorPrefab, Vector3.zero, Quaternion.identity);
            
            //l.GetComponent<LineConnector>().end1 = this.;
            l.GetComponent<LineConnector>().end2 = other.transform;
            Debug.Log("Connector created" + l);
        }

        PlayerMove pm = GetComponent<PlayerMove>();
        if (pm)
        {
            pm.SetTrailColor(Color.blue);
        }
        else
        {
            AutoNPCMotion am = GetComponent<AutoNPCMotion>();
            if (am)
            {
                am.SetTrailColor(Color.blue);
            }
            else
            {
                Debug.LogError("Neither player nor NPC found!!");   
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        Debug.Log("Objects parted");
        if (Equals(other.name, "Arena"))
        {
            Debug.Log("Arena hit");
            return;
        }
        PlayerMove pm = GetComponent<PlayerMove>();
        if (pm)
        {
            pm.SetTrailColor(Color.white);
        }
        else
        {
            AutoNPCMotion am = GetComponent<AutoNPCMotion>();
            if (am)
            {
                am.SetTrailColor(Color.white);
            }
            else
            {
                Debug.LogError("Neither player nor NPC found!!");
            }
        }

    }
}
