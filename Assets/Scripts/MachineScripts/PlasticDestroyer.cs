using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlasticDestroyer : MonoBehaviour
{
    public GameObject scope;
    public GameObject mainScope;
    public GameObject offPlasticScope;
    public GameObject mainOffPlasticScope;
    public GameObject plasticInScope;
    public float speed = 0;
    private int jump = 1;

    public Transform[] targetPositions;
    public Transform targetJump;
    private int currentTargetIndex = 0;
    private Tween moveTween;
    public PlayerController playerController;

    private void Start()
    {
        mainScope = scope;
        mainOffPlasticScope = offPlasticScope;
        
    }
    private void FixedUpdate()
    {
        if (offPlasticScope != null)
        {
            if (offPlasticScope.activeSelf)
                SetOffPlasticForwardMovement();

        }

        if (mainScope.activeSelf)
        {
            SetScopeMovement();
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("ChangerScope"))
        {
            scope = Instantiate(mainScope, mainScope.transform.position, mainScope.transform.rotation);
            scope.transform.SetParent(mainScope.transform.parent);
            scope.SetActive(true);
            Invoke("ActivatePlasticInScope", 4f);
        }
    }

    private void ActivatePlasticInScope()
    {
        playerController.plasticInScope.SetActive(true);
    }

    private void SetScopeMovement()
    {

        if (currentTargetIndex >= targetPositions.Length)
        {
            moveTween.Kill();
            return;
        }
        Transform targetPosition = targetPositions[currentTargetIndex];

        moveTween = transform.DOMove(targetPosition.position, 2).SetEase(Ease.Linear).OnComplete(() =>
        { 
            transform.DOJump(targetJump.position, 0.1f, jump, 1).SetEase(Ease.Linear);
            GetComponent<Collider>().enabled = true;
            currentTargetIndex++;
            //SetScopeMovement();
        });
    }


    private void SetOffPlasticForwardMovement()
    {

        transform.DOMoveZ(6, 3f).OnComplete(() =>
        {
            transform.DOMoveY(-1f, 1f);
        });

    }

    public void OpenOffPlastic()
    {
        offPlasticScope = Instantiate(mainOffPlasticScope, mainOffPlasticScope.transform.position, mainOffPlasticScope.transform.rotation);
        offPlasticScope.transform.SetParent(mainOffPlasticScope.transform.parent);
        offPlasticScope.SetActive(true);
    }

}
