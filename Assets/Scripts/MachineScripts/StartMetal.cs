
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;
using System.Linq;

public class StartMetal : MonoBehaviour
{
    public List<GameObject> collectableMetals = new List<GameObject>();
    public float speed = 0;
    public float jumpPower = 0f;
    public float jumpDuration = 0f;
    public GameObject groundMetal;

    

    private void Start()
    {
        foreach (GameObject metal in collectableMetals)
        {
            metal.SetActive(false);
        }

        if (collectableMetals.Count > 0)
        {
            ActivateMetalAndMove(collectableMetals[0]);
        }
    }

    private void ActivateMetalAndMove(GameObject metal)
    {
        int metalIndex = collectableMetals.IndexOf(metal);

        if (metalIndex >= 0 && metalIndex < collectableMetals.Count)
        {
            metal.SetActive(true);
            MoveMetalForward(metal);
        }
    }

    private void MoveMetalForward(GameObject metal)
    {
        int metalIndex = collectableMetals.IndexOf(metal);

        if (metalIndex >= 0)
        {
            Vector3 targetPosition = groundMetal.transform.position;
            metal.transform.DOMove(targetPosition, speed)
                .OnComplete(() =>
                {
                    JumpForward(metal);
                    SetNextMetalActiveAndMove(metal);
                });
        }
    }

    private void JumpForward(GameObject metal)
    {
        Vector3 jumpTarget = metal.transform.position + metal.transform.up * 0f / 3.2f;
        metal.transform.DOJump(jumpTarget, jumpPower, 1, jumpDuration).SetEase(Ease.Linear)

            .OnComplete(() =>
            {
                

                int activeMetalCount = collectableMetals.Count(x => x.activeSelf);              
                Vector3 newPosition = new Vector3(metal.transform.position.x, groundMetal.transform.position.y + 0.04f * activeMetalCount, metal.transform.position.z);
                metal.transform.position = newPosition;
                metal.transform.SetParent(transform);

            });


    }

    public void SetNextMetalActiveAndMove(GameObject currentMetal)
    {
        int currentMetalIndex = collectableMetals.IndexOf(currentMetal);

        if (currentMetalIndex >= 0 && currentMetalIndex + 1 < collectableMetals.Count)
        {
            GameObject nextMetal = collectableMetals[currentMetalIndex + 1];
            ActivateMetalAndMove(nextMetal);
        }
    }
}
