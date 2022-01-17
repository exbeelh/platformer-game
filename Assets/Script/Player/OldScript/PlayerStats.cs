using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerStats : MonoBehaviour
{
    [SerializeField]
    private float maxHealth;

    [SerializeField]
    private GameObject deathChunkParticle;

    private float currentHealth;

    private GameMaster GM;

    private void Start()
    {
        currentHealth = maxHealth;
        GM = GameObject.FindGameObjectWithTag("GM").GetComponent<GameMaster>();
        transform.position = GM.lastCheckPointPos;
    }

    public void DecreaseHealth(float amount)
    {
        currentHealth -= amount;

        if(currentHealth <= 0.0f)
        {
            Death();
        }
    }

    private void Death()
    {
        Instantiate(deathChunkParticle, transform.position, deathChunkParticle.transform.rotation);
        StartCoroutine(Respawn());
    }

    IEnumerator Respawn()
    {
        yield return new WaitForSeconds(0.075f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
