using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

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

    public TextMeshProUGUI carryValueText;
    public Image maxLabel;

    public MovingSphere Movement;

    private int carryShells;
    public int GetCarryShells()
    {
        return carryShells;
    }

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

        maxLabel.enabled = false;
        Movement = GetComponent<MovingSphere>();
        Movement.enabled = false;
    }

    public AudioClip onHit;
    public AudioClip onEat;
    public AudioClip onPickup;
    AudioSource audioSource;
    public float volume = 0.5f;

    public const float MAX_ENERGY = 100.0f;
    public const float BARNACLE_ENERGY = 10.0f;
    public float CurrentEnergy = MAX_ENERGY;
    public float EneregyRegenSpeed = 10.0f;

    public EnergyBar eneregyBar;


    // Start is called before the first frame update
    void Start()
    {
        currentTripDist = 0;
        playerstate = PlayerState.SAFE;

        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        audioSource = GetComponent<AudioSource>();
        eneregyBar.SetMaxValue(MAX_ENERGY);
    }

    // Update is called once per frame
    void Update()
    {

        if(playerstate != PlayerState.SAFE)
        {
            CheckPlayerState();
        }

        switch (playerstate)
        {
            case PlayerState.HIDING:
                float value = Time.deltaTime * EneregyRegenSpeed;
                RegenEnergy(value);
                break;
            default:
                break;
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

        RegenEnergy(BARNACLE_ENERGY);

        PlayerHealthBar.Instance.UpdateHealthValue(Health);
        audioSource.PlayOneShot(onEat, volume);
    }

    public void OnPickup()
    {
        audioSource.PlayOneShot(onPickup, volume);

        carryShells += 1;
        if (carryShells >= MAX_SHELLS)
        {
            carryShells = MAX_SHELLS;
            maxLabel.enabled = true;
        }
        carryValueText.text = "" + carryShells;
    }

    public void TakeDamage(int value)
    {
        if (Health - value <= 0)
        {
            GameManager.Instance.EndGame();
        }
        else
        {
            Health -= value;
            StartCoroutine("FlashDamage");
        }

        PlayerHealthBar.Instance.UpdateHealthValue(Health);
        audioSource.PlayOneShot(onHit, volume);
    }

    public void UseEnergy(float value)
    {
        CurrentEnergy -= value;
        if (CurrentEnergy < 0)
        {
            CurrentEnergy = 0;
        }
        eneregyBar.SetValue(CurrentEnergy);
    }

    public void RegenEnergy(float value)
    {
        CurrentEnergy += value;
        if (CurrentEnergy > MAX_ENERGY)
        {
            CurrentEnergy = MAX_ENERGY;
        }
        eneregyBar.SetValue(CurrentEnergy);
    }

    public void ResetEnergy()
    {
        CurrentEnergy = MAX_ENERGY;
        eneregyBar.SetValue(CurrentEnergy);
    }

    public void ResetCarry()
    {
        carryShells = 0;
        maxLabel.enabled = false;
        carryValueText.text = "" + carryShells;
    }

    public void Restart()
    {
        carryShells = 0;
        CurrentEnergy = MAX_ENERGY;
        Health = MaxHealth;
        PlayerHealthBar.Instance.UpdateHealthValue(Health);
        transform.position = new Vector3(0, 0.5f, -21.5f);
        Movement.enabled = true;
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
