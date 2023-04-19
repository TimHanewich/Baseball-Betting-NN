using System;

namespace ESPN
{
    public class StatePredictionPair
    {
        public float[] State {get; set;}
        public float[] Prediction {get; set;}

        public StatePredictionPair()
        {
            State = new float[]{};
            Prediction = new float[]{};
        }
    }
}