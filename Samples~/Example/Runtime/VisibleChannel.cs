using EventChannelUnit.Runtime;
using Unity.VisualScripting;
using UnityEngine;

namespace EventChannelUnit.Samples.Example.Runtime
{

    [CreateAssetMenu(menuName = "EventChannelUnit/Custom/Visible", fileName = "ViCh_", order = 0)]
    public class VisibleChannel : BoolChannel
    {
        public int index = 0;
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="value">visibility</param>
        public override void RaiseEvent(bool value)
        {
            base.RaiseEvent(value);
        }
    }

    [UnitTitle("On Visible Channel Event")]
    [UnitCategory("Channel")]
    public class VisibleChannelUnit : ChannelEventUnit<VisibleChannel, bool>
    {
        protected override string ValueOutputLabel => "Visible";
    }
}