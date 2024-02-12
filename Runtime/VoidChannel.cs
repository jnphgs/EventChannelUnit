using Unity.VisualScripting;
using UnityEngine;

namespace EventChannelUnit.Runtime
{
    [CreateAssetMenu(menuName = "EventChannelUnit/Void", fileName = "VCh_", order = 7)]
    public class VoidChannel : ChannelBase { }


    [UnitTitle("On Void Channel Event")]
    [UnitCategory("Channel")]
    public class VoidChannelUnit : ChannelEventUnit<VoidChannel> { }

}