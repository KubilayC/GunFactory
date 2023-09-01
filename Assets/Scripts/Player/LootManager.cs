using System.Collections;
using System.Collections.Generic;
using UnityEditor.EditorTools;
using UnityEngine;
using UnityEngine.UI;

public class LootManager : MonoBehaviour
{
    public static LootManager instance;
    public Text goldText;
    public Text metalText;
    public Text plasticText;
    public Button restartButton;






    public int gold = 0;
    public int metal = 0;
    public int plastic = 0;

    private void Awake()
    {
        instance = this;
    }
    void Start()
    {
        
        metalText.text = " " + metal.ToString();

        restartButton.gameObject.SetActive(false);

    }
    public void AddMetal()
    {
        metal += 1;       
        metalText.text = " " + metal.ToString();
        
    }
    
    public void DecreaseMetal()
    {
        metal-= 1;
        metalText.text =" "+ metal.ToString();
    }

    public void AddPlastic()
    {
        plastic += 1;
        plasticText.text = " " + plastic.ToString();
    }

    public void DecreasePlastic()
    {
        plastic -= 1;
        plasticText.text = " " + plastic.ToString();
    }

    public void AddGold()
    {

        goldText.text = " " + gold.ToString();

    }
    public void ShowRestartButton()
    {
        restartButton.gameObject.SetActive(true);
    }
}