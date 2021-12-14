using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConnectionPointGroup : MonoBehaviour
{
    public ConnectionPoint[] connectionPoints;

    public void DetachAll()
    {
        foreach (ConnectionPoint point in connectionPoints)
        {
            point.Detach();

            //Debug.Log(point.gameObject);
        }
    }

    public bool HasModule()
    {
        foreach (ConnectionPoint point in connectionPoints)
        {
            if (point.m_AttachPoint != null)
                return true;
        }

        return false;
    }
}
