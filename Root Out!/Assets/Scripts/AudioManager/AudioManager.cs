using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class Sound
{
    public string name;
    public AudioClip clip;

    [Range(0f, 1f)]
    public float volume = 1f;

    [Range(.1f, 3f)]
    public float pitch = 1f;

    // Indica si el sonido debe repetirse en bucle
    public bool loop = false;

    public bool playOnAwake = false; // Indica si el sonido debe reproducirse al iniciar
}

public class AudioManager : MonoBehaviour
{
    // Instancia est�tica del AudioManager para acceso global
    public static AudioManager instance;

    [Header("Audio Source")]
    [SerializeField] private AudioSource musicAudioSource; // Fuente de audio para m�sica
    [SerializeField] private AudioSource sfxAudioSource; // Fuente de audio para efectos de sonido

    [Header("Audio Clips")]
    public Sound[] musicClips; // Arreglo de clips de m�sica
    public Sound[] sfxClips; // Arreglo de clips de efectos de sonido
    public Sound[] randomClips; // Arreglo de clips de sonido aleatorios

    private Dictionary<string, Sound> musicDictionary; // Diccionario para la m�sica
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

        // Inicializar los diccionarios de audio y asegurar que los AudioSources est�n asignados
        InitializeAudioDictionaries();
        EnsureAudioSources();

        // Inicializar los vol�menes desde el inspector
        musicAudioSource.volume = GetMusicClipVolume();
        sfxAudioSource.volume = GetVFXVolume();

        // Reproducir autom�ticamente los sonidos que tienen playOnAwake configurado en true
        PlaySoundsOnAwake();
    }

    private void PlaySoundsOnAwake()
    {
        // Reproducir los clips de m�sica que tienen playOnAwake configurado en true
        //foreach (var sound in musicClips)
        //{
        //    if (sound.playOnAwake)
        //    {
        //        Debug.Log("Playing music on awake: " + sound.name);
        //        PlayMusic(sound.name);
        //    }
        //}

        //// Reproducir los efectos de sonido que tienen playOnAwake configurado en true
        //foreach (var sound in sfxClips)
        //{
        //    if (sound.playOnAwake)
        //    {
        //        Debug.Log("Playing SFX on awake: " + sound.name);
        //        PlaySFX(sound.name);
        //    }
        //}

        //// Reproducir los clips de sonido aleatorios que tienen playOnAwake configurado en true
        //foreach (var sound in randomClips)
        //{
        //    if (sound.playOnAwake)
        //    {
        //        Debug.Log("Playing random sound on awake: " + sound.name);
        //        PlaySFX(sound.name);
        //    }
        //}
    }

    private void InitializeAudioDictionaries()
    {
        // Inicializar el diccionario de m�sica
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

    private void EnsureAudioSources()
    {
        //// Asegurarse de que los AudioSources est�n asignados, y si no lo est�n, a�adirlos
        //if (musicAudioSource == null)
        //{
        //    musicAudioSource = gameObject.AddComponent<AudioSource>();
        //}

        //if (sfxAudioSource == null)
        //{
        //    sfxAudioSource = gameObject.AddComponent<AudioSource>();
        //}
    }

    public void PlayMusic(string name)
    {
        //// Asegurarse de que el AudioSource est� asignado
        //EnsureAudioSources();

        //// Reproducir el clip de m�sica correspondiente al nombre proporcionado
        //if (musicDictionary.TryGetValue(name, out var sound))
        //{
        //    Debug.Log("Playing music: " + name);
        //    musicAudioSource.clip = sound.clip;
        //    musicAudioSource.volume = sound.volume;
        //    musicAudioSource.pitch = sound.pitch;
        //    musicAudioSource.loop = sound.loop;
        //    musicAudioSource.Play();
        //}
        //else
        //{
        //    Debug.LogWarning("No se encontr� el clip de m�sica: " + name);
        //}
    }

    public void PlaySFX(string name)
    {
        // Asegurarse de que el AudioSource est� asignado
        //EnsureAudioSources();

        //// Reproducir el efecto de sonido correspondiente al nombre proporcionado
        //if (sfxDictionary.TryGetValue(name, out var sound))
        //{
        //    Debug.Log("Playing SFX: " + name);
        //    sfxAudioSource.PlayOneShot(sound.clip, sfxAudioSource.volume);
        //}
        //else
        //{
        //    Debug.LogWarning("No se encontr� el clip de efecto de sonido: " + name);
        //}
    }

    public void PlayRandomSound()
    {
        // Asegurarse de que el AudioSource est� asignado
        EnsureAudioSources();

        // Reproducir un clip de sonido aleatorio del arreglo de clips aleatorios
        if (randomClips.Length == 0)
        {
            Debug.LogWarning("No hay clips de sonido aleatorios disponibles.");
            return;
        }

        int index = Random.Range(0, randomClips.Length);
        Sound sound = randomClips[index];
        Debug.Log("Playing random sound: " + sound.name);
        sfxAudioSource.PlayOneShot(sound.clip, sfxAudioSource.volume);
    }

    public void StopAudio()
    {
        // Asegurarse de que el AudioSource est� asignado
        EnsureAudioSources();

        // Detener el audio en el AudioSource
        musicAudioSource.Stop();
        sfxAudioSource.Stop();
    }

    public void SetAudioVolume(float volume)
    {
        // Asegurarse de que el AudioSource est� asignado
        EnsureAudioSources();

        // Establecer el volumen del AudioSource
        musicAudioSource.volume = volume;
        sfxAudioSource.volume = volume;

        SetMusicClipsVolume(volume); // Establecer el volumen de los clips de m�sica
    }

    // M�todo para establecer el volumen de los VFX
    public void SetVFXVolume(float volume)
    {
        sfxAudioSource.volume = volume; // Actualizar el volumen del AudioSource de SFX
    }

    public void SetMusicClipsVolume(float volume)
    {
        musicAudioSource.volume = volume; // Actualizar el volumen del AudioSource de m�sica
    }

    public float GetMusicClipVolume()
    {
        return musicAudioSource.volume; // Devolver el volumen actual del AudioSource de m�sica
    }

    // M�todo para obtener el volumen actual de los VFX
    public float GetVFXVolume()
    {
        return sfxAudioSource.volume; // Devolver el volumen actual del AudioSource de SFX
    }
}