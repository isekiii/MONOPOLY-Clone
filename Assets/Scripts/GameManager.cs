using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public enum GameState {Ready, Player1, Buy, Player2, Player3, Player4, Pay, Manage, Trade}

public class GameManager : MonoBehaviour
{
    [SerializeField]
    GameObject  buyPanel, rollPanel, cardPanel, tradePanel, rollBtn, rollText, rentPanel, endBtn;
    
    [SerializeField]
    Image p1Panel, p2Panel, p3Panel, p4Panel;
    
    [SerializeField]
    Dice dice1, dice2;

    [SerializeField]
    TMP_Text result, rent;

    [SerializeField]
    FollowPlayer playerCam;

    int diceRes = 0;

    public float moveSpeed = 8f;

    
    public PropertyDisplay[] cards;
    public GameState state;
    bool finishedRolling = false;

    public int[] playerPos = new[] { 0, 0, 0, 0 };

    public int activePlayer = 0;

    public GameObject[] player;
    int [] tempPos = new []{ 0, 0, 0, 0};
    public bool destinationReached = false;
    public int[] destination = new []{ 0, 0, 0, 0};

    [SerializeField]
    CardDisplay cardDisplay;

    Color[] panelColorsOrig;

    BasicAI aiPlayer;
    bool isDoneRolling = false;

    void Start()
    {
        aiPlayer = player[1].GetComponent<BasicAI>();
        panelColorsOrig = new[] { p1Panel.color, p2Panel.color, p3Panel.color, p4Panel.color };
        result.text = "";
        playerPos[activePlayer] = 0;
        activePlayer = 0;
        state = GameState.Ready;

        tempPos[activePlayer] = playerPos[activePlayer];
    }

    void FixedUpdate()
    {
        isDoneRolling = dice1.isDoneRolling;
        ManageStates();
        SetActiveColor();
        
        
        
        destinationReached = playerPos[activePlayer] == destination[activePlayer];
        if (state == GameState.Player1 && finishedRolling)
        {
            int j = playerPos[activePlayer] + 1;
            if (!destinationReached)
            {
                if (Vector3.Distance(player[activePlayer].transform.position, cards[j].position[activePlayer].position) > 0.001f)
                {
                    player[activePlayer].transform.position = Vector3.MoveTowards(player[activePlayer].transform.position, 
                        cards[j].position[activePlayer].position, moveSpeed*Time.deltaTime);
                }
                else
                {
                    StartCoroutine(WaitFor(0.1f));
                    playerPos[activePlayer]++;
                    j++;
                    if (j > 39 || playerPos[activePlayer] > 39)
                    {
                       // j = -1;
                        playerPos[activePlayer] -= 40;
                    }
                }
            }
            else
            {
                tempPos[activePlayer] = playerPos[activePlayer];
                finishedRolling = false;
                if (cards[playerPos[activePlayer]].canBuy)
                {
                    state = GameState.Buy;
                    DisplayCardPanel();
                }
                else if (cards[playerPos[activePlayer]].owner != player[activePlayer].GetComponent<Player>().owner &&
                         (cards[playerPos[activePlayer]].card.type == CardType.Property ||
                             cards[playerPos[activePlayer]].card.type == CardType.Tax ||
                             cards[playerPos[activePlayer]].card.type == CardType.Utility ||
                             cards[playerPos[activePlayer]].card.type == CardType.TrainStation))
                {
                    state = GameState.Pay;
                    rent.text = $"You owe ${cards[playerPos[activePlayer]].card.rent}";
                }
                else if ( cards[playerPos[activePlayer]].card.type == CardType.GoToJail)
                {
                    GoToJail();
                }
                else
                {
                    state = GameState.Manage;
                    DisplayCardPanel();
                    ActionAI();
                }
            }
            if (playerPos[activePlayer] >= 0 && playerPos[activePlayer] < 10) playerCam.SetActivePosition((activePlayer * 4) + 0);
            if (playerPos[activePlayer] >= 10 && playerPos[activePlayer] < 20) playerCam.SetActivePosition((activePlayer * 4) + 1);
            if (playerPos[activePlayer] >= 20 && playerPos[activePlayer] < 30) playerCam.SetActivePosition((activePlayer * 4) + 2);
            if (playerPos[activePlayer] >= 30 && playerPos[activePlayer] < 40) playerCam.SetActivePosition((activePlayer * 4) + 3);
        }
        
    }

    void ManageStates()
    {
        if (player[activePlayer].GetComponent<Player>().isBankrupt)
        {
            Bankrupt(player[activePlayer].GetComponent<Player>().owner);
            activePlayer++;
        }

        if (state == GameState.Ready)
        {
            rollPanel.SetActive(true);
            cardPanel.SetActive(false);
            ActionAI();
        }
        else rollPanel.SetActive(false);
        
        if (state == GameState.Trade)
        {
            tradePanel.SetActive(true);
           // ActionAI();
        }
        else tradePanel.SetActive(false);
        
        if (state == GameState.Buy)
        {
            result.text = "";
            buyPanel.SetActive(true);
            ActionAI();
        }
        else buyPanel.SetActive(false);
        
        if (state == GameState.Pay)
        {
            rentPanel.SetActive(true);
            ActionAI();
        }
        else rentPanel.SetActive(false);
        
        if (state == GameState.Manage)
        {
            endBtn.SetActive(true);
            ActionAI();
        }
        else endBtn.SetActive(false);
    }

    void ActionAI()
    {
        if (player[activePlayer].GetComponent<Player>().isAI && aiPlayer.actionDone) aiPlayer.DoAI();
    }

    public void Pay()
    {
        
        if (cards[playerPos[activePlayer]].owner == Owner.None)
        {
            player[activePlayer].GetComponent<Player>().SpendMoney(cards[playerPos[activePlayer]].card.rent + 400);
        }
        else
        {
            player[activePlayer].GetComponent<Player>().SpendMoney(cards[playerPos[activePlayer]].card.rent + 400);
            AddMoneyToOwner(cards[playerPos[activePlayer]].owner, cards[playerPos[activePlayer]].card.rent );
        }

        state = GameState.Manage;
        ActionAI();
    }

    void AddMoneyToOwner(Owner owner, int money)
    {
        if (owner == Owner.P1) player[0].GetComponent<Player>().AddMoney(money);
        else if (owner == Owner.P2) player[1].GetComponent<Player>().AddMoney(money);
        else if (owner == Owner.P3) player[2].GetComponent<Player>().AddMoney(money);
        else if (owner == Owner.P4) player[3].GetComponent<Player>().AddMoney(money);
    }

    public void RollDices()
    {
        if (state == GameState.Ready)
        {
            rollBtn.SetActive(false);
            rollText.SetActive(true);
            dice1.RollDice();
            dice2.RollDice();
            result.text = "";
            StartCoroutine(WaitForDices());
        }
    }

    IEnumerator WaitForDices()
    {
        yield return new WaitForSeconds(4f);
        result.text = $"You rolled: {dice1.result + dice2.result}";
        diceRes = dice1.result + dice2.result;
        destination[activePlayer] += diceRes;
        if (destination[activePlayer] > 39)
        {
            destination[activePlayer] -= 40;
        }
        finishedRolling = true;
        state = GameState.Player1;
        
        Debug.Log(state);
    }

    IEnumerator WaitFor(float seconds)
    {
        float tmp = moveSpeed;
        moveSpeed = 0f;
        yield return new WaitForSeconds(seconds);
        moveSpeed = tmp;
    }

    public void BuyProperty()
    {
        Player p = player[activePlayer].GetComponent<Player>();
        if (p.GetMoney() >= cards[playerPos[activePlayer]].card.price)
        {
            p.SpendMoney(cards[playerPos[activePlayer]].card.price);
            cards[playerPos[activePlayer]].owner = p.owner;
            cards[playerPos[activePlayer]].canBuy = false;
        }
        else Debug.Log("Not enough money");
        state = GameState.Manage;
    }

    public void EndTurn()
    {
        if (activePlayer == 3)
        {
            activePlayer = 0;
        } else activePlayer++;
        state = GameState.Ready;
        ActionAI();
        rollBtn.SetActive(true);
        rollText.SetActive(false);
        if (playerPos[activePlayer] >= 0 && playerPos[activePlayer] < 10) playerCam.SetActivePosition((activePlayer * 4) + 0);
        if (playerPos[activePlayer] >= 10 && playerPos[activePlayer] < 20) playerCam.SetActivePosition((activePlayer * 4) + 1);
        if (playerPos[activePlayer] >= 20 && playerPos[activePlayer] < 30) playerCam.SetActivePosition((activePlayer * 4) + 2);
        if (playerPos[activePlayer] >= 30 && playerPos[activePlayer] < 40) playerCam.SetActivePosition((activePlayer * 4) + 3);
    }

    void DisplayCardPanel()
    {
        cardPanel.SetActive(true);
        cardDisplay.DisplayCard(cards[playerPos[activePlayer]].card);
    }

    void GoToJail()
    { 
        player[activePlayer].transform.position = cards[10].position[activePlayer].position;
        playerPos[activePlayer] = 10;
        destination[activePlayer] = 10;
        state = GameState.Manage;
    }

    void SetActiveColor()
    {
        if (activePlayer == 0)
        {
            p1Panel.color = new Color(panelColorsOrig[0].r, panelColorsOrig[0].g,panelColorsOrig[0].b, 0.6f);
            p2Panel.color = panelColorsOrig[1];
            p3Panel.color = panelColorsOrig[2];
            p4Panel.color = panelColorsOrig[3];
        }
        else if(activePlayer == 1)
        {
            p1Panel.color = panelColorsOrig[0];
            p2Panel.color = new Color(panelColorsOrig[1].r, panelColorsOrig[1].g,panelColorsOrig[1].b, 0.6f);
            p3Panel.color = panelColorsOrig[2];
            p4Panel.color = panelColorsOrig[3];
        }
        else if(activePlayer == 2)
        {
            p1Panel.color = panelColorsOrig[0];
            p2Panel.color = panelColorsOrig[1];
            p3Panel.color = new Color(panelColorsOrig[2].r, panelColorsOrig[2].g,panelColorsOrig[2].b, 0.6f);
            p4Panel.color = panelColorsOrig[3];
        }
        else if(activePlayer == 3)
        {
            p1Panel.color = panelColorsOrig[0];
            p2Panel.color = panelColorsOrig[1];
            p3Panel.color = panelColorsOrig[2];
            p4Panel.color = new Color(panelColorsOrig[3].r, panelColorsOrig[3].g,panelColorsOrig[3].b, 0.6f);
        }
    }

    void Bankrupt(Owner owner)
    {
        foreach (PropertyDisplay card in cards)
        {
            if (card.owner == owner)
            {
                card.owner = Owner.None;
            }
        }
    }

    public void Trade()
    {
        state = GameState.Trade;
    }
    
}
