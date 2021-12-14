using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConnectionPoint : MonoBehaviour
{
    [SerializeField] GameObject m_MainObject;

    public GameObject m_ConnectedObject = null;

    public AttachedPoint m_AttachPoint = null;

    public GameObject MainObject { get { return m_MainObject; }  }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Detach()
    {
        if (m_AttachPoint != null)
        {
            m_AttachPoint.DetachSingle();
            m_AttachPoint = null;
        }
    }
}
