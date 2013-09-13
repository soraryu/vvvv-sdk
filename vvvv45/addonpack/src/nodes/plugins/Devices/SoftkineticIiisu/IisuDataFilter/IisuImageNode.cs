#region usings
using System;
using System.ComponentModel.Composition;

using VVVV.PluginInterfaces.V1;
using VVVV.PluginInterfaces.V2;
using VVVV.Utils.VColor;
using VVVV.Utils.VMath;

using VVVV.Core.Logging;

using Iisu;

#endregion usings

namespace VVVV.Nodes
{
    [PluginInfo(Name = "Image", Category = "Device", Help = "Basic template with one value in/out", Tags = "", Version = "SoftkineticIisu")]
    public class IisuImageNode : IPluginEvaluate
    {
        #region fields & pins
        [Input("IisuData", IsSingle = true)]
        IDiffSpread<IisuData> FData;

        [Output("Depth Image Raw")]
        ISpread<double> FDepthImageRaw = new Spread<double>();

        [Output("ID Image Raw")]
        ISpread<double> FIdImageRaw = new Spread<double>();

        [Import()]
        ILogger FLogger;
        #endregion fields & pins

        //private HandIisuInput handInput = null;
        private double[] ImageRaw = new double[0];
        private double[] IdImageRaw = new double[0];

        //called when data for any output pin is requested
        public void Evaluate(int SpreadMax)
        {
            
            if (FData[0] != null)
            {
                IisuData data = FData[0];

                if (ImageRaw == null)
                    ImageRaw = new double[100];


                data.CiDepthImage.ToDepthValues(ref ImageRaw);
                // data.CiSceneLabelImage.ToDepthValues(ref IdImageRaw);
                FDepthImageRaw.AssignFrom(ImageRaw);
                // FIdImageRaw.AssignFrom(IdImageRaw);
            }
        }
    }
}
