using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HudController : MonoBehaviour
{

    public GameObject[] cardSlots = new GameObject[5];
    // Start is called before the first frame update
    void Start()
    {
        cardSlots[0] = transform.Find("Card_1").gameObject;
        cardSlots[1] = transform.Find("Card_2").gameObject;
        cardSlots[2] = transform.Find("Card_3").gameObject;
        cardSlots[3] = transform.Find("Card_4").gameObject;
        cardSlots[4] = transform.Find("Card_5").gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void AddCard(int slot, Sprite cardSprite)
    {
        // Get the Image component from the GameObject
        Image imageComponent = cardSlots[slot].GetComponent<Image>();

        if (imageComponent != null)
        {
            imageComponent.sprite = cardSprite; 
        }
        else
        {
            Debug.LogError($"GameObject at slot {slot} does not have an Image component.");
        }
    }
}
