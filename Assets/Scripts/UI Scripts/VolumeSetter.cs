using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VolumeSetter : MonoBehaviour
{
    [SerializeField] AudioSource[] playerSFX;
    [SerializeField] AudioSource envSFX;

    [SerializeField] Slider playerSlider, envSlider;
    void Update()
    {
        foreach (var src in playerSFX)
        {
            src.volume = playerSlider.value;
        }
        envSFX.volume = envSlider.value;
    }
}
