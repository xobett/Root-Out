using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;


public class TimeController : MonoBehaviour
{

    [SerializeField] private float rewindTime = 5f;  // Tiempo m�ximo a retroceder
    [SerializeField] private LayerMask affectedLayers;  // Capas que ser�n afectadas por el retroceso del tiempo

    private bool startRewind = true;  // Indica si se est� retrocediendo el tiempo
    private Dictionary<GameObject, List<PointInTime>> objectsPointsInTime;  // Diccionario para almacenar los estados de los objetos en el tiempo

    public Volume volume;

    void Start()
    {
        objectsPointsInTime = new Dictionary<GameObject, List<PointInTime>>();
    }

    // M�todo llamado en cada frame
    void Update()
    {
        TimeAction();
    }

    // M�todo llamado en cada frame fijo
    void FixedUpdate()
    {
        Logic();
    }

    // Controla la acci�n de retroceder el tiempo
    void TimeAction()
    {
        // Inicia el retroceso del tiempo al presionar la tecla Q
        if (Input.GetKey(KeyCode.Q))
        {
            startRewind = true;
            volume.enabled = true;
        }
        // Detiene el retroceso del tiempo al soltar la tecla Q
        else
        {
            startRewind = false;
            volume.enabled = false;
        }
    }
    void Logic()
    {
        // Si se est� retrocediendo el tiempo, llama a la funci�n Rewind
        if (startRewind == true)
        {
            Rewind();
        }
        // Si no se est� retrocediendo el tiempo, llama a la funci�n Record para registrar el estado actual
        else
        {
            Record();
        }
    }

    // Retrocede el tiempo para los objetos afectados
    void Rewind()
    {
        foreach (var item in objectsPointsInTime)
        {
            // Objeto que est� siendo afectado
            GameObject obj = item.Key;
            // Lista de puntos en el tiempo del objeto
            List<PointInTime> pointsInTime = item.Value;

            // Si hay puntos en el tiempo almacenados
            if (pointsInTime.Count > 0)
            {
                // Toma el primer punto en el tiempo
                PointInTime pointInTime = pointsInTime[0];
                // Restaura la posici�n y rotaci�n del objeto a ese punto en el tiempo
                obj.transform.SetPositionAndRotation(pointInTime.position, pointInTime.rotation);
                // Elimina ese punto de la lista
                pointsInTime.RemoveAt(0);
            }
        }
    }

    // Registra el estado actual de los objetos afectados
    void Record()
    {
        // Encuentra todos los objetos en las capas afectadas dentro de un radio de 100 unidades
        Collider[] colliders = Physics.OverlapSphere(transform.position, 100f, affectedLayers);

        foreach (var collider in colliders)
        {
            GameObject obj = collider.gameObject;

            // Si el objeto no est� en el diccionario, lo agrega con una lista vac�a
            if (!objectsPointsInTime.ContainsKey(obj))
            {
                objectsPointsInTime[obj] = new List<PointInTime>();
            }

            List<PointInTime> pointsInTime = objectsPointsInTime[obj];

            // Si la lista de puntos en el tiempo excede el tiempo m�ximo de retroceso, elimina el punto m�s antiguo
            if (pointsInTime.Count > Mathf.Round(rewindTime / Time.fixedDeltaTime))
            {
                pointsInTime.RemoveAt(pointsInTime.Count - 1);
            }

            // Inserta el estado actual del objeto al inicio de la lista
            pointsInTime.Insert(0, new PointInTime(obj.transform.position, obj.transform.rotation));
        }
    }
}

// Clase para almacenar la posici�n y rotaci�n del objeto en un punto en el tiempo
public class PointInTime
{
    // Posici�n del objeto
    public Vector3 position;
    // Rotaci�n del objeto
    public Quaternion rotation;

    // Constructor que inicializa la posici�n y rotaci�n del objeto
    public PointInTime(Vector3 _position, Quaternion _rotation)
    {
        position = _position;
        rotation = _rotation;
    }
}
