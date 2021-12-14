using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    [SerializeField] Text Timer;
    [SerializeField] Text Stage;
    [SerializeField] Text Kill;
    [SerializeField] Text TutorialText;
    [SerializeField] EnemySpawner Spawner;
    [SerializeField] CharController Player;
    [SerializeField] GameObject Blocker;

    [Header("UI Panel")]
    [SerializeField] GameObject MainMenu;
    [SerializeField] GameObject InGame;
    [SerializeField] GameObject TutorialCheck;
    [SerializeField] GameObject Credit;
    [SerializeField] GameObject Pause;
    [SerializeField] GameObject Ending;
    [SerializeField] GameObject CheatOnText;
    [SerializeField] GameObject Option;
    [SerializeField] Text EndingText;
    [SerializeField] GameObject GodMode;
    [SerializeField] Image MainMenuGun;

    [SerializeField] Slider BGMslider;
    [SerializeField] Slider SFXslider;
    [SerializeField] Text BGMtext;
    [SerializeField] Text SFXtext;


    public float BGMsound = 1f; 
    public float SFXsound = 1f;

    Color MenuGunOGColor;

    [Header("SpawnPoint")]
    [SerializeField] GameObject TutorialSpawnPoint;
    [SerializeField] GameObject NormalSpawnPoint;

    [SerializeField] GameObject cam;

    [SerializeField] private bool isTutorial;

    Vector3 PlayerStartPos;

    [SerializeField] int IncreseEnemy = 1;
    [SerializeField] float HealthRestore = 5;
    float totalTime = 0f;
    int totalStage = 1;
    [HideInInspector] public int totalEnemy;

    int StartEnemy;

    bool GunPickup = false;
    bool shoot = false;
    bool throwGun = false;
    bool throwGunStun = false;
    bool dash = false;
    bool stunDash = false;
    bool killAll = false;

    public bool CheatOn = false;

    public bool isPause;

    [SerializeField] GameObject DashBlocker;
    [SerializeField] GameObject EndBlocker;

    [SerializeField] GameObject[] DashDummy;
    [SerializeField] GameObject[] Dummy;
    [SerializeField] GameObject[] GunPoint;


    Wall[] walls;

    // Start is called before the first frame update
    void Start()
    {
        PlayerStartPos = NormalSpawnPoint.transform.position;
        walls = FindObjectsOfType(typeof(Wall)) as Wall[];

        StartEnemy = Spawner.maxEnemy;

        MenuGunOGColor = MainMenuGun.color;
    }

    // Update is called once per frame
    void Update()
    {
        BGMsound = BGMslider.value;
        SFXsound = SFXslider.value;

        gameObject.GetComponent<AudioSource>().volume = BGMsound;

        if (Option.activeInHierarchy)
        {
            BGMtext.text = Mathf.RoundToInt(BGMsound * 100).ToString();
            SFXtext.text = Mathf.RoundToInt(SFXsound * 100).ToString();
        }

        if (Player.isGodMode)
        {
            GodMode.SetActive(true);
        }
        else
        {
            GodMode.SetActive(false);
        }

        Stage.text = "Stage: " + totalStage.ToString();
        Kill.text = "Kill: " + totalEnemy.ToString();

        if (Input.GetKeyDown(KeyCode.Escape) && !MainMenu.activeInHierarchy)
        {
            GamePause();
        }

        if (isTutorial)
        {
            if(!GunPickup)
            {
                TutorialText.text = "[WASD] to Walk \n Walk over the gun to pick up";
                if (Player.Gun_curr != null)
                {
                    GunPickup = true;
                }
            }
            else if(!shoot)
            {
                TutorialText.text = "[Left click] To shoot";
                
                if (Input.GetMouseButtonDown(0))
                {
                    shoot = true;
                }
            }
            else if (!dash)
            {
                TutorialText.text = "[Space] To Dash";
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    dash = true;
                }
            }
            else if (!throwGun)
            {
                TutorialText.text = "[Right Click] To [Throw Gun]";
                if (Input.GetMouseButtonDown(1))
                {
                    throwGun = true;
                    DashBlocker.SetActive(false);
                }
            }
            else if (!throwGunStun)
            {
                TutorialText.text = "Try To [Throw Gun] At the enemy to [Stun] them";
                foreach(GameObject obj in DashDummy)
                {
                    if(obj.TryGetComponent(out Enemy enem))
                    {
                        if (enem.isStun)
                        {
                            throwGunStun = true;
                        }
                    }
                }
            }
            else if (!stunDash)
            {
                TutorialText.text = "Point your cursor at the [Stunned Enemy] and press [Space] \n to perform a [Stun Takedown] to kill the enemy and take their Gun";
                foreach (GameObject obj in DashDummy)
                {
                    if (!obj.activeInHierarchy)
                    {
                        stunDash = true;
                    }
                }
            }else if (!killAll)
            {
                TutorialText.text = "Kill all enemies to process to the next stage";

                if (AllKill())
                {
                    killAll = true;
                    EndBlocker.SetActive(false);
                }
            }
        }
        else
        {
            if (Player.hp_curr > 0)
            {
                totalTime += Time.deltaTime;
                Timer.text = totalTime.ToString();
            }

            if (Spawner.isAllInactive())
            {
                Blocker.SetActive(false);
            }

            if (CheatOn)
            {
                if (Input.GetKeyDown(KeyCode.N))
                {
                    NewStage();
                }

                if (Input.GetKeyDown(KeyCode.G))
                {
                    Player.isGodMode = !Player.isGodMode;
                }

                if (Player.Gun_curr == null)
                {
                    if (Input.GetKeyDown(KeyCode.T))
                    {
                        Player.Gun_curr = GameObject.Find("GunPool").GetComponent<ObjectPool>().GetObj();

                        Player.Gun_curr.GetComponent<Gun>().SetOwner(Player.gameObject);
                        Player.Gun_curr.SetActive(true);
                    }
                }
            }
        }
    }

    public void NewStage()
    {
        totalStage++;

        if (!isTutorial)
        {
            Spawner.maxEnemy += IncreseEnemy;
        }

        Player.hp_curr += HealthRestore;

        if (Player.hp_curr > Player.hp_max)
        {
            Player.hp_curr = Player.hp_max;
        }

        Blocker.SetActive(true);

        GameObject.Find("GunPool").GetComponent<ObjectPool>().SetActiveAll(false);
        GameObject.Find("EnemyPool").GetComponent<ObjectPool>().SetActiveAll(false);

        Spawner.ReSpawnEnemy();

        if (isTutorial)
        {
            totalEnemy = 0;
            TutorialText.gameObject.SetActive(false);
            isTutorial = false;
            Player.Gun_curr.GetComponent<Gun>().Reload(GameObject.Find("GunPool").GetComponent<ObjectPool>().GetObj().GetComponent<Gun>().bullet_max_amount);
        }

        Player.gameObject.transform.position = PlayerStartPos;

        Player.enemies = FindObjectsOfType(typeof(Enemy)) as Enemy[];

        GameObject cam = GameObject.Find("Main Camera");
        cam.transform.position = new Vector3(Player.gameObject.transform.position.x, Player.gameObject.transform.position.y, cam.transform.position.z);

        if (Player.Gun_curr != null)
        {
            Player.Gun_curr.SetActive(true);
        }

        foreach(Wall w in walls)
        {
            w.Change();
        }
    }

    private void ResetStage()
    {
        Player.gameObject.SetActive(true);

        GameObject.Find("GunPool").GetComponent<ObjectPool>().SetActiveAll(false);
        GameObject.Find("EnemyPool").GetComponent<ObjectPool>().SetActiveAll(false);

        Spawner.ReSpawnEnemy();

        Player.gameObject.transform.position = PlayerStartPos;
        Player.Respawn();

        Player.enemies = FindObjectsOfType(typeof(Enemy)) as Enemy[];

        totalTime = 0f;

        foreach (Wall w in walls)
        {
            w.Change();
        }
    }

    public void GameStart()
    {
        gameObject.GetComponent<AudioSource>().Play();

        Cursor.visible = false;

        GameObject.Find("GunPool").GetComponent<ObjectPool>().SetActiveAll(false);
        GameObject.Find("EnemyPool").GetComponent<ObjectPool>().SetActiveAll(false);


        Player.gameObject.SetActive(true);
        Player.Respawn();

        MainMenu.SetActive(false);

        isTutorial = !TutorialCheck.GetComponent<Toggle>().isOn;

        InGame.SetActive(true);

        Spawner.maxEnemy = StartEnemy;

        ResetStage();

        if (!isTutorial)
        {
            GameObject Gun_curr = GameObject.Find("GunPool").GetComponent<ObjectPool>().GetObj();

            Gun_curr.GetComponent<Gun>().SetOwner(Player.gameObject);
            Gun_curr.SetActive(true);

            Player.Gun_curr = Gun_curr;

            TutorialText.gameObject.SetActive(false);

            totalTime = 0;
            totalStage = 1;
            totalEnemy = 0;
        }
        else
        {
            ResetTutorial();

            totalStage = 0;
            Player.gameObject.transform.position = TutorialSpawnPoint.transform.position;
        }

        cam.transform.position = new Vector3(Player.gameObject.transform.position.x, Player.gameObject.transform.position.y, cam.transform.position.z);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void ActiveCredit(bool active)
    {
        Credit.SetActive(active);
    }

    public void ActiveMainMenu(bool active)
    {
        MainMenu.SetActive(active);
        if (active)
        {
            gameObject.GetComponent<AudioSource>().Stop();
            Player.gameObject.SetActive(false);
        }
    }

    public void ActivePause(bool active)
    {
        Pause.SetActive(active);
    }

    public void ActiveIngame(bool active)
    {
        InGame.SetActive(active);
    }

    public void ActiveEnding(bool active)
    {
        Ending.SetActive(active);
    }

    public void ActiveOption(bool active)
    {
        Option.SetActive(active);
        if (!active)
        {
            Player.audioSource.volume = SFXsound;

            Queue<GameObject> EnemyPool = GameObject.Find("EnemyPool").GetComponent<ObjectPool>().pooledObjs;
            foreach(GameObject enem in EnemyPool)
            {
                if (enem.TryGetComponent(out Enemy e))
                {
                    e.audioSource.volume = SFXsound;
                }
            }
        }
    }

    public void UpdateEnding()
    {
        EndingText.text = "Stage Progress: " + totalStage.ToString() + "\n" + "Total Kill: " + totalEnemy.ToString() + "\n" + "Time Taken: " + totalTime.ToString() + " Seconds";
    }

    public void GamePause()
    {
        if (!isPause)
        {
            isPause = true;
            Time.timeScale = 0;
            ActivePause(true);
            Cursor.visible = true;
        }
        else
        {
            isPause = false;
            Time.timeScale = 1;
            ActivePause(false);
            Cursor.visible = false;
        }
    }

    public void ResetTutorial()
    {
        GameObject.Find("EmptyGunPool").GetComponent<ObjectPool>().SetActiveAll(false);

        if (Player.Gun_curr != null)
        {
            Player.Gun_curr.SetActive(false);
            Player.Gun_curr = null;
        }

        TutorialText.gameObject.SetActive(true);

        GunPickup = false;
        shoot = false;
        throwGun = false;
        throwGunStun = false;
        dash = false;
        stunDash = false;
        killAll = false;

        DashBlocker.SetActive(true);
        EndBlocker.SetActive(true);

        foreach (GameObject obj in Dummy)
        {
            obj.SetActive(true);

            GameObject EmptGun = GameObject.Find("EmptyGunPool").GetComponent<ObjectPool>().GetObj();
            
            EmptGun.SetActive(true);

            obj.GetComponent<Enemy>().Gun_curr = EmptGun;
            EmptGun.GetComponent<Gun>().SetOwner(obj);
        }

        Player.enemies = FindObjectsOfType(typeof(Enemy)) as Enemy[];
    }

    public void CursorVisible(bool visible)
    {
        Cursor.visible = visible;
    }

    public void ToggleCheatOn()
    {
        if (!CheatOn)
        {
            CheatOn = true;
            CheatOnText.SetActive(true);
            MainMenuGun.color = Color.red;
        }
        else
        {
            CheatOn = false;
            CheatOnText.SetActive(false);
            MainMenuGun.color = MenuGunOGColor;
        }
    }

    private bool AllKill()
    {
        foreach (GameObject obj in Dummy)
        {
            if (obj.activeInHierarchy)
            {
                return false;
            }
        }

        return true;
    }

}
