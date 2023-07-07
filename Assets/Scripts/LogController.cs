using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LogController : MonoBehaviour
{
    public TMP_Text logText; //Log panel text
    private List<string> logs = new List<string>(); //Logs list
    public Animator anim; //Log panel animation
    bool isPlaying = false; //Is the animation currently playing

    public void AddLog(string log) //Adding a new log to the list
    {
        logs.Add(log); //Adding the log
        if (!isPlaying) //If the animation is not playing, start the animation
        {
            StartLog();
            isPlaying = true;
        }
    }

    private void StartLog() //Starting the animation
    {
        logText.text = logs[0]; //Setting the panel text
        anim.Play("LogPanelAnimation"); //Starting the animation
    }

    public void NextLog() //Showing next log
    {
        if (logs.Count > 1) //If there are other logs left
        {
            logs.RemoveAt(0); //Removing previous log from the list
            logText.text = logs[0]; //Updating the panel text
            anim.Play("LogPanelAnimation"); //Strting the animation
        }
        else //If there is no logs left
        {
            logs.RemoveAt(0); //Clearing the logs list
            isPlaying = false; //Stopping the animation
        }
    }
}
