using UnityEngine;
using UnityEditor;
using System.Reflection;

namespace Editor.ButtonMethod
{
    [CustomEditor(typeof(MonoBehaviour), true)]
    public class ButtonMethodEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            // Draw normal inspector
            DrawDefaultInspector();

            // Find all methods with [Button]
            var methods = target.GetType()
                .GetMethods(BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);

            foreach (var method in methods)
            {
                var buttonAttribute = method.GetCustomAttribute<ButtonAttribute>();
                if (buttonAttribute != null)
                {
                    string label = string.IsNullOrEmpty(buttonAttribute.Label) ? method.Name : buttonAttribute.Label;
                    if (GUILayout.Button(label))
                    {
                        method.Invoke(target, null);
                    }
                }
            }
        }
    }
}
