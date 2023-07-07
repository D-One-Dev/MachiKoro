using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameController : MonoBehaviour
{
    public Player[] players; //Players array
    public Card[] cards; //Cards array
    private int playerCount; //Amount of players
    public int currentPlayer = 0; //Index of the active player
    public TMP_Text curPlayerText; //Current player text
    public GameObject shopUI; //Shop ui object
    public Button dice1Button, dice2Button, shopButton, endTurnButton; //Action buttons
    public GameObject[] playersUI; //Players ui panels
    private int reqSightsCount; //Required amount of sights to win
    public GameObject[] chooseButtons; //Buttons for choosing the player getting the card effect
    public GameObject inventoryUI; //Inventory ui
    public GameObject inventoryContent; //Inventory contents
    public GameObject inventoryCardPrefab; //Inventory card prefab
    public GameObject logPanel; //Log panel object
    public Color[] cardColors; //Card colors

    private List<GameObject> UICardList = new List<GameObject>(); //Array of inventory card ui's

    private void Start()
    {
        if (PlayerPrefs.GetString("gameType", "base") == "base") reqSightsCount = 4; //Setting the required amount of sights
        playerCount = PlayerPrefs.GetInt("playerCount", 2); //Setting the amount of players
        players = new Player[playerCount]; //Creating a new array of players
        foreach (var playerUI in playersUI) playerUI.SetActive(false); //Disabling all player ui panels
        foreach (var chooseButton in chooseButtons) chooseButton.SetActive(false); //Disabling all player choose buttons
        for (int i = 0; i < players.Length; i++) //Setting the players up
        {
            players[i] = ScriptableObject.CreateInstance<Player>(); //Creating new player
            players[i].name = (i+1).ToString();
            players[i].coins = 3; //Setting the default amount of coins
            players[i].cards = new List<Card> {cards[0], cards[1]}; //Setting the default cards
            playersUI[i].SetActive(true); //Enabling the palyer ui
            chooseButtons[i].SetActive(true); //Enabling the choose button
        }
    }
    public void NewTurn(int diceValue) //New turn
    {
        dice1Button.interactable = false; //Disabling the roll 1 dice button
        dice2Button.interactable = false; //Disabling the roll 2 dice button
        shopButton.interactable = true; //Enabling the shop button
        endTurnButton.interactable = true; //Enabling the end turn button

        foreach (var player in players) //Red cards
        {
            foreach (var card in player.cards)
            {
                if (card.triggerValue.Contains(diceValue)) //If the card is thriggered by current dice value
                {
                    if (card.triggerType == 2 && players[currentPlayer] != player) //If the current card is red and it is not the current player's deck
                    {
                        Action(card.effectCommand, player); //Using the card's effect
                    }
                }
            }
        }

        foreach (var player in players) //Blue cards
        {
            foreach (var card in player.cards)
            {
                if (card.triggerValue.Contains(diceValue)) //If the card is thriggered by current dice value
                {
                    if (card.triggerType == 0) //If the current card is blue
                    {
                        Action(card.effectCommand, player); //Using the card's effect
                    }
                }
            }
        }

        foreach (var player in players) //Green cards
        {
            foreach (var card in player.cards)
            {
                if (card.triggerValue.Contains(diceValue)) //If the card is thriggered by current dice value
                {
                    if (card.triggerType == 1 && players[currentPlayer] == player) //If the current card is green and it is the current player's deck
                    {
                        Action(card.effectCommand, player); //Using the card's effect
                    }
                }
            }
        }

        foreach (var player in players) //Violet cards
        {
            foreach (var card in player.cards)
            {
                if (card.triggerValue.Contains(diceValue)) //If the card is thriggered by current dice value
                {
                    if (card.triggerType == 3 && players[currentPlayer] == player) //If the current card is violet and it is the current player's deck
                    {
                        Action(card.effectCommand, player); //Using the card's effect
                    }
                }
            }
        }
    }

    private void Action(string command, Player curPlayer) //Using the card's effect
    {
        Debug.LogFormat(command); //Logging the card command

        string[] commands = command.Split(); //Separating the command to components


        if (commands[0] == "coin+") //Adding the coins to the current player
        {
            int coins = Int32.Parse(commands[1]); //Getting the amount of coins from the command
            if (commands.Length == 2) //Take coins from the bank
            {
                AddLog("Игрок " + curPlayer.name + " получает монет из банка: " + coins);
                curPlayer.coins += coins; //Adding coins to the current player
            }
            else
            {
                switch (commands[2])
                {
                    case "anyPlayer": //Take coins form chosen player
                        {
                            AddLog("Игрок " + curPlayer.name + " получает монет от выбранного игрока: ДОДЕЛАТЬ");
                            break;
                        }
                    case "activePlayer": //Take coins form active player
                        {
                            if (players[currentPlayer].coins >= coins) //If the active player has enough coins
                            {
                                AddLog("Игрок " + curPlayer.name + " получает монет от игрока " + players[currentPlayer].name + ": " + coins);
                                players[currentPlayer].coins -= coins; //Take coins from the active player
                                curPlayer.coins += coins; //Add coins to the current player
                            }
                            else //If the active player doesnt have enough coins
                            {
                                AddLog("Игрок " + curPlayer.name + " получает монет от игрока " + players[currentPlayer].name + ": " + players[currentPlayer].coins);
                                curPlayer.coins += players[currentPlayer].coins; //Add all of the active player's coins to the current palyer
                                players[currentPlayer].coins = 0; //Set the active player's coins to 0
                            }
                            break;
                        }
                    case "allPlayers": //Take coins form all players
                        {
                            foreach (var player in players)
                            {
                                if(player != curPlayer)
                                {
                                    if (player.coins >= coins) //If the player has enough coins
                                    {
                                        AddLog("Игрок " + curPlayer.name + " получает монет от игрока " + player + ": " + coins);
                                        player.coins -= coins; //Take coins from the player
                                        curPlayer.coins += coins; //Add coins to the current player
                                    }
                                    else //If the player doesnt have enough coins
                                    {
                                        AddLog("Игрок " + curPlayer.name + " получает монет от игрока " + player + ": " + player.coins);
                                        curPlayer.coins += player.coins; //Add all coins to the current player
                                        player.coins = 0; //Set the player's coins to 0
                                    }
                                }
                            }
                            break;
                        }
                    case "gear": //Take coins form gear cards
                        {
                            foreach (var card in curPlayer.cards)
                            {
                                if (card.cardType == "gear")
                                {
                                    AddLog("Игрок " + curPlayer.name + " получает монет от карты с шестерней: " + coins);
                                    curPlayer.coins += coins; //Add coins to the current player
                                }
                            }
                            break;
                        }
                    case "wheat": //Take coins form wheat cards
                        {
                            foreach (var card in curPlayer.cards)
                            {
                                if (card.cardType == "wheat")
                                {
                                    AddLog("Игрок " + curPlayer.name + " получает монет от карты с пшеницей: " + coins);
                                    curPlayer.coins += coins; //Add coins to the current player
                                }
                            }
                            break;
                        }
                    case "cow": //Take coins form cow cards
                        {
                            foreach (var card in curPlayer.cards)
                            {
                                if (card.cardType == "cow")
                                {
                                    AddLog("Игрок " + curPlayer.name + " получает монет от карты с коровой: " + coins);
                                    curPlayer.coins += coins; //Add coins to the current player
                                }
                            }
                            break;
                        }
                }
            }
        }

        else if (commands[0] == "changeCard") //Change cards with other player
        {
            AddLog("Игрок " + curPlayer.name + " обменивается картой с выбранным игроком: ДОДЕЛАТЬ");
        }
        UpdatePlayersUI();
    }

    public void BuyCard(Card card) //Buying a card
    {
        if (players[currentPlayer].coins >= card.buildPrice) //If the active palyer has enough coins to buy a card
        {
            players[currentPlayer].coins -= card.buildPrice; //Take coins from the active player
            players[currentPlayer].cards.Add(card); //Add the new card to the active player's deck
            shopUI.SetActive(false); //Close the shop ui
            shopButton.interactable = false; //Disable the shop button
            UpdatePlayersUI(); //Update palyers ui
        }
    }

    public void EndTurn() //Ending the turn
    {
        int sightCount = 0; //Creating the sights counter
        foreach (var card in players[currentPlayer].cards) //Counting the active player's sights
        {
            if(card.cardType == "sight") sightCount++;
        }
        if(sightCount == reqSightsCount) //If the active palyer has the required amount of sights
        {
            Debug.LogWarningFormat("Game end! Winner: player " + currentPlayer+1.ToString()); //Game end
        }

        if (currentPlayer + 1 < playerCount) currentPlayer++; //Switching to the next player
        else currentPlayer = 0;

        curPlayerText.text = "Текущий игрок: " + (currentPlayer + 1).ToString(); //Updating the current player text

        shopButton.interactable = false; //Disabling the shop button
        endTurnButton.interactable = false; //Disabling the end turn button
        dice1Button.interactable = true; //Enabling the roll 1 dice button
        dice2Button.interactable = true; //Enabling the roll 2 dice button
    }

    private void UpdatePlayersUI() //Updating palyer's uis
    {
        for(int i = 0; i < playerCount; i++)
        {
            TMP_Text[] playerUITexts = playersUI[i].GetComponentsInChildren<TMP_Text>(); //Getting the player's ui texts
            playerUITexts[1].text = "Монет: " + players[i].coins; //Setting the player's coins texts
        }
    }

    public void OpenPlayerInventory(int playerID) //Opening the active player's inventory
    {
        if(UICardList.Count != 0) foreach (GameObject oldCard in UICardList) Destroy(oldCard.gameObject); //Clearing the inventory ui
        UICardList.Clear(); //Clearing the cards list
        foreach (var card in players[playerID].cards)
        {
            GameObject cardUI = Instantiate(inventoryCardPrefab, Vector2.zero, Quaternion.identity, inventoryContent.GetComponent<Transform>()); //Creting a new card ui
            UICardList.Add(cardUI); //Adding the new card to the cards list
            cardUI.GetComponentInChildren<Image>().color = cardColors[card.triggerType]; //Setting the card color
            TMP_Text[] cardTexts = cardUI.GetComponentsInChildren<TMP_Text>(); //Getting the card's texts
            cardTexts[0].text = card.cardName; //Setting the card name
            if(card.triggerValue.Length == 1) cardTexts[1].text = card.triggerValue[0].ToString(); //Setting the card trigger value (if single)
            else cardTexts[1].text = card.triggerValue[0].ToString() + "-" + card.triggerValue[card.triggerValue.Length-1].ToString(); //Setting the card trigger value (if multiple)
            switch (card.cardType) //Setting the card type
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
            cardTexts[3].text = card.buildPrice.ToString(); //Setting the card build price
            cardTexts[4].text = card.effect; //Setting the card effect text
        }
        inventoryUI.SetActive(true); //Opening the inventory ui
    }

    private void AddLog(string log)
    {
        logPanel.GetComponent<LogController>().AddLog(log);
    }
}