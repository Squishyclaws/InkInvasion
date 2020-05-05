using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillCo : MonoBehaviour
{
    public void triggerDeath(float killDuration)
    {
        StartCoroutine(DestroySelf(killDuration));
    }

    IEnumerator DestroySelf(float killDuration)
    {
        yield return new WaitForSeconds(killDuration);
        Destroy(gameObject);
    }
}
