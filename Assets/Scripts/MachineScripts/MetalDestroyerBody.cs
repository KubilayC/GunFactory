using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MetalDestroyerBody : MonoBehaviour
{
    public GameObject body;
    public GameObject mainBody;
    public GameObject offMetalBody;
    public GameObject mainOffMetalBody;
    public GameObject metalInBody;

    public float speed = 0;
    private int jump = 1;

    public Transform[] targetBodyPosition;
    public Transform[] targetPositions;
    public Transform targetJump;
    private int currentTargetIndex = 0;
    private Tween moveTween;
    public PlayerController playerController;

    private void Start()
    {
        mainBody = body;
        mainOffMetalBody = offMetalBody;
    }
    private void FixedUpdate()
    {
        if (offMetalBody != null)
        {
            if (offMetalBody.activeSelf)
                SetoffMetalBodyForwardMovement();

        }
        if (mainBody.activeSelf)
        {
            SetBodyMovement();
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("ChangerBody"))
        {
            body = Instantiate(mainBody, mainBody.transform.position, mainBody.transform.rotation);
            body.transform.SetParent(mainBody.transform.parent);
            body.SetActive(true);
            Invoke("ActivateMetalInBody", 4f);
            
        }
    }
    private void ActivateMetalInBody()
    {
        playerController.metalInBody.SetActive(true);
    }

    private void SetBodyMovement()
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

    private void SetoffMetalBodyForwardMovement()
    {
        transform.DOMoveZ(3, 2f).OnComplete(() =>
        {
            transform.DOMoveY(-1f, 1f);
        });

    }


    public void OpenOffMetalBody()
    {
        offMetalBody = Instantiate(mainOffMetalBody, mainOffMetalBody.transform.position, mainOffMetalBody.transform.rotation);
        offMetalBody.transform.SetParent(mainOffMetalBody.transform.parent);
        offMetalBody.SetActive(true);
    }
}
