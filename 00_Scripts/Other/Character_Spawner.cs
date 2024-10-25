using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

public class Character_Spawner : MonoBehaviour
{
    public Transform[] SpawnTransform = new Transform[6];
    public static Player[] players = new Player[6];
    private void Awake()
    {
        for(int i = 0; i < transform.childCount; i++)
        {
            SpawnTransform[i] = transform.GetChild(i);
        }

        Stage_Mng.m_ReadyEvent += OnReady;
    }

    private void OnReady()
    {
        for (int i = 0; i < Base_Mng.Character.m_Set_Character.Length; i++)
        {
            var data = Base_Mng.Character.m_Set_Character[i];
            if (data != null)
            {
                if (players[i] != null)
                {
                    if (players[i].CH_Data != data.Data)
                    {
                        Destroy(players[i].gameObject);
                        MakePlayer(data, i);
                    }
                }
                else
                {
                    MakePlayer(data, i);
                }
            }
        }
    }

    private void MakePlayer(Character_Holder data, int value)
    {
        string temp = data.Data.m_Character_Name;
        var go = Instantiate(Resources.Load<GameObject>("Character/" + temp));
        players[value] = go.GetComponent<Player>();
        go.transform.position = SpawnTransform[value].position;
        go.transform.LookAt(Vector3.zero);
    }
}