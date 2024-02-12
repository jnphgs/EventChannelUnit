using Unity.VisualScripting;
using UnityEngine;

namespace EventChannelUnit.Runtime
{

    [CreateAssetMenu(menuName = "EventChannelUnit/Vector3", fileName = "Vec3Ch_", order = 6)]
    public class Vector3Channel : ChannelBase<Vector3> { }

    [UnitTitle("On Vector3 Channel Event")]
    [UnitCategory("Channel")]
    public class Vector3ChannelUnit : ChannelEventUnit<Vector3Channel, Vector3> { }
    
}