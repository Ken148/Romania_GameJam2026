using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [Header("Sources")]
    [SerializeField] private AudioSource sfxSource;
    [SerializeField] private AudioSource musicSource;

    [Header("Floor 1")]
    [SerializeField] private AudioClip keyPickup;
    [SerializeField] private AudioClip keyDrop;
    [SerializeField] private AudioClip screwdriverPickup;
    [SerializeField] private AudioClip screwdriverDrop;
    [SerializeField] private AudioClip boxPush;
    [SerializeField] private AudioClip waterWalk;
    [SerializeField] private AudioClip objectSplash;
    [SerializeField] private AudioClip tenseMusic;
    [SerializeField] private AudioClip emergencyLightHum;
    [SerializeField] private AudioClip slidingDoorOpen;
    [SerializeField] private AudioClip unscrew;
    [SerializeField] private AudioClip paperPickup;
    [SerializeField] private AudioClip panelFall;
    [SerializeField] private AudioClip sparks;
    [SerializeField] private AudioClip floor2DoorOpen;

    [Header("Floor 2")]
    [SerializeField] private AudioClip woodHit;
    [SerializeField] private AudioClip woodBreak;
    [SerializeField] private AudioClip metalHit;
    [SerializeField] private AudioClip metalPlateFall;
    [SerializeField] private AudioClip electricityOff;
    [SerializeField] private AudioClip fuseRemove;
    [SerializeField] private AudioClip fuseInsert;
    [SerializeField] private AudioClip hammerSwing;

    [Header("Floor 3")]
    [SerializeField] private AudioClip boatSplash;
    [SerializeField] private AudioClip impact;
    [SerializeField] private AudioClip grunt;
    [SerializeField] private AudioClip glassBreak;
    [SerializeField] private AudioClip click;
    [SerializeField] private AudioClip endingMusic;
    [SerializeField] private AudioClip sea;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    private void Play(AudioClip clip)
    {
        if (clip != null)
            sfxSource.PlayOneShot(clip);
    }

    public void PlayKeyPickup() => Play(keyPickup);
    public void PlayKeyDrop() => Play(keyDrop);
    public void PlayScrewdriverPickup() => Play(screwdriverPickup);
    public void PlayScrewdriverDrop() => Play(screwdriverDrop);
    public void PlayBoxPush() => Play(boxPush);
    public void PlayWaterWalk() => Play(waterWalk);
    public void PlayObjectSplash() => Play(objectSplash);
    public void PlaySlidingDoorOpen() => Play(slidingDoorOpen);
    public void PlayUnscrew() => Play(unscrew);
    public void PlayPaperPickup() => Play(paperPickup);
    public void PlayPanelFall() => Play(panelFall);
    public void PlaySparks() => Play(sparks);
    public void PlayFloor2DoorOpen() => Play(floor2DoorOpen);

    public void PlayWoodHit() => Play(woodHit);
    public void PlayWoodBreak() => Play(woodBreak);
    public void PlayMetalHit() => Play(metalHit);
    public void PlayMetalPlateFall() => Play(metalPlateFall);
    public void PlayElectricityOff() => Play(electricityOff);
    public void PlayFuseRemove() => Play(fuseRemove);
    public void PlayFuseInsert() => Play(fuseInsert);
    public void PlayHammerSwing() => Play(hammerSwing);

    public void PlayBoatSplash() => Play(boatSplash);
    public void PlayImpact() => Play(impact);
    public void PlayGrunt() => Play(grunt);
    public void PlayGlassBreak() => Play(glassBreak);
    public void PlayClick() => Play(click);

    public void PlayMusic(AudioClip clip)
    {
        if (clip == null)
            return;

        musicSource.clip = clip;
        musicSource.Play();
    }

    public void PlayTenseMusic() => PlayMusic(tenseMusic);
    public void PlayEndingMusic() => PlayMusic(endingMusic);

    public void StartEmergencyLightHum()
    {
        sfxSource.clip = emergencyLightHum;
        sfxSource.loop = true;
        sfxSource.Play();
    }

    public void StopEmergencyLightHum()
    {
        if (sfxSource.clip == emergencyLightHum)
        {
            sfxSource.Stop();
            sfxSource.loop = false;
            sfxSource.clip = null;
        }
    }

    public void StartSea()
    {
        sfxSource.clip = sea;
        sfxSource.loop = true;
        sfxSource.Play();
    }

    public void StopSea()
    {
        if (sfxSource.clip == sea)
        {
            sfxSource.Stop();
            sfxSource.loop = false;
            sfxSource.clip = null;
        }
    }
}