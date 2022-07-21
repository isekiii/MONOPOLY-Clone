using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

public enum Owner {None, P1, P2, P3, P4}
public class PropertyDisplay : MonoBehaviour
{
    [SerializeField]
    GameObject colorMaterial;
    [SerializeField]
    TMP_Text name;
    [SerializeField]
    TMP_Text price;
    

    public bool canBuy = false;
    
    public Card card;

    public Transform[] position;

    public Owner owner;
    
    
    void Start()
    {
        if (card.type == CardType.Property)
        {
            colorMaterial.GetComponent<Renderer>().material.color = card.color;
            name.text = card.name;
            price.text = $"${card.price}";
        }
        else if (card.type == CardType.Utility)
        {
            name.text = card.name;
            price.text = $"${card.price}";
            
        }
        else if (card.type == CardType.CommunityChest)
        {
            name.text = card.name;
        }
        else if (card.type == CardType.Chance)
        {
            name.text = card.name;
        }
        else if (card.type == CardType.TrainStation)
        {
            name.text = card.name;
            price.text = $"${card.price}";
        }
        else if (card.type == CardType.Go)
        {
            name.text = card.name;
            price.text = "TAKE $200 AS YOU PASS";
        }
        else if (card.type == CardType.Jail)
        {
           // name.text = card.name;
           // price.text = "GO TO JAIL";
        }
        else if (card.type == CardType.FreePark)
        {
            name.text = "FREE";
            price.text = "PARKING";
        }
        else if (card.type == CardType.GoToJail)
        {
          //  name.text = card.name;
          //  price.text = "";
        }
        else if (card.type == CardType.Tax)
        {
            name.text = card.name;
            price.text = "PAY $200";
        }
        
    }

    

    
}
