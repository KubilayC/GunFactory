using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuyNPC: MonoBehaviour
{
    public float requiredStayTime = 1.0f;
    public Color litColor = Color.white;
    public GameObject activateNPC;
    public GameObject baitNPC;
    public int rotation;

    public int cost = 0;
    private bool canInteract = true; 
    private bool cubeInteracted = false;
    private bool hasEnoughGold = false;

    private Renderer squareRenderer;
    private Color initialColor;

    private Coroutine fillCoroutine;

    private float currentTime = 0.0f;

    private Quaternion openRotation;
    public GameObject doorObject;

    public PlayerController playerController;
    public LootManager lootManager;

    private void Start()
    {
        squareRenderer = GetComponent<Renderer>();
        initialColor = squareRenderer.material.color;
        openRotation = Quaternion.Euler(0f, rotation, 0);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && lootManager.gold >= cost)
        {
            if (lootManager.gold >= 100)
            {
                hasEnoughGold = true;
                canInteract = true;
                fillCoroutine = StartCoroutine(FillColorOverTime());
            }
            else
            {
                hasEnoughGold = false;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            canInteract = false;
            if (fillCoroutine != null)
            {
                StopCoroutine(fillCoroutine);
            }
            squareRenderer.material.color = initialColor;
            currentTime = 0.0f;
        }
    }

    private IEnumerator FillColorOverTime()
{
    while (currentTime < requiredStayTime && canInteract)
    {
        currentTime += Time.deltaTime;

        float progress = Mathf.Clamp01(currentTime / requiredStayTime);
        float colorHeight = Mathf.Lerp(0f, 1f, progress);

        squareRenderer.material.color = Color.Lerp(initialColor, litColor, colorHeight);

        yield return null;
    }

    if (hasEnoughGold && canInteract)
    {
        TryToBuyAndActivate();
    }
}
    private void StartDoorOpening()
    {
        StartCoroutine(RotateDoor());
    }

    private void StartNPCMovement(float delayInSeconds)
    {
        StartCoroutine(DelayedActivateNPC(delayInSeconds));
    }
    private void TryToBuyAndActivate()
    {
        lootManager.gold -= cost;
        UpdateGoldUI();

        StartDoorOpening();

        float npcActivationDelay = 2.0f; // Delay before NPC starts moving
        StartNPCMovement(npcActivationDelay);

        cubeInteracted = true;
    }

    private IEnumerator DelayedActivateNPC(float delayInSeconds)
    {
        yield return new WaitForSeconds(delayInSeconds);

        if (activateNPC != null)
        {
            activateNPC.SetActive(true);
            baitNPC.SetActive(false);
            StartCoroutine(RotateDoor());
        }
    }
    private void ActivateNPCWithDelay(float delayInSeconds)
    {
        StartCoroutine(DelayedActivateNPC(delayInSeconds));
    }
    private void ActivateNPC()
    {
        float delay = 2.0f; 
        ActivateNPCWithDelay(delay);
    }

    private void UpdateGoldUI()
    {
        if (lootManager.goldText != null && lootManager != null)
        {
            lootManager.goldText.text = lootManager.gold.ToString();
        }
    }

    private IEnumerator RotateDoor()
    {
        Quaternion startRotation = doorObject.transform.rotation;
        float rotationDuration = 1.0f; 
        float elapsedTime = 0.0f;

        while (elapsedTime < rotationDuration)
        {
            doorObject.transform.rotation = Quaternion.Slerp(startRotation, openRotation, elapsedTime / rotationDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }


        doorObject.transform.rotation = openRotation;
    }
}
