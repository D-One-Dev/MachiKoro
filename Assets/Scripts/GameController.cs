using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public Player[] players;
    public void NewRound(int diceValue)
    {
        foreach (var player in players)
        {
            foreach (var card in player.cards)
            {
                if (card.triggerValue == diceValue) Debug.LogFormat(card.effect);
            }
        }
    }
}
