using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlowbugChain
{
    private HashSet<Glowbug> glowbugs = new HashSet<Glowbug>();
    private HashSet<Glowbug> glowbugsInRange = new HashSet<Glowbug>();

    public void Add(Glowbug glowbug)
    {
        glowbugs.Add(glowbug);
        glowbug.rangeFinder.onGlowbugEnteredRange += RangeFinder_onGlowbugEnteredRange;
        glowbug.rangeFinder.onGlowbugLeftRange += RangeFinder_onGlowbugLeftRange;
    }

    public void Remove(Glowbug glowbug)
    {
        glowbugs.Remove(glowbug);
        glowbug.rangeFinder.onGlowbugEnteredRange -= RangeFinder_onGlowbugEnteredRange;
        glowbug.rangeFinder.onGlowbugLeftRange -= RangeFinder_onGlowbugLeftRange;
    }

    public void Clear()
    {
        foreach (Glowbug glowbug in glowbugs)
        {
            Remove(glowbug);
        }
    }

    private void RangeFinder_onGlowbugEnteredRange(Glowbug glowbug)
    {
        glowbugsInRange.Add(glowbug);
    }

    private void RangeFinder_onGlowbugLeftRange(Glowbug glowbug)
    {
        glowbugsInRange.Remove(glowbug);
    }

    public Glowbug GetLastToFlash(Glowbug exclude)
    {
        Glowbug lastToFlash = null;
        float lastTime = 0;
        HashSet<Glowbug> glowbugsInRangeOfChain = new HashSet<Glowbug>();

        foreach (Glowbug glowbug in glowbugsInRange)
        {
            if (glowbug == exclude) continue;

            float time = glowbug.lastFlashTime;
            if (!lastToFlash || time > lastTime)
            {
                lastToFlash = glowbug;
                lastTime = time;
            }
        }

        return lastToFlash;
    }
}
