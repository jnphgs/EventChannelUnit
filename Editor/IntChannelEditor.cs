using EventChannelUnit.Runtime;
using UnityEditor;
using UnityEngine.UIElements;

namespace EventChannelUnit.Editor
{
    [CustomEditor(typeof(IntChannel), true)]
    public class IntChannelEditor : ChannelBaseEditor<ChannelBase<int>, int>
    {
        protected override VisualElement CreateTestValueField()
        {
            var integerField = new IntegerField("Test Value")
            {
                value = TestValue
            };
            integerField.RegisterValueChangedCallback((e) =>
            {
                TestValue = e.newValue;
            });
            return integerField;
        }
    }
}
