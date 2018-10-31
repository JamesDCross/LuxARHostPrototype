﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerMove : NetworkBehaviour {

	public float speed = 3.0f;   // units per second
    public bool moveByGPS = true;  // True of GPS drives position, else use keyboard/gyro input
    public bool interpolateOn = true; // True if we are attempting interpolations
                                      // This may be temporary, as perhaps always should do it.
    public bool showDebug = false;
	public GameObject bulletPrefab;
	public Transform bulletSpawn;

	// Keep a reference to GPS->Game coordinate mapping object
    private ArenaMapper arena = null;

	private double timeOrigin = -1.0f;  // Used to normalise time reporting for diagnostics

    // Use this for initialization
    void Start () {
		ResetMotion ();

        if (Application.isMobilePlatform)
        {
            // Might not be the best place to do this, but OK for now.
            Input.location.Start(5, 2);   // Need best accuracy. Try (1,1)?

            // Wait until service initializes
 /*           int maxWait = 20;
            while (Input.location.status == LocationServiceStatus.Initializing && maxWait > 0)
            {
                yield return new WaitForSeconds(1);
                maxWait--;
            }
            // Service didn't initialize in 20 seconds
            if (maxWait < 1)
            {
                Debug.Log ("Timed out waiting for location services");
            }
*/
            if (Input.location.status == LocationServiceStatus.Failed)
            {
                Debug.Log("Unable to start GPS tracker");
            }

        }

        GameObject temp = GameObject.Find("Arena");
        if (temp == null)
        {
            Debug.LogError("Arena object not found");
            return;
        }
        arena = temp.GetComponent<ArenaMapper>();

        if (arena == null)
        {
            Debug.LogError("Unable to locate ArenaMapper component of Arena");
            return;
        }
    }

	public void ResetMotion () {
		// Call whenever goal position changes.

	}

    private void OnDestroy()
    {
        if (Input.location.isEnabledByUser) {
            Input.location.Stop();
            Debug.Log("GPS Stopped");
        }
    }


    // Update is called once per frame
    void Update () {
		// Change position based on target location
        if (!isLocalPlayer)
        {
            return;
        }

        if (Application.isMobilePlatform)
        {
            if (moveByGPS)
            {
                // GPS drives position of player
                if (arena != null)
                {
                    Vector3 worldpos = arena.GPStoArenaCoordinates(Input.location.lastData.latitude,
                                                                   Input.location.lastData.longitude);
                    // Temp - colour the player based on whether inside the arena or
                    // not in order to test out the GPS to Game coordinate mapping

                    Debug.Log(string.Format("GPS: {0} ", Input.location.lastData.timestamp));

                    if (arena.IsInsideArena(worldpos))
                    {
                        SetColor(Color.cyan);
                    }
                    else
                    {
                        SetColor(Color.red);
                    }

                    // Set position to mapped GPS location
                    if (interpolateOn)
                    {
                        // Smooth things out with some interpolation. Note that 
                        // this relies upon a fixed speed parameter, and this will
                        // probably only work for demo. Might need a dynamic speed
                        // value, or a different interpolation approach altogether.

                        // The step size is equal to speed times frame time.
                        float step = speed * Time.deltaTime;
                        // Move our position a step closer to the target.
                        transform.position = Vector3.MoveTowards(transform.position, worldpos, step);
                    }
                    else {
                        transform.position = worldpos;
                    }
                }
            }
            else
            {
                // Process mobile device input mechanism
                Vector3 dir = Vector3.zero;

                // Assume device held parallel to ground and home button to right

                // Remamp device acceleration axis to game coordinates:
                // 1. XY plane of device mapped onto XZ plane
                // 2. rotated 90 degrees around Y axis
                dir.x = -Input.acceleration.y;
                dir.z = Input.acceleration.x;

                // clamp accel vector to unit
                if (dir.sqrMagnitude > 1)
                {
                    dir.Normalize();
                }
                dir *= Time.deltaTime;
                transform.Translate(dir * speed);
            }
        }
        else
        {
            // Use keyboard/mouse for computers
            var x = Input.GetAxis("Horizontal") * Time.deltaTime * 150.0f;
            var z = Input.GetAxis("Vertical") * Time.deltaTime * speed;

            transform.Rotate(0, x, 0);
            transform.Translate(0, 0, z);

			if (Input.GetKeyDown(KeyCode.O)) {
				Camera.main.fieldOfView *= 0.9f;
			}
			if (Input.GetKeyDown (KeyCode.P)) {
				Camera.main.fieldOfView *= 1.1f;
			}

            if (Input.GetKeyDown(KeyCode.Space))
            {
                CmdFire();
            }
        }
	}

    private void OnGUI()
    {
        if (!showDebug)
        {
            return;
        }
        // Let's grab some GPS coordinates and dump them out
        if (Input.location.status == LocationServiceStatus.Running)
        {
			// Initialise time origin - since GPS time is with respect to seconds since 1970 we get 
			// big numbers that create noise.
			if (timeOrigin < 0.0f) {
				timeOrigin = Input.location.lastData.timestamp;
			}

            string text = string.Format("GPS: {0}:({1},{2})",
                                        Input.location.lastData.timestamp - timeOrigin,
                                        Input.location.lastData.latitude,
                                        Input.location.lastData.longitude);
            GUI.Label(new Rect(10, 200, 400, 60), text);
            Debug.Log(text);
            /*
            if (arena != null)
            {
                Vector3 worldpos = arena.GPStoArenaCoordinates(Input.location.lastData.latitude,
                                                               Input.location.lastData.longitude);

                text = string.Format("WLD: {0}, {1}", worldpos.x, worldpos.z);
                GUI.Label(new Rect(10, 200, 400, 60), text);
                Debug.Log(text);

                if (arena.IsInsideArena(worldpos))
                {
                    GUI.backgroundColor = Color.green;
                    GUI.Label(new Rect(10, 300, 400, 60), "Inside Arena");
                }
                else
                {
                    GUI.backgroundColor = Color.red;
                    GUI.Label(new Rect(10, 300, 400, 60), "Outside Arena!");
                }
            }
            */
        }
        else
        {
            string text = string.Format("GPS Status={0}", Input.location.status.ToString());
            GUI.Label(new Rect(10, 200, 400, 60), text);
        }
    }

    public override void OnStartLocalPlayer()
    {
        //base.OnStartLocalPlayer();
        //SetColor(Color.blue);
	}

    public void SetColor (Color newcolor)
	{
		// Sets particle start colour
		GameObject p = this.transform.Find("Player Flare").gameObject;

		if (p) {
			ParticleSystem psys = p.GetComponent<ParticleSystem>();

			var main = psys.main;
			main.startColor = newcolor;
		} else {
			Debug.LogError ("Unable to set colour - Flare object not found.");
		}
	}

    [Command]
	void CmdFire ()
	{
		// Create the bullet from the Bullet Prefab
		var bullet = (GameObject)Instantiate (bulletPrefab, bulletSpawn.position, bulletSpawn.rotation);

		// Add some velocity
		bullet.GetComponent<Rigidbody>().velocity = bullet.transform.forward * 6;

        // Spawn the bullet on the clients
        NetworkServer.Spawn(bullet);

		// Destroy after 2 seconds
		Destroy(bullet, 2.0f);
	}
}
