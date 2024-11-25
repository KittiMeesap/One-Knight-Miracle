using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmorItem : MonoBehaviour
{
    public enum ArmorType { Light, Heavy } 
    public ArmorType armorType;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerStats playerArmor = other.GetComponent<PlayerStats>();

            if (playerArmor != null)
            {
                if (armorType == ArmorType.Light)
                {
                    playerArmor.AddArmor("Light"); 
                }
                else if (armorType == ArmorType.Heavy)
                {
                    playerArmor.AddArmor("Heavy");
                }

                Destroy(gameObject);
            }
        }
    }
}
