using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spear : MonoBehaviour
{
    [SerializeField] int m_EnemyKill_Score = 10;

    // Update is called once per frame
    void Update()
    {
        float height = 2f * Camera.main.orthographicSize;
        float width = height * Camera.main.aspect;

        if (transform.position.x + (Mathf.Abs(transform.localScale.x)) < (-width / 2))
        {
            gameObject.SetActive(false);
        }
        else if (transform.position.x - (Mathf.Abs(transform.localScale.x)) > (width / 2))
        {
            gameObject.SetActive(false);
        }
        else if ((transform.position.y + (Mathf.Abs(transform.localScale.y))) < (-height / 2))
        {
            gameObject.SetActive(false);
        }
        else if ((transform.position.y - (Mathf.Abs(transform.localScale.y))) > (height / 2))
        {
            gameObject.SetActive(false);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Wall") || collision.gameObject.layer == LayerMask.NameToLayer("Platform"))
        {
            gameObject.GetComponent<Rigidbody2D>().velocity = Vector2.left * PlayerController.Instance.m_base_move_speed;
        }

        if (collision.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            GameController.Instance.AddScore(m_EnemyKill_Score);
            collision.gameObject.SetActive(false);

            SoundPlayer.Instance.PlaySFX(6);

            GameController.Instance.m_kill++;
        }
    }
}
