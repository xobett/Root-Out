using System.Collections.Generic;
using System.Security.Principal;
using UnityEngine.SceneManagement;
using UnityEngine;

#region Sound Class

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
#endregion

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;  // Instancia est�tica del AudioManager para acceso global

    [Header("Audio Source")]
    [SerializeField] private AudioSource musicAudioSource; // Fuente de audio para m�sica

    [Header("Music Clips")]
    [SerializeField] public Sound[] musicClips;

    [SerializeField] private string mainMenu = "Main Menu";
    [SerializeField] private string creditsScene = "Credits";
    [SerializeField] private string teamRoles = "Member Credits";

    private Dictionary<string, Sound> musicDictionary; // Diccionario para la m�sica

    private void Awake()
    {
        // Configurar la instancia del AudioManager para que persista entre escenas
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // No destruir este objeto al cargar una nueva escena
        }

        // Inicializar el diccionario de m�sica y asegurar que el AudioSource est� asignado
        InitializeAudioDictionary();
        EnsureAudioSource();

        // Inicializar el volumen desde el inspector
        musicAudioSource.volume = GetMusicClipVolume();

        // Reproducir autom�ticamente los sonidos que tienen playOnAwake configurado en true
        //PlaySoundsOnAwake();
    }

    private void PlaySoundsOnAwake()
    {
        var currentScene = SceneManager.GetActiveScene();

        // Reproducir los clips de m�sica que tienen playOnAwake configurado en true
        if (currentScene.name != "Main Menu")
        {
            foreach (var sound in musicClips)
            {
                if (sound.playOnAwake)
                {
                    Debug.Log("Playing music on awake: " + sound.name);
                    PlayMusic(sound.name);
                }
            } 
        }
    }

    private void InitializeAudioDictionary()
    {

        musicDictionary = new Dictionary<string, Sound>();
        foreach (var sound in musicClips)
        {
            musicDictionary.Add(sound.name, sound);
        }
    }

    private void EnsureAudioSource() // M�todo para asegurarse de que el AudioSource est� asignado
    {
        // Asegurarse de que el AudioSource est� asignado, y si no lo est�, a�adirlo
        if (musicAudioSource == null)
        {
            musicAudioSource = gameObject.AddComponent<AudioSource>();
        }
    }

    public void PlayMusic(string name) // Metodo para reproducir m�sica
    {
        // Asegurarse de que el AudioSource est� asignado
        EnsureAudioSource();

        // Reproducir el clip de m�sica correspondiente al nombre proporcionado
        if (musicDictionary.TryGetValue(name, out var sound))
        {
            musicAudioSource.clip = sound.clip;
            musicAudioSource.volume = sound.volume;
            musicAudioSource.pitch = sound.pitch;
            musicAudioSource.loop = sound.loop;
            musicAudioSource.Play();
        }
        else
        {
            Debug.LogWarning("No se encontr� el clip de m�sica: " + name);
        }
    }

    public void StopAudio() // M�todo para detener el audio
    {
        // Asegurarse de que el AudioSource est� asignado
        EnsureAudioSource();

        // Detener el audio en el AudioSource
        musicAudioSource.Stop();
    }

    public void SetAudioVolume(float volume) // M�todo para establecer el volumen del AudioSource
    {
        // Asegurarse de que el AudioSource est� asignado
        EnsureAudioSource();

        // Establecer el volumen del AudioSource
        musicAudioSource.volume = volume;

        SetMusicClipsVolume(volume); // Establecer el volumen de los clips de m�sica
    }

    public void SetMusicClipsVolume(float volume) // Metodo para controlar el volumen de los clips
    {
        musicAudioSource.volume = volume; // Actualizar el volumen del AudioSource de m�sica
    }

    public float GetMusicClipVolume() // M�todo para obtener el volumen del clip de m�sica
    {
        return musicAudioSource.volume; // Devolver el volumen actual del AudioSource de m�sica
    }
}
