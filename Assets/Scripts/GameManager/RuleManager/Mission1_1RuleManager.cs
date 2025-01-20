using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mission1_1RuleManager : RuleManager
{
    void Wave1Ended()
    {
        Blockades[0].enabled = false;
    }

    void Trigger1Triggered()
    {
        Blockades[0].enabled = true;
        Waves[1].SendMessage("WaveStart");
    }

    void Wave2Ended()
    {
        Blockades[1].enabled = false;
    }
}
