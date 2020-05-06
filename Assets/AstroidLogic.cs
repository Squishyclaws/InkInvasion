using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AstroidLogic : MonoBehaviour
{
    [SerializeField] private GameObject healthPrefab;
    [SerializeField] private GameObject deathSFX;
    [SerializeField] private GameObject explostionFX;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("playerBullet"))
        {
            Debug.Log("Astroid Hit");
            Destroy(other.gameObject);
            Instantiate(healthPrefab, transform.position, Quaternion.identity);
            Instantiate(explostionFX, transform.position, Quaternion.identity);
            GameObject sound = Instantiate(deathSFX);
            sound.GetComponent<KillCo>().triggerDeath(.2f);
            Destroy(gameObject);
        }
    }
}
