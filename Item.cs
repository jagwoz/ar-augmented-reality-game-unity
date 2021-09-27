using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour {
    public float rotationSpeed;
    Transform itemRotation;
    public GameObject player;
    public GameObject stageController;
    float timer;
    bool toDelete;
    Transform itemTtmeBox;
    Transform actualItemTimeBox;
    GameObject itemTimeText;
    TextMesh itemTimeTxt;
    public GameObject cameraPosition;
    float maxTime;
    float actualTime;
    float actualTimeProcent;
    int actualText;
    float actualInterval;
    int itemType;

    void Start()
    {
        actualInterval = 100.0f - (100.0f / 6.0f);
        maxTime = 10.0f;
        actualText = 5;
        itemTtmeBox = transform.GetChild(5).transform;
        actualItemTimeBox = transform.GetChild(5).GetChild(1).transform;
        itemTimeText = transform.GetChild(5).GetChild(2).gameObject;
        itemTimeTxt = itemTimeText.GetComponentInChildren<TextMesh>();
        itemType = Random.Range(0, 5);
        itemRotation = transform.GetChild(itemType).transform;
        if(itemType == 4) itemType = Random.Range(0, 4);
        itemRotation.gameObject.SetActive(false);
        itemTtmeBox.gameObject.SetActive(false);
    }

    void Update()
    {
        timer += Time.deltaTime;
        itemRotation.transform.position = new Vector3(transform.position.x, itemRotation.transform.position.y, transform.position.z);
        itemTtmeBox.transform.position = new Vector3(transform.position.x, itemTtmeBox.transform.position.y, transform.position.z);
        itemTtmeBox.transform.rotation = Quaternion.Slerp(itemTtmeBox.transform.rotation,
            Quaternion.LookRotation(itemTtmeBox.position - cameraPosition.transform.position),
            rotationSpeed * Time.deltaTime);

        actualTime = timer - 1.0f;
        actualTimeProcent = 100.0f - (actualTime / maxTime * 100.0f);
        if(actualTimeProcent <= actualInterval)
        {
            actualInterval -= (100.0f / 6.0f);
            actualText -= 1;
        } 
        itemTimeTxt.text = "" + actualText.ToString();
        actualItemTimeBox.localScale = new Vector3(actualTimeProcent * 0.7f / 100.0f, actualItemTimeBox.localScale.y, actualItemTimeBox.localScale.z);
        float newX = (100.0f - actualTimeProcent) * 0.35f / 100.0f;
        actualItemTimeBox.localPosition = new Vector3(-newX, actualItemTimeBox.localPosition.y, actualItemTimeBox.localPosition.z);

        if (timer >= 1)
        {
            itemRotation.gameObject.SetActive(true);
            itemTtmeBox.gameObject.SetActive(true);

            if(timer >= 11)
            {
                toDelete = true;
            }
        }
    }

    public bool IsToDelete()
    {
        return toDelete;
    }

    public int GetItemType()
    {
        return itemType;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            switch (itemType) {
                case 0: player.GetComponent<Player>().HealthActualization(-50.0f); break;
                case 1: player.GetComponent<Player>().HealthActualization(100.0f); break;
                case 2: int weaponType = Random.Range(0, 3); player.GetComponent<Player>().AddWeapon(weaponType); break;
                case 3: int ammoType = Random.Range(0, 3); player.GetComponent<Player>().AddAmmo(ammoType); break;
            }
            Delete();
        }
        if(other.tag == "Enemy")
        {
            if (itemType == 0) other.GetComponent<Enemy>().healthActualization(-100.0f);
            Delete();
        }
    }

    public void Delete()
    {
        toDelete = true;     
    }
}
