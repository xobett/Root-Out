using System.Collections.Generic;
using UnityEngine;


public class AudioManagerSFX : MonoBehaviour
{
    #region Sound Class
    [System.Serializable]
    public class Sound
    {
        public string name; // Nombre del sonido
        public AudioClip clip; // Clip de audio
        [Range(0f, 1f)] public float volume = 1f; // Volumen del sonido
        [Range(0.1f, 3f)] public float pitch = 1f; // Tono del sonido
    }
    #endregion


    public static AudioManagerSFX Instance; // Singleton para acceso global
    public List<Sound> sounds; // Lista de sonidos configurables
    private Dictionary<string, AudioSource> soundDictionary; // Diccionario para acceso rápido

    private void Awake()
    {
        // Configurar Singleton
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        // Inicializar el diccionario de sonidos
        soundDictionary = new Dictionary<string, AudioSource>();
        foreach (var sound in sounds)
        {
            AudioSource source = gameObject.AddComponent<AudioSource>();
            source.clip = sound.clip;
            source.volume = sound.volume;
            source.pitch = sound.pitch;
            source.playOnAwake = false;
            soundDictionary[sound.name] = source;
        }
    }

    public void PlaySFX(string name) 
    {
        if (soundDictionary.TryGetValue(name, out var source))
        {
            source.Play();
            Debug.Log($"Reproduciendo sonido: {name}");
        }
        else
        {
            Debug.LogWarning($"El sonido '{name}' no fue encontrado.");
        }
    }

    
    public void Stop(string name) // Detener un sonido específico
    {
        if (soundDictionary.TryGetValue(name, out var source))
        {
            source.Stop();
        }
        else
        {
            Debug.LogWarning($"El sonido '{name}' no fue encontrado.");
        }
    }
}
