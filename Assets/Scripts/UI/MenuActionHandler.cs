using System;
using UnityEngine;
using static UI.CanvasType;
using static UI.ButtonType;

namespace UI
{
    public enum ButtonType
    {
        StartGame,
        ResumeGame,
        OpenSettings,
        LeaveSettings,
        Controls,
        QuitGame,
        RestartGame,
    }
    
    /**
     * Handle side effects when a menu button is selected.
     */
    public class MenuActionHandler : MonoBehaviour
    {
        private CanvasManager _canvasManager;

        public GameManager gameManager;

        private void Awake()
        {
            _canvasManager = GameObject.FindGameObjectWithTag("Canvas").GetComponent<CanvasManager>();
        }

        public void OnButtonClicked(ButtonType button)
        {
            switch (button)
            {
                case StartGame:
                    _canvasManager.SwitchCanvas(GameUI);
                    PlayerController.Instance.Movement.enabled = true;
                    break;
                case ResumeGame:
                    _canvasManager.SwitchCanvas(GameUI);
                    break;
                case LeaveSettings:
                    _canvasManager.GoBackToLastCanvas();
                    break;
                case Controls:
                    // TODO add controls canvas
                    break;
                case QuitGame:
                    #if UNITY_EDITOR
                        // Application.Quit() does not work in the editor so
                        // UnityEditor.EditorApplication.isPlaying need to be set to false to end the game
                        UnityEditor.EditorApplication.isPlaying = false;
                    #else
                        Application.Quit();
                    #endif
                    break;
                case RestartGame:
                    Debug.Log("CANVAS RESTART!");
                    GameManager.Instance.RestartGame();
                    _canvasManager.SwitchCanvas(GameUI);
                    break;
                case OpenSettings:
                    _canvasManager.SwitchCanvas(Settings);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(button), button, null);
            }
        }
    }
}