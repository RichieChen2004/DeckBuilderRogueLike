using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StaminaBar : MonoBehaviour
{

    public Slider slider;
    private float sliderDuration = 0.2f;

    public void SetMaxStamina(float stamina)
    {
        slider.maxValue = stamina;
        slider.value = stamina;
    }

    public void setStamina(float stamina)
    {
        StartCoroutine(SmoothTransition(stamina));
    }

    private IEnumerator SmoothTransition(float targetValue)
    {
        float elapsedTime = 0f;
        float startingValue = slider.value;

        while (elapsedTime < sliderDuration)
        {
            elapsedTime += Time.deltaTime;
            slider.value = Mathf.Lerp(startingValue, targetValue, elapsedTime / sliderDuration);
            yield return null; // Wait until the next frame
        }

        slider.value = targetValue; // Ensure it ends exactly at the target value
    }
}
