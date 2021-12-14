using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : MonoBehaviour
{
    [SerializeField] public float m_block_speed = 2f;
    [SerializeField] public float m_block_scale_x;

    public bool isBeforeReward = false;

    Rigidbody2D m_rb;

    // Start is called before the first frame update
    void Start()
    {
        m_rb = gameObject.GetComponent<Rigidbody2D>();
    }

    private void OnEnable()
    {
        for(int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).gameObject.SetActive(true);
        }

        isBeforeReward = false;
    }

    // Update is called once per frame
    void Update()
    {
        m_rb.velocity = Vector2.left * m_block_speed;
        
        float height = 2f * Camera.main.orthographicSize;
        float width = height * Camera.main.aspect;

        if ((transform.position.x + (m_block_scale_x / 2)) < (-width / 2))
        {
            BlockSpawner.Instance.SpawnBlock();


            if (isBeforeReward)
            {
                PlayerController.Instance.DecSpeed();
            }
            else
            {
                PlayerController.Instance.AddSpeed();
            }
            
            BlockSpawner.Instance.ChangeBlockSpeed();

            GameController.Instance.m_block++;

            Destroy(gameObject);
        }
    }
}
