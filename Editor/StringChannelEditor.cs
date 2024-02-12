using EventChannelUnit.Runtime;
using UnityEditor;
using UnityEngine.UIElements;

namespace EventChannelUnit.Editor
{
    [CustomEditor(typeof(StringChannel), true)]
    public class StringChannelEditor : ChannelBaseEditor<ChannelBase<string>, string>
    {
        protected override VisualElement CreateTestValueField()
        {
            var textField = new TextField("Test Value")
            {
                value = TestValue
            };
            textField.RegisterValueChangedCallback((e) =>
            {
                TestValue = e.newValue;
            });
            return textField;
        }
    }
}
