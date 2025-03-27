using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


namespace Weapons
{
    public class WeaponsBase : MonoBehaviour
    {
        public enum WeaponType
        {
            SpreadShot,
            Automatic,
            BurstFire,
            SingleShot
        }

        #region Weapons variables

        [Header("Tipo de apuntado")]
        [SerializeField] private PlayerAimState tipoDeApuntado;

        [Header("Punto de Mira")]
        [SerializeField] public Transform aiming; // Punto de Mira

        [Header("Tipo de Arma")]
        [SerializeField] public WeaponType weaponType; // Tipo de Arma

        [Header("Dispersión de Disparo")]
        [SerializeField] protected float spreadAngle = 0f; // Ángulo de dispersión
        [SerializeField] protected bool horizontalSpread = false; // Indica si la dispersión es solo horizontal

        [Header("Opciones de Disparo")]
        [SerializeField] private bool autoFire = false; // Indica si el arma dispara automáticamente
        [SerializeField] private bool shootUpwards = false; // Indica si el arma dispara hacia arriba
        [SerializeField] private bool shootDownwards = false; // Indica si el arma dispara hacia abajo

        [Header("Tipo de Bala")]
        [SerializeField] protected GameObject bulletPrefab; // Prefab de la bala

        [Header("Munición")]
        [SerializeField] public int currentAmmo; // Munición actual
        [SerializeField] public int maxAmmo; // Capacidad máxima de munición
        [SerializeField] public int bulletReserve; // Reserva de balas
        [SerializeField] public int maxBulletReserve; // necesito esta variable para que el upgrade haga effecto correctamente

        [Header("Estadísticas")]
        [SerializeField] public float damage; // Daño 
        [SerializeField] protected float range; // Alcance 
        [SerializeField] protected float fireRate; // Cadencia de disparo
        [SerializeField] public float reloadTime; // Recargar
        [SerializeField] protected float bulletForce = 20f; // Fuerza con la que se dispara la bala
        [SerializeField] protected float lifeTimeBullets; // Tiempo de vida de la bala antes de desaparecer

        [Header("Burst fire options")]
        [SerializeField] protected int bulletsPerBurst = 3; // Número de balas por ráfaga
        [SerializeField] protected float burstRate = 0.1f; // Tiempo entre disparos en una ráfaga
        [SerializeField] protected float burstDistance = 0.1f; // Distancia entre balas en una ráfaga
        [SerializeField] protected float burstPause = 0.5f;
        protected float nextTimeToFire = 0f;  // Tiempo entre disparos

        #endregion

        #region Explosive Bullets Boolean

        [HideInInspector] public bool canInstantiateExplosion = true; // Controla si se puede instanciar la explosión
        [HideInInspector] public bool explosionUpgradeActivated = false; // Controla si la mejora de explosión ha sido activada

        #endregion

        #region References

        protected Image canvasRecargaImage;
        protected Animation animacionRecarga;
        protected TextMeshProUGUI bulletText;

        #endregion

        #region Start & Update
        protected virtual void Start()
        {
            currentAmmo = maxAmmo;  // Inicializar la munición actual al valor máximo permitido
            bulletReserve = maxBulletReserve;

            // Buscar y desactivar el componente Image de canvasRecarga al inicio
            GameObject canvasRecarga = GameObject.Find("Recarga");
            if (canvasRecarga != null)
            {
                canvasRecargaImage = canvasRecarga.GetComponent<Image>();
                animacionRecarga = canvasRecarga.GetComponent<Animation>();
                if (canvasRecargaImage != null)
                {
                    canvasRecargaImage.enabled = false;
                }
            }
        }

        protected virtual void Update()
        {
            FireNReload(); // Método que controla el disparo y la recarga
            UpdateAmmoText(); // Actualiza el texto de munición
        }

        #endregion

        #region Aim State
        protected void SetNewAimState() // Establece el nuevo estado de apuntado
        {
            var playerMovement = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>();

            playerMovement.SetAnimationState(tipoDeApuntado); // Llama al método SetAnimationState en el script PlayerMovement
        }
        #endregion

        #region Fire & Reload
        void FireNReload()
        {
            if (autoFire && CanShoot())
            {
                nextTimeToFire = Time.time + 1f / fireRate; // Calcula el tiempo hasta el próximo disparo permitido
                Shoot(); // Llama al método Shoot para disparar
            }
            else if (weaponType == WeaponType.Automatic)
            {
                // Detecta si se mantiene presionado el botón y si es posible disparar
                if (Input.GetKey(KeyCode.Mouse0) && CanShoot())
                {
                    nextTimeToFire = Time.time + 1f / fireRate; // Calcula el tiempo hasta el próximo disparo permitido
                    Shoot(); // Llama al método Shoot para disparar
                }
            }
            else if (weaponType == WeaponType.BurstFire)
            {
                // Detecta si se presiona el botón y si es posible disparar en ráfagas
                if (Input.GetKey(KeyCode.Mouse0) && CanShoot())
                {
                    Shoot();
                }
            }
            else
            {
                // Detecta si se presiona el botón y si es posible disparar
                if (Input.GetKey(KeyCode.Mouse0) && CanShoot())
                {
                    nextTimeToFire = Time.time + 1f / fireRate; // Calcula el tiempo hasta el próximo disparo permitido
                    Shoot(); // Llama al método Shoot para disparar
                }

                else if (Input.GetKeyDown(KeyCode.Mouse0) && !CanShoot())
                {
                    Debug.Log("No ammo left!"); // Mensaje de depuración si no se puede disparar
                }
            }
            Reload();
        }
        #endregion

        #region Logic Reload
        protected virtual IEnumerator ReloadCoroutine()  // Corrutina que maneja la lógica de recarga
        {
            if (canvasRecargaImage != null)
            {
                canvasRecargaImage.enabled = true; // Activar la imagen de recarga
            }

            if (animacionRecarga != null)
            {
                animacionRecarga.Play(); // Reproducir la animación de recarga
            }

            yield return new WaitForSeconds(reloadTime); // Espera el tiempo de recarga

            if (animacionRecarga != null)
            {
                animacionRecarga.Stop(); // Detener la animación de recarga
            }
            if (canvasRecargaImage != null)
            {
                canvasRecargaImage.enabled = false; // Desactivar la imagen de recarga
            }

            // Calcula cuántas balas se necesitan para recargar completamente
            int bulletsToReload = maxAmmo - currentAmmo;

            if (bulletReserve >= bulletsToReload)
            {
                bulletReserve -= bulletsToReload; // Reduce la reserva de balas
                currentAmmo = maxAmmo; // Rellena la munición actual al máximo
            }
            else
            {
                currentAmmo += bulletReserve; // Añade las balas restantes de la reserva a la munición actual
                bulletReserve = 0; // Agota la reserva de balas
            }

            Debug.Log("Reloaded. Ammo: " + currentAmmo + ", Bullets in Reserve: " + bulletReserve);
        }

        protected virtual void Reload() // Método que maneja la recarga
        {
            // Detecta si se presiona la tecla de recarga (R)
            if (Input.GetKeyDown(KeyCode.R))
            {
                if (canvasRecargaImage != null) // Comprueba si la imagen de recarga no es nula
                {
                    canvasRecargaImage.enabled = true; // Activar la imagen de recarga
                }

                if (bulletReserve > 0)
                {
                    StartCoroutine(ReloadCoroutine()); // Inicia la recarga si hay balas en la reserva
                }
                else
                {
                    Debug.Log("No bullets left in reserve!"); // Mensaje de depuración si no hay balas en la reserva
                }
            }
        }
        #endregion

        #region Logic Fire
        protected virtual void Shoot()
        {
            int numberBullets = weaponType == WeaponType.BurstFire ? bulletsPerBurst : 1; // Obtener el número de balas a disparar
            UseAmmo(numberBullets); // Reduce la munición actual
            FireBullet(numberBullets); // Dispara una bala (instancia el prefab)
        }
        protected bool CanShoot() // Método que verifica si es posible disparar
        {
            // Comprueba que haya pasado suficiente tiempo desde el último disparo y que haya munición disponible
            return Time.time >= nextTimeToFire && currentAmmo > 0;
        }
        protected void UseAmmo(int numberBullets)  // Método que reduce la munición actual al disparar
        {
            currentAmmo -= numberBullets;
        }
        protected virtual void FireBullet(int numberBullets, int burstIndex = 0)   // Método que dispara una bala (instancia el prefab)
        {
            if (bulletPrefab != null)
            {
                for (int i = 0; i < numberBullets; i++)
                {
                    Vector3 direction = weaponType == WeaponType.SpreadShot ? GetSpreadDirection(aiming.forward) : GetNonSpreadDirection(aiming.forward, i, numberBullets);

                    // Ajustar la dirección de disparo hacia arriba o hacia abajo
                    if (shootUpwards)
                    {
                        direction = Vector3.up;
                    }
                    else if (shootDownwards)
                    {
                        direction = Vector3.down;
                    }

                    Vector3 positionOffset = weaponType == WeaponType.BurstFire ? aiming.forward * burstDistance * burstIndex : Vector3.zero;
                    GameObject bullet = Instantiate(bulletPrefab, aiming.position + positionOffset, Quaternion.LookRotation(direction));
                    Rigidbody rb = bullet.GetComponent<Rigidbody>();
                    rb.AddForce(direction * bulletForce, ForceMode.Impulse);

                    // Verifica si el prefab de la bala tiene el componente IBullet
                    if (bullet.TryGetComponent<IBullet>(out var bulletComponent))
                    {
                        bulletComponent.SetDamage(damage); // Establece el daño de la bala
                    }
                    else
                    {
                        Debug.LogWarning("Bullet prefab does not have BulletDamage component.");
                    }

                    Destroy(bullet, lifeTimeBullets); // Destruye la bala después de que expire el tiempo de vida especificado.
                }
            }
            else
            {
                Debug.LogWarning("Bullet prefab not assigned!");
            }
        }
        #endregion

        #region Spread & NonSpread Direction
        private Vector3 GetSpreadDirection(Vector3 baseDirection) // Método para obtener una dirección con dispersión
        {
            float spreadRadius = Mathf.Tan(spreadAngle * Mathf.Deg2Rad); // Convertir ángulo a radianes y calcular el radio de dispersión
            Vector2 randomPoint = Random.insideUnitCircle * spreadRadius; // Generar un punto aleatorio dentro del círculo de dispersión

            Vector3 spread;
            if (horizontalSpread)
            {
                spread = new Vector3(randomPoint.x, 0, randomPoint.y); // Dispersión solo en horizontal
            }
            else
            {
                spread = new Vector3(randomPoint.x, randomPoint.y, 0); // Dispersión en todas las direcciones
            }

            return (baseDirection + spread).normalized; // Ajustar la dirección base con la dispersión
        }

        // Método para obtener una dirección sin dispersión
        private Vector3 GetNonSpreadDirection(Vector3 baseDirection, int bulletIndex, int totalBullets)
        {
            if (totalBullets == 1)
            {
                return baseDirection; // Si solo hay una bala, dispara en la dirección base
            }

            float angleStep = 360f / totalBullets; // Calcula el ángulo entre cada bala
            float angle = bulletIndex * angleStep; // Calcula el ángulo para la bala actual
            Quaternion rotation = Quaternion.Euler(0, angle, 0); // Crea una rotación basada en el ángulo
            return rotation * baseDirection; // Aplica la rotación a la dirección base
        }
        #endregion

        #region FireBurst
        public IEnumerator FireBurst()
        {
            for (int i = 0; i < bulletsPerBurst; i++)
            {
                if (CanShoot())
                {
                    FireBullet(1, i); // Dispara una bala por ráfaga
                    UseAmmo(1); // Reduce la munición actual
                    yield return new WaitForSeconds(burstRate); // Espera el tiempo entre disparos en una ráfaga
                }
                else
                {
                    break;
                }
            }
            nextTimeToFire = Time.time + burstPause; // Añade una pausa después de la ráfaga
        }
        protected virtual int GetNumBullets() // Método virtual para obtener el número de balas, será sobrescrito en las clases derivadas
        {
            return weaponType == WeaponType.BurstFire ? bulletsPerBurst : 1; // Valor predeterminado
        }
        #endregion

        #region Ammo Text
        protected virtual void UpdateAmmoText() // Actualiza el texto de munición
        {
            if (bulletText != null)
            {
                bulletText.text = $"{currentAmmo} / {bulletReserve}"; // Actualiza el texto con la munición actual y máxima
            }
        }

        protected virtual void ActivateBulletText()
        {
            if (bulletText == null)
            {
                GameObject bulletTextObject = GameObject.Find("Ammo Text");
                if (bulletTextObject != null)
                {
                    bulletText = bulletTextObject.GetComponent<TextMeshProUGUI>();
                }
            }

            if (bulletText != null)
            {
                bulletText.gameObject.SetActive(true); // Activar el texto de munición
                UpdateAmmoText(); // Actualizar el texto de munición
            }
        }
        #endregion

        #region Gizmos
        protected void OnDrawGizmos()
        {
            Gizmos.color = Color.red; // Establece el color de los Gizmos a rojo.
            Gizmos.DrawRay(aiming.position, aiming.forward * range); // Dibuja una línea de Gizmos desde el punto de mira en la dirección del frente hasta el rango especificado.

            // Dibujar el cono de visión
            if (spreadAngle > 0) // Comprueba si el ángulo de dispersión es mayor que 0.
            {
                float halfSpread = spreadAngle / 2; // Calcula la mitad del ángulo de dispersión para los límites del cono.
                Vector3 leftBoundary = Quaternion.Euler(0, -halfSpread, 0) * aiming.forward * range; // Calcula el límite izquierdo del cono de visión.
                Vector3 rightBoundary = Quaternion.Euler(0, halfSpread, 0) * aiming.forward * range; // Calcula el límite derecho del cono de visión.
                Gizmos.color = Color.yellow;
                Gizmos.DrawRay(aiming.position, leftBoundary); // Dibuja una línea de Gizmos para el límite izquierdo del cono de visión.
                Gizmos.DrawRay(aiming.position, rightBoundary); // Dibuja una línea de Gizmos para el límite derecho del cono de visión.

                // Dibujar líneas adicionales para una mejor visualización
                int segments = 10; // Define el número de segmentos (líneas adicionales) dentro del cono de visión.
                for (int i = 1; i < segments; i++) // Itera a través de los segmentos.
                {
                    float angle = -halfSpread + (i * (spreadAngle / segments)); // Calcula el ángulo para cada segmento.
                    Vector3 direction = Quaternion.Euler(0, angle, 0) * aiming.forward * range; // Calcula la dirección para cada segmento.
                    Gizmos.DrawRay(aiming.position, direction); // Dibuja una línea de Gizmos para cada segmento dentro del cono de visión.
                }
            }
        }
        #endregion
    }
}
