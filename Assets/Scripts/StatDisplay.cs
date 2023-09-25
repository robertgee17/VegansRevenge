using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class StatDisplay : MonoBehaviour
{
    private Image img;
    private TextMeshProUGUI text;
    public float value;
    public string statName;
    // Start is called before the first frame update
    void Awake()
    {
        img = GetComponentInChildren<Image>();
        text = GetComponentInChildren<TextMeshProUGUI>();
    }
    private IEnumerator delayFontSize()
    {
        yield return 0;
        text.fontSize = GetComponent<RectTransform>().rect.height;
    }

    public void setValue(float v)
    {
        text.text = v + " " + statName;
        gameObject.SetActive(true);
    }
    public void setValue(float v, string name)
    {
        text.text = v + " " + name;
        gameObject.SetActive(true);
    }
    public void setValue(string name)
    {
        text.text = name;
        gameObject.SetActive(true);
    }
}
