using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameController : MonoBehaviour
{
    public Player[] players;
    public Card[] cards;
    private int playerCount;
    public int currentPlayer = 0;
    public TMP_Text curPlayerText;
    public GameObject shopUI;
    public Button dice1Button, dice2Button, shopButton, endTurnButton;
    public GameObject[] playersUI;
    private int reqSightsCount;
    public GameObject[] chooseButtons;
    public GameObject inventoryUI;
    public GameObject inventoryContent;
    public GameObject inventoryCardPrefab;
    public Color[] cardColors;

    private List<GameObject> cardList = new List<GameObject>();

    private void Start()
    {
        if (PlayerPrefs.GetString("gameType", "base") == "base") reqSightsCount = 4;
        playerCount = PlayerPrefs.GetInt("playerCount", 2);
        players = new Player[playerCount];
        foreach (var playerUI in playersUI) playerUI.SetActive(false);
        foreach (var chooseButton in chooseButtons) chooseButton.SetActive(false);
        for (int i = 0; i < players.Length; i++)
        {
            players[i] = ScriptableObject.CreateInstance<Player>();
            players[i].coins = 3;
            players[i].cards = new List<Card> {cards[0], cards[1]};
            playersUI[i].SetActive(true);
            chooseButtons[i].SetActive(true);
        }
    }
    public void NewRound(int diceValue)
    {
        dice1Button.interactable = false;
        dice2Button.interactable = false;
        shopButton.interactable = true;
        endTurnButton.interactable = true;

        foreach (var player in players) //red cards
        {
            foreach (var card in player.cards)
            {
                if (card.triggerValue.Contains(diceValue))
                {
                    if (card.triggerType == 2 && players[currentPlayer] != player)
                    {
                        Action(card.effectCommand, player);
                    }
                }
            }
        }

        foreach (var player in players) //blue cards
        {
            foreach (var card in player.cards)
            {
                if (card.triggerValue.Contains(diceValue))
                {
                    if (card.triggerType == 0)
                    {
                        Action(card.effectCommand, player);
                    }
                }
            }
        }

        foreach (var player in players) //green cards
        {
            foreach (var card in player.cards)
            {
                if (card.triggerValue.Contains(diceValue))
                {
                    if (card.triggerType == 1 && players[currentPlayer] == player)
                    {
                        Action(card.effectCommand, player);
                    }
                }
            }
        }

        foreach (var player in players) //violet cards
        {
            foreach (var card in player.cards)
            {
                if (card.triggerValue.Contains(diceValue))
                {
                    if (card.triggerType == 3 && players[currentPlayer] == player)
                    {
                        Action(card.effectCommand, player);
                    }
                }
            }
        }
    }

    private void Action(string command, Player curPlayer) 
    {
        Debug.LogFormat(command);
        string[] commands = command.Split();


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
                            if (players[currentPlayer].coins >= coins)
                            {
                                players[currentPlayer].coins -= coins;
                                curPlayer.coins += coins;
                            }
                            else
                            {
                                curPlayer.coins += players[currentPlayer].coins;
                                players[currentPlayer].coins = 0;
                            }
                            break;
                        }
                    case "allPlayers": //take coins form all players
                        {
                            foreach (var player in players)
                            {
                                if(player != curPlayer)
                                {
                                    if (player.coins >= coins)
                                    { 
                                        player.coins -= coins;
                                        curPlayer.coins += coins;
                                    }
                                    else
                                    {
                                        curPlayer.coins += player.coins;
                                        player.coins = 0;
                                    }
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
        UpdatePlayersUI();
    }

    public void BuyCard(Card card)
    {
        if (players[currentPlayer].coins >= card.buildPrice)
        {
            players[currentPlayer].coins -= card.buildPrice;
            players[currentPlayer].cards.Add(card);
            shopUI.SetActive(false);
            shopButton.interactable = false;
            UpdatePlayersUI();
        }
    }

    public void EndTurn()
    {
        int sightCount = 0;
        foreach (var card in players[currentPlayer].cards)
        {
            if(card.cardType == "sight") sightCount++;
        }
        if(sightCount == reqSightsCount) //game end
        {
            Debug.LogWarningFormat("Game end! Winner: player " + currentPlayer+1.ToString());
        }
        if (currentPlayer + 1 < playerCount) currentPlayer++;
        else currentPlayer = 0;
        curPlayerText.text = "Текущий игрок: " + (currentPlayer + 1).ToString();
        shopButton.interactable = false;
        endTurnButton.interactable = false;
        dice1Button.interactable = true;
        dice2Button.interactable = true;
    }

    private void UpdatePlayersUI()
    {
        for(int i = 0; i < playerCount; i++)
        {
            TMP_Text[] playerUITexts = playersUI[i].GetComponentsInChildren<TMP_Text>();
            playerUITexts[1].text = "Монет: " + players[i].coins;
        }
    }

    public void OpenPlayerInventory(int playerID)
    {
        if(cardList.Count != 0) foreach (GameObject oldCard in cardList) Destroy(oldCard.gameObject);
        cardList.Clear();
        foreach (var card in players[playerID].cards)
        {
            GameObject cardUI = Instantiate(inventoryCardPrefab, Vector2.zero, Quaternion.identity, inventoryContent.GetComponent<Transform>());
            cardList.Add(cardUI);
            cardUI.GetComponentInChildren<Image>().color = cardColors[card.triggerType];
            TMP_Text[] cardTexts = cardUI.GetComponentsInChildren<TMP_Text>();
            cardTexts[0].text = card.cardName;
            if(card.triggerValue.Length == 1) cardTexts[1].text = card.triggerValue[0].ToString();
            else cardTexts[1].text = card.triggerValue[0].ToString() + "-" + card.triggerValue[card.triggerValue.Length-1].ToString();
            switch (card.cardType)
            {
                case "sight":
                    {
                        cardTexts[2].text = "Достопримечательность";
                        break;
                    }
                case "wheat":
                    {
                        cardTexts[2].text = "Пшеница";
                        break;
                    }
                case "trade":
                    {
                        cardTexts[2].text = "Торговля";
                        break;
                    }
                case "tower":
                    {
                        cardTexts[2].text = "Вышка";
                        break;
                    }
                case "cup":
                    {
                        cardTexts[2].text = "Чашка";
                        break;
                    }
                case "factory":
                    {
                        cardTexts[2].text = "Завод";
                        break;
                    }
                case "cow":
                    {
                        cardTexts[2].text = "Корова";
                        break;
                    }
                case "gear":
                    {
                        cardTexts[2].text = "Шестерня";
                        break;
                    }
                case "fruit":
                    {
                        cardTexts[2].text = "Фрукт";
                        break;
                    }
                default:
                    {
                        cardTexts[2].text = "Error";
                        break;
                    }
            }
            cardTexts[3].text = card.buildPrice.ToString();
            cardTexts[4].text = card.effect;
        }
        inventoryUI.SetActive(true);
    }
}