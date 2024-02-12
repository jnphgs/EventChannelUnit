using Unity.VisualScripting;
using UnityEngine;

namespace EventChannelUnit.Runtime
{

    [CreateAssetMenu(menuName = "EventChannelUnit/String", fileName = "SCh_", order = 4)]
    public class StringChannel : ChannelBase<string> { }

    [UnitTitle("On String Channel Event")]
    [UnitCategory("Channel")]
    public class StringChannelUnit : ChannelEventUnit<StringChannel, string> { }
    
}