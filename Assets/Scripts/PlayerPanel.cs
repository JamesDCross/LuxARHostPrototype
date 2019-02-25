using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class PlayerPanel : NetworkBehaviour {

    protected Image panelImage;


    void Start()
    {
        // Turn off UI panel for player if in server mode
        panelImage = GetComponent<Image>();

        if (!isServer)
        {
            // Disable if running on moile platform

            if (!panelImage)
            {
                Debug.LogError("Could not find UI subbpanel");
                return;
            }
            SetVisibility(false);
        }

    }

    public void SetVisibility(bool visible)
    {
        foreach (Transform t in transform)
        {
            t.gameObject.SetActive(visible);
        }

        if (panelImage != null)
        {
            panelImage.enabled = visible;
        }
    }

}
