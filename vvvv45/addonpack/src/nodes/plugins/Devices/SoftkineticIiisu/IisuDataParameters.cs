using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VVVV.Nodes
{
    public class IisuParameters
    {

        // ReadOnly params
        private float frameRate;
        private string camName;
        private string camModel;


        // maybe dont need these ones when using CI mode
        private float depthHFOV;
        private float depthVFOV;
        private int depthWidth;
        private int depthHeight;

        private float confidenceHFOV;
        private float confidenceVFOV;
        private int confidenceWidth;
        private int confidenceHeight;

        private float colorHFOV;
        private float colorVFOV;
        private int colorWidth;
        private int colorHeight;



        // ReadWrite params

        // DS specific
        private int dsAmplitudeThreshold;
        private int dsLightIntensity;


        // CI specific
        private bool ciBig5ActivationEnabled;
        private float ciActivationMaxDistance;
        private bool ciStabilizationEnabled;
        private float ciStabilizationStrength;

    }
}
