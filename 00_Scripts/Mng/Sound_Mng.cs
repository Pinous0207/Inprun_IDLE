using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sound_Mng
{
    public AudioSource[] _audioSource = new AudioSource[(int)Sound.Max];
    private Dictionary<string, AudioClip> _audioClips = new Dictionary<string, AudioClip>();

    private GameObject _soundRoot = null;

    public float BGMValue = 1.0f, VFXValue = 1.0f;
    public void Init()
    {
        BGMValue = PlayerPrefs.GetFloat("BGM");
        VFXValue = PlayerPrefs.GetFloat("VFX");

        if(_soundRoot == null)
        {
            _soundRoot = GameObject.Find("@SoundRoot");
            if(_soundRoot == null)
            {
                _soundRoot = new GameObject { name = "@SoundRoot" };
                Object.DontDestroyOnLoad(_soundRoot);

                string[] soundTypeName = System.Enum.GetNames(typeof(Sound));
                for(int i = 0; i < soundTypeName.Length - 1; i++)
                {
                    GameObject go = new GameObject { name = soundTypeName[i] };
                    _audioSource[i] = go.AddComponent<AudioSource>();
                    go.transform.parent = _soundRoot.transform;
                }

                _audioSource[(int)Sound.BGM].loop = true;
            }
        }
    }
     
    
    public bool Play(Sound type, string path)
    {
        if(string.IsNullOrEmpty(path))
        {
            Debug.Log("경로에 오디오 파일이 존재하지 않습니다.");
            return false;
        }

        AudioSource audioSource = _audioSource[(int)type];
        
        if (path.Contains("Sound/") == false)
        {
            path = string.Format("Sound/{0}", path);
        }
   
        if (type == Sound.BGM)
        {
            AudioClip audioClip = Resources.Load<AudioClip>(path);

            if (audioSource.isPlaying)
                audioSource.Stop();

            audioSource.clip = audioClip;
          
            audioSource.Play();
        }
        else if(type == Sound.Effect)
        {
            AudioClip audioClip = GetAudioClip(path);
            if (audioClip == null)
                return false;

            audioSource.PlayOneShot(audioClip);
            return true;
        }
        return false;
    }

    private AudioClip GetAudioClip(string path)
    {
        AudioClip audioClip = null;

        if (_audioClips.TryGetValue(path, out audioClip))
            return audioClip;

        audioClip = Resources.Load<AudioClip>(path);

        _audioClips.Add(path, audioClip);
        return audioClip;
    }
}
