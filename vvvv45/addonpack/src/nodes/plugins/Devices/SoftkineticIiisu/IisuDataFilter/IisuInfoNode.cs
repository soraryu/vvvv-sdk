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
    [PluginInfo(Name = "Information", Category = "Device", Help = "Basic template with one value in/out", Tags = "", Version = "SoftkineticIisu")]
    public class IisuInfoNode : IPluginEvaluate
    {
        #region fields & pins
        [Input("Device", IsSingle = true)]
        IDiffSpread<IDevice> FDevice;

        [Input("IisuData", IsSingle = true)]
        IDiffSpread<IisuData> FData;

        [Output("Parameter Names")]
        ISpread<string> FParameterNames;

        [Output("Data Names")]
        ISpread<string> FDataNames;

        [Output("Accelerometer Values")]
        ISpread<Vector3D> FValues;

        /*
        [Output("Event Names")]
        ISpread<string> FEventNames;

        [Output("Command Names")]
        ISpread<string> FCommandNames;
        */

        [Import()]
        ILogger FLogger;
        #endregion fields & pins

        //private HandIisuInput handInput = null;


        //called when data for any output pin is requested
        public void Evaluate(int SpreadMax)
        {
            if (FDevice == null || FDevice[0] == null || FData[0] == null) return;

            var device = FDevice[0];
            var data = FData[0];
            FParameterNames.AssignFrom(device.GetParametersNameCollection());
            FDataNames.AssignFrom(device.GetDataNameCollection(false));

            FValues.SliceCount = 1;
            FValues[0] = data.SourceAcceleration.ToVector3();
        }
    }
}
