using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttachedPoint : MonoBehaviour
{
    [SerializeField] GameObject m_MainObject;

    [SerializeField] Collider m_CollideObject = null;

    ConnectionPoint connectionPoint = null;

    public GameObject m_AttachedObject = null;

    [SerializeField] GameObject m_Display;

    [SerializeField] ConnectionPoint m_ConnectP;

    bool isDetached = false;

    Transform hitPos = null;

    ObjectPool m_ExplosionPool;

    PlayerMovement playerMovement;

    // Start is called before the first frame update
    void Start()
    {
        m_ExplosionPool = GameObject.Find("ExplosionPool").GetComponent<ObjectPool>();
        playerMovement = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isDetached)
        {
            isDetached = false;

            DetachModule();
        }

        if(playerMovement.isAttaching && m_MainObject.layer == LayerMask.NameToLayer("Body_Player"))
        {
            if (m_ConnectP != null)
            {
                if (m_AttachedObject == null && m_ConnectP.gameObject.activeInHierarchy)
                {
                    m_Display.SetActive(true);
                }
            }
            else if (m_MainObject.tag == "Player")
            {
                if (m_AttachedObject == null)
                {
                    m_Display.SetActive(true);
                }
            }
        }
        else
        {
            m_Display.SetActive(false);
        }

    }

    void Attach()
    {
        if (m_CollideObject != null)
        {
            if (m_CollideObject.gameObject.TryGetComponent(out ConnectionPoint connection))
            {
                if (connection.MainObject.layer != LayerMask.NameToLayer("Body_Module"))
                {
                    /*m_AttachedObject = connection.MainObject;

                    connectionPoint = connection;

                    connectionPoint.m_AttachPoint = this;
                    connectionPoint.m_ConnectedObject = m_MainObject;
                    connectionPoint.gameObject.SetActive(false);*/

                    connectionPoint = connection;
                    connection.gameObject.SetActive(false);

                    return;
                }

                //Debug.Log("Attach from" + gameObject.name + " " + m_MainObject.name + " with " + m_CollideObject.name + " " + connection.MainObject.name);

                connection.MainObject.transform.position = transform.position;

                connection.MainObject.transform.parent = transform;

                #region SetAngle

                float angle = connection.MainObject.transform.localEulerAngles.y;

                angle = (angle > 180) ? angle - 360 : angle;

                if (angle > -225 && angle <= -135)
                {
                    connection.MainObject.transform.localEulerAngles = new Vector3(0, -180, 0);
                } 
                else if (angle > -135 && angle <= -45)
                {
                    connection.MainObject.transform.localEulerAngles = new Vector3(0, -90, 0);
                }
                else if (angle > -45 && angle <= 45)
                {
                    connection.MainObject.transform.localEulerAngles = new Vector3(0, 0, 0);
                }
                else if (angle > 45 && angle <= 135)
                {
                    connection.MainObject.transform.localEulerAngles = new Vector3(0, 90, 0);
                }
                else if (angle > 135 && angle <= 180)
                {
                    connection.MainObject.transform.localEulerAngles = new Vector3(0, 180, 0);
                }

                #endregion

                m_CollideObject = null;

                m_AttachedObject = connection.MainObject;
                m_AttachedObject.layer = m_MainObject.layer;
                Destroy(m_AttachedObject.GetComponent<Rigidbody>());

                connectionPoint = connection;

                connectionPoint.m_AttachPoint = this;
                connectionPoint.m_ConnectedObject = m_MainObject;
                connectionPoint.gameObject.SetActive(false);

                if(m_AttachedObject.TryGetComponent(out hpSystem hp))
                {
                    hp.ResetHP();
                    hp.invulnerable = false;
                }
            }
        }

    }

    public void Detach()
    {
        isDetached = true;

        hitPos = null;
    }

    void DetachWithHitPos(Transform HitPos)
    {
        isDetached = true;

        hitPos = HitPos;
    }

    public void DetachModule()
    {
        if (m_AttachedObject != null)
        {
            m_AttachedObject.layer = LayerMask.NameToLayer("Body_Module");
            m_AttachedObject.transform.parent = null;

            if (m_AttachedObject.TryGetComponent(out hpSystem hp))
            {
                hp.invulnerable = true;
            }

            Rigidbody rigid = m_AttachedObject.GetComponent<Rigidbody>();

            if (rigid == null)
            {
                rigid = m_AttachedObject.AddComponent<Rigidbody>();
            }

            rigid.mass = 50;
            rigid.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;

            Vector3 detachDir = connectionPoint.MainObject.transform.position - m_MainObject.transform.position;

            rigid.AddForce(detachDir.normalized * 1500, ForceMode.Impulse);

            ChangeLayer(m_AttachedObject);

            m_AttachedObject = null;

            Explode();
        }

        if (connectionPoint != null)
        {
            connectionPoint.gameObject.SetActive(true);
            connectionPoint.m_ConnectedObject = null;
            connectionPoint.m_AttachPoint = null;
            connectionPoint = null;
        }
    }

    public void DetachSingle()
    {
        if (m_AttachedObject != null)
        {
            m_AttachedObject.layer = LayerMask.NameToLayer("Body_Module");
            m_AttachedObject.transform.parent = null;

            if (m_AttachedObject.TryGetComponent(out hpSystem hp))
            {
                hp.invulnerable = true;
            }

            Rigidbody rigid = m_AttachedObject.GetComponent<Rigidbody>();

            if (rigid == null)
            {
                rigid = m_AttachedObject.AddComponent<Rigidbody>();
            }

            rigid.mass = 50;
            rigid.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;

            m_AttachedObject.layer = LayerMask.NameToLayer("Body_Module");
            m_AttachedObject = null;

            Explode();
        }
        
        if (connectionPoint != null && isActiveAndEnabled)
            StartCoroutine("EnableConnectPoint");
    }

    IEnumerator EnableConnectPoint()
    {
        yield return new WaitForSeconds(0.2f);

        if (connectionPoint != null)
        {
            connectionPoint.gameObject.SetActive(true);
            connectionPoint.m_ConnectedObject = null;
            connectionPoint.m_AttachPoint = null;
            connectionPoint = null;
        }
    }

    void Explode()
    {
        GameObject obj = m_ExplosionPool.GetObj();

        if (obj == null)
            return;

        ParticleSystem explode = obj.GetComponent<ParticleSystem>();

        Sound sound = GetComponentInParent<Sound>();

        if (explode != null)
        {
            explode.gameObject.transform.position = this.transform.position;

            explode.gameObject.SetActive(true);
            explode.Play();
        }

        if (sound != null)
        {
            sound.PlayRandom();
        }
    }

    void ChangeLayer(GameObject obj)
    {
        AttachedPoint[] attachedPoints = obj.GetComponentsInChildren<AttachedPoint>();

        foreach (AttachedPoint atp in attachedPoints)
        {
            if (atp.m_AttachedObject == null)
                continue;

            atp.m_AttachedObject.layer = LayerMask.NameToLayer("Body_Module");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("ConnectPoint"))
        {
            if(other.TryGetComponent(out ConnectionPoint cp))
            {
                if(cp.MainObject.TryGetComponent(out Module mod))
                {
                    if (!mod.Attachable && (m_MainObject.layer == LayerMask.NameToLayer("Body_Player")))
                        return;
                }
            }

            m_CollideObject = other;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (m_CollideObject == other)
        {
            m_CollideObject = null;
        }
    }
}
