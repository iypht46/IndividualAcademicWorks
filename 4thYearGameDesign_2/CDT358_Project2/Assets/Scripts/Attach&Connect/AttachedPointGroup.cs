using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttachedPointGroup : MonoBehaviour
{
    public AttachedPoint[] attachedPoints;

    public bool HasObject()
    {
        foreach (AttachedPoint p in attachedPoints)
        {
            if (p.m_AttachedObject != null)
                return true;
        }

        return false;
    }
}
