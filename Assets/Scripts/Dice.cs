using UnityEngine;
using TMPro;

public class Dice : MonoBehaviour
{
    public TMP_Text diceValue; //Dice value text
    public GameObject gameController; //Game controller object
    private int ReturnDiceValue(bool twoDiceMode) //Getting the new dice value
    {
        if (twoDiceMode) //If the player rolled 2 dice
        {
            int rand1 = Random.Range(1, 7);
            int rand2 = Random.Range(1, 7);
            return rand1 + rand2; //Returning the sum of the 2 dice
        }
        else //If the player rolled 1 dice
        {
            int rand = Random.Range(1, 7);
            return rand; //Returning the dice value
        }
    }

    public void RollOneDice() //Rolling 1 dice
    {
        int value = ReturnDiceValue(false); //Getting a new dice value
        diceValue.text = value.ToString(); //Updating the dice value text
        gameController.GetComponent<GameController>().NewTurn(value); //Starting a new turn
    }

    public void RollTwoDice()
    {
        int value = ReturnDiceValue(true); //Getting a new dice value
        diceValue.text = value.ToString(); //Updating the dice value text
        gameController.GetComponent<GameController>().NewTurn(value); //Starting a new turn
    }
}
