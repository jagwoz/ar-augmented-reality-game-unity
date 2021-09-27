using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {
    public float rotationSpeed;
    public float movementSpeed;
    public float fieldOfView;
    public float enemyDistance;
    public Transform PlayerRotation;
    public GameObject radar;
    public GameObject shootAnimation1;
    public GameObject shootAnimation2;
    public GameObject shootAnimation3;

    float timer;
    bool changeRotation;
    bool changePosition;
    Vector3 newPos;

    Vector3 startPos;
    Vector3 startLocalPos;
    Quaternion startRotation;

    bool isAtacking;
    float playerAttack;
    float playerAttackSpeed;
    float attackTimer;

    bool changeHP;
    float playerHP;
    float actualHealthProcent;
    float actualHP;
    public Transform playerHealthBox;
    public Transform actualPlayerHealthBox;
    public GameObject playerHealthText;
    private TextMesh playerHealthTxt;
    public GameObject cameraPosition;

    public GameObject hudWeapon1;
    public GameObject hudWeapon3;
    public GameObject hudWeapon2;
    public GameObject hudAmmo1;
    private TextMesh hudAmmo1Txt;
    public GameObject hudAmmo2;
    private TextMesh hudAmmo2Txt;
    public GameObject hudAmmo3;
    private TextMesh hudAmmo3Txt;
    bool weapon2U = false;
    bool weapon3U = false;
    int[] ammoCount = new int[] { 25, 0, 0 };

    bool canAttack;
    Vector3 target;
    int targetID;
    GameObject weapon1;
    GameObject weapon2;
    GameObject weapon3;
    readonly float weapon1Attack = 250.0f;
    readonly float weapon2Attack = 125.0f;
    readonly float weapon3Attack = 200.0f;
    readonly float weapon1AttackSpeed = 0.5f;
    readonly float weapon2AttackSpeed = 0.125f;
    readonly float weapon3AttackSpeed = 0.25f;
    int actualWeapon;

    void Start () {
        hudAmmo1Txt = hudAmmo1.GetComponentInChildren<TextMesh>();
        hudAmmo2Txt = hudAmmo2.GetComponentInChildren<TextMesh>();
        hudAmmo3Txt = hudAmmo3.GetComponentInChildren<TextMesh>();
        hudWeapon2.SetActive(false);
        hudWeapon3.SetActive(false);
        hudAmmo2.SetActive(false);
        hudAmmo3.SetActive(false);
        canAttack = false;
        attackTimer = 0;
        isAtacking = false;
        playerAttack = weapon1Attack;
        playerAttackSpeed = weapon1AttackSpeed;
        attackTimer = 0.0f;
        target = new Vector3(1000.0f, 1000.0f, 1000.0f);
        actualWeapon = 0;
        weapon1 = transform.GetChild(0).GetChild(11).gameObject;
        weapon2 = transform.GetChild(0).GetChild(12).gameObject;
        weapon3 = transform.GetChild(0).GetChild(13).gameObject;
        weapon1.SetActive(true);
        weapon2.SetActive(false);
        weapon3.SetActive(false);
        shootAnimation1 = transform.GetChild(0).GetChild(11).GetChild(0).gameObject;
        shootAnimation2 = transform.GetChild(0).GetChild(12).GetChild(0).gameObject;
        shootAnimation3 = transform.GetChild(0).GetChild(13).GetChild(0).gameObject;
        shootAnimation1.SetActive(false);
        shootAnimation2.SetActive(false);
        shootAnimation3.SetActive(false);
        startPos = transform.position;
        startLocalPos = transform.localPosition;
        startRotation = PlayerRotation.transform.rotation;
        playerHealthTxt = playerHealthText.GetComponentInChildren<TextMesh>();
        actualHP = playerHP = 4000.0f;
        actualHealthProcent = actualHP / playerHP * 100.0f;
        changeRotation = false;
        changePosition = false;
        changeHP = false;
    }

    void Update () {
        timer += Time.deltaTime;
        PlayerRotation.transform.position = new Vector3(transform.position.x, PlayerRotation.transform.position.y, transform.position.z);
        playerHealthBox.transform.position = new Vector3(transform.position.x, playerHealthBox.transform.position.y, transform.position.z);
        playerHealthBox.transform.rotation = Quaternion.Slerp(playerHealthBox.transform.rotation,
            Quaternion.LookRotation(playerHealthBox.position - cameraPosition.transform.position),
            rotationSpeed * Time.deltaTime);
        radar.GetComponent<DynamicElements>().RadarPositionUpdate(0, transform.localPosition);
        hudAmmo1Txt.text = "" + ammoCount[0];
        hudAmmo2Txt.text = "" + ammoCount[1];
        hudAmmo3Txt.text = "" + ammoCount[2];

        if (isAtacking)
        {
            attackTimer += Time.deltaTime;
            Quaternion actualRotation = PlayerRotation.transform.rotation;
            PlayerRotation.transform.rotation = Quaternion.Slerp(PlayerRotation.transform.rotation,
                Quaternion.LookRotation(transform.position - target),
                rotationSpeed * Time.deltaTime);
            if ((PlayerRotation.transform.rotation == actualRotation) && attackTimer >= playerAttackSpeed && ammoCount[actualWeapon] > 0)
            {
                DoAnimationShoot();
                canAttack = true;
                attackTimer = 0.0f;
            }
            else if (PlayerRotation.transform.rotation != actualRotation) EndAnimationShoot();
        }
        else
        {
            EndAnimationShoot();
            if (changeRotation)
            {
                Quaternion actualRotation = PlayerRotation.transform.rotation;
                PlayerRotation.transform.rotation = Quaternion.Slerp(PlayerRotation.transform.rotation,
                    Quaternion.LookRotation(transform.position - newPos),
                    rotationSpeed * Time.deltaTime);
                if (PlayerRotation.transform.rotation == actualRotation) changeRotation = false;
            }
            else if (changePosition)
            {
                float step = movementSpeed * Time.deltaTime;
                transform.position = Vector3.MoveTowards(transform.position, newPos, step);
                PlayerRotation.transform.position = new Vector3(transform.position.x, PlayerRotation.transform.position.y, transform.position.z);
                playerHealthBox.transform.position = new Vector3(transform.position.x, playerHealthBox.transform.position.y, transform.position.z);

                if (Vector3.Distance(transform.position, newPos) < 0.001f)
                {
                    newPos *= -1.0f;
                    PlayerRotation.transform.position = new Vector3(transform.position.x, PlayerRotation.transform.position.y, transform.position.z);
                    playerHealthBox.transform.position = new Vector3(transform.position.x, playerHealthBox.transform.position.y, transform.position.z);
                    changePosition = false;
                }
            }
        }
        

        if (changeHP)
        {
            actualHealthProcent = actualHP / playerHP * 100.0f;
            playerHealthTxt.text = "" + actualHealthProcent.ToString() + "%";
            actualPlayerHealthBox.localScale = new Vector3(actualHealthProcent*0.7f/100.0f, actualPlayerHealthBox.localScale.y, actualPlayerHealthBox.localScale.z);
            float newX = (100.0f - actualHealthProcent) * 0.35f / 100.0f;
            actualPlayerHealthBox.localPosition = new Vector3(-newX, actualPlayerHealthBox.localPosition.y, actualPlayerHealthBox.localPosition.z);
            changeHP = false;
        }
    }

    public void Respawn()
    {
        transform.localPosition = new Vector3(startLocalPos.x, transform.localPosition.y, startLocalPos.z);
        PlayerRotation.transform.localPosition = new Vector3(startLocalPos.x, PlayerRotation.transform.localPosition.y, startLocalPos.z);
        playerHealthBox.transform.localPosition = new Vector3(startLocalPos.x, playerHealthBox.transform.localPosition.y, startLocalPos.z);
        PlayerRotation.transform.rotation = startRotation;
        playerHealthBox.transform.rotation = startRotation;
        actualHP = playerHP;
        hudWeapon2.SetActive(false);
        hudWeapon3.SetActive(false);
        weapon2U = false;
        weapon3U = false;
        ammoCount = new int[] { 25, 0, 0 };
    }

    public void Move(Vector3 newPosition)
    {
        newPos = newPosition;
        changeRotation = true;
        changePosition = true;
        RemoveTarget();
    }

    public void AmmoMinus()
    {
        switch (actualWeapon)
        {
            case 0: ammoCount[0] -= 1; break;
            case 1: ammoCount[1] -= 1; break;
            case 2: ammoCount[2] -= 1; break;
        }
    }

    public void HealthActualization(float hp)
    {
        actualHP += hp;
        if (actualHP <= 0.0f){ actualHP = 0.0f; }
        else if (actualHP > playerHP){ actualHP = playerHP; }
        changeHP = true;
    }

    public void ChangeWeapon()
    {
        switch (actualWeapon)
        {
            case 0: if (weapon2U) actualWeapon = 1; else if (weapon3U) actualWeapon = 2; break;
            case 1: if (weapon3U) actualWeapon = 2; else actualWeapon = 0; break;
            case 2: actualWeapon = 0; break;
        }
        radar.GetComponent<DynamicElements>().PointerPositionUpdate(actualWeapon);

        weapon1.SetActive(false);
        weapon2.SetActive(false);
        weapon3.SetActive(false);
        switch (actualWeapon)
        {
            case 0: weapon1.SetActive(true); playerAttack = weapon1Attack; playerAttackSpeed = weapon1AttackSpeed; break;
            case 1: weapon2.SetActive(true); playerAttack = weapon2Attack; playerAttackSpeed = weapon2AttackSpeed; break;
            case 2: weapon3.SetActive(true); playerAttack = weapon3Attack; playerAttackSpeed = weapon3AttackSpeed; break;
        }
    }

    public void AddAmmo(int ammo)
    {
        switch (ammo)
        {
            case 0: ammoCount[0] += 15; break;
            case 1: if (weapon2U) ammoCount[1] += 25; else AddAmmo(0); break;
            case 2: if (weapon3U) ammoCount[2] += 10; else AddAmmo(0); break;
        }
    }
    public void AddWeapon(int weapon_id)
    {
        switch (weapon_id)
        {
            case 0: AddAmmo(weapon_id); break;
            case 1: if (weapon2U) AddAmmo(weapon_id); else { weapon2U = true; hudWeapon2.SetActive(true); hudAmmo2.SetActive(true); } break;
            case 2: if (weapon3U) AddAmmo(weapon_id); else { weapon3U = true; hudWeapon3.SetActive(true); hudAmmo3.SetActive(true); } break;
        }
    }

    public float GetAttack() { return playerAttack; }

    public void SetCanAttack() { canAttack = false; }

    public bool GetCanAttack() { return canAttack; }

    public Vector3 GetNewPos() { return newPos; }

    public float GetHp() { return (actualHP / playerHP * 100.0f); }

    public void SetTarget(Vector3 pos, int id)
    {
        if(Vector3.Distance(transform.position, pos) < Vector3.Distance(transform.position, target) && id != targetID && !changePosition)
        {
            target = pos;
            targetID = id;
            isAtacking = true;
        }
    }

    public void RemoveTarget()
    {
        target = new Vector3(1000.0f, 1000.0f, 1000.0f);
        EndAnimationShoot();
        isAtacking = false;
        targetID = 0;
    }

    public Vector3 GetTarget(){ return target; }
    public int GetTargetID(){ return targetID; }

    public void DoAnimationShoot()
    {
        switch (actualWeapon)
        {
            case 0: shootAnimation1.SetActive(true); break;
            case 1: shootAnimation2.SetActive(true); break;
            case 2: shootAnimation3.SetActive(true); break;
        }
    }

    public void EndAnimationShoot()
    {
            shootAnimation1.SetActive(false);
            shootAnimation2.SetActive(false);
            shootAnimation3.SetActive(false);
    }
}