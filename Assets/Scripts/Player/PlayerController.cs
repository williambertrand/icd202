using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum PlayerState
{
    SAFE,
    HIDING,
    IN_OPEN
}

public class PlayerController : MonoBehaviour
{

    public static PlayerController Instance; 

    const int MAX_SHELLS = 10;

    public Text carryValueText;
    public int CarryShells
    {
        get
        {
            return carryShells;
        }
        set
        {
            if (carryShells == MAX_SHELLS)
            {
                carryShells = MAX_SHELLS;
            }
            else
            {
                carryShells = value;
            }
            carryValueText.text = "" + carryShells;
        }
    }

    private int carryShells;

    public int Health;
    public int MaxHealth = 5;

    public PlayerState playerstate;
    public float currentTripDist;

    private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        Health = MaxHealth;

    }


    // Start is called before the first frame update
    void Start()
    {
        currentTripDist = 0;
        playerstate = PlayerState.SAFE;

        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {

        if(playerstate != PlayerState.SAFE)
        {
            CheckPlayerState();
        }

    }


    void CheckPlayerState()
    {
        int layerMask = 1 << 8;
        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.up, out hit, 2.5f, layerMask))
        {
            playerstate = PlayerState.HIDING;
        }
        else
        {
            playerstate = PlayerState.IN_OPEN;
        }
    }

    public void EatItem(int value)
    {
        if (Health + value > MaxHealth)
        {
            Health = MaxHealth;
        }
        else
        {
            Health += value;
        }

        PlayerHealthBar.Instance.UpdateHealthValue(Health);
    }

    public void TakeDamage(int value)
    {
        if (Health - value <= 0)
        {
            //GameManager.Instance.EndGame();
        }
        else
        {
            Health -= value;
            StartCoroutine("FlashDamage");
        }

        PlayerHealthBar.Instance.UpdateHealthValue(Health);
    }


    //Flash red when taking damage
    IEnumerator FlashDamage()
    {
        for (int n = 0; n < 3; n++)
        {
            spriteRenderer.color = new Color(1f, 0f, 0f, 0.75f);
            yield return new WaitForSeconds(0.1f);
            spriteRenderer.color = new Color(1f, 1f, 1f, 1f);
            yield return new WaitForSeconds(0.1f);
        }
    }
}
