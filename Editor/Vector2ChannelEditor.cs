using EventChannelUnit.Runtime;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace EventChannelUnit.Editor
{
    [CustomEditor(typeof(Vector2Channel), true)]
    public class Vector2ChannelEditor : ChannelBaseEditor<ChannelBase<Vector2>, Vector2>
    {
        protected override VisualElement CreateTestValueField()
        {
            var vector2Field = new Vector2Field("Test Value")
            {
                value = TestValue
            };
            vector2Field.RegisterValueChangedCallback((e) =>
            {
                TestValue = e.newValue;
            });
            return vector2Field;
        }
    }
}
