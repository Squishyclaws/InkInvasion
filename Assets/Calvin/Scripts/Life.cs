using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Life : MonoBehaviour
{
    [SerializeField] AudioClip loseLife;
    [SerializeField] float loseLifeVolume;
    [SerializeField] public GameObject[] hearts;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    public void MinusLife5(int heartnumber)
    {
        hearts[heartnumber].SetActive(false);
    }
    
    public void AddLife(int heartnumber)
    {
        hearts[heartnumber].SetActive(true);
    }




}
