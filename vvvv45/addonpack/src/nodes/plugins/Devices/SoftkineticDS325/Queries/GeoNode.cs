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
    /// Queries so-called GeoNodes from the camera, which are labeled body parts with coordinates.
    /// </summary>
    [PluginInfo(Name = "GeoNode", Category = "Devices", Help = "Basic template with one value in/out", Tags = "", Version = "DS325", Author = "herbst")]
    public class DS325GeoNode : IPluginEvaluate
    {
        [Input("Device Handle", IsSingle = true)]
        public ISpread<DS325Device> FDeviceHandle;
        
        [Input("Hand")]
        public ISpread<PXCMGesture.GeoNode.Label> FHandLabel;
        
        [Input("Device Property")]
        public ISpread<ISpread<PXCMGesture.GeoNode.Label>> FGeoNodeLabel;

        /*
        [Output("Device Property Value")]
        public ISpread<ISpread<float>> FGeoNode;
        */

        [Output("Found")]
        public ISpread<bool> found;
        [Output("Time Stamp")]
        public ISpread<double> timeStamp;
        [Output("User")]
        public ISpread<Int32> user;
        //[Output("Label")]
        public ISpread<PXCMGesture.GeoNode.Label> body = new Spread<PXCMGesture.GeoNode.Label>();
        [Output("Side")]
        public ISpread<PXCMGesture.GeoNode.Side> side;
        [Output("Confidence")]
        public ISpread<UInt32> confidence;
        [Output("Position World")]
        public ISpread<Vector3D> positionWorld;
        [Output("Position Image")]
        public ISpread<Vector3D> positionImage;
        [Output("Radius World")]
        public ISpread<float> radiusWorld;
        [Output("Radius Image")]
        public ISpread<float> radiusImage;
        [Output("Mass Center World")]
        public ISpread<Vector3D> massCenterWorld;
        [Output("Mass Center Image")]
        public ISpread<Vector3D> massCenterImage;
        [Output("Normal")]
        public ISpread<Vector3D> normal;
        [Output("Openness")]
        public ISpread<UInt32> openness;
        [Output("Openness State")]
        public ISpread<PXCMGesture.GeoNode.Openness> opennessState;

        Vector3D FlipYZ(Vector3D v)
        {
            return new Vector3D(v.x, v.z, v.y);
        }

        DS325Device device;

        public void Evaluate(int SpreadMax)
        {
            if (FDeviceHandle[0] == null) return;
            device = FDeviceHandle[0];

            //SpreadMax = FHandLabel.SliceCount * FGeoNodeLabel.SliceCount;

            // bin size -1: alles
            // bin size 0: nichts
            // bin size 1: 1 für jeden selector
            // bin size n: n für jeden selector

            SpreadMax = 0;
            for (int i = 0; i < FHandLabel.SliceCount; i++)
            {
                SpreadMax += FGeoNodeLabel[i].SliceCount;
            }

            found.SliceCount = SpreadMax;
            timeStamp.SliceCount = SpreadMax;
            user.SliceCount = SpreadMax;
            body.SliceCount = SpreadMax;
            side.SliceCount = SpreadMax;
            confidence.SliceCount = SpreadMax;
            positionWorld.SliceCount = SpreadMax;
            positionImage.SliceCount = SpreadMax;
            radiusWorld.SliceCount = SpreadMax;
            radiusImage.SliceCount = SpreadMax;
            massCenterWorld.SliceCount = SpreadMax;
            massCenterImage.SliceCount = SpreadMax;
            normal.SliceCount = SpreadMax;
            openness.SliceCount = SpreadMax;
            opennessState.SliceCount = SpreadMax;
            
            // tja, wie hier spreaden?
            // macht eine Auswahl von Features pro Hand Sinn? Eigentlich nicht...

            for (int j = 0; j < FHandLabel.SliceCount; j++)
            {
                PXCMGesture.GeoNode.Label handMask = (PXCMGesture.GeoNode.Label)(int)FHandLabel[j];
                for (int i = 0; i < FGeoNodeLabel[j].SliceCount; i++)
                {
                    int index = j * FGeoNodeLabel[j].SliceCount + i;
                    PXCMGesture.GeoNode nData;
                    // hier muss noch das LABEL_BODY_HAND_LEFT weg bzw. beide Hände erkannt werden
                    // TODO
                    if (device.Pipeline.QueryGeoNode(handMask | FGeoNodeLabel[j][i], out nData))
                    {
                        // print("geonode left-hand (x=" + nData.positionImage.x + ", y=" + nData.positionImage.y + ")");

                        timeStamp[index] = (double)nData.timeStamp;
                        user[index] = nData.user;
                        body[index] = nData.body;
                        side[index] = nData.side;
                        confidence[index] = nData.confidence;
                        positionWorld[index] = FlipYZ(nData.positionWorld);
                        positionImage[index] = nData.positionImage;
                        radiusWorld[index] = nData.radiusWorld;
                        radiusImage[index] = nData.radiusImage;
                        massCenterWorld[index] = FlipYZ(nData.massCenterWorld);
                        massCenterImage[index] = nData.massCenterImage;
                        normal[index] = FlipYZ(nData.normal);
                        openness[index] = nData.openness;
                        opennessState[index] = nData.opennessState;
                    }

                    found[index] = positionWorld[index].z != 0;
                }
            }
        }
    }
}