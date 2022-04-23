using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    private static AudioManager instance;

    [SerializeField]
    private AudioSource buttonClickSound;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public static void PlayButtonClick()
    {
        instance.buttonClickSound.pitch = Random.Range(0.95f, 1.05f);
        instance.buttonClickSound.Play();
    }
}
