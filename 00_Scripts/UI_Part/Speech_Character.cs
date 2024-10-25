using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Speech_Character : MonoBehaviour
{
    [SerializeField] private SpeechBubble bubble;
    [SerializeField] private string[] speech_Types;
    [SerializeField] private Camera cam;
    public void Init()
    {
        StopAllCoroutines();
        StartCoroutine(Speech_Coroutine());
    }

    public void DisableCoroutine()
    {
        StopAllCoroutines();
    }

    IEnumerator Speech_Coroutine()
    {
        if (Utils.UI_Holder.Count < 0) yield break;
        yield return new WaitForSeconds(Random.Range(0.0f, 1.0f));
        var go = Instantiate(bubble, Utils.UI_Holder.Peek().transform);
        go.Init(transform, speech_Types, cam);
        yield return new WaitForSeconds(Random.Range(6.0f, 10.0f));

        StartCoroutine(Speech_Coroutine());
    }
}
