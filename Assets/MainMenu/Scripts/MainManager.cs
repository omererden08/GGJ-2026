using UnityEngine;

public class MainManager : MonoBehaviour
{
    void Start()
    {
        AudioManager.Instance.PlayMusic(0);
        AudioManager.Instance.SetMusicVolume(1f);
        AudioManager.Instance.SetSFXVolume(0.5f);
    }

}
