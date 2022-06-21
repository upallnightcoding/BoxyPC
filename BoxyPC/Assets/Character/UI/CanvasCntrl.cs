using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasCntrl : MonoBehaviour
{
    [SerializeField] private GameObject mainMenuPanel;
    [SerializeField] private GameObject playerPanel;
    [SerializeField] private GameObject gamePanel;
    [SerializeField] private GameObject optionsPanel;
    [SerializeField] private GameObject scoreBoardPanel;
    [SerializeField] private GameObject rulesPanel;
    [SerializeField] private GameObject quitPanel;

    private Screen activeScreen = Screen.MAINMENU_SCREEN;

    public void GotoQuitPanel() => ChangePanel(Screen.QUIT_SCREEN);

    public void GotoMainMenuPanel() => ChangePanel(Screen.MAINMENU_SCREEN);

    public void GotoRulesPanel() => ChangePanel(Screen.RULES_SCREEN);

    public void GotoPlayerPanel() => ChangePanel(Screen.PLAYER_SCREEN);

    public void GotoOptionsPanel() => ChangePanel(Screen.OPTIONS_SCREEN);

    public void GotoScoreBoard() => ChangePanel(Screen.SCOREBOARD_SCREEN);

    public void GotoGamePanel()
    {
        ChangePanel(Screen.GAME_SCREEN);
        BoardCntrl.StartPlay();
    }

    public void BackToMainMenuStopGame()
    {
        GotoMainMenuPanel();
        BoardCntrl.StopPlay();
    }

    private void ChangePanel(Screen newScreen)
    {
        ChangePanelScreen(activeScreen, false);
        ChangePanelScreen(newScreen, true);

        activeScreen = newScreen;
    }

    private void ChangePanelScreen(Screen newScreen, bool activeSw)
    {
        switch(newScreen)
        {
            case Screen.MAINMENU_SCREEN:
                mainMenuPanel.SetActive(activeSw);
                break;
            case Screen.PLAYER_SCREEN:
                playerPanel.SetActive(activeSw);
                break;
            case Screen.OPTIONS_SCREEN:
                optionsPanel.SetActive(activeSw);
                break;
            case Screen.GAME_SCREEN:
                gamePanel.SetActive(activeSw);
                break;
            case Screen.SCOREBOARD_SCREEN:
                scoreBoardPanel.SetActive(activeSw);
                break;
            case Screen.RULES_SCREEN:
                rulesPanel.SetActive(activeSw);
                break;
            case Screen.QUIT_SCREEN:
                quitPanel.SetActive(activeSw);
                break;
        }
    }
}

public enum Screen
{
    MAINMENU_SCREEN,
    PLAYER_SCREEN,
    GAME_SCREEN,
    OPTIONS_SCREEN,
    SCOREBOARD_SCREEN,
    RULES_SCREEN,
    QUIT_SCREEN
}
