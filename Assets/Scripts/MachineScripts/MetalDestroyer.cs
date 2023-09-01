using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;

public class MetalDestroyer : MonoBehaviour
{
    public GameObject stock;
    public GameObject mainStock;
    public GameObject offMetalStock;
    public GameObject mainOffMetalStock;
    public float speed = 0;
    private int jump = 1;

    public Transform[] targetStockPosition;

    public Transform[] targetPositions;

    public Transform targetJump;
    private int currentTargetIndex = 0;
    private Tween moveTween;
    public PlayerController playerController;

    private void Start()
    {
        mainStock = stock;
        mainOffMetalStock = offMetalStock;
    }
    private void FixedUpdate()
    {
        if (offMetalStock != null)
        {
            if (offMetalStock.activeSelf)
                SetoffMetalForwardMovement();

        }
        if (mainStock.activeSelf)
        {
            SetStockMovement();
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("ChangerStock"))
        {
            
            
            stock = Instantiate(mainStock, mainStock.transform.position, mainStock.transform.rotation);
            stock.transform.SetParent(mainStock.transform.parent);
            stock.SetActive(true);
            Invoke("ActivateMetalInStock", 4f);
        }
    }
    private void ActivateMetalInStock()
    {
       playerController.metalInStock.SetActive(true);
    }

    private void SetStockMovement()
    {
        if (currentTargetIndex >= targetPositions.Length)
        {
            moveTween.Kill();
            return;
        }
        Transform targetPosition = targetPositions[currentTargetIndex];

        moveTween = transform.DOMove(targetPosition.position, 1).SetEase(Ease.Linear).OnComplete(() =>
        {
            transform.DOJump(targetJump.position, 0.2f, jump, 1).SetEase(Ease.Linear);
            GetComponent<Collider>().enabled = true;
            currentTargetIndex++;
            //SetStockMovement();
        });
    }

    private void SetoffMetalForwardMovement()
    {
        Transform targetPosition = targetStockPosition[currentTargetIndex];

        moveTween = transform.DOMove(targetPosition.position, speed).SetEase(Ease.Linear);

        moveTween.OnComplete(() =>
        {

        });
    }

    public void OpenOffMetal()
    {
        offMetalStock = Instantiate(mainOffMetalStock, mainOffMetalStock.transform.position, mainOffMetalStock.transform.rotation);
        offMetalStock.transform.SetParent(mainOffMetalStock.transform.parent);
        offMetalStock.SetActive(true);
    }
}

