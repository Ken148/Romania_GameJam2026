using System.Collections;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    // =========================================================
    // GLOBAL AUDIO CONTROL
    // =========================================================

    [Header("========== GLOBAL AUDIO ==========")]

    [Tooltip(
        "Temporarily disables ALL sounds. " +
        "The current individual Sound Enabled settings are saved. " +
        "Turning this back off restores those settings."
    )]
    [SerializeField] private bool disableAllSounds = false;

    private bool[] savedSoundEnabledStates;
    private bool previousDisableAllSounds;

    // =========================================================
    // SOUND DATA
    // =========================================================

    [System.Serializable]
    public class Sound
    {
        [Header("Sound Enabled")]

        [Tooltip(
            "If disabled, this individual sound will never play."
        )]
        public bool soundEnabled = true;

        [Header("Audio Clip")]

        public AudioClip clip;

        [Header("Object That Plays Sound")]

        [Tooltip(
            "Assign the GameObject associated with this action. " +
            "Other scripts can call PlayActionSound(gameObject) " +
            "and the AudioManager will find this sound."
        )]
        public GameObject soundObject;

        [Header("Play On Start")]

        [Tooltip(
            "Automatically play this sound when the scene starts."
        )]
        public bool playOnStart = false;

        [Tooltip(
            "How many seconds to wait after the game starts before playing."
        )]
        [Min(0f)]
        public float startDelay = 0f;

        [Header("Fade In")]

        [Tooltip(
            "Fade from Start Volume to End Volume when Play On Start is enabled."
        )]
        public bool fadeInOnStart = false;

        [Tooltip(
            "Volume when the sound begins playing."
        )]
        [Range(0f, 1f)]
        public float startVolume = 0f;

        [Tooltip(
            "Maximum volume the sound will reach."
        )]
        [Range(0f, 1f)]
        public float endVolume = 0.04f;

        [Tooltip(
            "How many seconds the fade takes."
        )]
        [Min(0f)]
        public float fadeInDuration = 5f;

        [Header("Settings")]

        [Tooltip(
            "Playback speed of this sound."
        )]
        [Range(0.1f, 3f)]
        public float speed = 1f;

        [Header("Loop")]

        [Tooltip(
            "If enabled, this sound will continuously loop when played."
        )]
        public bool loop = false;

        [Header("3D Sound")]

        [Tooltip(
            "Maximum distance at which this sound can be heard."
        )]
        [Range(1f, 100f)]
        public float maxDistance = 50f;

        [HideInInspector]
        public AudioSource source;
    }

    // =========================================================
    // SOURCES
    // =========================================================

    [Header("Sources")]

    [Tooltip(
        "AudioSource used for normal one-shot sound effects."
    )]
    [SerializeField] private AudioSource sfxSource;

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

        // -----------------------------------------------------
        // SETUP ALL SOUNDS
        // -----------------------------------------------------

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

        // -----------------------------------------------------
        // SAVE INITIAL SETTINGS
        // -----------------------------------------------------

        SaveCurrentSoundStates();

        previousDisableAllSounds =
            disableAllSounds;

        // -----------------------------------------------------
        // IF DISABLE ALL WAS ALREADY ENABLED
        // -----------------------------------------------------

        if (disableAllSounds)
        {
            DisableAllSoundStates();
            StopAllManagedAudio();
        }
    }

    // =========================================================
    // UPDATE
    // =========================================================

    private void Update()
    {
        if (disableAllSounds != previousDisableAllSounds)
        {
            if (disableAllSounds)
            {
                SaveCurrentSoundStates();
                DisableAllSoundStates();
                StopAllManagedAudio();
            }
            else
            {
                RestoreSavedSoundStates();
            }

            previousDisableAllSounds =
                disableAllSounds;
        }
    }

    // =========================================================
    // SAVE SOUND STATES
    // =========================================================

    private void SaveCurrentSoundStates()
    {
        Sound[] allSounds =
            GetAllSounds();

        savedSoundEnabledStates =
            new bool[allSounds.Length];

        for (int i = 0; i < allSounds.Length; i++)
        {
            if (allSounds[i] != null)
            {
                savedSoundEnabledStates[i] =
                    allSounds[i].soundEnabled;
            }
            else
            {
                savedSoundEnabledStates[i] =
                    false;
            }
        }
    }

    // =========================================================
    // DISABLE ALL SOUND STATES
    // =========================================================

    private void DisableAllSoundStates()
    {
        Sound[] allSounds =
            GetAllSounds();

        foreach (Sound sound in allSounds)
        {
            if (sound == null)
                continue;

            sound.soundEnabled = false;
        }
    }

    // =========================================================
    // RESTORE SAVED SOUND STATES
    // =========================================================

    private void RestoreSavedSoundStates()
    {
        if (savedSoundEnabledStates == null)
            return;

        Sound[] allSounds =
            GetAllSounds();

        int count =
            Mathf.Min(
                allSounds.Length,
                savedSoundEnabledStates.Length
            );

        for (int i = 0; i < count; i++)
        {
            if (allSounds[i] != null)
            {
                allSounds[i].soundEnabled =
                    savedSoundEnabledStates[i];
            }
        }
    }

    // =========================================================
    // STOP ALL MANAGED AUDIO
    // =========================================================

    private void StopAllManagedAudio()
    {
        Sound[] allSounds =
            GetAllSounds();

        foreach (Sound sound in allSounds)
        {
            if (sound == null)
                continue;

            if (sound.source == null)
                continue;

            sound.source.Stop();
            sound.source.volume = 0f;
        }

        if (sfxSource != null)
            sfxSource.Stop();
    }

    // =========================================================
    // GET ALL SOUNDS
    // =========================================================

    private Sound[] GetAllSounds()
    {
        return new Sound[]
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
                "AudioManager: A Sound has no AudioClip assigned."
            );

            return;
        }

        AudioSource source = null;

        // -----------------------------------------------------
        // USE ASSIGNED OBJECT
        // -----------------------------------------------------

        if (sound.soundObject != null)
        {
            source =
                sound.soundObject.GetComponent<AudioSource>();

            if (source == null)
            {
                source =
                    sound.soundObject.AddComponent<AudioSource>();
            }
        }

        // -----------------------------------------------------
        // CREATE AUDIO OBJECT
        // -----------------------------------------------------

        else
        {
            GameObject audioObject =
                new GameObject("AudioSource");

            audioObject.transform.SetParent(transform);

            source =
                audioObject.AddComponent<AudioSource>();
        }

        // -----------------------------------------------------
        // CONFIGURE SOURCE
        // -----------------------------------------------------

        source.clip =
            sound.clip;

        source.pitch =
            sound.speed;

        source.loop =
            sound.loop;

        source.playOnAwake =
            false;

        source.spatialBlend =
            1f;

        source.maxDistance =
            sound.maxDistance;

        sound.source =
            source;

        // Always start silent.
        source.volume =
            0f;

        // -----------------------------------------------------
        // PLAY ON START
        // -----------------------------------------------------

        if (sound.playOnStart &&
            sound.soundEnabled &&
            !disableAllSounds)
        {
            StartCoroutine(
                PlayOnStartRoutine(sound)
            );
        }
    }

    // =========================================================
    // PLAY ON START
    // =========================================================

    private IEnumerator PlayOnStartRoutine(
        Sound sound)
    {
        if (!CanPlaySound(sound))
            yield break;

        if (sound.startDelay > 0f)
        {
            yield return new WaitForSeconds(
                sound.startDelay
            );
        }

        if (!CanPlaySound(sound))
            yield break;

        if (sound.fadeInOnStart)
        {
            yield return StartCoroutine(
                FadeInSound(sound)
            );
        }
        else
        {
            sound.source.volume =
                Mathf.Clamp01(
                    sound.endVolume
                );

            sound.source.Play();
        }
    }

    // =========================================================
    // CAN PLAY SOUND
    // =========================================================

    private bool CanPlaySound(Sound sound)
    {
        if (sound == null)
            return false;

        if (!sound.soundEnabled)
            return false;

        if (disableAllSounds)
            return false;

        if (sound.source == null)
            return false;

        if (sound.clip == null)
            return false;

        return true;
    }

    // =========================================================
    // FADE IN
    // =========================================================

    private IEnumerator FadeInSound(
        Sound sound)
    {
        if (!CanPlaySound(sound))
            yield break;

        AudioSource source =
            sound.source;

        float startVolume =
            Mathf.Clamp01(
                sound.startVolume
            );

        float endVolume =
            Mathf.Clamp01(
                sound.endVolume
            );

        startVolume =
            Mathf.Min(
                startVolume,
                endVolume
            );

        source.volume =
            startVolume;

        source.Play();

        if (sound.fadeInDuration <= 0f)
        {
            source.volume =
                endVolume;

            yield break;
        }

        float elapsed = 0f;

        while (
            elapsed <
            sound.fadeInDuration)
        {
            if (!CanPlaySound(sound))
            {
                source.Stop();
                source.volume = 0f;

                yield break;
            }

            elapsed +=
                Time.deltaTime;

            float t =
                Mathf.Clamp01(
                    elapsed /
                    sound.fadeInDuration
                );

            float currentVolume =
                Mathf.Lerp(
                    startVolume,
                    endVolume,
                    t
                );

            source.volume =
                Mathf.Min(
                    currentVolume,
                    endVolume
                );

            yield return null;
        }

        source.volume =
            endVolume;
    }

    // =========================================================
    // PLAY SOUND
    // =========================================================

    private void PlaySound(Sound sound)
    {
        if (!CanPlaySound(sound))
            return;

        // =====================================================
        // ONE SHOT
        // =====================================================

        if (!sound.loop)
        {
            if (sfxSource == null)
            {
                Debug.LogError(
                    "AudioManager: sfxSource is not assigned!"
                );

                return;
            }

            sfxSource.pitch =
                sound.speed;

            sfxSource.PlayOneShot(
                sound.clip,
                sound.endVolume
            );

            Debug.Log(
                "AudioManager: Playing one-shot: " +
                sound.clip.name
            );

            return;
        }

        // =====================================================
        // LOOP
        // =====================================================

        if (sound.source == null)
            return;

        sound.source.pitch =
            sound.speed;

        sound.source.loop =
            true;

        sound.source.volume =
            sound.endVolume;

        if (!sound.source.isPlaying)
        {
            sound.source.Play();
        }
    }

    // =========================================================
    // GENERIC ACTION SOUND SYSTEM
    // =========================================================

    /// <summary>
    /// Call this from any gameplay script when an action happens.
    ///
    /// Example:
    /// AudioManager.Instance.PlayActionSound(gameObject);
    ///
    /// The AudioManager searches all configured sounds and
    /// plays the sound whose Sound Object matches the object
    /// that called this function.
    ///
    /// Exact matches and parent/child relationships are supported.
    /// </summary>
    public void PlayActionSound(GameObject actionObject)
    {
        if (actionObject == null)
        {
            Debug.LogWarning(
                "AudioManager: actionObject is NULL."
            );

            return;
        }

        Debug.Log(
            "PlayActionSound called with: " +
            actionObject.name
        );

        if (disableAllSounds)
        {
            Debug.LogWarning(
                "AudioManager: ALL SOUNDS ARE DISABLED."
            );

            return;
        }

        Sound[] allSounds =
            GetAllSounds();

        foreach (Sound sound in allSounds)
        {
            if (sound == null)
                continue;

            if (sound.soundObject == null)
                continue;

            if (!sound.soundEnabled)
                continue;

            Debug.Log(
                "Checking: " +
                sound.soundObject.name +
                " | Clip: " +
                (sound.clip != null
                    ? sound.clip.name
                    : "NULL") +
                " | Enabled: " +
                sound.soundEnabled
            );

            // -------------------------------------------------
            // EXACT MATCH
            // -------------------------------------------------

            if (sound.soundObject == actionObject ||
                actionObject.transform.IsChildOf(sound.soundObject.transform) ||
                sound.soundObject.transform.IsChildOf(actionObject.transform))
            {
                Debug.Log(
                    "MATCH FOUND! Playing: " +
                    sound.clip.name
                );

                PlaySound(sound);
                return;
            }

            // -------------------------------------------------
            // ACTION OBJECT IS A CHILD
            // -------------------------------------------------

            if (actionObject.transform.IsChildOf(
                sound.soundObject.transform))
            {
                Debug.Log(
                    "PARENT MATCH FOUND! Playing: " +
                    (sound.clip != null
                        ? sound.clip.name
                        : "NULL")
                );

                PlaySound(sound);
                return;
            }

            // -------------------------------------------------
            // CONFIGURED OBJECT IS A CHILD
            // -------------------------------------------------

            if (sound.soundObject.transform.IsChildOf(
                actionObject.transform))
            {
                Debug.Log(
                    "CHILD MATCH FOUND! Playing: " +
                    (sound.clip != null
                        ? sound.clip.name
                        : "NULL")
                );

                PlaySound(sound);
                return;
            }
        }

        Debug.LogWarning(
            "NO SOUND MATCH FOUND FOR: " +
            actionObject.name
        );
    }

    // =========================================================
    // PLAY AT WORLD POSITION
    // =========================================================

    public void PlayAtPosition(
        Sound sound,
        Vector3 position)
    {
        if (!CanPlaySound(sound))
            return;

        GameObject audioObject =
            new GameObject(
                "3D_Audio"
            );

        audioObject.transform.position =
            position;

        AudioSource source =
            audioObject.AddComponent<AudioSource>();

        source.clip =
            sound.clip;

        source.volume =
            sound.endVolume;

        source.pitch =
            sound.speed;

        source.spatialBlend =
            1f;

        source.maxDistance =
            sound.maxDistance;

        source.playOnAwake =
            false;

        source.loop =
            sound.loop;

        source.Play();

        if (!sound.loop)
        {
            float destroyTime =
                sound.clip.length /
                Mathf.Max(
                    0.01f,
                    Mathf.Abs(
                        sound.speed
                    )
                );

            Destroy(
                audioObject,
                destroyTime
            );
        }
    }

    // =========================================================
    // LOOPING SOUNDS
    // =========================================================

    private void PlayLoop(
        Sound sound)
    {
        if (!CanPlaySound(sound))
            return;

        if (sound.source == null)
            return;

        sound.source.pitch =
            sound.speed;

        sound.source.volume =
            sound.endVolume;

        sound.source.loop =
            true;

        if (!sound.source.isPlaying)
        {
            sound.source.Play();
        }
    }

    // =========================================================
    // STOP
    // =========================================================

    private void StopSound(
        Sound sound)
    {
        if (sound == null ||
            sound.source == null)
            return;

        sound.source.Stop();

        sound.source.loop =
            sound.loop;

        sound.source.volume =
            0f;
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

    public void StartBoxPush()
        => PlayLoop(boxPush);

    public void StopBoxPush()
        => StopSound(boxPush);

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
    // TENSE MUSIC
    // =========================================================

    public void PlayTenseMusic()
        => PlaySound(tenseMusic);

    // =========================================================
    // ENDING MUSIC
    // =========================================================

    public void PlayEndingMusic()
        => PlaySound(endingMusic);

    // =========================================================
    // EMERGENCY LIGHT HUM
    // =========================================================

    public void StartEmergencyLightHum()
    {
        PlayLoop(
            emergencyLightHum
        );
    }

    public void StopEmergencyLightHum()
    {
        StopSound(
            emergencyLightHum
        );
    }

    // =========================================================
    // SEA
    // =========================================================

    public void StartSea()
    {
        PlayLoop(sea);
    }

    public void StopSea()
    {
        StopSound(sea);
    }
}