using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketLifecycle : MonoBehaviour {

    public float movementSpeed;
    bool toDelete;
    float timer;
    float acceleration;
    float rotateAcceleration;
    GameObject rocket;
    public GameObject player;
    public GameObject stageController;
    public GameObject radarDynamic;
    int myGUID;

    void Start () {
        acceleration = 1.00000001f;
        rotateAcceleration = 1.0001f;
        toDelete = false;
        //myGUID = Random.Range(10000, 99000);
        //radarDynamic.GetComponent<DynamicElements>().AddNewElement(1);
    }

    void Update () {
        timer += Time.deltaTime;

        if (timer >= 1)
        {
            acceleration *= acceleration;
            rocket.GetComponent<MeshRenderer>().enabled = true;
            Vector3 newPos = new Vector3(rocket.transform.position.x, rocket.transform.position.y, rocket.transform.position.z - 10.0f); 
            float step = movementSpeed * Time.deltaTime * acceleration;
            rocket.transform.position = Vector3.MoveTowards(rocket.transform.position, newPos, step);
            rocket.transform.eulerAngles = new Vector3(
                rocket.transform.eulerAngles.x,
                rocket.transform.eulerAngles.y,
                rocket.transform.eulerAngles.z + rotateAcceleration
            );

            if (timer >= 5)
            {
                Delete();
            }
        }
    }

    public bool IsToDelete()
    {
        return toDelete;
    }

    public void SetRocket(GameObject obj)
    {
        rocket = obj;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            Delete();
            player.GetComponent<Player>().HealthActualization(-100.0f);
        }
        else if (other.tag == "Enemy")
        {
            other.GetComponentInParent<Enemy>().healthActualization(-100.0f);
            Delete();
        }
    }

    public void Delete()
    {
        toDelete = true;
        //radarDynamic.GetComponent<DynamicElements>().RemoveElement(1, myGUID);
    }

    public int GetGUID()
    {
        return myGUID;
    }
}
