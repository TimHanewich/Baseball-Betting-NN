using System;

namespace ESPN
{
    public class StateProbabilityPair
    {
        public float[] State {get; set;}
        public float Probability {get; set;}

        public StateProbabilityPair()
        {
            State = new float[]{};
        }
    }
}