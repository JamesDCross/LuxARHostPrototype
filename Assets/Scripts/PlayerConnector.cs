using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerConnector : NetworkBehaviour
{
    public List<GameObject> lineConnectorPrefabs;
    public Transform lineTransform;

    //Temp to change connect prefab
    public int selectLinePrefab;

    // Map of collider (other object) to the line object joining this to
    // that collider.
    Dictionary<GameObject, GameObject> lineMap;


    public bool hasObjectinMap(GameObject other)
    {
        return lineMap.ContainsKey(other);
    }

    // Use this for initialization
    void Start()
    {
        lineMap = new Dictionary<GameObject, GameObject>();

        selectLinePrefab = 0;
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
        if (lineConnectorPrefabs[selectLinePrefab] != null)
        {
            // Only create a line if we are not already linked to this object

            // Firstly see if this object has the collider in its map.
            if (!lineMap.ContainsKey(other.gameObject))
            {
                // Secondly, check the other object does not have this
                // one in its own map (i.e. guard against the fact that both
                // objects will get triggered by each other, but we only one one line
                // created betweem them, not two).
                PlayerConnector p = other.gameObject.GetComponent<PlayerConnector>();

                if (!p.hasObjectinMap(gameObject))
                {
                    var l = (GameObject)Instantiate(lineConnectorPrefabs[selectLinePrefab], Vector3.zero, Quaternion.identity);

                    l.GetComponent<LineConnector>().end1 = transform;
                    l.GetComponent<LineConnector>().end2 = other.transform;

                    lineMap.Add(other.gameObject, l);
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
