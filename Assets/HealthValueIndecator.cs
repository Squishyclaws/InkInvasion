using System.Collections;
using System.Collections.Generic;
using System.Net.Mime;
using UnityEngine;
using UnityEngine.UI;

public class HealthValueIndecator : MonoBehaviour
{
    [SerializeField]
    private Text textValue;

    public void setText(int healValue)
    {
        textValue.text = "+" + healValue;
        DestrySelf();
    }

    void DestrySelf()
    {
        GetComponent<KillCo>().triggerDeath(1f);
    }
}
