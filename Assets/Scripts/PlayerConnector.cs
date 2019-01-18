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
    }

    private void OnTriggerExit(Collider other)
    {
        Debug.Log("Objects parted");
    }
}
