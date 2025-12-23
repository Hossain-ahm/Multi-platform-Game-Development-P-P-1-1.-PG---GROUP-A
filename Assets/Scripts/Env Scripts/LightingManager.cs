using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightingManager : MonoBehaviour
{
    [SerializeField] private Light dirLight;
    [SerializeField] private LightingPreset preset;
    [SerializeField, Range(0, 24)] private float timeOfDay;
    [SerializeField, Range(0, 1)] private float cycleSpeed;
    [SerializeField] Material cloudMat;
    private void Update()
    {
        if (preset == null)
        {
            return;
        }
        if (Application.isPlaying)
        {
            timeOfDay += Time.deltaTime * cycleSpeed;
            timeOfDay %= 24;
            UpdateLighting(timeOfDay / 24f);
        }
    }
    private void UpdateLighting(float timePercent)
    {
        RenderSettings.ambientLight = preset.AmbientColor.Evaluate(timePercent);
        RenderSettings.fogColor = preset.FogColor.Evaluate(timePercent);

        if (dirLight != null)
        {
            dirLight.color = preset.DirectionalColor.Evaluate(timePercent);
            dirLight.transform.localRotation = Quaternion.Euler(new Vector3((timePercent * 360f) - 90f, 170f, 0));
            cloudMat.color = preset.CloudColor.Evaluate(timePercent) * (20f * preset.CloudHDR.Evaluate(timePercent).r);
        }
    }
    private void OnValidate()
    {
        if (dirLight != null)
        {
            return;
        }
        if (RenderSettings.sun != null)
        {
            dirLight = RenderSettings.sun;
        }
        else
        {
            Light[] lights = FindObjectsOfType<Light>();
            foreach (Light light in lights)
            {
                if (light.type == LightType.Directional)
                {
                    dirLight = light;
                    return;
                }
            }
        }
    }
}
