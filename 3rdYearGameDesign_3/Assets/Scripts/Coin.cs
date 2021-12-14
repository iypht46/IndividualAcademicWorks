using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
    [SerializeField] int m_value = 5;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            GameController.Instance.AddScore(m_value);
            SoundPlayer.Instance.PlaySFX1(Random.Range(3, 6));

            this.gameObject.SetActive(false);
        }
    }
}
