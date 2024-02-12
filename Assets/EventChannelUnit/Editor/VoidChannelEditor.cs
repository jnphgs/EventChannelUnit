using EventChannelUnit.Runtime;
using UnityEditor;
using UnityEngine.UIElements;

namespace EventChannelUnit.Editor
{
    [CustomEditor(typeof(ChannelBase), true)]
    public class VoidChannelEditor : ChannelBaseEditor
    {
        protected override VisualElement CreateTestValueField()
        {
            return new VisualElement();
        }
    }
}
