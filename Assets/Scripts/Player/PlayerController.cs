using System;
using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Movement Script")]
    [SerializeField] private PlayerMovement playerMovement;
    [SerializeField] private PlayerAim playerAiming;
    [SerializeField] private PlayerShooting playerShooting;
    [SerializeField] private PlayerStealth playerStealth;
    [SerializeField] private PlayerInventory playerInventory;
    [SerializeField] private PlayerInteraction playerInteraction;
    [SerializeField] private LifeModule playerLife;
    [SerializeField] private PlayerMana PlayerMana;
    [SerializeField] private AudioSource footSource;

    bool isInUI = false;

    #region ENABLE-DISABLE Input
    private void OnEnable()
    {
        MissionClearUI.OnMissionClear += SetOnGameOverOrClear;
        IntroSequence.OnIntroFinished += SetOnStarted;
    }
    private void OnDisable()
    {
        MissionClearUI.OnMissionClear -= SetOnGameOverOrClear;
        IntroSequence.OnIntroFinished -= SetOnStarted;
    }
    #endregion

    private void Awake()
    {
        playerMovement.SetAimScriptReference(playerAiming);
    }

    // Update is called once per frame
    void Update()
    {
        if (!isInUI)
        {
            playerAiming.HandleAiming();
            playerMovement.HandleMovementAnimation();
            playerShooting.HandleShooting();
        }
    }

    private void FixedUpdate()
    {
        if (!isInUI)
        {
            playerMovement.HandleMovement();
        }
    }

    private void SetOnGameOverOrClear()
    {
        isInUI = true;
        this.gameObject.GetComponent<Collider2D>().enabled = false;
        this.gameObject.GetComponent<AudioSource>().enabled = false;
        footSource.enabled = false;
        playerLife.enabled = false;
        PlayerMana.enabled = false;
        playerStealth.enabled = false;
        playerInventory.enabled = false;
        playerInteraction.enabled = false;
        playerShooting.enabled = false;
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        if (rb)
        {
            rb.linearVelocity = Vector2.zero;
            rb.angularVelocity = 0f;
            rb.simulated = false;
        }
    }

    private void SetOnStarted()
    {
        isInUI = false;
        this.gameObject.GetComponent<Collider2D>().enabled = true;
        this.gameObject.GetComponent<AudioSource>().enabled = true;
        footSource.enabled = true;
        playerLife.enabled = true;
        PlayerMana.enabled = true;
        playerStealth.enabled = true;
        playerInventory.enabled = true;
        playerInteraction.enabled = true;
        playerShooting.enabled = true;
    }
}
