using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEditor.EditorTools;
using UnityEngine;

public class GetIn : MonoBehaviour
{
    public float requiredStayTime = 1.0f;
    public Color litColor = Color.white;
    public int rotation;

    private bool canInteract = true;
    private bool cubeInteracted = false;

    private Renderer squareRenderer;
    private Color initialColor;

    private Coroutine fillCoroutine;

    private float currentTime = 0.0f;

    private bool isPlayerInside = false;
    private Quaternion openRotation;
    private bool isFirstInteraction = true;
    public PlayerController playerController;
    public Zombie zombie;
    public GetOut getOut;
    
    
    void Start()
    {
        squareRenderer = GetComponent<Renderer>();
        initialColor = squareRenderer.material.color;
        openRotation = Quaternion.Euler(0f, rotation, 0);
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !getOut.gate.activeSelf)
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
            canInteract = false;
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

        getOut.gate.SetActive(true);
        getOut.OpenDoor();

        foreach (GameObject zombie in zombie.zombies)
        {
            zombie.SetActive(false);
        }

        isFirstInteraction = true;
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
