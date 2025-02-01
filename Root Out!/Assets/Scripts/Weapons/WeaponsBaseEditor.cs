using UnityEditor;
using Weapons;

// Esta línea indica que esta clase es un editor personalizado para la clase WeaponsBase.
[CustomEditor(typeof(WeaponsBase))]
public class WeaponsBaseEditor : Editor
{
    // Este método sobrescribe el método OnInspectorGUI de la clase base Editor.
    // Se llama cada vez que se dibuja el inspector para el objeto WeaponsBase.
    public override void OnInspectorGUI()
    {
        // Obtiene una referencia al objeto WeaponsBase que se está inspeccionando.
        WeaponsBase weapon = (WeaponsBase)target;

        // Actualiza el objeto serializado
        serializedObject.Update();

        // Dibuja un menú desplegable en el inspector para seleccionar el tipo de arma.
        // El valor seleccionado se asigna a la propiedad weaponType del objeto WeaponsBase.
        weapon.weaponType = (WeaponsBase.WeaponType)EditorGUILayout.EnumPopup("Tipo de Arma", weapon.weaponType);

        // Dibuja un campo en el inspector para la propiedad aiming del objeto WeaponsBase.
        EditorGUILayout.PropertyField(serializedObject.FindProperty("aiming"));

        // Dependiendo del tipo de arma seleccionado, se dibujan diferentes campos en el inspector.
        switch (weapon.weaponType)
        {
            case WeaponsBase.WeaponType.SpreadShot:
                // Si el tipo de arma es SpreadShot, se dibujan los campos spreadAngle y horizontalSpread.
                EditorGUILayout.PropertyField(serializedObject.FindProperty("spreadAngle"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("horizontalSpread"));
                break;

            case WeaponsBase.WeaponType.BurstFire:
                // Si el tipo de arma es BurstFire, se dibujan los campos bulletsPerBurst, burstRate, burstDistance y burstPause.
                EditorGUILayout.PropertyField(serializedObject.FindProperty("bulletsPerBurst"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("burstRate"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("burstDistance"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("burstPause"));
                break;
        }

        // Dibuja campos en el inspector para las propiedades comunes a todos los tipos de armas.
        EditorGUILayout.PropertyField(serializedObject.FindProperty("autoFire"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("automaticShoot"));

        EditorGUILayout.PropertyField(serializedObject.FindProperty("bulletPrefab"));

        EditorGUILayout.PropertyField(serializedObject.FindProperty("currentAmmo"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("bulletReserve"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("maxAmmo"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("damage"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("range"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("reloadTime"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("bulletForce"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("lifeTimeBullets"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("fireRate"));


        EditorGUILayout.PropertyField(serializedObject.FindProperty("aimAssist"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("aimAssistTag"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("aimAssistStrength"));

        // Aplica las modificaciones realizadas a las propiedades serializadas.
        serializedObject.ApplyModifiedProperties();
    }
}
