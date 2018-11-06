//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

#if ENABLE_UNET

namespace UnityEngine.Networking
{
    [AddComponentMenu("Network/NetworkManagerHUD")]
    [RequireComponent(typeof(NetworkManager))]
    [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]

    public class NetworkManagerHUD : MonoBehaviour
    {
        public NetworkManager manager;

        [SerializeField] public bool showGUI = true;
        [SerializeField] public int offsetX;
        [SerializeField] public int offsetY;
        [SerializeField] public int buttonHeight = 20;

        // Runtime variable
        bool showServer = false;

        void Awake()
        {
            manager = GetComponent<NetworkManager>();
        }

        void Update()
        {
            if (!showGUI)

                return;

            if (!NetworkClient.active && !NetworkServer.active && manager.matchMaker == null)
            {
                if (Input.GetKeyDown(KeyCode.S))
                {
                    manager.StartServer();
                }

                if (Input.GetKeyDown(KeyCode.H))
                {
                    manager.StartHost();
                }

                if (Input.GetKeyDown(KeyCode.C))
                {
                    manager.StartClient();
                }
            }

            if (NetworkServer.active && NetworkClient.active)
            {
                if (Input.GetKeyDown(KeyCode.X))
                {
                    manager.StopHost();
                }
            }
        }

        void OnGUI()
        {
            if (!showGUI)
                return;

            int xpos = 10 + offsetX;
            int ypos = 40 + offsetY;
            int spacing = buttonHeight + 4;   // 24

            if (!NetworkClient.active && !NetworkServer.active && manager.matchMaker == null)
            {
                if (GUI.Button(new Rect(xpos, ypos, 200, buttonHeight), "LAN Host(H)"))
                {
                    manager.StartHost();
                }

                ypos += spacing;

                if (GUI.Button(new Rect(xpos, ypos, 105, buttonHeight), "LAN Client(C)"))
                {
                    manager.StartClient();
                }

                manager.networkAddress = GUI.TextField(new Rect(xpos + 100, ypos, 95, buttonHeight), manager.networkAddress);

                ypos += spacing;

                if (GUI.Button(new Rect(xpos, ypos, 200, buttonHeight), "LAN Server Only(S)"))
                {
                    manager.StartServer();
                }

                ypos += spacing;
            }
            else
            {
                if (NetworkServer.active)
                {
                    GUI.Label(new Rect(xpos, ypos, 300, buttonHeight), "Server: port=" + manager.networkPort);
                    ypos += spacing;
                }

                if (NetworkClient.active)
                {
                    GUI.Label(new Rect(xpos, ypos, 300, buttonHeight), "Client: address=" + manager.networkAddress + " port=" + manager.networkPort);
                    ypos += spacing;
                }
            }

            if (NetworkClient.active && !ClientScene.ready)
            {
                if (GUI.Button(new Rect(xpos, ypos, 200, buttonHeight), "Client Ready"))
                {
                    ClientScene.Ready(manager.client.connection);

                    if (ClientScene.localPlayers.Count == 0)
                    {
                        ClientScene.AddPlayer(0);
                    }
                }
                ypos += spacing;
            }

            if (NetworkServer.active || NetworkClient.active)
            {
                if (GUI.Button(new Rect(xpos, ypos, 200, buttonHeight), "Stop (X)"))
                {
                    manager.StopHost();
                }
                ypos += spacing;
            }

            if (!NetworkServer.active && !NetworkClient.active)
            {
                ypos += 10;

                if (manager.matchMaker == null)
                {
                    if (GUI.Button(new Rect(xpos, ypos, 200, buttonHeight), "Enable Match Maker (M)"))
                    {
                        manager.StartMatchMaker();
                    }
                    ypos += spacing;
                }
                else
                {
                    if (manager.matchInfo == null)
                    {
                        if (manager.matches == null)
                        {
                            if (GUI.Button(new Rect(xpos, ypos, 200, buttonHeight), "Create Internet Match"))
                            {
                                manager.matchMaker.CreateMatch(manager.matchName, manager.matchSize, true, "", "", "", 0, 0, manager.OnMatchCreate);
                            }

                            ypos += spacing;

                            GUI.Label(new Rect(xpos, ypos, 100, buttonHeight), "Room Name:");

                            manager.matchName = GUI.TextField(new Rect(xpos + 100, ypos, 100, buttonHeight), manager.matchName);

                            ypos += spacing;
                            ypos += 10;

                            if (GUI.Button(new Rect(xpos, ypos, 200, buttonHeight), "Find Internet Match"))
                            {
                                manager.matchMaker.ListMatches(0, buttonHeight, "", true, 0, 0, manager.OnMatchList);
                            }
                            ypos += spacing;
                        }
                        else
                        {
                            foreach (var match in manager.matches)
                            {
                                if (GUI.Button(new Rect(xpos, ypos, 200, buttonHeight), "Join Match:" + match.name))
                                {
                                    manager.matchName = match.name;
                                    manager.matchSize = (uint)match.currentSize;
                                    manager.matchMaker.JoinMatch(match.networkId, "", "", "", 0, 0, manager.OnMatchJoined);
                                }
                                ypos += spacing;
                            }
                        }
                    }

                    if (GUI.Button(new Rect(xpos, ypos, 200, buttonHeight), "Change MM server"))
                    {
                        showServer = !showServer;
                    }
                    if (showServer)
                    {
                        ypos += spacing;

                        if (GUI.Button(new Rect(xpos, ypos, 100, buttonHeight), "Local"))
                        {
                            manager.SetMatchHost("localhost", 1337, false);
                            showServer = false;
                        }
                        ypos += spacing;

                        if (GUI.Button(new Rect(xpos, ypos, 100, buttonHeight), "Internet"))
                        {
                            manager.SetMatchHost("mm.unet.unity3d.com", 443, true);
                            showServer = false;
                        }

                        ypos += spacing;
                        if (GUI.Button(new Rect(xpos, ypos, 100, buttonHeight), "Staging"))
                        {
                            manager.SetMatchHost("staging-mm.unet.unity3d.com", 443, true);
                            showServer = false;
                        }
                    }

                    ypos += spacing;
                    GUI.Label(new Rect(xpos, ypos, 300, buttonHeight), "MM Uri: " + manager.matchMaker.baseUri);
                    ypos += spacing;

                    if (GUI.Button(new Rect(xpos, ypos, 200, buttonHeight), "Disable Match Maker"))
                    {
                        manager.StopMatchMaker();
                    }
                    ypos += spacing;
                }
            }
        }
    }
};

#endif //ENABLE_UNET
