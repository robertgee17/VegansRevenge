using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SlaughterIcon : MonoBehaviour
{
    public Image animalImage;
    public Image fillImage;
    public void updateStatus(gameManager.AnimalType type,float percent)
    {
        animalImage.sprite = gameManager.current.animalPrefabs[(int)type].GetSprite();
        fillImage.fillAmount = percent;
    }
}
