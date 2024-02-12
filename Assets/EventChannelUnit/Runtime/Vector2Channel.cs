using Unity.VisualScripting;
using UnityEngine;

namespace EventChannelUnit.Runtime
{

    [CreateAssetMenu(menuName = "EventChannelUnit/Vector2", fileName = "Vec2Ch_", order = 5)]
    public class Vector2Channel : ChannelBase<Vector2> { }

    [UnitTitle("On Vector2 Channel Event")]
    [UnitCategory("Channel")]
    public class Vector2ChannelUnit : ChannelEventUnit<Vector2Channel, Vector2> { }
    
}