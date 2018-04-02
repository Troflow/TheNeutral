using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Neutral
{

	public enum Lite {
        RED,
        YELLOW,
        GREEN,
        CYAN,
        BLUE,
        MAGENTA,
        GREY,
        BLACK,
        WHITE,
        MIXED
    };

    public enum ColorWheelSystemType {
        Local,
        Global
    };

    public enum EnemyType
    {
        Minor,
        Major,
        Remy,
        Mojo
    };

    public enum BlinkableType
    {
        TriggerableObstacle,
        NonTriggerableObstacle,
        BlinkSplineLine,
        SightSplineLine,
        BlinkPulseBar,
        SightPulseBar,
        Enemy
    };

    public enum BlinkState
    {
        EyesClosed,
        EyesOpen,
    };

    public enum PlayerActionState
    {
        NonActing,
        Dashing,
        CounterState,
        Exploiting,
        Attacking,
    };

    public enum SplineBoxType
    {
        Standard,
        Blockade
    };

    public enum SplineLineType
    {
        Sight,
        Blinkable,
        Persistent
    };

    public enum PulseState
    {
        MovingEyesOpen,
        MovingEyesClosed,
        StaticEyesClosed,
    };
}
