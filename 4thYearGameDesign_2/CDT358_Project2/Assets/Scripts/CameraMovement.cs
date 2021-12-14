using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    [SerializeField] Transform m_FollowTarget;
    [SerializeField] float m_HeightMultiply = 2.0f;
    [SerializeField] int m_MaxHeight = 10;

    [SerializeField] float m_Speed = 10f;


    Player player;

    Vector3 m_Offset = Vector3.zero;

    // Start is called before the first frame update
    void Start()
    {
        m_Offset = transform.position;

        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
    }

    // Update is called once per frame
    void Update()
    {
    }

    private void FixedUpdate()
    {

        Vector3 Target = m_FollowTarget.position + m_Offset + (transform.forward * -1 * (m_HeightMultiply * Mathf.Clamp(player.GetTotalModule(), 0, m_MaxHeight)));


        transform.position = Vector3.Lerp(transform.position, Target, m_Speed * Time.deltaTime);
    }
}
