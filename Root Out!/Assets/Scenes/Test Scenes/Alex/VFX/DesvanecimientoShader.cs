using System.Collections;
using UnityEngine;

public class DesvanecimientoShader : MonoBehaviour
{
    [SerializeField] private Material explosionRedChibi;

    [SerializeField, Tooltip("Valor inicial de explosion")]
    private float valorDesvanecido;

    [SerializeField] private float velocidadDesvanecimiento;

    // Update is called once per frame
    void Update()
    {
        
    }

    [ContextMenu("Boton de desvanecer")]
    private void EmpezarCorrutina()
    {
        StartCoroutine(DesvanecerExplosion());
    }

    private IEnumerator DesvanecerExplosion()
    {
        float tiempo = 0;
        float nuevoValor = explosionRedChibi.GetFloat("_SmoothStep");

        while (tiempo < 1)
        {
            nuevoValor = Mathf.Lerp(nuevoValor, valorDesvanecido, tiempo);
            explosionRedChibi.SetFloat("SmoothStep", nuevoValor);
            tiempo += Time.deltaTime * velocidadDesvanecimiento;

            yield return null;
        }

        yield return null;
    }
}
