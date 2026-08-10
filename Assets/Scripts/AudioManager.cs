using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    // =========================================================
    // SOUND DATA
    // =========================================================

    [System.Serializable]
    public class Sound
    {
        [Header("Sound Name")]
        public string soundName;

        [Header("Audio Clip")]
        public AudioClip clip;

        [Header("Object That Plays Sound")]
        [Tooltip("The GameObject where the AudioSource will be created/used.")]
        public GameObject soundObject;

        [Header("Play On Start")]
        [Tooltip("Automatically play this sound when the scene starts.")]
        public bool playOnStart = false;

        [Header("Settings")]
        [Range(0f, 1f)]
        public float volume = 1f;

        [Range(0.1f, 3f)]
        public float pitch = 1f;

        [Header("Loop")]
        [Tooltip("If enabled, this sound will continuously loop when played.")]
        public bool loop = false;

        [Header("3D Sound")]
        public bool is3D = false;

        [Range(0f, 1f)]
        public float spatialBlend = 1f;

        [Range(1f, 100f)]
        public float maxDistance = 50f;

        [HideInInspector]
        public AudioSource source;
    }

    // =========================================================
    // SOURCES
    // =========================================================

    [Header("Sources")]
    [SerializeField] private AudioSource sfxSource;
    [SerializeField] private AudioSource musicSource;

    // =========================================================
    // FLOOR 1
    // =========================================================

    [Header("========== FLOOR 1 ==========")]

    [SerializeField] private Sound keyPickup;
    [SerializeField] private Sound keyDrop;
    [SerializeField] private Sound screwdriverPickup;
    [SerializeField] private Sound screwdriverDrop;
    [SerializeField] private Sound boxPush;
    [SerializeField] private Sound waterWalk;
    [SerializeField] private Sound objectSplash;

    [SerializeField] private Sound tenseMusic;

    [SerializeField] private Sound emergencyLightHum;

    [SerializeField] private Sound slidingDoorOpen;
    [SerializeField] private Sound unscrew;
    [SerializeField] private Sound paperPickup;
    [SerializeField] private Sound panelFall;
    [SerializeField] private Sound sparks;
    [SerializeField] private Sound floor2DoorOpen;

    // =========================================================
    // FLOOR 2
    // =========================================================

    [Header("========== FLOOR 2 ==========")]

    [SerializeField] private Sound woodHit;
    [SerializeField] private Sound woodBreak;
    [SerializeField] private Sound metalHit;
    [SerializeField] private Sound metalPlateFall;
    [SerializeField] private Sound electricityOff;
    [SerializeField] private Sound fuseRemove;
    [SerializeField] private Sound fuseInsert;
    [SerializeField] private Sound hammerSwing;

    // =========================================================
    // FLOOR 3
    // =========================================================

    [Header("========== FLOOR 3 ==========")]

    [SerializeField] private Sound boatSplash;
    [SerializeField] private Sound impact;
    [SerializeField] private Sound grunt;
    [SerializeField] private Sound glassBreak;
    [SerializeField] private Sound click;

    [SerializeField] private Sound endingMusic;
    [SerializeField] private Sound sea;

    // =========================================================
    // AWAKE
    // =========================================================

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        SetupSound(keyPickup);
        SetupSound(keyDrop);
        SetupSound(screwdriverPickup);
        SetupSound(screwdriverDrop);
        SetupSound(boxPush);
        SetupSound(waterWalk);
        SetupSound(objectSplash);

        SetupSound(tenseMusic);

        SetupSound(emergencyLightHum);

        SetupSound(slidingDoorOpen);
        SetupSound(unscrew);
        SetupSound(paperPickup);
        SetupSound(panelFall);
        SetupSound(sparks);
        SetupSound(floor2DoorOpen);

        SetupSound(woodHit);
        SetupSound(woodBreak);
        SetupSound(metalHit);
        SetupSound(metalPlateFall);
        SetupSound(electricityOff);
        SetupSound(fuseRemove);
        SetupSound(fuseInsert);
        SetupSound(hammerSwing);

        SetupSound(boatSplash);
        SetupSound(impact);
        SetupSound(grunt);
        SetupSound(glassBreak);
        SetupSound(click);

        SetupSound(endingMusic);
        SetupSound(sea);
    }

    // =========================================================
    // SETUP SOUND
    // =========================================================

    private void SetupSound(Sound sound)
    {
        if (sound == null)
            return;

        if (sound.clip == null)
        {
            Debug.LogWarning(
                "AudioManager: No AudioClip assigned to " +
                sound.soundName
            );

            return;
        }

        AudioSource source = null;

        // -----------------------------------------------------
        // IF A SOUND OBJECT WAS ASSIGNED
        // -----------------------------------------------------

        if (sound.soundObject != null)
        {
            // Try to find an existing AudioSource first.
            source = sound.soundObject.GetComponent<AudioSource>();

            // If there isn't one, create one.
            if (source == null)
            {
                source = sound.soundObject.AddComponent<AudioSource>();
            }
        }

        // -----------------------------------------------------
        // IF NO SOUND OBJECT WAS ASSIGNED
        // -----------------------------------------------------

        else
        {
            GameObject audioObject =
                new GameObject("Audio_" + sound.soundName);

            audioObject.transform.SetParent(transform);

            source = audioObject.AddComponent<AudioSource>();
        }

        // -----------------------------------------------------
        // CONFIGURE AUDIO SOURCE
        // -----------------------------------------------------

        source.clip = sound.clip;
        source.volume = sound.volume;
        source.pitch = sound.pitch;

        // IMPORTANT:
        // The Inspector Loop checkbox controls the AudioSource.
        source.loop = sound.loop;

        source.playOnAwake = false;

        if (sound.is3D)
        {
            source.spatialBlend = sound.spatialBlend;
            source.maxDistance = sound.maxDistance;
        }
        else
        {
            source.spatialBlend = 0f;
        }

        sound.source = source;

        // -----------------------------------------------------
        // AUTOMATIC PLAY ON SCENE START
        // -----------------------------------------------------

        if (sound.playOnStart)
        {
            source.Play();
        }
    }

    // =========================================================
    // GENERIC PLAY
    // =========================================================

    public void Play(string soundName)
    {
        Sound sound = FindSound(soundName);

        if (sound == null)
        {
            Debug.LogWarning(
                "AudioManager: Could not find sound: " + soundName
            );

            return;
        }

        PlaySound(sound);
    }

    private void PlaySound(Sound sound)
    {
        if (sound == null)
            return;

        if (sound.clip == null)
        {
            Debug.LogWarning(
                "AudioManager: No AudioClip assigned to " +
                sound.soundName
            );

            return;
        }

        if (sound.source != null)
        {
            sound.source.pitch = sound.pitch;
            sound.source.volume = sound.volume;

            // -------------------------------------------------
            // LOOPING SOUND
            // -------------------------------------------------

            if (sound.loop)
            {
                sound.source.loop = true;

                if (!sound.source.isPlaying)
                {
                    sound.source.Play();
                }
            }

            // -------------------------------------------------
            // NORMAL ONE-SHOT SOUND
            // -------------------------------------------------

            else
            {
                sound.source.loop = false;

                sound.source.PlayOneShot(
                    sound.clip,
                    sound.volume
                );
            }
        }
        else if (sfxSource != null)
        {
            // Fallback AudioSource does not support the
            // individual Sound loop setting.
            sfxSource.PlayOneShot(
                sound.clip,
                sound.volume
            );
        }
    }

    // =========================================================
    // FIND SOUND
    // =========================================================

    private Sound FindSound(string soundName)
    {
        Sound[] allSounds =
        {
            keyPickup,
            keyDrop,
            screwdriverPickup,
            screwdriverDrop,
            boxPush,
            waterWalk,
            objectSplash,

            tenseMusic,

            emergencyLightHum,

            slidingDoorOpen,
            unscrew,
            paperPickup,
            panelFall,
            sparks,
            floor2DoorOpen,

            woodHit,
            woodBreak,
            metalHit,
            metalPlateFall,
            electricityOff,
            fuseRemove,
            fuseInsert,
            hammerSwing,

            boatSplash,
            impact,
            grunt,
            glassBreak,
            click,

            endingMusic,
            sea
        };

        foreach (Sound sound in allSounds)
        {
            if (sound == null)
                continue;

            if (sound.soundName == soundName)
                return sound;
        }

        return null;
    }

    // =========================================================
    // PLAY AT WORLD POSITION
    // =========================================================

    public void PlayAtPosition(
        string soundName,
        Vector3 position)
    {
        Sound sound = FindSound(soundName);

        if (sound == null)
        {
            Debug.LogWarning(
                "AudioManager: Could not find sound: " +
                soundName
            );

            return;
        }

        if (sound.clip == null)
            return;

        GameObject audioObject =
            new GameObject("3D_Audio_" + soundName);

        audioObject.transform.position = position;

        AudioSource source =
            audioObject.AddComponent<AudioSource>();

        source.clip = sound.clip;
        source.volume = sound.volume;
        source.pitch = sound.pitch;
        source.spatialBlend = 1f;
        source.maxDistance = sound.maxDistance;
        source.playOnAwake = false;

        // Respect the Inspector Loop checkbox.
        source.loop = sound.loop;

        source.Play();

        // If looping, don't destroy the object automatically.
        // It will remain until StopAtPosition or the object is destroyed.
        if (!sound.loop)
        {
            float destroyTime =
                sound.clip.length / Mathf.Abs(sound.pitch);

            Destroy(audioObject, destroyTime);
        }
    }

    // =========================================================
    // PLAY SOUND FROM ITS ASSIGNED OBJECT
    // =========================================================

    public void PlayFromObject(string soundName)
    {
        Sound sound = FindSound(soundName);

        if (sound == null)
        {
            Debug.LogWarning(
                "AudioManager: Could not find sound: " +
                soundName
            );

            return;
        }

        if (sound.source == null)
        {
            Debug.LogWarning(
                "AudioManager: No AudioSource available for " +
                soundName
            );

            return;
        }

        sound.source.pitch = sound.pitch;
        sound.source.volume = sound.volume;

        // -----------------------------------------------------
        // LOOPING
        // -----------------------------------------------------

        if (sound.loop)
        {
            sound.source.loop = true;

            if (!sound.source.isPlaying)
            {
                sound.source.Play();
            }
        }

        // -----------------------------------------------------
        // NORMAL SOUND
        // -----------------------------------------------------

        else
        {
            sound.source.loop = false;

            sound.source.PlayOneShot(
                sound.clip,
                sound.volume
            );
        }
    }

    // =========================================================
    // LOOPING SOUNDS
    // =========================================================

    public void PlayLoop(string soundName)
    {
        Sound sound = FindSound(soundName);

        if (sound == null)
        {
            Debug.LogWarning(
                "AudioManager: Could not find sound: " +
                soundName
            );

            return;
        }

        if (sound.source == null)
        {
            Debug.LogWarning(
                "AudioManager: No AudioSource available for " +
                soundName
            );

            return;
        }

        if (sound.clip == null)
            return;

        sound.source.pitch = sound.pitch;
        sound.source.volume = sound.volume;

        // Force looping for this specific call.
        sound.source.loop = true;

        if (!sound.source.isPlaying)
        {
            sound.source.Play();
        }
    }

    public void Stop(string soundName)
    {
        Sound sound = FindSound(soundName);

        if (sound == null)
            return;

        if (sound.source == null)
            return;

        sound.source.Stop();

        // IMPORTANT:
        // Restore the Inspector setting instead of forcing
        // loop to false permanently.
        sound.source.loop = sound.loop;
    }

    // =========================================================
    // MUSIC
    // =========================================================

    public void PlayMusic(AudioClip clip)
    {
        if (clip == null)
            return;

        if (musicSource == null)
            return;

        musicSource.clip = clip;
        musicSource.loop = true;
        musicSource.Play();
    }

    public void StopMusic()
    {
        if (musicSource == null)
            return;

        musicSource.Stop();
    }

    // =========================================================
    // FLOOR 1
    // =========================================================

    public void PlayKeyPickup()
        => PlaySound(keyPickup);

    public void PlayKeyDrop()
        => PlaySound(keyDrop);

    public void PlayScrewdriverPickup()
        => PlaySound(screwdriverPickup);

    public void PlayScrewdriverDrop()
        => PlaySound(screwdriverDrop);

    public void PlayBoxPush()
        => PlaySound(boxPush);

    public void PlayWaterWalk()
        => PlaySound(waterWalk);

    public void PlayObjectSplash()
        => PlaySound(objectSplash);

    public void PlaySlidingDoorOpen()
        => PlaySound(slidingDoorOpen);

    public void PlayUnscrew()
        => PlaySound(unscrew);

    public void PlayPaperPickup()
        => PlaySound(paperPickup);

    public void PlayPanelFall()
        => PlaySound(panelFall);

    public void PlaySparks()
        => PlaySound(sparks);

    public void PlayFloor2DoorOpen()
        => PlaySound(floor2DoorOpen);

    // =========================================================
    // FLOOR 2
    // =========================================================

    public void PlayWoodHit()
        => PlaySound(woodHit);

    public void PlayWoodBreak()
        => PlaySound(woodBreak);

    public void PlayMetalHit()
        => PlaySound(metalHit);

    public void PlayMetalPlateFall()
        => PlaySound(metalPlateFall);

    public void PlayElectricityOff()
        => PlaySound(electricityOff);

    public void PlayFuseRemove()
        => PlaySound(fuseRemove);

    public void PlayFuseInsert()
        => PlaySound(fuseInsert);

    public void PlayHammerSwing()
        => PlaySound(hammerSwing);

    // =========================================================
    // FLOOR 3
    // =========================================================

    public void PlayBoatSplash()
        => PlaySound(boatSplash);

    public void PlayImpact()
        => PlaySound(impact);

    public void PlayGrunt()
        => PlaySound(grunt);

    public void PlayGlassBreak()
        => PlaySound(glassBreak);

    public void PlayClick()
        => PlaySound(click);

    // =========================================================
    // MUSIC
    // =========================================================

    public void PlayTenseMusic()
        => PlayMusic(tenseMusic.clip);

    public void PlayEndingMusic()
        => PlayMusic(endingMusic.clip);

    // =========================================================
    // EMERGENCY LIGHT HUM
    // =========================================================

    public void StartEmergencyLightHum()
    {
        PlayLoop("EmergencyLightHum");
    }

    public void StopEmergencyLightHum()
    {
        Stop("EmergencyLightHum");
    }

    // =========================================================
    // SEA
    // =========================================================

    public void StartSea()
    {
        PlayLoop("Sea");
    }

    public void StopSea()
    {
        Stop("Sea");
    }
}
