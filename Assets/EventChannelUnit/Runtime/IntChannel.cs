using Unity.VisualScripting;
using UnityEngine;

namespace EventChannelUnit.Runtime
{

    [CreateAssetMenu(menuName = "EventChannelUnit/Int", fileName = "ICh_", order = 3)]
    public class IntChannel : ChannelBase<int> { }

    [UnitTitle("On Int Channel Event")]
    [UnitCategory("Channel")]
    public class IntChannelUnit : ChannelEventUnit<IntChannel, int> { }
    
}