using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Sound
{

    public string name;

    public AudioClip clip;

    [Range(0f, 1f)]
    public float volume = 0.5f;

    [Range(.1f, 3f)]
    public float pitch = 1f;

    // Indica si el sonido debe repetirse en bucle
    public bool loop = false;
}

public class AudioManager : MonoBehaviour
{
    // Instancia estática del AudioManager para acceso global
    public static AudioManager instance;

    //[Header("Audio Source")]
    [SerializeField] public AudioSource audioSource; // Fuente de audio única para reproducir sonidos

    [Header("Audio Clips")]
    public Sound[] musicClips; // Arreglo de clips de música
    public Sound[] sfxClips; // Arreglo de clips de efectos de sonido
    public Sound[] randomClips; // Arreglo de clips de sonido aleatorios

    private Dictionary<string, Sound> musicDictionary; // Diccionario para la música
    private Dictionary<string, Sound> sfxDictionary; // Diccionario para los efectos de sonido

    private void Awake()
    {
        // Configurar la instancia del AudioManager para que persista entre escenas
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // No destruir este objeto al cargar una nueva escena
        }
        else
        {
            Destroy(gameObject);
        }

        // Inicializar los diccionarios de audio y asegurar que el AudioSource está asignado
        InitializeAudioDictionaries();
        EnsureAudioSource();
    }

    private void InitializeAudioDictionaries()
    {
        // Inicializar el diccionario de música
        musicDictionary = new Dictionary<string, Sound>();
        foreach (var sound in musicClips)
        {
            musicDictionary.Add(sound.name, sound);
        }

        // Inicializar el diccionario de efectos de sonido
        sfxDictionary = new Dictionary<string, Sound>();
        foreach (var sound in sfxClips)
        {
            sfxDictionary.Add(sound.name, sound);
        }
    }

    private void EnsureAudioSource()
    {
        // Asegurarse de que el AudioSource está asignado, y si no lo está, añadir uno nuevo
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
    }

    public void PlayMusic(string name)
    {
        // Asegurarse de que el AudioSource está asignado
        EnsureAudioSource();

        // Reproducir el clip de música correspondiente al nombre proporcionado
        if (musicDictionary.TryGetValue(name, out var sound))
        {
            audioSource.clip = sound.clip;
            audioSource.volume = sound.volume;
            audioSource.pitch = sound.pitch;
            audioSource.loop = sound.loop;
            audioSource.Play();
        }
        else
        {
            Debug.LogWarning("No se encontró el clip de música: " + name);
        }
    }

    public void PlaySFX(string name)
    {
        // Asegurarse de que el AudioSource está asignado
        EnsureAudioSource();

        // Reproducir el efecto de sonido correspondiente al nombre proporcionado
        if (sfxDictionary.TryGetValue(name, out var sound))
        {
            audioSource.PlayOneShot(sound.clip, sound.volume);
        }
        else
        {
            Debug.LogWarning("No se encontró el clip de efecto de sonido: " + name);
        }
    }

    public void PlayRandomSound()
    {
        // Asegurarse de que el AudioSource está asignado
        EnsureAudioSource();

        // Reproducir un clip de sonido aleatorio del arreglo de clips aleatorios
        if (randomClips.Length == 0)
        {
            Debug.LogWarning("No hay clips de sonido aleatorios disponibles.");
            return;
        }

        int index = Random.Range(0, randomClips.Length);
        Sound sound = randomClips[index];
        audioSource.PlayOneShot(sound.clip, sound.volume);
    }

    public void StopAudio()
    {
        // Asegurarse de que el AudioSource está asignado
        EnsureAudioSource();

        // Detener el audio en el AudioSource
        audioSource.Stop();
    }

    public void SetAudioVolume(float volume)
    {
        // Asegurarse de que el AudioSource está asignado
        EnsureAudioSource();

        // Establecer el volumen del AudioSource
        audioSource.volume = volume;
    }
}