using System;
using System.Linq;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public Player[] players;

    private void Start()
    {
        foreach (var player in players)
        {
            player.cards.Clear();
            player.coins = 3;
        }
    }
    public void NewRound(int diceValue)
    {
        foreach (var player in players)
        {
            foreach (var card in player.cards)
            {
                if (card.triggerValue.Contains(diceValue))
                {
                    //if(card.triggerType == 0)
                    //{
                        Action(card.effectCommand, player);
                    //}
                }
            }
        }
    }

    private void Action(string command, Player curPlayer) 
    {
        string[] commands = command.Split();
        foreach (var cmd in commands)
        {
            Debug.Log(cmd);
        }


        if (commands[0] == "coin+")
        {
            int coins = Int32.Parse(commands[1]);
            if (commands.Length == 2) //take coins from the bank
            {
                curPlayer.coins += coins;
            }
            else
            {
                switch (commands[2])
                {
                    case "anyPlayer": //take coins form chosen player
                        {
                            break;
                        }
                    case "activePlayer": //take coins form active player
                        { 
                            break;
                        }
                    case "allPlayers": //take coins form all players
                        {
                            foreach (var player in players)
                            {
                                if(player != curPlayer)
                                {
                                    player.coins -= coins;
                                    curPlayer.coins += coins;
                                }
                            }
                            break;
                        }
                    case "gear": //take coins form gear cards
                        {
                            foreach (var card in curPlayer.cards)
                            {
                                if (card.cardType == "gear")
                                {
                                    curPlayer.coins += coins;
                                }
                            }
                            break;
                        }
                    case "wheat": //take coins form wheat cards
                        {
                            foreach (var card in curPlayer.cards)
                            {
                                if (card.cardType == "wheat")
                                {
                                    curPlayer.coins += coins;
                                }
                            }
                            break;
                        }
                    case "cow": //take coins form cow cards
                        {
                            foreach (var card in curPlayer.cards)
                            {
                                if (card.cardType == "cow")
                                {
                                    curPlayer.coins += coins;
                                }
                            }
                            break;
                        }
                }
            }
        }

        else if (commands[0] == "changeCard") //change cards with other player
        {

        }
    }
}
