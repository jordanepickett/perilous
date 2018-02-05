using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Infantry : Unit {


	// Use this for initialization
	void Start () {
        SetState(state.IDLE);
	}
	
	// Update is called once per frame
	void Update () {
        switch (State)
        {
            case state.IDLE:
                Debug.Log("Idle");
                break;
            case state.MOVING:
                Debug.Log("Moving");
                break;
            case state.WORKING:
                break;
        }
	}
}
