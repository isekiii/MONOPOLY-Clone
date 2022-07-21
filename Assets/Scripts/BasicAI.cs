using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BasicAI:MonoBehaviour
{
    GameManager m_GameManager;
    
    public bool actionDone = true;

    void Start()
    {
        m_GameManager = GameObject.FindObjectOfType<GameManager>();
        actionDone = true;
    }

    


    public void DoAI()
    {
            if (m_GameManager.state == GameState.Ready)
            {
                StartCoroutine(DoRoll());
            }

            if (m_GameManager.state == GameState.Buy)
            {
                StartCoroutine(DoBuy());
            }

            if (m_GameManager.state == GameState.Pay)
            {
                StartCoroutine(DoPay());
            }
            
            if (m_GameManager.state == GameState.Manage)
            {
                StartCoroutine(DoEndTurn());
            }
    }

    IEnumerator DoRoll()
    {
        
        actionDone = false;
        yield return new WaitForSeconds(2f);
        m_GameManager.RollDices();
        yield return new WaitForSeconds(4f);
        actionDone = true;
    }
    IEnumerator DoEndTurn()
    {
        actionDone = false;
        yield return new WaitForSeconds(2f);
        m_GameManager.EndTurn();
        actionDone = true;
    }
    IEnumerator DoBuy()
    {
        actionDone = false;
        yield return new WaitForSeconds(2f);
        m_GameManager.BuyProperty();
        actionDone = true;
    }
    
    IEnumerator DoPay()
    {
        actionDone = false;
        yield return new WaitForSeconds(2f);
        m_GameManager.Pay();
        actionDone = true;
    }

}
