
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;
using System.Linq;

public class StartPlastic : MonoBehaviour
{
    public List<GameObject> collectablePlastics = new List<GameObject>();
    public float speed = 0;
    public float jumpPower = 0f;
    public float jumpDuration = 0f;
    public GameObject groundPlastic;
    public StartMetal startMetal;

    

    private void Start()
    {
        foreach (GameObject plastic in collectablePlastics)
        {
            plastic.SetActive(false);
        }

        if (collectablePlastics.Count > 0)
        {
            ActivatePlasticAndMove(collectablePlastics[0]);
        }
    }
    
    private void ActivatePlasticAndMove(GameObject plastic)
    {
        int plasticIndex = collectablePlastics.IndexOf(plastic);

        if (plasticIndex >= 0 && plasticIndex < collectablePlastics.Count)
        {
            plastic.SetActive(true);
            MovePlasticForward(plastic);
        }
    }

    private void MovePlasticForward(GameObject plastic)
    {
        int plasticIndex = collectablePlastics.IndexOf(plastic);

        if (plasticIndex >= 0)
        {
            Vector3 targetPosition = groundPlastic.transform.position;
            plastic.transform.DOMove(targetPosition, speed)
                .OnComplete(() =>
                {
                    JumpForward(plastic);
                    SetNextPlasticActiveAndMove(plastic);
                });
        }
    }

    private void JumpForward(GameObject plastic)
    {
        Vector3 jumpTarget = plastic.transform.position + plastic.transform.up * 0f / 3.2f;
        plastic.transform.DOJump(jumpTarget, jumpPower, 1, jumpDuration).SetEase(Ease.Linear)
            .OnComplete(() =>
            {
                int activePlasticCount = collectablePlastics.Count(x => x.activeSelf);
                Vector3 newPosition = new Vector3(plastic.transform.position.x, groundPlastic.transform.position.y + 0.08f * activePlasticCount, plastic.transform.position.z);
                plastic.transform.position = newPosition;
                plastic.transform.SetParent(transform);
            });


    }

    private void SetNextPlasticActiveAndMove(GameObject currentPlastic)
    {
        int currentPlasticIndex = collectablePlastics.IndexOf(currentPlastic);

        if (currentPlasticIndex >= 0 && currentPlasticIndex + 1 < collectablePlastics.Count)
        {
            GameObject nextPlastic = collectablePlastics[currentPlasticIndex + 1];
            ActivatePlasticAndMove(nextPlastic);
        }
    }
}
