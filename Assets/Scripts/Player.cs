using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Player", menuName = "Player")]
public class Player : ScriptableObject
{
    public int coins; //The amount of player's coins
    public List<Card> cards; //Player's cards
    public string playerName; //Player name
}
