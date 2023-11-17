using System.Collections;
using System.Collections.Generic;
using UnityEditor.Playables;
using UnityEngine;

public class Dash : MonoBehaviour
{
    PlayerMovement playerMovement;

    [SerializeField] private float dashSpeed;
    [SerializeField] private float dashTime;
    private bool IsOnCooldown;
    [SerializeField] private float cooldown;
    private float cooldownTimer;

    private void Start()
    {
        playerMovement = GetComponent<PlayerMovement>();
        cooldownTimer = cooldown;
    }

    private void Update()
    {
        if (Input.GetKeyDown("space"))
        {
            if (IsOnCooldown == false)
                StartCoroutine(DashAbility());
            IsOnCooldown  = true;
        }
        playerMovement.dashVelocity = 1f;

        if (IsOnCooldown)
        {
            cooldownTimer -= Time.deltaTime;
            if (cooldownTimer <= 0f)
            {
                IsOnCooldown = false;
                cooldownTimer = cooldown;
            }
        }
    }

    IEnumerator DashAbility()
    {
        float startTime = Time.time;
        while (Time.time < startTime + dashTime) 
        {
            playerMovement.dashVelocity = dashSpeed;
            yield return null;
        }
    }
}
