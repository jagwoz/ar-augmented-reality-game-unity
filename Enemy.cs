using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour {

    public float rotationSpeed;
    public float movementSpeed;
    public float fieldOfView;
    public float enemyDistance;
    Transform enemyRotation;
    GameObject enemy;
    public GameObject player;
    public GameObject stageController;
    int myGUID;
    GameObject shootAnimation1;
    GameObject shootAnimation2;
    GameObject shootAnimation3;
    bool attacking;
    bool running;
    float enemyAttack;
    float enemyAttackSpeed;
    float distance;
    float timer;
    bool changeRotation;
    bool changePosition;
    bool isVisible;
    bool toDelete;
    float enemyAttackView;
    Vector3 newPos;
    Vector3 startPos;
    Vector3 startLocalPos;
    Quaternion startRotation;
    int topPosition;
    bool hunting;
    float enemyFoV;
    bool changeHP;
    float enemyHP;
    float actualHealthProcent;
    float actualHP;
    bool isActive;
    int number;
    bool isSet;
    bool moving;
    bool rotating;
    float attackTimer;
    bool goDown;
    Transform enemyHealthBox;
    Transform actualEnemyHealthBox;
    GameObject enemyHealthText;
    TextMesh enemyHealthTxt;
    public GameObject cameraPosition;

    void Start () {
        enemyAttack = 50.0f;
        enemyAttackSpeed = 1.0f;
        hunting = false;
        topPosition = 0;
        myGUID = Random.Range(10000, 99000);
        isSet = false;
        actualHP = enemyHP = 1000.0f;
        actualHealthProcent = actualHP / enemyHP * 100.0f;
        changeRotation = false;
        changePosition = false;
        enemyFoV = 0.3f;
        enemyAttackView = 0.2f;
        changeHP = false;
        isActive = false;
        moving = true;
        rotating = false;
        goDown = false;
        attacking = false;
        running = false;
        attackTimer = 1.0f;
        enemyRotation = transform.GetChild(0).transform;
        enemyHealthBox = transform.GetChild(1).transform;
        enemyRotation.gameObject.SetActive(false);
        enemyHealthBox.gameObject.SetActive(false);
        actualEnemyHealthBox = transform.GetChild(1).GetChild(1).transform;
        enemyHealthText = transform.GetChild(1).GetChild(2).gameObject;
        enemyHealthTxt = enemyHealthText.GetComponentInChildren<TextMesh>();
        int random = Random.Range(1, 3);
        topPosition += random;
        newPos = new Vector3(transform.position.x, transform.position.y, transform.position.z - random * 0.12f);
        shootAnimation1 = transform.GetChild(0).GetChild(11).GetChild(0).gameObject;
        shootAnimation2 = transform.GetChild(0).GetChild(12).GetChild(0).gameObject;
        shootAnimation3 = transform.GetChild(0).GetChild(13).GetChild(0).gameObject;
        shootAnimation1.SetActive(false);
        shootAnimation2.SetActive(false);
        shootAnimation3.SetActive(false);
    }

    void Update () {
        timer += Time.deltaTime;
        enemyRotation.transform.position = new Vector3(transform.position.x, enemyRotation.transform.position.y, transform.position.z);
        enemyHealthBox.transform.position = new Vector3(transform.position.x, enemyHealthBox.transform.position.y, transform.position.z);
        enemyHealthBox.transform.rotation = Quaternion.Slerp(enemyHealthBox.transform.rotation,
            Quaternion.LookRotation(enemyHealthBox.position - cameraPosition.transform.position),
            rotationSpeed * Time.deltaTime);

        if(timer >= 1)
        {
            if (!isActive)
            {
                enemyRotation.gameObject.SetActive(true);
                enemyHealthBox.gameObject.SetActive(true);
                isActive = true;
            }

            if(Vector3.Distance(transform.position, player.transform.position) <= enemyFoV)
            {
                player.GetComponent<Player>().SetTarget(transform.position, myGUID);
                if (player.GetComponent<Player>().GetTargetID() == myGUID && player.GetComponent<Player>().GetCanAttack())
                {
                    healthActualization(-player.GetComponent<Player>().GetAttack());
                    player.GetComponent<Player>().AmmoMinus();
                    player.GetComponent<Player>().SetCanAttack();
                    player.GetComponent<Player>().DoAnimationShoot();
                }
            }

            if ((Vector3.Distance(transform.position, player.transform.position) <= enemyFoV) && !hunting)
            {
                moving = false;
                rotating = false;
                hunting = true;
                shootAnimation2.SetActive(true);
            }

            if (changeHP)
            {
                actualHealthProcent = actualHP / enemyHP * 100.0f;
                enemyHealthTxt.text = "" + actualHealthProcent.ToString() + "%";
                actualEnemyHealthBox.localScale = new Vector3(actualHealthProcent * 0.7f / 100.0f, actualEnemyHealthBox.localScale.y, actualEnemyHealthBox.localScale.z);
                float newX = (100.0f - actualHealthProcent) * 0.35f / 100.0f;
                actualEnemyHealthBox.localPosition = new Vector3(-newX, actualEnemyHealthBox.localPosition.y, actualEnemyHealthBox.localPosition.z);
                changeHP = false;
            }

            if (hunting)
            {
                if (Vector3.Distance(transform.position, player.transform.position) <= enemyAttackView) attacking = true;
                if (Vector3.Distance(transform.position, player.transform.position) >= enemyFoV) attacking = false;
                if (attacking) {
                    attackTimer += Time.deltaTime;
                    Quaternion actualRotation = enemyRotation.transform.rotation;
                    enemyRotation.transform.rotation = Quaternion.Slerp(enemyRotation.transform.rotation,
                        Quaternion.LookRotation(transform.position - player.transform.position),
                        rotationSpeed * Time.deltaTime);
                    if ((enemyRotation.transform.rotation == actualRotation) && attackTimer >= enemyAttackSpeed)
                    {
                        attack();
                        attackTimer = 0.0f;
                    }
                }
                else
                {
                    Quaternion actualRotation = enemyRotation.transform.rotation;
                    enemyRotation.transform.rotation = Quaternion.Slerp(enemyRotation.transform.rotation,
                        Quaternion.LookRotation(transform.position - player.transform.position),
                        rotationSpeed * Time.deltaTime);
                    if (enemyRotation.transform.rotation == actualRotation)
                    {
                        newPos = player.transform.position;
                        float step = movementSpeed * Time.deltaTime;
                        transform.position = Vector3.MoveTowards(transform.position, newPos, step);
                        enemyRotation.transform.position = new Vector3(transform.position.x, enemyRotation.transform.position.y, transform.position.z);
                        enemyHealthBox.transform.position = new Vector3(transform.position.x, enemyHealthBox.transform.position.y, transform.position.z);
                    }
                }
            }
            else
            {
                if (moving)
                {
                    float step = movementSpeed * Time.deltaTime;
                    transform.position = Vector3.MoveTowards(transform.position, newPos, step);
                    enemyRotation.transform.position = new Vector3(transform.position.x, enemyRotation.transform.position.y, transform.position.z);
                    enemyHealthBox.transform.position = new Vector3(transform.position.x, enemyHealthBox.transform.position.y, transform.position.z);

                    if ((Vector3.Distance(transform.position, newPos) < 0.001f))
                    {
                        newPos *= -1.0f;
                        enemyRotation.transform.position = new Vector3(transform.position.x, enemyRotation.transform.position.y, transform.position.z);
                        enemyHealthBox.transform.position = new Vector3(transform.position.x, enemyHealthBox.transform.position.y, transform.position.z);
                        moving = false;
                        int random = Random.Range(0, 2);
                        if (!goDown)
                        {
                            if (topPosition == 7) hunting = true;
                            else if ((random == 0 && number > 0) || number == 7)
                            {
                                number -= 1;
                                newPos = new Vector3(transform.position.x - 0.12f, transform.position.y, transform.position.z);
                            }
                            else
                            {
                                number += 1;
                                newPos = new Vector3(transform.position.x + 0.12f, transform.position.y, transform.position.z);

                            }
                            goDown = true;
                        }
                        else
                        {
                            random = Random.Range(1, 3);
                            if (topPosition + random > 7)
                            {
                                random = 7 - topPosition;
                                topPosition = 7;
                            }
                            else topPosition += random;
                            newPos = new Vector3(transform.position.x, transform.position.y, transform.position.z - random * 0.12f);
                            goDown = false;                           
                        }
                        rotating = true;
                    }
                }
                else if (rotating)
                {
                    Quaternion actualRotation = enemyRotation.transform.rotation;
                    enemyRotation.transform.rotation = Quaternion.Slerp(enemyRotation.transform.rotation,
                        Quaternion.LookRotation(transform.position - newPos),
                        rotationSpeed * Time.deltaTime);
                    if (enemyRotation.transform.rotation == actualRotation)
                    {
                        rotating = false;
                        moving = true;
                    }
                }
            }
        }
    }

    public int getGuid()
    {
        return myGUID;
    }

    public float getHp()
    {
        return (actualHP / enemyHP * 100.0f);
    }

    void attack()
    {
        player.GetComponent<Player>().HealthActualization(-enemyAttack);
    }

    public void healthActualization(float hp)
    {
        actualHP += hp;
        if (actualHP <= 0.0f)
        {
            actualHP = 0.0f;
            delete();
        }
        else if (actualHP > enemyHP)
        {
            actualHP = enemyHP;
        }
        changeHP = true;
    }

    public bool isToDelete()
    {
        return toDelete;
    }

    public void setEnemy(int i)
    {
        number = i;
        isSet = true;
    }

    public GameObject getEnemy()
    {
        return gameObject;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            delete();
            player.GetComponent<Player>().HealthActualization(-100.0f);
        }
        else if(other.tag == "Enemy")
        {
            if(myGUID != other.GetComponentInParent<Enemy>().getGuid())
            {
                delete();
            }
        }
    }

    public void delete()
    {
        toDelete = true;
        if (player.GetComponent<Player>().GetTargetID() == myGUID) player.GetComponent<Player>().RemoveTarget();
        stageController.GetComponent<StagesControllPanel>().enemiesToKill[stageController.GetComponent<StagesControllPanel>().ActualStagePointer] -= 1;
    }
}