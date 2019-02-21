using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LobbyHostingPanel : MonoBehaviour {

    protected Image panelImage;

    // Use this for initialization
    void Start () {

        panelImage = GetComponent<Image>();

        if (Application.isMobilePlatform) {
            // Disable if running on moile platform

            if (!panelImage) {
                Debug.LogError("Could not find hosting subbpanel");
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
