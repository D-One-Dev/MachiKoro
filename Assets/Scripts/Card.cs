using UnityEngine;

[CreateAssetMenu(fileName = "Card", menuName = "Card")]
public class Card : ScriptableObject
{
    public string cardName;
    public string cardType;
    public int triggerValue;
    public int triggerType; //0-all players, 1-your turn
    public string effect;
    public int buildPrice;
}
