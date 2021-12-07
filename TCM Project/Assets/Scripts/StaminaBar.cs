using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StaminaBar : MonoBehaviour
{
    public Slider staminaBar;

    private float maxStamina;

    void Start()
    {
        maxStamina = GameObject.Find("PlayerObject").GetComponent<PlayerController>().maxStamina;

        staminaBar.maxValue = maxStamina;
        staminaBar.value = maxStamina;
    }

    void Update()
    {
        staminaBar.value = GameObject.Find("PlayerObject").GetComponent<PlayerController>().currentStamina; ;
    }

}
