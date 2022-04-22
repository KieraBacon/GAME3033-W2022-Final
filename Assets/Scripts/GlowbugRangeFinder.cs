using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class GlowbugRangeFinder : MonoBehaviour
{
    public delegate void GlowbugRangeFinderEventHandler(Glowbug glowbug);
    public event GlowbugRangeFinderEventHandler onGlowbugEnteredRange;
    public event GlowbugRangeFinderEventHandler onGlowbugLeftRange;
    public HashSet<Glowbug> glowbugsInRange = new HashSet<Glowbug>();

    private void OnTriggerEnter(Collider other)
    {
        Glowbug glowbug = other.GetComponent<Glowbug>();
        if (glowbug)
        {
            glowbugsInRange.Add(glowbug);
            onGlowbugEnteredRange?.Invoke(glowbug);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        Glowbug glowbug = other.GetComponent<Glowbug>();
        if (glowbug)
        {
            glowbugsInRange.Remove(glowbug);
            onGlowbugLeftRange?.Invoke(glowbug);
        }
    }
}
