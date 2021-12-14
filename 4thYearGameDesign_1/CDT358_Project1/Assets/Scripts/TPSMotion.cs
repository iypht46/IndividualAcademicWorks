using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TPSMotion : MonoBehaviour
{
    private TPSMovement m_movement;
    private Rigidbody m_rb;

    [SerializeField] Transform Body;

    void Start()
    {
        m_movement = GetComponent<TPSMovement>();
    }

    // Update is called once per frame
    void Update()
    {
        //Body.transform.localPosition = Vector3.zero;
    }
}
