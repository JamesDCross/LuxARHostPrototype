using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class LuxNetworkManager : NetworkManager {

    public void OnClientConnect(UnityEngine.Networking.NetworkConnection conn)
    {
        //Your custom player spawning logic
        //Decide here who is a zombie and who is a human, etc.

        Debug.Log("Client connect");

        base.OnClientConnect(conn);

    }
}
