using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Render_Manager : MonoBehaviour
{
   public static Render_Manager instance;

    private void Start()
    {
        if(instance == null)
        {
            instance = this;
        }
    }

    public Camera cam;
    public Render_Hero HERO;
    public Render_Gacha GACHA;
    public Render_Dungeon DUNGEON;

    public Vector2 ReturnScreenPoint(Transform pos)
    {
        return cam.WorldToScreenPoint(pos.position);
    }
}
