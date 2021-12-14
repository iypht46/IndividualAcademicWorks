using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    private static EnemySpawner m_instance;
    public static EnemySpawner Instance { get { return m_instance; } }

    [SerializeField] bool m_SpawnEnemy = true;
    public bool Spawn { get { return m_SpawnEnemy; } }

    [SerializeField] float m_DistanceX;
    [SerializeField] float m_DistanceZ;

    [SerializeField] float m_BoundX;
    [SerializeField] float m_BoundZ;
    [SerializeField] float m_OffsetY;

    [SerializeField] Transform m_tmpPoint;

    GameObject m_Player;
    List<GameObject> m_Enemies = new List<GameObject>();

    [SerializeField] GameObject[] enemyTypes;

    [SerializeField] float m_SpawnDelay = 1f;
    float m_SpawnT = 0;

    [SerializeField] GameObject[] bossTypes;
    int bossIndex = 0;


    public int m_NeedToSpawn = 0;

    bool isSpawning = false;

    bool isBoss = false;

    GameObject LastEnemy = null;

    [SerializeField] public Vector2Int m_EnemyRange = new Vector2Int(0, 3);

    int m_RangeCount = 0;

    private void Awake()
    {
        if (m_instance != null && m_instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            m_instance = this;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        m_Player = GameObject.FindGameObjectWithTag("Player");
        m_SpawnT = m_SpawnDelay;
    }

    // Update is called once per frame
    void Update()
    {
        if(isSpawning && m_SpawnEnemy)
        {
            m_SpawnT += Time.deltaTime;

            if (m_SpawnT > m_SpawnDelay)
            {
                m_SpawnT = 0;

                if (m_NeedToSpawn == 0)
                    isSpawning = false;

                if (LastEnemy != null)
                    LastEnemy.GetComponent<Enemy>().MoveToAssignPos();

                if (isBoss)
                {
                    isBoss = false;
                    return;
                }

                if (isSpawning)
                {
                    LastEnemy = RandomSpawnAt(GetPos());
                    m_NeedToSpawn--;
                }
                else
                {
                    LastEnemy = null;
                }
            }
        }
    }

    public void SetSpawnNumber(int number)
    {
        m_SpawnT = m_SpawnDelay;
        m_NeedToSpawn = number;
        isSpawning = true;
    }

    public bool IncreaseEnemyRange()
    {
        if ((m_EnemyRange.y + 1) > enemyTypes.Length)
            return false;

        m_EnemyRange += new Vector2Int(1, 1);

        return true;
    }

    public bool AllDead()
    {
        foreach (GameObject obj in m_Enemies)
        {
            if (obj.activeInHierarchy)
                return false;
        }

        return true;
    }

    public int GetNumberEnemy(bool active)
    {
        int tmp = 0;

        foreach (GameObject obj in m_Enemies)
        {
            if (obj.activeInHierarchy == active)
                tmp++;
        }

        return tmp;
    }

    public void ClearList()
    {
        foreach (GameObject obj in m_Enemies)
        {
            Destroy(obj);
        }

        m_Enemies.Clear();
    }

    GameObject RandomSpawnAt(Vector3 pos)
    {
        //GameObject obj = enemyPools[Random.Range(0, enemyPools.Length)].GetObj();

        GameObject obj = Instantiate(enemyTypes[Random.Range(m_EnemyRange.x, m_EnemyRange.y)]);

        if (obj != null)
        {
            obj.transform.position = m_tmpPoint.position;

            if(obj.TryGetComponent(out Enemy enemy))
            {
                enemy.m_AssignedPos = pos;
            }

            obj.SetActive(true);
            m_Enemies.Add(obj);
        }

        return obj;
    }

    public GameObject SpawnBoss()
    {
        GameObject obj = Instantiate(bossTypes[bossIndex]);

        if (obj != null)
        {
            obj.transform.position = m_tmpPoint.position;

            if (obj.TryGetComponent(out Enemy enemy))
            {
                enemy.m_AssignedPos = GetPos();
            }

            obj.SetActive(true);
            m_Enemies.Add(obj);
        }

        isBoss = true;
        isSpawning = true;

        LastEnemy = obj;

        bossIndex++;

        bossIndex = Mathf.Clamp(bossIndex, 0, bossTypes.Length - 1);

        return obj;
    }


    public Vector3 GetPos()
    {
        float PosX, PosZ;

        int randDirX = Random.Range(0, 2);
        int randDirZ = Random.Range(0, 2);

        PosX = GetRandomPos(randDirX, m_Player.transform.position.x, m_DistanceX, m_BoundX);
        PosZ = GetRandomPos(randDirZ, m_Player.transform.position.z, m_DistanceZ, m_BoundZ);

        return new Vector3(PosX, m_OffsetY, PosZ);
    }

    float GetRandomPos(int Dir, float CenterPos, float Distace, float Bound)
    {
        float Pos;

        if (Dir == 0)
        {
            float Edge = CenterPos - Distace;

            if (Edge > (Bound * -1))
            {
                Pos = Random.Range(Bound * -1, Edge);
            }
            else
            {
                Edge = CenterPos + Distace;
                Pos = Random.Range(Edge, Bound);
            }

        }
        else
        {
            float Edge = CenterPos + Distace;

            if (Edge < Bound)
            {
                Pos = Random.Range(Edge, Bound);
            }
            else
            {
                Edge = CenterPos - Distace;
                Pos = Random.Range(Bound * -1, Edge);
            }
        }

        return Pos;
    }
}
