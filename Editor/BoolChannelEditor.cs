using EventChannelUnit.Runtime;
using UnityEditor;
using UnityEngine.UIElements;

namespace EventChannelUnit.Editor
{
    [CustomEditor(typeof(ChannelBase<bool>), true)]
    public class BoolChannelEditor : ChannelBaseEditor<ChannelBase<bool>, bool>
    {
        protected override VisualElement CreateTestValueField()
        {
            var toggle = new Toggle("Test Value")
            {
                value = TestValue
            };
            toggle.RegisterValueChangedCallback((e) =>
            {
                TestValue = e.newValue;
            });
            return toggle;
        }
    }
}
