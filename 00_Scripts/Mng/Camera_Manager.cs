using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera_Manager : MonoBehaviour
{
    public static Camera_Manager instance = null;
    private void Awake()
    {
        if (instance == null) instance = this;
    }

    float m_Distance = 4.0f;
    
    [Range(0.0f, 10.0f)]
    [SerializeField] private float Distance_Value;
    
    [Space(20f)]
    [Header("## Camera Shake")]
    [Range(0.0f, 10.0f)]
    [SerializeField] private float Duration;
    [Range(0.0f, 10.0f)][SerializeField] private float Power;

    Vector3 OriginalPos;
    bool isCameraShake = false;
    Camera cam;

    private void Start()
    {
        cam = GetComponent<Camera>();
        OriginalPos = transform.localPosition;
    }

    private void Update()
    {
        cam.orthographicSize = Mathf.Lerp(cam.orthographicSize, Distance(), Time.deltaTime * 2.0f);
    }

    float Distance()
    {
        var players = Spawner.m_Players.ToArray();
        float maxDistance = m_Distance;

        foreach(var player in players)
        {
            float targetDistance = Vector3.Distance(Vector3.zero, player.transform.position) + Distance_Value;

            if(targetDistance > maxDistance)
            {
                maxDistance = targetDistance;
            }
        }
        return maxDistance;
    }

    public void CameraShake()
    {
        if (isCameraShake) return;
        isCameraShake = true;
        StartCoroutine(CameraShake_Coroutine());
    }

    IEnumerator CameraShake_Coroutine()
    {
        float timer = 0.0f;

        while (timer <= Duration)
        {
            transform.localPosition = Random.insideUnitSphere * Power + OriginalPos;

            timer += Time.deltaTime;
            yield return null;
        }

        transform.localPosition = OriginalPos;
        isCameraShake = false;
    }
}
