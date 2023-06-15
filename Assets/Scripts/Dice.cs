using UnityEngine;
using TMPro;

public class Dice : MonoBehaviour
{
    public TMP_Text diceValue;
    public GameObject gameController;
    private int ReturnDiceValue(bool twoDiceMode)
    {
        if (twoDiceMode)
        {
            int rand1 = Random.Range(1, 7);
            int rand2 = Random.Range(1, 7);
            return rand1 + rand2;
        }
        else
        {
            int rand = Random.Range(1, 7);
            return rand;
        }
    }

    public void RollOneDice()
    {
        int value = ReturnDiceValue(false);
        diceValue.text = value.ToString();
        gameController.GetComponent<GameController>().NewRound(value);
    }

    public void RollTwoDice()
    {
        int value = ReturnDiceValue(true);
        diceValue.text = value.ToString();
        gameController.GetComponent<GameController>().NewRound(value);
    }
}
