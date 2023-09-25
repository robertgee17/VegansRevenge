using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;

public class healthBar : MonoBehaviour
{
    public GameObject uiPrefab;
    public Transform target;
    float visibleTime = 5f;
    float lastMadeVisibleTime;
    public Vector3 scale=new Vector3(1,1,1);

    Transform ui;
    Image healthSlider;
    Transform cam;
    // Start is called before the first frame update
    void Awake()
    {
        Vector3 defaultScale = uiPrefab.transform.localScale;
        cam = Camera.main.transform;
        foreach (Canvas c in FindObjectsOfType<Canvas>())
        {
            if (c.renderMode == RenderMode.WorldSpace)
            {
                ui = Instantiate(uiPrefab, c.transform).transform;
                ui.transform.localScale = new Vector3(defaultScale.x * scale.x, defaultScale.y * scale.y, defaultScale.z * scale.z);
                healthSlider = ui.GetChild(0).GetComponent<Image>();
                ui.gameObject.SetActive(true);
                break;
            }
        }
        GetComponent<ComponentHealth>().OnHealthChanged += OnHealthChanged;
        if (ui != null)
        {
            ui.gameObject.SetActive(false);
        }
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (target == null)
        {
            Destroy(ui.gameObject);
            ui.gameObject.SetActive(false);
        }
        if (ui != null)
        {
            ui.position = target.position;
            ui.forward = -cam.forward;

            if((Time.time - lastMadeVisibleTime > visibleTime) && healthSlider.fillAmount==1)
            {
                ui.gameObject.SetActive(false);
            }
        }

    }
    void OnHealthChanged(float maxHealth,float currentHealth)
    {
        if (ui != null)
        {
            ui.gameObject.SetActive(true);
            lastMadeVisibleTime = Time.time;

            float healthPercent = (float)currentHealth/maxHealth;
            healthSlider.fillAmount = healthPercent;
            if (currentHealth <= 0)
            {
                Destroy(ui.gameObject);
            }
        }
    }
    public void activate()
    {
        ui.gameObject.SetActive(true);
    }
    public void deactivate()
    {
        ui.gameObject.SetActive(false);
    }
    private void OnDestroy()
    {
        try
        {
            Destroy(ui.gameObject);
        }catch(Exception e)
        {

        }
        
    }
}
