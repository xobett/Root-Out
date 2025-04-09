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
        public bool loop = false; // Indica si el sonido debe repetirse en bucle
        [HideInInspector] public AudioSource source; // Fuente de audio asociada
    }
    #endregion

    public static AudioManagerSFX Instance; // Singleton para acceso global
    public List<Sound> sounds; // Lista de sonidos configurables

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

        // Inicializar los AudioSources para cada sonido
        foreach (var sound in sounds)
        {
            sound.source = gameObject.AddComponent<AudioSource>();
            sound.source.clip = sound.clip;
            sound.source.volume = sound.volume;
            sound.source.pitch = sound.pitch;
            sound.source.loop = sound.loop;
        }
    }

    public void PlaySFX(string name)
    {
        var sound = sounds.Find(s => s.name == name);
        if (sound != null)
        {
            sound.source.Play();
            Debug.Log($"Reproduciendo sonido: {name}");
        }
        else
        {
            Debug.LogWarning($"El sonido '{name}' no fue encontrado.");
        }
    }

    public void Stop(string name)
    {
        var sound = sounds.Find(s => s.name == name);
        if (sound != null)
        {
            sound.source.Stop();
        }
        else
        {
            Debug.LogWarning($"El sonido '{name}' no fue encontrado.");
        }
    }
}
