﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerConnector : NetworkBehaviour
{
    public GameObject lineConnectorPrefab;
    public Transform lineTransform;

    // Map of collider (other object) to the line object joining this to
    // that collider.
    Dictionary<GameObject, GameObject> lineMap;


    public bool hasObjectInMap(GameObject other)
    {
        Debug.Assert(lineMap != null, "Linemap cannot be null!");
        return lineMap.ContainsKey(other);
    }

    // Use this for initialization
    void Start()
    {
        lineMap = new Dictionary<GameObject, GameObject>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (Equals(other.name, "Arena"))
        {
            // Would probaly be better not making Arena collidable.
            Debug.Log("Arena hit");
            return;
        }

        // Instantiate a line connector
        if (lineConnectorPrefab != null)
        {
            // Only create a line if we are not already linked to this object

            // Firstly see if this object has the collider in its map.
            Debug.Assert(other != null, "Null collider provided to OnTriggerEnter!");
            Debug.Assert(other.gameObject != null, "other.gameobject should not be null");
            Debug.Assert(lineMap != null, "Linemap must not be null!");

            if (!lineMap.ContainsKey(other.gameObject))
            {
                // Secondly, check the other object does not have this
                // one in its own map (i.e. guard against the fact that both
                // objects will get triggered by each other, but we only want one line
                // created betweem them, not two).
                PlayerConnector p = other.gameObject.GetComponent<PlayerConnector>();

                if (!p.hasObjectInMap(gameObject))
                {
                    var l = (GameObject)Instantiate(lineConnectorPrefab, Vector3.zero, Quaternion.identity);

                    l.GetComponent<LineConnector>().end1 = transform;
                    l.GetComponent<LineConnector>().end2 = other.transform;

                    lineMap.Add(other.gameObject, l);

                    // Temp:
                    //NetworkServer.Spawn(l);
                }
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (Equals(other.name, "Arena"))
        {
            Debug.Log("Arena exit");
            return;
        }

        // Find line previously connected   
        if (lineMap.ContainsKey(other.gameObject))
        {
            Destroy(lineMap[other.gameObject]);
            lineMap.Remove(other.gameObject);
        }
    }
}
