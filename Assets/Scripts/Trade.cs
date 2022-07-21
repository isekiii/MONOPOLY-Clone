using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class Trade : MonoBehaviour
{
    [SerializeField]
    TMP_InputField moneyOffer;

    public TMP_Dropdown playerChoice, propertyChoice;

    [SerializeField]
    GameManager m_GameManager;

    List<string> players;
    List<string> properties;

    void OnEnable()
    {
        players = new List<string>();
        for (int i = 0; i < 4; i++)
        {
            if (i != m_GameManager.activePlayer)
            {
                players.Add(m_GameManager.player[i].GetComponent<Player>().owner.ToString());
            }
        }
        
        
        playerChoice.ClearOptions();
        playerChoice.AddOptions(players);
        
    }



    public void PopulatePropertyDropdown()
    {
        string owner = players[playerChoice.value];
        Owner result;
        Owner.TryParse(owner, true, out result);
        Debug.Log("Parsed this:" + result);
        properties = new List<string>();
        foreach (PropertyDisplay prop in m_GameManager.cards)
        {
            if (prop.owner == result)
            {
                properties.Add(prop.card.name);
            }
        }
        propertyChoice.ClearOptions();
        propertyChoice.AddOptions(properties);
    }

    public void TradeProperty()
    {
        m_GameManager.player[m_GameManager.activePlayer].GetComponent<Player>().SpendMoney(int.Parse(moneyOffer.text));

        selectedProperty().owner = m_GameManager.player[m_GameManager.activePlayer].GetComponent<Player>().owner;

        m_GameManager.state = GameState.Manage;
    }

    PropertyDisplay selectedProperty()
    {
        foreach (PropertyDisplay prop in m_GameManager.cards)
        {
            if (prop.card.name == properties[propertyChoice.value])
            {
                return prop;
            }
        }

        return null;
    }
}
