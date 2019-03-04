using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class PlayerMainMenu : MonoBehaviour {

    //Main menu for player view, mainly only a bunch of callback called by the UI (setup throught the Inspector)

   public void OnEnable ()
    {
        // Menu initialisation
    }

    public void OnClickFindMe ()
    {
        // Change player avatar to highlight position.
        // Could also temporarily show the player name.
        // TODO: find player object and call a method on that to do the work.

        Debug.Log("Find Me! Clicked");
    }

    public void OnClickBack ()
    {
        // Returns back to lobby
        // TODO - should be able to grab code from lobby example.

    }
}
