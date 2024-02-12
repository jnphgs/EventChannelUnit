using Unity.VisualScripting;
using UnityEngine;

namespace EventChannelUnit.Runtime
{

    [CreateAssetMenu(menuName = "EventChannelUnit/Bool", fileName = "BCh_", order = 1)]
    public class BoolChannel : ChannelBase<bool> { }

    [UnitTitle("On Bool Channel Event")]
    [UnitCategory("Channel")]
    public class BoolChannelUnit : ChannelEventUnit<BoolChannel, bool> { }
    
}