using System;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class ButtonAudioHelper : MonoBehaviour
{
    private Button button;

    private void Awake()
    {
        button = GetComponent<Button>();
    }

    private void OnEnable()
    {
        button.onClick.AddListener(AudioManager.PlayButtonClick);
    }

    private void OnDisabled()
    {
        button.onClick.RemoveListener(AudioManager.PlayButtonClick);
    }
}
