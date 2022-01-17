using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static AudioClip dash, land, death, attack;
    static AudioSource audioSrc;
    // Start is called before the first frame update
    void Start()
    {
        dash = Resources.Load<AudioClip>("dash");
        land = Resources.Load<AudioClip>("land");
        death = Resources.Load<AudioClip>("death");
        attack = Resources.Load<AudioClip>("attack");

        audioSrc = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public static void PlaySound(string clip)
    {
        switch (clip)
        {
            case "dash":
                audioSrc.PlayOneShot(dash);
                break;
            case "land":
                audioSrc.PlayOneShot(land);
                break;
            case "death":
                audioSrc.PlayOneShot(death);
                break;
            case "attack":
                audioSrc.PlayOneShot(attack);
                break;
        }
    }
}
