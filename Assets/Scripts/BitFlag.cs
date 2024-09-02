using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class BitFlag : MonoBehaviour
{
    [Flags]
    public enum RewardFlags : short
    {
        none = 0,
        bullet = 1 << 0,
        armor = 1 << 1,
        potion = 1 << 2
    }

    [Flags]
    public enum StateFlags : short
    {
        none = 0,
        slow = 1 << 0,
        stun = 1 << 1,
        chaos = 1 << 2,
        angry = 1 << 3,
        bleeding = 1 << 4,
        darkness = 1 << 5,
        cold = 1 << 6,
        hungry = 1 << 7
    }

    [Flags]
    public enum UIStateFlags : short
    {
        none = 0, //0b0000_0000_0000_0000
        uiActivated = 1 << 0, //0b0000_0000_0000_0001
        uiHidden = 1 << 1, //0b0000_0000_0000_0010
        inventoryActivated = 1 << 2, //0b0000_0000_0000_0100
        cookActivated = 1 << 3,
        achActivated = 1 << 4,
        optionActivated = 1 << 5,
    }

}
