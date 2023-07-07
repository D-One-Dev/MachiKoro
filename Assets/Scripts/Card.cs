using UnityEngine;
[CreateAssetMenu(fileName = "Card", menuName = "Card")]
public class Card : ScriptableObject
{
    public string cardName; //Card name
    public string cardType; //Card type
    public int[] triggerValue; //Card trigger value (values)
    public int triggerType; //0-blue card(any player turn), 1-green card(your turn),
                            //2-red card(other player turn), 3-violet card(your turn),
                            //4-brown card(sights)
    public string effect; //Card effect
    public string effectCommand; //Card command
    public int buildPrice; //Card build price
}