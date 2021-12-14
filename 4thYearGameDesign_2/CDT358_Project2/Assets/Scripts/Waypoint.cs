using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Waypoint : MonoBehaviour
{
    [SerializeField] private Sprite WaypointSprite;
    [SerializeField] private Color WaypointColor = Color.white;
    [SerializeField] private Vector3 WaypointOffset;

    [SerializeField] bool DisableWhenOnScreen;

    WaypointUI WaypointObj = null;

    private void OnEnable()
    {
        if (WaypointObj != null)
            WaypointObj.gameObject.SetActive(true);
    }

    private void OnDisable()
    {
        if (WaypointObj != null)
            WaypointObj.gameObject.SetActive(false);
    }

    // Start is called before the first frame update
    void Start()
    {
        GameObject wp = WaypointGroup.Instance.CreateWaypoint(); ;
        WaypointObj = wp.GetComponent<WaypointUI>();

        if (WaypointObj != null)
        {
            WaypointObj.target = this.transform;

            WaypointObj.img.sprite = WaypointSprite;
            WaypointObj.img.color = WaypointColor;
            WaypointObj.offset = WaypointOffset;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (WaypointObj != null)
        {
            WaypointObj.img.sprite = WaypointSprite;
            WaypointObj.img.color = WaypointColor;

            WaypointObj.offset = WaypointOffset;

            WaypointObj.gameObject.SetActive(true);
        }

        Vector2 pos = Camera.main.WorldToScreenPoint(transform.position);

        WaypointObj.isOnScreen = false;

        if (!((pos.x > Screen.width) || (pos.x < 0) || (pos.y > Screen.height) || (pos.y < 0)))
        {
            WaypointObj.isOnScreen = true;

            if (DisableWhenOnScreen)
            {
                if (WaypointObj != null)
                    WaypointObj.gameObject.SetActive(false);
            }
        }
    }
}
