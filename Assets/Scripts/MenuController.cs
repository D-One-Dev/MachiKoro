using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    public Slider playerCountSlider;
    private int playerCountSliderValue = 2;
    public TMP_Text playerCountText;
    public void SliderUpdate()
    {
        playerCountSliderValue = Mathf.RoundToInt(playerCountSlider.value);
        playerCountText.text = "Игроков: " + playerCountSliderValue.ToString();
    }

    public void StartBaseGame()
    {
        PlayerPrefs.SetInt("playerCount", playerCountSliderValue);
        PlayerPrefs.SetString("gameType", "base");
        SceneManager.LoadScene("gameplay");
    }
}
