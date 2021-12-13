using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StaminaBar : MonoBehaviour
{
    public Slider staminaBar;

    private float maxStamina;
    private float stamina;
    private bool isEnabled;

    void Start()
    {
        maxStamina = GameObject.Find("PlayerObject").GetComponent<PlayerController>().maxStamina;

        staminaBar.maxValue = maxStamina;
        staminaBar.value = maxStamina;
    }

    private void Update()
    {
        staminaBar.value = GameObject.Find("PlayerObject").GetComponent<PlayerController>().currentStamina;
        stamina = staminaBar.value;

        if (stamina >= maxStamina && isEnabled) { staminaBar.gameObject.SetActive(false); isEnabled = false; }
        else if (stamina < maxStamina && !isEnabled) { staminaBar.gameObject.SetActive(true); isEnabled = true; }
    }
}
