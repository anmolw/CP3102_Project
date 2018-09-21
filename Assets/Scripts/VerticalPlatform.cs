using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VerticalPlatform : MonoBehaviour {

    private PlatformEffector2D effector;
    public float waittime;

	// Use this for initialization
	void Start () {
        effector = GetComponent<PlatformEffector2D>();

	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyUp(KeyCode.DownArrow))
        { waittime = 0.1f; }

        if (Input.GetKey(KeyCode.DownArrow))
        {
            if (waittime <= -0.1f)
            {
                effector.rotationalOffset = 180f;
                waittime -= Time.deltaTime;
                if (waittime <= -0.8f)
                {
                    effector.rotationalOffset = 360f;
                }


                //effector.rotationalOffset = 360f;

            }
            else
            {
               // effector.rotationalOffset = 360f;
                waittime -= Time.deltaTime;
            }
        }
        if (Input.GetKey(KeyCode.UpArrow))
        {
            effector.rotationalOffset = 0;
        }
	}
}
