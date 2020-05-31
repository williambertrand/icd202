using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{

    const float MIN_DIST_REFRESH = 100.0f;

    public PlayerStats playerStats;
    public PlayerController player;

    //TODO: playerStash UI
    public Text stashText;

    public WorldManager worldManager;


    

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
        playerStats.shellStash += player.CarryShells;
        stashText.text = "" + playerStats.shellStash;
        if(player.currentTripDist > MIN_DIST_REFRESH)
        {
            //worldManager.Refresh();
            //TODO: Bug with refresh not creating container objects on spawn and not deleteing on refresh
        }

        player.CarryShells = 0;
    }

    public void OnSafeZoneExit()
    {
        player.currentTripDist = 0;
    }
}
