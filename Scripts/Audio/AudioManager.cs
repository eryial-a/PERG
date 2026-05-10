using UnityEngine;

public class AudioManager : MonoBehaviour
{
    // Audio source divisions for organization
    [Header("Audio Sources")]
    [SerializeField] AudioSource musicSource;
    [SerializeField] AudioSource SFXSource;

    // All the different audio clips necessary for the project
    [Header("Audio Clips")]
    public AudioClip MainMenuMusic;
    public AudioClip buttonClick;
    public AudioClip cardDraw;
    public AudioClip cardSelect;
    public AudioClip cardUnselect;
    public AudioClip enemyAttack;
    public AudioClip enemyDeath;
    // boss and common use same hit noise
    public AudioClip enemyHit;
    public AudioClip bossAttack;
    public AudioClip bossDeath;

    //play background music
    private void Start()
    {
        musicSource.clip = MainMenuMusic;
        musicSource.Play();
    }

    // To play an audioclip not listed above
    public void PlaySFX(AudioClip clip)
    {
        SFXSource.PlayOneShot(clip);
    }


}
