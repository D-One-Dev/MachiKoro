using UnityEngine;
[CreateAssetMenu(fileName = "Card", menuName = "Card")]
public class Card : ScriptableObject
{
    public string cardName;
    public string cardType;
    public int[] triggerValue;
    public int triggerType; //0-blue card(any player turn), 1-green card(your turn),
                            //2-red card(other player turn), 3-violet card(your turn)
    public string effect;
    public string effectCommand;
    public int buildPrice;
}