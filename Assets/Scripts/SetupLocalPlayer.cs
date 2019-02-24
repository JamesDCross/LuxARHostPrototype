using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class SetupLocalPlayer : NetworkBehaviour {

    [SyncVar]
    public string pname = "lux player";

    [SyncVar]
    public Color playerColor = Color.white;

    [Command] 
    public void CmdChangeName(string newName)
    {
        pname = newName;
        //this.GetComponentInChildren<TextMesg>().text = pname;
    }

	// Use this for initialization
	void Start () {
		if (isLocalPlayer)
        {
            // Set all components to chosen color
            // NB: We proably just want to set flare colour
            // Renderer[] rends = GetComponentInChildren<Renderer>();
            // foreach (Renderer r in rends)
            //     r.material.color = playerColor;
            PlayerMove p = this.GetComponent<PlayerMove>();
            if (p)
            {
                p.SetColor(playerColor);
            }
        }
	}
	
	// Update is called once per frame
	void Update () {
        //this.GetComponentInChildren<TextMesg>().text = pname;
    }
}
