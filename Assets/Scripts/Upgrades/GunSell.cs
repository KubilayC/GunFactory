using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunSell : MonoBehaviour

{
    public float requiredStayTime = 1.0f;
    public Color litColor = Color.white;

    private bool isPlayerInside = false;
    private float currentTime = 0.0f;
    private Coroutine fillCoroutine;

    private Renderer squareRenderer;
    private Color initialColor;

    public GameObject baitScope;
    public GameObject baitMag;
    public GameObject baitStock;
    public GameObject baitBody;


    public PlayerController playerController;
    public LootManager lootManager;

    private bool allPartsPresent = false;
    private bool isFirstInteraction = true;

    private void Start()
    {
        squareRenderer = GetComponent<Renderer>();
        initialColor = squareRenderer.material.color;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && playerController.offStock.activeSelf && playerController.offBody.activeSelf && playerController.offMag.activeSelf && playerController.offScope.activeSelf)
        {
            isPlayerInside = true;
            if (isFirstInteraction)
            {
                isFirstInteraction = false;
                squareRenderer.material.color = initialColor; 
            }
            fillCoroutine = StartCoroutine(FillColorOverTime());
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInside = false;
            if (fillCoroutine != null)
            {
                StopCoroutine(fillCoroutine);
            }
        }
    }

    private IEnumerator FillColorOverTime()
    {
        currentTime = 0.0f; 
        Color startColor = squareRenderer.material.color; 

        while (currentTime < requiredStayTime)
        {
            currentTime += Time.deltaTime;

            float progress = Mathf.Clamp01(currentTime / requiredStayTime);
            float colorHeight = Mathf.Lerp(0f, 1f, progress);

            squareRenderer.material.color = Color.Lerp(startColor, litColor, colorHeight);

            yield return null;
        }

        squareRenderer.material.color = litColor;

        StartCoroutine(LightUpSquare());
    }

    private IEnumerator LightUpSquare()
    {
        yield return new WaitForSeconds(1.0f);

        if (playerController.offStock.activeSelf && playerController.offScope.activeSelf && playerController.offMag.activeSelf && playerController.offBody.activeSelf)
        {
            playerController.offStock.SetActive(false);
            playerController.offScope.SetActive(false);
            playerController.offMag.SetActive(false);
            playerController.offBody.SetActive(false);
            
            baitStock.SetActive(true);
            baitScope.SetActive(true);
            baitBody.SetActive(true);
            baitMag.SetActive(true);
            MarkPartAsPresent();

            if (allPartsPresent)
            {
                lootManager.gold += 100;
                lootManager.AddGold();
                ResetTableForNextGun();
            }

            isFirstInteraction = true; 

        }
    }
    public void MarkPartAsPresent()
    {
        if (baitScope.activeSelf && baitMag.activeSelf && baitStock.activeSelf && baitBody.activeSelf)
        {
            allPartsPresent = true;
        }
    }
    void ResetTableForNextGun()
    {

        playerController.offStock.SetActive(false);
        playerController.offScope.SetActive(false);
        playerController.offMag.SetActive(false);
        playerController.offBody.SetActive(false);

        baitStock.SetActive(false);
        baitScope.SetActive(false);
        baitBody.SetActive(false);
        baitMag.SetActive(false);

        allPartsPresent = false;
        StartCoroutine(ResetCubeColor());
    }
    private IEnumerator ResetCubeColor()
    {
        float duration = 2.0f; 
        float elapsedTime = 0.0f;

        Color currentColor = squareRenderer.material.color;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;

            float t = Mathf.Clamp01(elapsedTime / duration);
            Color newColor = Color.Lerp(currentColor, initialColor, t);

            squareRenderer.material.color = newColor;

            yield return null;
        }


        squareRenderer.material.color = initialColor;
    }
}





