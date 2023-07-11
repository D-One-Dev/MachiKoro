using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    public Slider playerCountSlider; //Player amount slider
    private int playerCountSliderValue = 2; //Default palyer amount value
    public TMP_Text playerCountText; //Player amount text
    private void Start()
    {
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = 120;
    }
    public void SliderUpdate() //Triggres when the slider value is changed
    {
        playerCountSliderValue = Mathf.RoundToInt(playerCountSlider.value); //Getting the slider value
        playerCountText.text = "Игроков: " + playerCountSliderValue.ToString(); //Updating the player amount text
    }

    public void StartBaseGame()
    {
        PlayerPrefs.SetInt("playerCount", playerCountSliderValue); //Saving the player amount
        PlayerPrefs.SetString("gameType", "base"); //Saving the game type
        SceneManager.LoadScene("gameplay"); //Loading the gameplay scene
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
