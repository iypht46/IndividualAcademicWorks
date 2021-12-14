using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Playables;

public class BlockSpawner : MonoBehaviour
{
    private static BlockSpawner m_instance;
    public static BlockSpawner Instance { get { return m_instance; } }

    [SerializeField] Block StartingBlock;
    [SerializeField] GameObject[] BlockPool;
    [SerializeField] GameObject[] RewardBlockPool;

    [SerializeField] int m_RestBlockAt = 10;

    [SerializeField] Vector2Int[] m_SpawnRange;

    int m_SpawnRangeIndex = 0;

    int m_BlockSpawnCount;

    Block LastBlock;

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

    void Start()
    {
        StartingBlock.gameObject.transform.position = Vector3.zero;

        LastBlock = StartingBlock;

        m_BlockSpawnCount = 0;
    }

    public void ChangeBlockSpeed()
    {
        GameObject[] blocks = GameObject.FindGameObjectsWithTag("Block");

        foreach (GameObject obj in blocks)
        {
            if (obj.TryGetComponent(out Block b))
            {
                b.m_block_speed = PlayerController.Instance.m_base_move_speed;
            }
        }
    }

    public void SpawnBlock()
    {
        GameObject obj = null;
        Block tmp = null;

        if (m_BlockSpawnCount == m_RestBlockAt)
        {
            obj = Instantiate(RewardBlockPool[Random.Range(0, RewardBlockPool.Length)]);
            m_BlockSpawnCount = 0;

            IncreaseSpawnRangeIndex(1);
        }
        else
        {
            obj = Instantiate(BlockPool[Random.Range(m_SpawnRange[m_SpawnRangeIndex].x, m_SpawnRange[m_SpawnRangeIndex].y + 1)]);
            m_BlockSpawnCount++;
        }

        if (obj != null)
        {
            tmp = obj.GetComponent<Block>();

            if (m_BlockSpawnCount == m_RestBlockAt)
            {
                tmp.isBeforeReward = true;
            }
        }

        if (tmp != null)
        {
            tmp.gameObject.transform.position = new Vector3(LastBlock.transform.position.x + (LastBlock.GetComponent<Block>().m_block_scale_x / 2f) + (tmp.m_block_scale_x / 2f) - 0.5f, LastBlock.transform.position.y, LastBlock.transform.position.z);

            tmp.gameObject.SetActive(true);

            LastBlock = tmp;
        }
    }

    public void IncreaseSpawnRangeIndex(int add)
    {
        m_SpawnRangeIndex += add;

        if (m_SpawnRangeIndex > m_SpawnRange.Length - 1)
        {
            m_SpawnRangeIndex = m_SpawnRange.Length - 1;
        }
    }

}
