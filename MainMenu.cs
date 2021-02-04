using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class MainMenu : MonoBehaviour
{

    public Text DisplayText;
    // Start is called before the first frame update

    public const string mainText = "Welcome to Space Adventure";
    public const string startText = "Navigate each level before the time runs out!\n\nTry collect as many coins as possible!\n\nGo, venture forth, I believe in you!";
    public const string controlsText = "Controls:\nMovement:\nLeft - A\nRight - D\nJump - W\n\nUse left and right click to increase and decrease the gravity around you!";
    public const string quitText = "Quit the game! Go Coward! never return!";

    void Start()
    {
        DisplayText.text = mainText;
    }

    public void PlayGame()
    {
        SceneManager.LoadScene("Multiplayer Level 1");
    }

    public void Quit()
    {
        Application.Quit();
    }

    public void UpdateText(string type)
    {
        switch (type)
        {
            case "Controls":
                DisplayText.text = controlsText;
                break;
            case "Start":
                DisplayText.text = startText;
                break;
            case "Exit":
                DisplayText.text = quitText;
                break;
        }
    }
}
