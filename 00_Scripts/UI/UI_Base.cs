using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Base : MonoBehaviour
{
    protected bool _init = false;
    public virtual bool Init()
    {
        if (_init)
            return false;

        return _init = true;
    }

    private void Start()
    {
        Base_Mng.Sound.Play(Sound.Effect, "Click");
        Init();
    }

    public virtual void DisableOBJ()
    {
        Utils.UI_Holder.Pop();
        Base_Mng.Sound.Play(Sound.Effect, "Click");
        Destroy(this.gameObject);
    }
}
