using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WaypointUI : MonoBehaviour
{
    public Transform target;
    public Vector3 offset;
    public Image img;

    public GameObject waypointGroup;

    public bool isOnScreen = false;

    RectTransform parentRect;

    private void Awake()
    {
        img = gameObject.GetComponent<Image>();
        img.enabled = false; 
    }

    // Update is called once per frame
    void Update()
    {
        parentRect = waypointGroup.GetComponent<RectTransform>();

        float minX = (img.GetPixelAdjustedRect().width / 2) - (parentRect.rect.width / 2);
        float maxX = (parentRect.rect.width / 2) - (img.GetPixelAdjustedRect().width / 2);

        float minY = (img.GetPixelAdjustedRect().height / 2) - (parentRect.rect.height/ 2);
        float maxY = (parentRect.rect.height / 2) - (img.GetPixelAdjustedRect().height / 2);

        Vector2 pos = Camera.main.WorldToScreenPoint(target.position + offset) - new Vector3(Screen.width / 2, Screen.height / 2, 0);

        if (Vector3.Dot((target.position - Camera.main.transform.position), Camera.main.transform.forward) < 0)
        {
            if (pos.x < Screen.width / 2)
            {
                pos.x = maxX;
            }
            else
            {
                pos.x = minX;
            }

            if (pos.y < Screen.height / 2)
            {
                pos.y = maxY;
            }
            else
            {
                pos.y = minY;
            }
        }

        pos.x = pos.x * (parentRect.rect.width / Screen.width);
        pos.y = pos.y * (parentRect.rect.height / Screen.height);

        pos.x = Mathf.Clamp(pos.x, minX, maxX);
        pos.y = Mathf.Clamp(pos.y, minY, maxY);

        GetComponent<RectTransform>().localPosition = pos;

        img.enabled = true;

        if (!isOnScreen)
        {
            Vector3 originalRotation = GetComponent<RectTransform>().eulerAngles;

            GetComponent<RectTransform>().eulerAngles = new Vector3(originalRotation.x, originalRotation.y, (Mathf.Atan2(pos.y, pos.x) * Mathf.Rad2Deg) + 90);
        }
        else
        {
            GetComponent<RectTransform>().localEulerAngles = Vector3.zero;
        }
    }
}
