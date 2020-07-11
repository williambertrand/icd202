using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UI;

public class GameManager : MonoBehaviour
{

    const float MIN_DIST_REFRESH = 100.0f;

    public PlayerStats playerStats;
    public PlayerController player;

    public TextMeshProUGUI stashText;
    public WorldManager worldManager;


    private int lastStash = 0;
    private const int EnemyDelta = 5; //Every 5 shells add more birds

    public static GameManager Instance;

    public CanvasManager canvasManager;
    public TextMeshProUGUI GameOverText;

    public bool isGameOver = false;


    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        playerStats = new PlayerStats();

    }

    /**
     * Upon entering the safe zone:
     * - Add shells to player stash
     * - add other info to player stats:
     * e.g. most time spent outside safe zone, furthest disatnce traveled? etc
     */
    public void OnSafeZoneEnter()
    {
        playerStats.shellStash += player.GetCarryShells();
        stashText.text = "" + playerStats.shellStash;
        if(player.currentTripDist > MIN_DIST_REFRESH)
        {
            worldManager.Refresh();
        }

        player.playerstate = PlayerState.SAFE;
        player.ResetCarry();

        PlayerTrail.Instance.ClearTrail();

        if(playerStats.shellStash - lastStash > EnemyDelta)
        {
            EnemyManager.Instance.AddEnemies(2);
        }

        lastStash = playerStats.shellStash;
        player.ResetEnergy();
    }

    public void OnSafeZoneExit()
    {
        player.currentTripDist = 0;
        player.playerstate = PlayerState.IN_OPEN;
    }

    public void EndGame()
    {
        if (isGameOver) return;

        isGameOver = true;
        PlayerController.Instance.Movement.enabled = false;
        GameOverText.text = $"You stashed {playerStats.shellStash} shells!";
        canvasManager.SwitchCanvas(CanvasType.GameOver);
        LeaderBoardService.Instance.gamePlayerScore = playerStats.shellStash;
        LeaderBoardService.Instance.ShowLeaderBoardForPlayerScore(playerStats.shellStash);
    }


    public void RestartGame()
    {
        playerStats.shellStash = 0;
        PlayerController.Instance.Restart();
        EnemyManager.Instance.Reset();
        PlayerTrail.Instance.ClearTrail();
        worldManager.Refresh();
        isGameOver = false;
    }
}
