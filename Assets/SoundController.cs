using UnityEngine;

public class SoundController : MonoBehaviour
{
    public enum soundeffect
    {
        VASE_BREAK, // done
        PAINTINGFALL, // done
        LAMPFALL, // done
        FIRESTART, // done
        FIREALARM,
        SPRINKLERS, // done
        TAPESTRYMOVE, // done
        THUD, // done
        SWEEP, // done
        FIREEXTINGUISH, // done
        PICKUP
    }

    [SerializeField]
    private AudioClip[] allCips;

    [SerializeField] private AudioSource soundeffectsource;
    [SerializeField] private AudioSource musicsource;
    [SerializeField] private AudioSource footstepSource;

    public static SoundController current = null;

    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    void Awake()
    {
        if (current == null)
        {
            current = this;
        }
        else
        {
            Debug.LogError("Doofus you moofus you have two sound systems!.");
        }
    }

    public void PlayMusic()
    {
        musicsource.Play();
    }

    public void StopMusic()
    {
        musicsource.Stop();
    }

    public void PlaySound(int so)
    {
        soundeffectsource.PlayOneShot(allCips[so]);
    }

    public void StopAll()
    {
        soundeffectsource.Stop();
    }

    public void PlayWalkSound()
    {
        footstepSource.Play();
    }

    public void StopWalkSound()
    {
        footstepSource.Stop();
    }
}
