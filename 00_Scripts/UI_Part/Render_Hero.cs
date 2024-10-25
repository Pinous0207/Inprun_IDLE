using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class Render_Hero : MonoBehaviour
{
    public Transform[] Circles;
    public Transform Pivot;
    public GameObject[] particles;
    private List<GameObject> CharacterObj = new List<GameObject>();
    public void GetParticle(bool m_B)
    {
        for(int i = 0; i< particles.Length; i++)
        {
            particles[i].SetActive(m_B);
        }
    }

    public void InitHero()
    {
        for(int i = 0; i < CharacterObj.Count; i++)
        {
            Destroy(CharacterObj[i]);
        }
        CharacterObj.Clear();

        for(int i = 0; i< Base_Mng.Character.m_Set_Character.Length; i++)
        {
            if (Base_Mng.Character.m_Set_Character[i] != null)
            {
                string temp = Base_Mng.Character.m_Set_Character[i].Data.m_Character_Name;
                var go = Instantiate(Resources.Load<GameObject>("Character/" + temp));
                CharacterObj.Add(go);
                for (int j = 0; j < go.transform.childCount;j++)
                {
                    go.transform.GetChild(j).gameObject.layer = LayerMask.NameToLayer("Render_Layer");
                }
                go.transform.parent = transform;
                go.GetComponent<Player>().enabled = false;
                go.transform.position = Circles[i].position;
                go.transform.LookAt(Pivot.position);
            }
        }
    }
}
