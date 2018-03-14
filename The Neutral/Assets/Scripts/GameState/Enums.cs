using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Neutral
{

	public enum Lite {
        RED,
        BLUE,
        YELLOW,
        GREEN,
        BROWN,
        GOLD,
        BLACK,
        WHITE,
        GRAY,
        PURPLE,
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
        Triggerable,
        NonTriggerable,
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
    }

}
