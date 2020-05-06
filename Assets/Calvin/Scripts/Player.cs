﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using UnityEngine;
using UnityEngine.UIElements;

public class Player : MonoBehaviour
{
    //config params
    [Header("Player Stats")] [SerializeField]
    float moveSpeed = 10f;

    [SerializeField] float padding = 1f;
    [SerializeField] public int health = 1000;
    [SerializeField] GameObject deathVFX;
    [SerializeField] float DurationOfexplosion = 1f;
    [SerializeField] Color red, white;
    [SerializeField] GameObject tutPanel;
    Life life;
    [SerializeField] private GameObject healthValue;

    //Sprite changes
    [Header("Sprite Changes")] [SerializeField]
    Sprite FlyLeft;

    [SerializeField] Sprite FlyStatic;
    [SerializeField] Sprite FlyUp;
    [SerializeField] Sprite FlyDown;
    [SerializeField] Sprite FlyRight;

    //[Header("Sound Imput")] [SerializeField]
    //AudioClip deathSound;

    //[SerializeField] float deathSoundVolume = 0.7f;
    //[SerializeField] AudioClip singleFireSound;
    //[SerializeField] float singleFireSoundVolume;
    //[SerializeField] AudioClip flySound;
    //[SerializeField] float flySoundVolume;
    //[SerializeField] AudioClip hitSound;
    //[SerializeField] float hitSoundVolume;

    [SerializeField] private GameObject deathSound, impactSound;
    private AudioSource playerSource;

    [SerializeField] private AudioClip playerMoveSound;


    [Header("Projectile")] [SerializeField]
    float projectileSpeed = 10f;

    [SerializeField] float ProjectileFiringPeriod = 0.1f;
    [SerializeField] GameObject laserPrefab;

    Coroutine firingCoroutine;

    [SerializeField] float xMin;
    [SerializeField] float xMax;
    [SerializeField] float yMin;
    [SerializeField] float yMax;


    // Start is called before the first frame update
    void Start()
    {
        life = FindObjectOfType<Life>();
        playerSource = GetComponent<AudioSource>();
        playerSource.clip = playerMoveSound;
        // SetUpMoveBoundaries();
    }

    private void Tut()
    {
        tutPanel.SetActive(false);
    }

    private void Fire()

    {
        if (Input.GetButtonDown("Fire1"))
        {
            Tut();
            firingCoroutine = StartCoroutine(FireContinuously());
        }

        if (Input.GetButtonUp("Fire1"))
        {
            StopCoroutine(firingCoroutine);
        }
    }

    IEnumerator FireContinuously()

    {
        while (true)

        {
            var offsetlaser = new Vector3(0, 4f, 0);
            GameObject laser = Instantiate(
                laserPrefab,
                transform.position + offsetlaser,
                Quaternion.identity) as GameObject;

            //AudioSource.PlayClipAtPoint(singleFireSound, Camera.main.transform.position, singleFireSoundVolume);

            laser.GetComponent<Rigidbody2D>().velocity = new Vector2(0, projectileSpeed);


            yield return new WaitForSeconds(ProjectileFiringPeriod);
        }
    }


    private void Move()
    {
        var deltaY = Input.GetAxis("Vertical") * Time.deltaTime * moveSpeed;
        var deltaX = Input.GetAxis("Horizontal") * Time.deltaTime * moveSpeed;

        var newXPos = Mathf.Clamp(transform.position.x + deltaX, xMin, xMax);
        var newYPos = Mathf.Clamp(transform.position.y + deltaY, yMin, yMax);

        transform.position = new Vector2(newXPos, newYPos);
    }

    /*private void SetUpMoveBoundaries()
    {
        Camera gameCamera = Camera.main;
        xMin = gameCamera.ViewportToWorldPoint(new Vector3(0, 0, 0)).x + padding;
        xMax = gameCamera.ViewportToWorldPoint(new Vector3(1, 0, 0)).x - padding;
        yMin = gameCamera.ViewportToWorldPoint(new Vector3(0, 0, 0)).y + padding;
        yMax = gameCamera.ViewportToWorldPoint(new Vector3(0, 1, 0)).y - padding;

    }*/

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A))
        {
            //AudioSource.PlayClipAtPoint(flySound, Camera.main.transform.position, flySoundVolume);
            playerSource.Play();
            this.gameObject.GetComponent<SpriteRenderer>().sprite = FlyLeft;
        }

        else if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W))
        {
            //AudioSource.PlayClipAtPoint(flySound, Camera.main.transform.position, flySoundVolume);
            playerSource.Play();
            this.gameObject.GetComponent<SpriteRenderer>().sprite = FlyUp;
        }

        else if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S))
        {
            //AudioSource.PlayClipAtPoint(flySound, Camera.main.transform.position, flySoundVolume);
            playerSource.Play();
            this.gameObject.GetComponent<SpriteRenderer>().sprite = FlyDown;
        }

        else if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D))
        {
            //AudioSource.PlayClipAtPoint(flySound, Camera.main.transform.position, flySoundVolume);
            playerSource.Play();
            this.gameObject.GetComponent<SpriteRenderer>().sprite = FlyRight;
        }
        else if (Input.anyKey == false)
        {
            this.gameObject.GetComponent<SpriteRenderer>().sprite = FlyStatic;
        }

        Move();
        Fire();
    }

    IEnumerator ChangeColor()
    {
        this.gameObject.GetComponent<SpriteRenderer>().color = Color.red;
        yield return new WaitForSeconds(0.02f);
        this.gameObject.GetComponent<SpriteRenderer>().color = Color.white;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        //AudioSource.PlayClipAtPoint(hitSound, Camera.main.transform.position, hitSoundVolume);
        Debug.Log(other.name + " has hit you");

        if (other.gameObject.CompareTag("healthPack"))
        {
            int healAmount = other.gameObject.GetComponent<HealthPack>().healValue;
            health += healAmount;
            GameObject healIndecator = Instantiate(healthValue, transform.position, Quaternion.identity);
            healIndecator.GetComponent<HealthValueIndecator>().setText(healAmount);
            HealthUpdateAdd();
            Destroy(other.gameObject);
        }
        
        GameObject sound = Instantiate(impactSound, transform.position, Quaternion.identity);
        sound.GetComponent<KillCo>().triggerDeath(.2f);

        Debug.Log(other.name + " has hit");
        DamageDealer damageDealer = other.gameObject.GetComponent<DamageDealer>();
        if (!damageDealer)
        {
            return;
        }

        ProcessHit(damageDealer);
        StartCoroutine(ChangeColor());
    }

    private void ProcessHit(DamageDealer damageDealer)
    {
        health -= damageDealer.GetDamage();
        damageDealer.Hit();
        HealthUpdate();
        if (health <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        FindObjectOfType<Level>().LoadGameOver();
        GameObject explosion = Instantiate(deathVFX, transform.position, transform.rotation);

        GameObject sound = Instantiate(deathSound, transform.position, Quaternion.identity);
        sound.GetComponent<KillCo>().triggerDeath(1f);

        Destroy(explosion, DurationOfexplosion);
        Destroy(gameObject);
        
        //AudioSource.PlayClipAtPoint(deathSound, Camera.main.transform.position, deathSoundVolume);
    }

    private void HealthUpdate()
    {
        if (health <= 800)
        {
            life.MinusLife5(0);
        }

        if (health <= 600)
        {
            life.MinusLife5(1);
        }

        if (health <= 400)
        {
            life.MinusLife5(2);
        }

        if (health <= 200)
        {
            life.MinusLife5(3);
        }

        if (health <= 0)
        {
            life.MinusLife5(4);
        }
    }
    
    private void HealthUpdateAdd()
    {
        if (health >= 800)
        {
            life.AddLife(0);
        }

        if (health >= 600)
        {
            life.AddLife(1);
        }

        if (health >= 400)
        {
            life.AddLife(2);
        }

        if (health >= 200)
        {
            life.AddLife(3);
        }

        if (health >= 0)
        {
            life.AddLife(4);
        }
    }
}