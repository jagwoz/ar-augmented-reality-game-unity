using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketLauncherLifecycle : MonoBehaviour {

    public float movementSpeed;
    bool toDelete;
    float timer;

	void Start () {
        toDelete = false;
    }
	
	void Update () {
        timer += Time.deltaTime;

        if(timer >= 1)
        {
            toDelete = true;
        }
    }

    public bool isToDelete()
    {
        return toDelete;
    }
}
