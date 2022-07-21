using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Player : MonoBehaviour
{
   // [SerializeField]
   // TMP_Text propertyField;
    [SerializeField]
    string name;

    [SerializeField]
    TMP_Text playerMoney, playerName;

    public Owner owner;
    
    int money = 2300;

    public bool isAI = false;
    public bool isBankrupt = false;

    void Start()
    {
        playerName.text = name;
    }

    void OnTriggerEnter(Collider other)
    {
      //  propertyField.text = other.gameObject.GetComponent<PropertyDisplay>().card.name;
      if (other.gameObject.GetComponent<PropertyDisplay>().card.type == CardType.Go)
      {
          AddMoney(200);
      }
    }

    void Update()
    {
        playerMoney.text = $"${money}";
        if (money < 0)
        {
            isBankrupt = true;
        }
    }

    public void AddMoney(int amount) { money += amount; }

    public void SpendMoney(int amount) { money -= amount; }
    
    public int GetMoney() {return money;}
}
