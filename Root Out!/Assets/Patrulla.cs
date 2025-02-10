using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


    [System.Serializable]
    public class Recorrido
    {
        public string nombreRecorrido;
        public List<Transform> puntosDePatrullaje = new List<Transform>();
        public bool isActive = false; // Indica si el recorrido está activo
    }
public class Patrulla : MonoBehaviour
{
        public List<Recorrido> recorridos = new List<Recorrido>();
        public float tiempoDeVigilancia;
        private int puntoActual = 0; // Índice del punto actual
        private bool direccionAdelante = true; // Para alternar entre adelante y atrás
        private bool patrullando = true; // Para controlar si está patrullando
        private Recorrido recorridoActual; // El recorrido actualmente activo
        Animator animatorWalk;
        NavMeshAgent agent;

        private void Start()
        {
            agent = GetComponent<NavMeshAgent>();
            animatorWalk = GetComponent<Animator>();
            // Encontrar el primer recorrido activo
            SetRecorridoActivo();
        }

        private void SetRecorridoActivo()
        {
            recorridoActual = recorridos.Find(r => r.isActive);
            if (recorridoActual != null)
            {
                StartCoroutine(Patrullar());
            }
        }

        public IEnumerator Patrullar()
        {
            while (patrullando)
            {
                // Verificar si hay un recorrido activo
                if (recorridoActual == null)
                {
                    yield break;
                }

                Movement();

                // Obtener el punto de patrullaje actual
                Transform targetPoint = recorridoActual.puntosDePatrullaje[puntoActual];

                // Establecer el destino del agente
                agent.SetDestination(targetPoint.position);

                // Esperar hasta que el agente llegue al destino
                yield return new WaitUntil(() => !agent.pathPending && agent.remainingDistance <= agent.stoppingDistance);

                // Verificar si el agente ha alcanzado el punto de patrullaje
                if (!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance)
                {
                    Debug.Log("Ya llegó al punto");

                    // Idle();

                    yield return new WaitForSeconds(tiempoDeVigilancia);

                    // Cambiar el índice del punto actual según la dirección
                    if (direccionAdelante)
                    {
                        puntoActual++;
                        if (puntoActual >= recorridoActual.puntosDePatrullaje.Count)
                        {
                            puntoActual = recorridoActual.puntosDePatrullaje.Count - 1;
                            direccionAdelante = false; // Cambiar la dirección
                        }
                    }
                    else
                    {
                        puntoActual--;
                        if (puntoActual < 0)
                        {
                            puntoActual = 0;
                            direccionAdelante = true; // Cambiar la dirección
                        }
                    }
                }
            }
        }

        void Movement()
        {
            animatorWalk.SetBool("IsMoving", true);
        }

        void Idle()
        {
            animatorWalk.SetBool("IsMoving", false);
        }

        // Método para cambiar el estado de actividad de los recorridos
        public void CambiarEstadoRecorridos(string nombreRecorrido)
        {
            // Desactivar todos los recorridos
            foreach (var recorrido in recorridos)
            {
                recorrido.isActive = false;
            }

            // Activar el recorrido con el nombre especificado
            var nuevoRecorrido = recorridos.Find(r => r.nombreRecorrido == nombreRecorrido);
            if (nuevoRecorrido != null)
            {
                nuevoRecorrido.isActive = true;
            }

            // Actualizar el recorrido actual
            SetRecorridoActivo();
        }
    }


