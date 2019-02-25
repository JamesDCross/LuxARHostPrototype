using UnityEngine;
using UnityEngine.Networking;
using System.Collections.Generic;

public class AutoNPCSpawner : NetworkBehaviour
{
    public GameObject autoNPCPrefab;
    public int numberOfNPCs;
    public int spawnGroupSize = 10;  // How many to spawn at once
    public float waitBeforeSpawn = 5.0f;
    public float spawnGroupWait = 5.0f;

    private int currentNPCCount = 0;
    //temp
    public List<GameObject> players; 

    public override void OnStartServer()
    {
        // Rather than spawn all at once, we do a few at a time.
        if (numberOfNPCs > 0)
        {
            InvokeRepeating("SpawnNPCs", waitBeforeSpawn, spawnGroupWait);
        }
    }

    void SpawnNPCs()
    {
        // Randomly spawns a set number of NPC's until we have reached a limit
        for (int i = 0; i < spawnGroupSize; i++)
        {
            var spawnPosition = new Vector3(
                Random.Range(-40.0f, 40.0f),
                0.0f,
                Random.Range(-40.0f, 40.0f));
            var spawnRotation = Quaternion.Euler(
                0.0f,
                Random.Range(0, 180),
                0.0f);
            var npc = (GameObject.Instantiate(autoNPCPrefab, spawnPosition, spawnRotation));
            NetworkServer.Spawn(npc);
            //temp
            players.Add(npc);

            currentNPCCount++;
        }
        if (currentNPCCount >= numberOfNPCs)
        {
            CancelInvoke("SpawnNPCs");
        }
    }
    //temp
    void Update()
    {
        // temp change player line connector (all)
        if (Input.GetKey(KeyCode.C))
        {
            foreach (GameObject g in players)
            {
                g.GetComponent<PlayerConnector>().selectLinePrefab = 1;
            }
        }
        if (Input.GetKey(KeyCode.V))
        {
            foreach (GameObject g in players)
            {
                g.GetComponent<PlayerConnector>().selectLinePrefab = 0;
            }
        }
        if (Input.GetKey(KeyCode.B))
        {
            foreach (GameObject g in players)
            {
                g.GetComponent<PlayerConnector>().selectLinePrefab = 2;
            }
        }
    }

  
}

