using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MuteManager : MonoBehaviour
{
    public static MuteManager Instance { get; private set; }

    private bool isMuted = false;

    private void Awake()
    {
        // Singleton pattern
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public bool IsMuted()
    {
        return isMuted;
    }

    public void ToggleMute()
    {
        isMuted = !isMuted;
        AudioListener.volume = isMuted ? 0f : 1f;
    }
}

