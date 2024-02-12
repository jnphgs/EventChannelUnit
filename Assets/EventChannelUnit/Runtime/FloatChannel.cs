using Unity.VisualScripting;
using UnityEngine;

namespace EventChannelUnit.Runtime
{

    [CreateAssetMenu(menuName = "EventChannelUnit/Float", fileName = "FCh_", order = 2)]
    public class FloatChannel : ChannelBase<float> { }

    [UnitTitle("On Float Channel Event")]
    [UnitCategory("Channel")]
    public class FloatChannelUnit : ChannelEventUnit<FloatChannel, float> { }
    
}