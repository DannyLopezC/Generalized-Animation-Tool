using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(GeneralizedAnimation))]
public class MyClassDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        // Aquí dibuja la interfaz para seleccionar un objeto de la clase MyClass
        // y agregarlo a la lista objectsToShow de MyTimelineWindow
    }
}