using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{

    const float MIN_DIST_REFRESH = 100.0f;

    public PlayerStats playerStats;
    public PlayerController player;

    //TODO: playerStash UI
    public TextMeshProUGUI stashText;

    public WorldManager worldManager;

    private int lastStash = 0;
    private const int EnemyDelta = 5; //Every 5 shells add more birds



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
    }

    public void OnSafeZoneExit()
    {
        player.currentTripDist = 0;
        player.playerstate = PlayerState.IN_OPEN;
    }
}
