using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIHealthBar : MonoBehaviour
{
    public static UIHealthBar instance { get; private set; } //Gte healthbar instance

    public Image mask; //Get mask
    float originalSize; //Set size

    void Awake()
    {
        instance = this; //Set the instance to this gameobject
    }

    // Start is called before the first frame update
    void Start()
    {
        originalSize = mask.rectTransform.rect.width; //Get size to the mask width
    }

    //Update the health bar with the player's current health
    public void SetValue(float value)
    {
        mask.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, originalSize * value);
    }
}
