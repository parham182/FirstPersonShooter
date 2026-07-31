using UnityEngine;

public class FootStepSound : MonoBehaviour
{
    [SerializeField] AudioSource footStepAudioSource;
    [SerializeField] AudioClip[] walkClips;

    [SerializeField] float walkVolume;
    [SerializeField] float runVolume;

    // call in animation action frame
    public void PlayWalkSound() {
        footStepAudioSource.volume = SettingsManager.instance.soundFxVolume * walkVolume;
        footStepAudioSource.pitch = Random.Range(0.95f, 1.05f);
        footStepAudioSource.PlayOneShot(walkClips[Random.Range(0, walkClips.Length)]);
    }

    // call in animation action frame
    public void PlayRunSound() {
        footStepAudioSource.volume = SettingsManager.instance.soundFxVolume * runVolume;
        footStepAudioSource.pitch = Random.Range(0.95f, 1.05f);
        footStepAudioSource.PlayOneShot(walkClips[Random.Range(0, walkClips.Length)]);
    }
}
