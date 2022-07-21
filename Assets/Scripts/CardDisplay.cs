using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CardDisplay : MonoBehaviour
{
    [SerializeField]
    GameObject colorImg;

    [SerializeField]
    TMP_Text name, rent, price;

    public void DisplayCard(Card card)
    {
        if (card.type == CardType.Property || card.type == CardType.TrainStation)
        {
            colorImg.GetComponent<Image>().color= card.color;
            name.text = card.name;
            rent.text = $"Rent ${card.rent}";
            price.text = $"Price ${card.price}";
        }

        if (card.type == CardType.Chance)
        {
            colorImg.GetComponent<Image>().color= Color.white;
            name.text = card.name;
            rent.text = $"";
            price.text = $"Random card";
        }
        if (card.type == CardType.CommunityChest)
        {
            colorImg.GetComponent<Image>().color= Color.white;
            name.text = card.name;
            rent.text = $"";
            price.text = $"Random card";
        }
        if (card.type == CardType.Go)
        {
            colorImg.GetComponent<Image>().color= Color.white;
            name.text = card.name;
            rent.text = $"";
            price.text = $"Collect $200 as you pass";
        }
        if (card.type == CardType.Jail)
        {
            colorImg.GetComponent<Image>().color= Color.white;
            name.text = card.name;
            rent.text = $"";
            price.text = $"";
        }
        if (card.type == CardType.FreePark)
        {
            colorImg.GetComponent<Image>().color= Color.white;
            name.text = card.name;
            rent.text = $"";
            price.text = $"";
        }
        if (card.type == CardType.GoToJail)
        {
            colorImg.GetComponent<Image>().color= Color.white;
            name.text = card.name;
            rent.text = $"";
            price.text = $"Go straight to jail";
        }
        
        if (card.type == CardType.Tax)
        {
            colorImg.GetComponent<Image>().color= Color.white;
            name.text = card.name;
            rent.text = $"";
            price.text = $"Pay $200";
        }
        
        
        
        
    }
}
