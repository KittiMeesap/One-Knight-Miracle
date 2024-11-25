using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Potion : MonoBehaviour
{
    public enum PotionType { Speed, Strength, Invincibility }
    public PotionType potionType;

    public float effectDuration = 5f;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerStats playerStats = other.GetComponent<PlayerStats>();
            PlayerController playerController = other.GetComponent<PlayerController>();

            if (playerStats != null && playerController != null)
            {
                switch (potionType)
                {
                    case PotionType.Speed:
                        playerController.ActivateSpeedBoost(effectDuration);
                        break;
                    case PotionType.Strength:
                        playerStats.ActivateDamageBoost(effectDuration);
                        break;
                    case PotionType.Invincibility:
                        playerStats.ActivateImmortality(effectDuration);
                        break;
                }

                Destroy(gameObject);
            }
        }
    }
}
