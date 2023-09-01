using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PlasticDestroyerMag : MonoBehaviour
{
    public GameObject mag;
    public GameObject mainMag;
    public GameObject offPlasticMag;
    public GameObject mainOffPlasticMag;
    public GameObject plasticInMag;
    public float speed = 0;
    private int jump = 1;
    
    public Transform[] targetMagPosition;
    public Transform[] targetPositions;
    public Transform targetJump;
    private int currentTargetIndex = 0;
    private Tween moveTween;
    public PlayerController playerController;

    private void Start()
    {
        mainMag = mag;
        mainOffPlasticMag = offPlasticMag;
    }
    private void FixedUpdate()
    {
        if (offPlasticMag != null)
        {
            if (offPlasticMag.activeSelf)
                SetOffPlasticMagForwardMovement();

        }

        if (mainMag.activeSelf)
        {
            SetMagMovement();
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("ChangerMag"))
        {
            mag = Instantiate(mainMag, mainMag.transform.position, mainMag.transform.rotation);
            mag.transform.SetParent(mainMag.transform.parent);
            mag.SetActive(true);
            Invoke("ActivatePlasticInMag", 4f);
        }
    }
    private void ActivatePlasticInMag()
    {
        playerController.plasticInMag.SetActive(true);
    }

    private void SetMagMovement()
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
            //SetMagMovement();
        });
    }


    private void SetOffPlasticMagForwardMovement()
    {
        transform.DOMoveZ(3, 3f).OnComplete(() =>
        {
            transform.DOMoveY(-3f, 2f);
        });

    }

    public void OpenOffPlasticMag()
    {
        offPlasticMag = Instantiate(mainOffPlasticMag, mainOffPlasticMag.transform.position, mainOffPlasticMag.transform.rotation);
        offPlasticMag.transform.SetParent(mainOffPlasticMag.transform.parent);
        offPlasticMag.SetActive(true);
    }

}
