using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [Header("Sources")]
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioSource sfxSource;

    [Header("Music")]
    [SerializeField] private AudioClip music;
    [Range(0f, 1f)]
    [SerializeField] private float musicVolume = 0.35f;

    [Header("Effects")]
    [SerializeField] private AudioClip jump;
    [SerializeField] private AudioClip land;
    [SerializeField] private AudioClip collect;
    [SerializeField] private AudioClip hurt;
    [SerializeField] private AudioClip win;
    [SerializeField] private AudioClip gameOver;
    [SerializeField] private AudioClip spell;
    [SerializeField] private AudioClip attack;
    [SerializeField] private AudioClip brewGood;
    [SerializeField] private AudioClip brewBad;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        if (music != null)
        {
            musicSource.clip = music;
            musicSource.loop = true;
            musicSource.volume = musicVolume;
            musicSource.Play();
        }
    }

    void Play(AudioClip clip)
    {
        if (clip != null) sfxSource.PlayOneShot(clip);
    }

    public void PlayJump() { Play(jump); }
    public void PlayLand() { Play(land); }
    public void PlayCollect() { Play(collect); }
    public void PlayHurt() { Play(hurt); }
    public void PlayWin() { Play(win); }
    public void PlayGameOver() { Play(gameOver); }
    public void PlaySpell() { Play(spell); }
    public void PlayAttack() { Play(attack); }
    public void PlayBrew(bool good) { Play(good ? brewGood : brewBad); }
}