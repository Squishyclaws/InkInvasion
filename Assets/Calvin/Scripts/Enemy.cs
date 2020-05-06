using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    // Start is called before the first frame update
    [Header("Enemy Stats")] [SerializeField]
    public float health = 100;

    [SerializeField] int scoreValue = 150;
    [SerializeField] private bool isBoss;
    private BossHP bossHP;
    [SerializeField] private Color damageColour;


    [Header("Shooting")] [SerializeField] float shotCounter;
    [SerializeField] float minTimeBetweenShots = 0.2f;
    [SerializeField] float maxTimeBetweenShots = 3f;
    [SerializeField] GameObject projectile;
    [SerializeField] float projectileSpeed = 10f;


    [Header("Sound effects")] [SerializeField]
    GameObject deathVFX;

    [SerializeField] private GameObject deathSound, impactSound;
    

    [SerializeField] float DurationOfexplosion = 1f;
    //[SerializeField] AudioClip deathSound;
    //[SerializeField] float deathSoundVolume = 0.7f;
    //[SerializeField] AudioClip hitSound;
    //[SerializeField] float hitSoundVolume;
    //[SerializeField] AudioClip fireSound;
    //[SerializeField] float fireSoundVolume;

    void Start()
    {
        if (isBoss)
        {
            bossHP = GetComponent<BossHP>();
        }
        shotCounter = Random.Range(minTimeBetweenShots, maxTimeBetweenShots);
        //playerSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        CountDownAndShoot();
    }

    private void CountDownAndShoot()
    {
        shotCounter -= Time.deltaTime;

        if (shotCounter <= 0f)
        {
            Fire();
            shotCounter = Random.Range(minTimeBetweenShots, maxTimeBetweenShots);
        }
    }


    private void Fire()
    {
        GameObject laser = Instantiate(
                projectile,
                transform.position,
                Quaternion.identity)
            as GameObject;
        //AudioSource.PlayClipAtPoint(fireSound, Camera.main.transform.position, fireSoundVolume);
        laser.GetComponent<Rigidbody2D>().velocity = new Vector2(0, -projectileSpeed);
    }

    IEnumerator ChangeColor()
    {
        this.gameObject.GetComponent<SpriteRenderer>().color = damageColour;
        yield return new WaitForSeconds(0.03f);
        this.gameObject.GetComponent<SpriteRenderer>().color = Color.white;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        //AudioSource.PlayClipAtPoint(hitSound, Camera.main.transform.position, hitSoundVolume);
        //playerSource.clip = hitSound;
        //playerSource.Play();
        
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
        if (isBoss)
        {
            bossHP.updateHealth(health);
        }
        if (health <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        FindObjectOfType<GameSession>().AddToScore(scoreValue);
        GameObject explosion = Instantiate(deathVFX, transform.position, Quaternion.identity);
        Destroy(explosion, DurationOfexplosion);
        
        GameObject sound = Instantiate(deathSound, transform.position, Quaternion.identity);
        sound.GetComponent<KillCo>().triggerDeath(1f);
        
        //AudioSource.PlayClipAtPoint(deathSound, Camera.main.transform.position, deathSoundVolume);
        Destroy(gameObject);
    }
}