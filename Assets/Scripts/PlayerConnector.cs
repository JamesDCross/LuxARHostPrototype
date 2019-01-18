using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerConnector : MonoBehaviour {

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
