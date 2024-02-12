using EventChannelUnit.Runtime;
using UnityEditor;
using UnityEngine.UIElements;

namespace EventChannelUnit.Editor
{
    [CustomEditor(typeof(FloatChannel), true)]
    public class FloatChannelEditor : ChannelBaseEditor<ChannelBase<float>, float>
    {
        protected override VisualElement CreateTestValueField()
        {
            var floatField = new FloatField("Test Value")
            {
                value = TestValue
            };
            floatField.RegisterValueChangedCallback((e) =>
            {
                TestValue = e.newValue;
            });
            return floatField;
        }
    }
}
