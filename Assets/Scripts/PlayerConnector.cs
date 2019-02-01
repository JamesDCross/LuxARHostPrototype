using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerConnector : NetworkBehaviour
{
    public GameObject lineConnectorPrefab;
    public Transform lineTransform;

    Dictionary<Collider, GameObject> lineMap;

    // Use this for initialization
    void Start()
    {
        lineMap = new Dictionary<Collider, GameObject>();
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
            if (!lineMap.ContainsKey(other))
            {
                // Only create a line if we are not already linked to this object
                var l = (GameObject)Instantiate(lineConnectorPrefab, Vector3.zero, Quaternion.identity);

                l.GetComponent<LineConnector>().end1 = transform;
                l.GetComponent<LineConnector>().end2 = other.transform;

                lineMap.Add(other, l);
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
        if (lineMap.ContainsKey(other))
        {
            Destroy(lineMap[other]);
            lineMap.Remove(other);
        }
    }
}
