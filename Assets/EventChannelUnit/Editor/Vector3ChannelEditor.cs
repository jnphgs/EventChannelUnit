using EventChannelUnit.Runtime;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace EventChannelUnit.Editor
{
    [CustomEditor(typeof(Vector3Channel), true)]
    public class Vector3ChannelEditor : ChannelBaseEditor<ChannelBase<Vector3>, Vector3>
    {
        protected override VisualElement CreateTestValueField()
        {
            var vector3Field = new Vector3Field("Test Value")
            {
                value = TestValue
            };
            vector3Field.RegisterValueChangedCallback((e) =>
            {
                TestValue = e.newValue;
            });
            return vector3Field;
        }
    }
}