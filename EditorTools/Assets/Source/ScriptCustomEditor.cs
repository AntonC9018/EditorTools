using Unity.VisualScripting;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace EditorTools
{
    [CustomEditor(typeof(Script))]
    public class ScriptCustomEditor : Editor
    {
        Script t => (Script) target;
        
        SerializedProperty GetSerializedProperty(string path) => serializedObject.FindProperty(path);

        public override VisualElement CreateInspectorGUI()
        {
            var customInspector = new VisualElement();
            customInspector.name = "root";
            
            var layout = new VisualElement();
            layout.name = "layout";
            customInspector.Add(layout);

            const string suffix = nameof(t._data) + ".";

            {
                var a = new IntegerField();
                var name = nameof(t._data.thing);
                a.label = name;
                a.name = name;
                a.RegisterValueChangedCallback(context =>
                {
                    var prop = serializedObject.FindPropertyOrFail(suffix + nameof(t._data.thing));
                    prop.intValue = context.newValue;
                    serializedObject.ApplyModifiedProperties();
                    
                    t.OnThingChanged(context.previousValue, context.newValue);
                    t.OnAnyChanged();
                });
                layout.Add(a);
            }

            {
                var a = new TextField();
                var name = nameof(t._data.someName);
                a.label = name;
                a.name = name;
                a.RegisterValueChangedCallback(context =>
                {
                    var prop = serializedObject.FindPropertyOrFail(suffix + nameof(t._data.someName));
                    prop.stringValue = context.newValue;
                    serializedObject.ApplyModifiedProperties();
                    
                    t.OnAnyChanged();
                });
                layout.Add(a);
            }

            {
                var parent_someStuff = new Foldout();
                var name_someStuff = nameof(t._data.someStuff);
                parent_someStuff.name = name_someStuff;
                parent_someStuff.text = name_someStuff;
                
                {
                    var a = new IntegerField();
                    const string path = suffix + nameof(t._data.someStuff) + "." + nameof(t._data.someStuff.nestedThing); 
                    a.name = path;
                    a.label = nameof(t._data.someStuff.nestedThing);
                    a.RegisterValueChangedCallback(context =>
                    {
                        var prop = serializedObject.FindPropertyOrFail(path);
                        prop.intValue = context.newValue;
                        serializedObject.ApplyModifiedProperties();

                        t.OnAnyChanged();
                    });
                    parent_someStuff.Add(a);
                }
                
                layout.Add(parent_someStuff);
            }

            {
                var applyButton = new Button();
                const string text = "Save changes to disk"; 
                applyButton.name = text;
                applyButton.text = text;
                applyButton.clicked += () =>
                {
                    Debug.Log("Saving to disk (fake)");
                };
                layout.Add(applyButton);
            }

            return customInspector;
        }
    }
}