using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private float CameraThreshold = 2;
    [SerializeField] private float lerpSpeed = 0.2f;
    [SerializeField] private float distdivFactor = 4;

    [Header("CameraShaking")]

    public bool ScreenShake;
    public float ShakeTime = 0.25f;
    public float ShakeAmount = 0.7f;
    public float DecreaseFactor = 1.0f;

    GameObject player;
    GameObject mouse;

    bool isMove;

    Vector3 TargetPos;
    Vector3 originalPos;

    float T = 0.0f;



    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player");
        mouse = GameObject.Find("Mouse");

        T = ShakeTime;
    }

    public void screenShake(float amount)
    {
        ShakeAmount = amount;
        ScreenShake = true;
    }

    public void screenShake()
    {
        ScreenShake = true;
    }



    // Update is called once per frame
    void FixedUpdate()
    {
        if (Vector3.Distance(player.transform.position, mouse.transform.position) > CameraThreshold)
        {
            transform.position = Vector3.Lerp(transform.position, new Vector3(player.transform.position.x, player.transform.position.y, transform.position.z) + ((mouse.transform.position - player.transform.position) / distdivFactor), lerpSpeed * Time.deltaTime);
        }
        else
        {
            transform.position = Vector3.Lerp(transform.position, new Vector3(player.transform.position.x, player.transform.position.y, transform.position.z), (lerpSpeed / 2) * Time.deltaTime);
        }

        if (ScreenShake)
        {
            if (T > 0)
            {
                transform.position = originalPos + (Random.insideUnitSphere * ShakeAmount);
                T -= Time.deltaTime * DecreaseFactor;
            }
            else
            {
                transform.position = originalPos;
                T = ShakeTime;
                ScreenShake = false;
            }
        }
        else
        {
            originalPos = transform.position;
        }
    }
}
