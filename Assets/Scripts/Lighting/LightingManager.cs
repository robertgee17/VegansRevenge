using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class LightingManager : MonoBehaviour
{
    [SerializeField] private Light Sunlight;
    [SerializeField] private float sunIntensity;
    [SerializeField] private float dayRatio;
    [SerializeField] private LightingPreset Preset;

    [SerializeField, Range(0, 24)] private float TimeOfDay;
    [SerializeField] private float clamp;

    [SerializeField] private Vector3 offset;

    private void Update()
    {
        if (Preset == null)
            return;
        if (Application.isPlaying)
        {
            //TimeOfDay += Time.deltaTime;
            if (TimeOfDay <= clamp || TimeOfDay >= 24f - clamp)
            {
                TimeOfDay += Time.deltaTime;
            }
            else
            {
                TimeOfDay += Time.deltaTime / dayRatio;
            }
            TimeOfDay %= 24;
            UpdateLighting();
        }
        else
        {
            UpdateLighting();
        }
    }

    private void UpdateLighting()
    {
        //float time = Mathf.Clamp(TimeOfDay, clamp, 24 - clamp);
        float time = TimeOfDay;
        float timePercent = time / 24f;
        RenderSettings.ambientLight = Preset.AmbientColor.Evaluate(timePercent);
        RenderSettings.fogColor = Preset.FogColor.Evaluate(timePercent);

        if (Sunlight != null)
        {
            Sunlight.color = Preset.DirectionalColor.Evaluate(timePercent);
            //Sunlight.intensity = sunIntensity;
            //Sunlight.transform.localRotation = Quaternion.Euler(new Vector3((timePercent * 360f) - 90f + offset.x, 170f + offset.y, 0 + offset.z));
            if (time >= clamp && time <= 24 - clamp)
            {
                Sunlight.enabled = true;
                Sunlight.intensity = sunIntensity;
                Sunlight.transform.localRotation = Quaternion.Euler(new Vector3((timePercent * 360f) - 90f + offset.x, 170f + offset.y, 0 + offset.z));
            }
            else
            {

                //Sunlight.intensity = moonIntensity;
                //Sunlight.transform.localRotation = Quaternion.Euler(new Vector3(90, 0, 0));
                Sunlight.enabled = false;
            }
        }
    }

    private void OnValidate()
    {
        if (Sunlight != null)
        {
            return;
        }
        if (RenderSettings.sun != null)
        {
            Sunlight = RenderSettings.sun;
        }
        else
        {
            Light[] lights = FindObjectsOfType<Light>();
            foreach(Light light in lights)
            {
                if(light.type == LightType.Directional)
                {
                    Sunlight = light;
                    return;
                }
            }
        }
    }
}
