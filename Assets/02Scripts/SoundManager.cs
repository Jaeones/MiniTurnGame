using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class SoundManager : MonoBehaviour
{
   public static SoundManager instance;

    AudioSource myAudio;
    public AudioClip[] attackSounds; 

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        myAudio = GetComponent<AudioSource>();
    }

    public void PlayAttackSound(int num)
    {
        myAudio.PlayOneShot(attackSounds[num]);
    }
}
