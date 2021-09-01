using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 100;
    public int currentHealth;

    public float invincibilityFlashDelay = 0.2f;
    public bool isInvincible = false;
    public float invincibilityTimeAfterHit = 3f;
    public SpriteRenderer graphics;

    public HealthBar healthBar;


    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.H))
        {
            TakeDamage(20);
        }
    }

    public void TakeDamage(int damage)
    {
        if (!isInvincible)
        {
            currentHealth -= damage;
            healthBar.SetHealth(currentHealth);

            //vérifier que le joueur est vivant
            if (currentHealth <= 0)
            {
                Die();
                return;
            }

            isInvincible = true;
            StartCoroutine(InvicibilityFlash());
            StartCoroutine(HandleInvicibilityDelay());
        }
        
    }

    public void Die()
    {
        Debug.Log("Le joueur est éliminé");
        //bloquer mouvement du perso
        MovePlayer.instance.enabled = false;
        //jouer l'animation d'élimination

        //empecher les interraction physics avec 'autre elemetn de la scene
        MovePlayer.instance.rb.bodyType = RigidbodyType2D.Kinematic;
        //if faut désactiver les deux box xollider ici en plus de mettre en kinematic

        GameOverManager.instance.OnPlayerDeath();
    }


    public IEnumerator InvicibilityFlash()
    {
        while (isInvincible)
        {
            graphics.color = new Color(1f, 1f, 1f, 0f);
            yield return new WaitForSeconds(invincibilityFlashDelay);
            graphics.color = new Color(1f, 1f, 1f, 1f);
            yield return new WaitForSeconds(invincibilityFlashDelay);
        }

    }
    public IEnumerator HandleInvicibilityDelay()
    {
        yield return new WaitForSeconds(invincibilityTimeAfterHit);
        isInvincible = false;
    }
}
