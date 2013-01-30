#region usings
using System;
using System.ComponentModel.Composition;

using VVVV.PluginInterfaces.V1;
using VVVV.PluginInterfaces.V2;
using VVVV.Utils.VColor;
using VVVV.Utils.VMath;
using VVVV.PluginInterfaces.V2.EX9;
using VVVV.Utils.SlimDX;
using VVVV.Core.Logging;
using SlimDX;
using SlimDX.Direct3D9;

#endregion usings

namespace VVVV.Nodes.DS325
{
    /// <summary>
    /// Get detected gestures. Only a few basic gestures are supported.
    /// </summary>
    [PluginInfo(Name = "Gesture", Category = "Devices", Help = "Basic template with one value in/out", Tags = "", Version = "DS325", Author = "herbst")]
    public class DS325Gesture : IPluginEvaluate
    {
        [Input("Device Handle", IsSingle = true)]
        public ISpread<DS325Device> FDeviceHandle;

        [Input("Body Label")]
        public ISpread<PXCMGesture.GeoNode.Label> FBodyLabel;

        /*
        [Output("Device Property Value")]
        public ISpread<ISpread<float>> FGeoNode;
        */

        [Output("Time Stamp")]
        public ISpread<double> timeStamp;
        [Output("User")]
        public ISpread<int> user;
        
        
        //[Output("Body Label")]
        public ISpread<PXCMGesture.GeoNode.Label> body = new Spread<PXCMGesture.GeoNode.Label>();
        [Output("Gesture Label")]
        public ISpread<PXCMGesture.Gesture.Label> label;
        
        [Output("Confidence")]
        public ISpread<double> confidence;
        [Output("Active")]
        public ISpread<bool> active;

        Vector3D FlipYZ(Vector3D v)
        {
            return new Vector3D(v.x, v.z, v.y);
        }

        DS325Device device;

        public void Evaluate(int SpreadMax)
        {
            if (FDeviceHandle[0] == null) return;
            device = FDeviceHandle[0];

            timeStamp.SliceCount = SpreadMax;
            user.SliceCount = SpreadMax;
            body.SliceCount = SpreadMax;
            label.SliceCount = SpreadMax;
            confidence.SliceCount = SpreadMax;
            active.SliceCount = SpreadMax;

            // tja, wie hier spreaden?
            // macht eine Auswahl von Features pro Hand Sinn? Eigentlich nicht...

            for (int i = 0; i < FBodyLabel.SliceCount; i++)
            {
                PXCMGesture.Gesture gdata;
                
                // if (PXCUPipeline.QueryGesture(PXCMGesture.GeoNode.Label.LABEL_ANY, out gdata))
                if (device.Pipeline.QueryGesture(FBodyLabel[i], out gdata))
                {
                    // print("geonode left-hand (x=" + nData.positionImage.x + ", y=" + nData.positionImage.y + ")");

                    timeStamp[i] = gdata.timeStamp;
                    user[i] = gdata.user;
                    body[i] = gdata.body;
                    label[i] = gdata.label;
                    confidence[i] = gdata.confidence;
                    active[i] = gdata.active;
                }
            }
        }
    }
}