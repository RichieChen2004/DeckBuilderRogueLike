using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{

    public Slider slider;
    private float sliderDuration = 0.2f;

    public void SetMaxHealth(float health)
    {
        slider.maxValue = health;
        slider.value = health;
    }

    public void setHealth(float targetHealth)
    {
        StartCoroutine(SmoothTransition(targetHealth));
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
