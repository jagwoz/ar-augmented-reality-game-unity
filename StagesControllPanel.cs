using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class StagesControllPanel : MonoBehaviour {
    bool stageActive;
    bool dialogueActive;
    bool stageInfoActive;
    bool countdownActive;
    bool customInfoActive;
    bool roundActive;
    public GameObject dialogueText;
    private TextMesh dialogueTxt;
    public GameObject countdownText;
    private TextMesh countdownTxt;
    public GameObject levelText;
    private TextMesh levelTxt;
    public GameObject remainText;
    private TextMesh remainTxt;
    public GameObject healthText;
    private TextMesh healthTxt;
    public GameObject pointText;
    public GameObject pointsSumText;
    private TextMesh pointsSumTxt;

    public GameObject allElements;
    public GameObject backgroundEnd;
    public GameObject endPointsText;
    private TextMesh endPointsTxt;

    bool firstGamestateIsPossible;
    bool secGamestateIsPossible;

    public GameObject setPositionButtons;
    public GameObject explosion;
    public GameObject explosion2;
    List<GameObject> explosions;

    public int itemDelay;
    private int points;

    int GAMESTATE;
    float countdownTimer;
    int countdownValue;
    float timer;
    float timer_e;
    float timer_i;
    public float rocketsDelay;
    public float enemiesDelay;
    public string endOfCountdownText;
    public string startGameInfo;

    public GameObject player;
    List<GameObject> rockets;
    List<GameObject> rocketsLaunchers;
    List<GameObject> enemies;
    List<GameObject> items;
    List<GameObject> enemiesLaunchers;
    List<GameObject> itemsLaunchers;
    public GameObject rocketLanucher;
    public GameObject itemLanucher;
    public GameObject enemyLanucher;
    public GameObject rocket;
    public GameObject enemy;
    public GameObject item;
    public GameObject background;

    public List<int> enemiesToKill;
    public List<int> toFix;

    public string endOfStagesText;
    public int StagesNumber;
    public int ActualStagePointer = 0;

    bool newGame = false;
    public List<string> WordsBeforeStage;
    public List<string> WordsAfterStage;
    public List<int> CountWordsBeforeStage;
	public List<int> CountWordsAfterStage;
    int ActualSizeTextBeforeStagePointer = 0;
    int ActualSizeTextAfterStagePointer = 0;
    int ActualTextBeforeStagePointer = 0;
    int ActualTextAfterStagePointer = 0;

    void Start () {
        GAMESTATE = 3;
        points = 0;
        firstGamestateIsPossible = false;
        secGamestateIsPossible = false;
        rockets = new List<GameObject>();
        rocketsLaunchers = new List<GameObject>();
        enemies = new List<GameObject>();
        enemiesLaunchers = new List<GameObject>();
        explosions = new List<GameObject>();
        items = new List<GameObject>();
        itemsLaunchers = new List<GameObject>();
        countdownTxt = countdownText.GetComponentInChildren<TextMesh>();
        dialogueTxt = dialogueText.GetComponentInChildren<TextMesh>();
        levelTxt = levelText.GetComponentInChildren<TextMesh>();
        remainTxt = remainText.GetComponentInChildren<TextMesh>();
        healthTxt = healthText.GetComponentInChildren<TextMesh>();
        pointsSumTxt = pointsSumText.GetComponentInChildren<TextMesh>();
        endPointsTxt = endPointsText.GetComponentInChildren<TextMesh>();
        countdownText.SetActive(false);
        levelText.SetActive(false);
        pointsSumText.SetActive(false);
        pointText.SetActive(false);
        remainText.SetActive(false);
        dialogueText.SetActive(true);
        healthText.SetActive(true);
        healthTxt.text = "100%";
    }
	
	void Update () {
        switch (GAMESTATE)
        {
            case 0:
                if (!dialogueActive)
                {
                    dialogueTxt.text = WordsBeforeStage[ActualTextBeforeStagePointer].ToString();
                    dialogueText.SetActive(true);
                    dialogueActive = true;
                    ActualTextBeforeStagePointer += 1;
                }
                timer += Time.deltaTime;
                if (timer >= 3 && ActualTextBeforeStagePointer < ActualSizeTextBeforeStagePointer)
                {
                    dialogueTxt.text = WordsBeforeStage[ActualTextBeforeStagePointer].ToString();
                    ActualTextBeforeStagePointer += 1;
                    timer = 0.0f;
                    timer_e = 0.0f;
                    timer_i = 0.0f;
                }
                else if (ActualTextBeforeStagePointer == ActualSizeTextBeforeStagePointer)
                {
                    secGamestateIsPossible = true;
                    countdownValue = 3;
                }   
                break;
            case 1:
                countdownTimer += Time.deltaTime;
                if (countdownTimer >= 1 && countdownValue >= -1)
                {
                    countdownText.SetActive(true);
                    if (countdownValue == 0)
                    {
                        countdownTxt.text = endOfCountdownText;
                    }
                    else if (countdownValue == -1)
                    {
                        countdownText.SetActive(false);
                        levelText.SetActive(true);
                        pointsSumText.SetActive(true);
                        pointText.SetActive(true);
                        remainText.SetActive(true);
                        GAMESTATE += 1;
                    }
                    else
                    {
                        countdownTxt.text = countdownValue.ToString();
                    }
                    countdownValue = countdownValue - 1;
                    countdownTimer = 0.0f;
                }

                break;
            case 2:
                pointsSumTxt.text = "" + points;
                endPointsTxt.text = "" + points + " PKT";
                levelTxt.text = "POZIOM: " + (ActualStagePointer + 1);
                remainTxt.text = "POZOSTAŁO: " + enemiesToKill[ActualStagePointer] + " PRZECIWNIKÓW";
                healthTxt.text = player.GetComponent<Player>().GetHp() + "%";

                timer += Time.deltaTime;
                timer_e += Time.deltaTime;
                timer_i += Time.deltaTime;
                if (timer >= rocketsDelay)
                {
                    for(int i=0; i < (ActualStagePointer + 1); i++)
                    {
                        int randomX = Random.Range(0, 7);
                        Vector3 newPos = new Vector3(rocketLanucher.transform.position.x + (randomX * 0.12f), rocketLanucher.transform.position.y, rocketLanucher.transform.position.z);
                        GameObject rocketLauncherClone = Instantiate(rocketLanucher, newPos, rocketLanucher.transform.rotation) as GameObject;
                        rocketLauncherClone.SetActive(true);
                        rocketLauncherClone.AddComponent<RocketLauncherLifecycle>();
                        rocketsLaunchers.Add(rocketLauncherClone);

                        newPos = new Vector3(rocket.transform.position.x + (randomX * 0.12f), rocket.transform.position.y, rocket.transform.position.z);
                        GameObject rocketClone = Instantiate(rocket, newPos, rocket.transform.rotation) as GameObject;
                        rocketClone.SetActive(true);
                        rocketClone.GetComponent<MeshRenderer>().enabled = false;
                        rocketClone.AddComponent<RocketLifecycle>();
                        rocketClone.GetComponent<RocketLifecycle>().SetRocket(rocketClone);
                        rockets.Add(rocketClone);
                    }
                    timer = 0.0f;
                }
                foreach (GameObject i in rockets)
                {
                    if (i.GetComponent<RocketLifecycle>().IsToDelete())
                    {
                        if(i.transform.position.z >= 1.3f)
                        {
                            GameObject explosionClone = Instantiate(explosion2, new Vector3(i.transform.position.x, explosion.transform.position.y, i.transform.position.z - 0.05f), explosion2.transform.rotation) as GameObject;
                            explosionClone.SetActive(true);
                            explosionClone.AddComponent<DestroyEffect>();
                            explosions.Add(explosionClone);
                        }
                        i.SetActive(false);
                        rockets.Remove(i);
                        points += ((ActualStagePointer + 1) * 50);
                    }
                }
                foreach (GameObject i in rocketsLaunchers)
                {
                    if (i.GetComponent<RocketLauncherLifecycle>().isToDelete())
                    {
                        i.SetActive(false);
                    }
                }
                if ((timer_e >= enemiesDelay) && enemies.Count < (ActualStagePointer + 1))
                {
                    int randomX = Random.Range(0, 7);

                    Vector3 newPos = new Vector3(enemyLanucher.transform.position.x + (randomX * 0.12f), enemyLanucher.transform.position.y, enemyLanucher.transform.position.z);
                    GameObject enemyLauncherClone = Instantiate(enemyLanucher, newPos, enemyLanucher.transform.rotation) as GameObject;
                    enemyLauncherClone.SetActive(true);
                    enemyLauncherClone.AddComponent<RocketLauncherLifecycle>();
                    enemiesLaunchers.Add(enemyLauncherClone);

                    newPos = new Vector3(enemy.transform.position.x + (randomX * 0.12f), enemy.transform.position.y, enemy.transform.position.z);
                    GameObject enemyClone = Instantiate(enemy, newPos, enemy.transform.rotation) as GameObject;
                    enemyClone.SetActive(true);
                    enemyClone.AddComponent<Enemy>();
                    enemyClone.GetComponent<Enemy>().setEnemy(randomX);
                    enemies.Add(enemyClone);
                    timer_e = 0.0f;
                }
                if ((timer_i >= itemDelay) && items.Count < 2)
                {
                    int randomX = Random.Range(0, 7);
                    int randomY = Random.Range(0, 7);

                    Vector3 newPos = new Vector3(itemLanucher.transform.position.x + (randomX * 0.12f), itemLanucher.transform.position.y, itemLanucher.transform.position.z - (randomY * 0.12f));
                    GameObject itemLauncherClone = Instantiate(itemLanucher, newPos, itemLanucher.transform.rotation) as GameObject;
                    itemLauncherClone.SetActive(true);
                    itemLauncherClone.AddComponent<RocketLauncherLifecycle>();
                    itemsLaunchers.Add(itemLauncherClone);

                    newPos = new Vector3(item.transform.position.x + (randomX * 0.12f), item.transform.position.y, item.transform.position.z - (randomY * 0.12f));
                    GameObject itemClone = Instantiate(item, newPos, item.transform.rotation) as GameObject;
                    itemClone.SetActive(true);
                    itemClone.AddComponent<Item>();
                    items.Add(itemClone);

                    timer_i = 0.0f;
                }

                foreach (GameObject i in enemies)
                {
                    if (i.GetComponent<Enemy>().isToDelete())
                    {
                        GameObject explosionClone = Instantiate(explosion, new Vector3(i.transform.position.x, explosion.transform.position.y, i.transform.position.z), explosion.transform.rotation) as GameObject;
                        explosionClone.SetActive(true);
                        explosionClone.AddComponent<DestroyEffect>();
                        explosions.Add(explosionClone);
                        i.SetActive(false);
                        enemies.Remove(i);
                        points += ((ActualStagePointer + 1) * 100);
                    }
                }
                foreach (GameObject i in enemiesLaunchers)
                {
                    if (i.GetComponent<RocketLauncherLifecycle>().isToDelete())
                    {
                        i.SetActive(false);
                    }
                }
                foreach (GameObject i in items)
                {
                    if (i.GetComponent<Item>().IsToDelete())
                    {
                        if(i.GetComponent<Item>().GetItemType() == 0)
                        {
                            GameObject explosionClone = Instantiate(explosion, new Vector3(i.transform.position.x, explosion.transform.position.y, i.transform.position.z), explosion.transform.rotation) as GameObject;
                            explosionClone.SetActive(true);
                            explosionClone.AddComponent<DestroyEffect>();
                            explosions.Add(explosionClone);
                        }
                        i.SetActive(false);
                        items.Remove(i);
                    }
                }
                foreach (GameObject i in itemsLaunchers)
                {
                    if (i.GetComponent<RocketLauncherLifecycle>().isToDelete())
                    {
                        i.SetActive(false);
                    }
                }

                if(player.GetComponent<Player>().GetHp() <= 0)
                {
                    GAMESTATE = 4;
                    player.SetActive(false);
                    clear();
                }

                if (enemiesToKill[ActualStagePointer] <= 0)
                {
                    if (ActualStagePointer + 1 < StagesNumber)
                    {
                        GAMESTATE += 1;
                        ActualStagePointer += 1;
                    }
                    else GAMESTATE = 4;

                    clear();
                }
                break;
            case 3:
                if (ActualTextBeforeStagePointer == 0 && firstGamestateIsPossible == false)
                {
                    dialogueTxt.text = startGameInfo.ToString();
                    firstGamestateIsPossible = true;
                }
                else if (ActualTextBeforeStagePointer > 0)
                {
                    if (!dialogueActive)
                    {
                        dialogueTxt.text = WordsAfterStage[ActualTextAfterStagePointer].ToString();
                        dialogueText.SetActive(true);
                        dialogueActive = true;
                        ActualTextAfterStagePointer += 1;
                    }
                    timer += Time.deltaTime;
                    if (timer >= 3 && ActualTextAfterStagePointer < ActualSizeTextAfterStagePointer)
                    {

                        dialogueTxt.text = WordsAfterStage[ActualTextAfterStagePointer].ToString();
                        ActualTextAfterStagePointer += 1;
                        timer = 0.0f;
                    }
                    else if (ActualTextAfterStagePointer == ActualSizeTextAfterStagePointer)
                    {
                        firstGamestateIsPossible = true;
                    }
                }
                break;
            case 4:
                if (!dialogueActive)
                {
                    allElements.SetActive(false);
                    backgroundEnd.SetActive(true);
                    dialogueText.SetActive(true);
                    dialogueActive = true;
                }
                timer += Time.deltaTime;
                if (timer > 5)
                {
                    dialogueTxt.text = "JEŻELI CHCESZ ROZPOCZĄĆ NOWĄ GRĘ, NACIŚNIEJ START!";
                    allElements.SetActive(true);
                    backgroundEnd.SetActive(false);
                    newGame = true;
                }
                break;
        }
    }

    public void ChangeStage()
    {
        if (GAMESTATE == 0 && secGamestateIsPossible == true)
        {
            GAMESTATE = 1;
            timer = 0.0f;
            ActualSizeTextAfterStagePointer += CountWordsAfterStage[ActualStagePointer];
            dialogueText.SetActive(false);
            secGamestateIsPossible = false;
        }
        else if (GAMESTATE == 3 && firstGamestateIsPossible == true)
        {
            GAMESTATE = 0;
            timer = 0.0f;
            ActualSizeTextBeforeStagePointer += CountWordsBeforeStage[ActualStagePointer];
            firstGamestateIsPossible = false;
            dialogueText.SetActive(false);
            dialogueActive = false;
            setPositionButtons.SetActive(false);
        }
        else if (GAMESTATE == 4 && newGame == true)
        {
            GAMESTATE = 3;
            timer = 0.0f;
            firstGamestateIsPossible = false;
            enemiesToKill = new List<int>(toFix);
            secGamestateIsPossible = false;
            rockets = new List<GameObject>();
            rocketsLaunchers = new List<GameObject>();
            enemies = new List<GameObject>();
            enemiesLaunchers = new List<GameObject>();
            explosions = new List<GameObject>();
            ActualTextBeforeStagePointer = 0;
            ActualTextAfterStagePointer = 0;
            ActualSizeTextBeforeStagePointer = 0;
            ActualSizeTextAfterStagePointer = 0;
            ActualStagePointer = 0;
            dialogueActive = true;
            dialogueText.SetActive(true);
            newGame = false;
            points = 0;
            player.GetComponent<Player>().Respawn();
            healthTxt.text = "100%";
            setPositionButtons.SetActive(true);
            player.SetActive(true);
        }
    }

    void clear()
    {
        foreach (GameObject i in rocketsLaunchers) { i.SetActive(false); }
        foreach (GameObject i in rockets) { i.SetActive(false); }
        foreach (GameObject i in enemiesLaunchers) { i.SetActive(false); }
        foreach (GameObject i in enemies) { i.SetActive(false); }
        foreach (GameObject i in explosions) { i.SetActive(false); }
        foreach (GameObject i in items) { i.SetActive(false); }
        foreach (GameObject i in itemsLaunchers) { i.SetActive(false); }
        rockets = new List<GameObject>();
        rocketsLaunchers = new List<GameObject>();
        enemies = new List<GameObject>();
        enemiesLaunchers = new List<GameObject>();
        explosions = new List<GameObject>();
        items = new List<GameObject>();
        itemsLaunchers = new List<GameObject>();
        levelText.SetActive(false);
        pointsSumText.SetActive(false);
        pointText.SetActive(false);
        remainText.SetActive(false);
        dialogueText.SetActive(false);
        dialogueActive = false;
        player.GetComponent<Player>().EndAnimationShoot();

        timer = 0.0f;
        timer_e = 0.0f;
        timer_i = 0.0f;
    }
}
