using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthDisplay : MonoBehaviour
{
    [SerializeField]Image fill;
    Slider healthSlider;
    Movement player;

    void Start()
    {
        healthSlider = FindObjectOfType<Slider>();
        player = FindObjectOfType<Movement>();

        if(player.IsFuelRestricted)
        {
            player.onFuelChanged += UpdateFuel;
        }
        else
        {
            gameObject.SetActive(false);
        }

        fill = healthSlider.fillRect.GetComponent<Image>();
        fill.color = Color.green;

        healthSlider.maxValue = player.GetFuel();
        healthSlider.value = player.GetFuel();
    }

    void UpdateFuel()
    {
        healthSlider.value = player.GetFuel();

        if(healthSlider.value < healthSlider.maxValue / 3)
        {
            fill.color = Color.red;
        }
        else
        {
            fill.color = Color.blue;
        }
    }
}
