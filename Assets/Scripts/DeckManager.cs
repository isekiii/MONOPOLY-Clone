using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DeckManager : MonoBehaviour
{
    public PropertyDisplay[] properties;

    [SerializeField]
    Image[] cards;
    [SerializeField]
    Owner player;

    void Start()
    {
        foreach (Image img in cards)
        {
            Color clr = img.color;
            clr.a = 0.1f;
            img.color = clr;
        }
    }

    void FixedUpdate()
    {
        UpdateCards();
    }

    void UpdateCards()
    {
        for (int i = 0; i < properties.Length; i++)
        {
            if (properties[i].owner == player)
            {
                Color clr = cards[i].color;
                clr.a = 1.0f;
                cards[i].color = clr;
            }
            else
            {
                Color clr = cards[i].color;
                clr.a = 0.1f;
                cards[i].color = clr;
            }
        }
    }
}
