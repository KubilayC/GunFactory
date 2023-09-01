using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TableDestroyer : MonoBehaviour
{
    public GameObject table;
    public GameObject table2;
    public GameObject table3;
    public GameObject table4;

    public PlayerController playerController;

    private void FixedUpdate()
    {
        TableSpawn();
    }
    private void TableSpawn()
    {
        if (playerController.bagStock.activeSelf)
        {
            table.SetActive(true);
        }
        else
        {
            table.SetActive(false);
        }
        if (playerController.bagScope.activeSelf)
        {
            table2.SetActive(true);
        }
        else
        {
            table2.SetActive(false);
        }

        if (playerController.bagMag.activeSelf)
        {
            table3.SetActive(true);
        }
        else
        {
            table3.SetActive(false);
        }
        if (playerController.bagBody.activeSelf)
        {
            table4.SetActive(true);
        }
        else
        {
            table4.SetActive(false);
        }
    }

}
