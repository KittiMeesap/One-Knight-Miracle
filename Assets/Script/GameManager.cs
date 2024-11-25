using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public GameObject healthBar;     
    public GameObject healthPrefab;    

    public GameObject armorBar;        
    public GameObject armorPrefab;     

    private PlayerStats playerHealth; 

    private List<GameObject> healthObjects = new List<GameObject>();
    private List<GameObject> armorObjects = new List<GameObject>();

    void Start()
    {
        playerHealth = FindObjectOfType<PlayerStats>();

        UpdateHealthUI(playerHealth.currentHealth);
        UpdateArmorUI(playerHealth.currentArmorSlots);
    }
    public void UpdateHealthUI(int currentHealth)
    {
        foreach (GameObject health in healthObjects)
        {
            Destroy(health);
        }
        healthObjects.Clear(); 

        for (int i = 0; i < currentHealth; i++)
        {
            if (healthPrefab != null)
            {
                GameObject healthIcon = Instantiate(healthPrefab, healthBar.transform);
                healthObjects.Add(healthIcon); 
            }
        }
    }

    public void UpdateArmorUI(int currentArmorSlots)
    {
        foreach (GameObject armor in armorObjects)
        {
            Destroy(armor);
        }
        armorObjects.Clear(); 

        for (int i = 0; i < currentArmorSlots; i++)
        {
            if (armorPrefab != null)
            {
                GameObject armorIcon = Instantiate(armorPrefab, armorBar.transform);
                armorObjects.Add(armorIcon); 
            }
        }
    }
}