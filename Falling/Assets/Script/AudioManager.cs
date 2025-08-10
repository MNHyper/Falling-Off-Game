
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [Header("---------- Audio Source ----------")]
    [SerializeField] AudioSource musicSource;
    [SerializeField] AudioSource SFXSouecr;


    [Header("---------- Audio Clip ----------")]
    public AudioClip button;
    public AudioClip day_night;
    public AudioClip death;
    public AudioClip background;
    public AudioClip jump;
    public AudioClip mainmenu;
    public AudioClip warning;
    public AudioClip doublejump;

    [SerializeField] GameObject MuteMusicImage;
    [SerializeField] GameObject MuteSFXImage;


    private void Start()
    {

        musicSource.clip = background;
        musicSource.Play();


        SFXSouecr.volume = PlayerPrefs.GetFloat("SFX", 1);
        musicSource.volume = PlayerPrefs.GetFloat("Music", 1);
        if (SFXSouecr.volume == 0) MuteSFXImage.SetActive(true);
        if (musicSource.volume == 0) MuteMusicImage.SetActive(true);
    }

    public void PlaySFX(AudioClip clip)
    {

        SFXSouecr.PlayOneShot(clip);

    }
    public void StopMusic()
    {
        musicSource.Stop();
    }    
    public void MuteMusicButton()
    {
        if(musicSource.volume == 1)
        {
            musicSource.volume = 0;
            MuteMusicImage.SetActive(true);

           PlayerPrefs.SetFloat("Music", 0);
        }
        else
        {
            musicSource.volume = 1;
            MuteMusicImage.SetActive(false);

            PlayerPrefs.SetFloat("Music", 1);
        }
    }
    public void MuteSFXButton()
    {
        if(SFXSouecr.volume == 1)
        {
            SFXSouecr.volume = 0;
            MuteSFXImage.SetActive(true);

            PlayerPrefs.SetFloat("SFX", 0);
        }
        else
        {
            SFXSouecr.volume = 1;
            MuteSFXImage.SetActive(false);

            PlayerPrefs.SetFloat("SFX", 1);
        }
    }

}
