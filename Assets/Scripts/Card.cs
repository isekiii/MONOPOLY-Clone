using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CardType {Property, CommunityChest, Chance, Utility, TrainStation, Tax, Go, FreePark, Jail, GoToJail}

[CreateAssetMenu(fileName = "New Card", menuName = "Card")]
public class Card : ScriptableObject
{
    public CardType type;
    public new string name;
    public Color color;
    public int price;
    public Sprite sprite;
    public int rent;


}
