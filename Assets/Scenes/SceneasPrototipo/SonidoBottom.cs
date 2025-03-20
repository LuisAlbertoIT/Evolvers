using UnityEngine;

public class SonidoBottom : MonoBehaviour
{
    private AudioSource sonidoBotom;
    public AudioClip AudioClip;
    public AudioClip switchAudio;
    public AudioClip attackAudio;
    void Start()
    {
        sonidoBotom = GetComponent<AudioSource>();
    }

    // Update is called once per frame
  public void ClickAudioOn()
    {
        sonidoBotom.pitch = 7f;
        sonidoBotom.PlayOneShot(AudioClip);
    }

    public void  SwitchAudioOn()
    {
        sonidoBotom.pitch = 2.0f;
        sonidoBotom?.PlayOneShot(switchAudio);

    }
    public void AttackAudio()
    {
        sonidoBotom.PlayOneShot(attackAudio);
    }
}
