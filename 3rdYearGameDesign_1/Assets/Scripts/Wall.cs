using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wall : MonoBehaviour
{
    [SerializeField] float OffSetX = 10;
    [SerializeField] float OffSetY = 10;

    Vector3 StartingPos;

    void Awake()
    {
        StartingPos = this.gameObject.transform.position;
    }

    public void Change()
    {
        float ChangeX = Random.Range(-OffSetX, OffSetX);
        float ChangeY = Random.Range(-OffSetY, OffSetY);

        int Rot = Random.Range(0, 2);

        Vector3 newPos = StartingPos + new Vector3(ChangeX, ChangeY, 0);

        gameObject.transform.position = newPos;

        gameObject.transform.eulerAngles = new Vector3(0.0f, 0.0f, gameObject.transform.eulerAngles.z + 90.0f);
    }
}
