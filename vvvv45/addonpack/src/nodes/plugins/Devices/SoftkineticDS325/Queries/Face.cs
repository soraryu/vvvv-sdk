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
    /// Query face structures. Split them with the appropriate "Face (... Split)" nodes.
    /// </summary>
    [PluginInfo(Name = "Face", Category = "Devices", Help = "Basic template with one value in/out", Version = "DS325", Author = "herbst")]
    public class DS325Face : IPluginEvaluate
    {
        [Input("Device Handle", IsSingle = true)]
        public ISpread<DS325Device> FDeviceHandle;
        
        [Input("Face Count", IsSingle = true, DefaultValue = 1)]
        ISpread<int> FFaceCount;

        [Input("Face Labels")]
        ISpread<PXCMFaceAnalysis.Landmark.Label> FFaceLabels;

        [Output("Face ID")]
        ISpread<int> FFaceID;

        [Output("Timestamp")]
        ISpread<double> FTimestamp;
        
        [Output("Detection Data")]
        ISpread<PXCMFaceAnalysis.Detection.Data> FDetectionData;

        [Output("Landmark Data")]
        ISpread<PXCMFaceAnalysis.Landmark.LandmarkData> FLandmarkData;

        [Output("Landmark Pose Data")]
        ISpread<PXCMFaceAnalysis.Landmark.PoseData> FPoseData;

        DS325Device device;

        public void Evaluate(int SpreadMax)
        {
            if (FDeviceHandle[0] == null) return;
            device = FDeviceHandle[0];

            int faceCount = FFaceCount[0];

            FDetectionData.SliceCount = faceCount;
            FLandmarkData.SliceCount = faceCount;
            FPoseData.SliceCount = faceCount;
            FFaceID.SliceCount = faceCount;
            FTimestamp.SliceCount = faceCount;

            Int32 faceId;
            UInt64 timeStamp;

            FLandmarkData.SliceCount = 0;

            for (int i = 0; i < faceCount; i++)
            {
                if (device.Pipeline.QueryFaceID(i, out faceId, out timeStamp))
                {
                    //print("face(id=" + faceId + ", timeStamp=" + timeStamp + ")");

                    PXCMFaceAnalysis.Detection.Data detectionData;
                    if (device.Pipeline.QueryFaceLocationData(faceId, out detectionData))
                        FDetectionData[i] = detectionData;
                    //print("face location(id=" + faceId + ", x=" + detectionData.rectangle.x + ", y=" + detectionData.rectangle.y + ", w=" + detectionData.rectangle.w + ", h=" + detectionData.rectangle.h + ")");

                    foreach (var label in FFaceLabels)
                    {
                        PXCMFaceAnalysis.Landmark.LandmarkData[] landmarkData = new PXCMFaceAnalysis.Landmark.LandmarkData[1];
                        device.Pipeline.QueryFaceLandmarkData(faceId, label, ref landmarkData);
                        FLandmarkData.AddRange(landmarkData);
                    }
                    //print("landmark left-eye (id=" + faceId + ", x=" + landmarkData.position.x + ", y=" + landmarkData.position.y + ")");

                    PXCMFaceAnalysis.Landmark.PoseData poseData;
                    if (device.Pipeline.QueryFaceLandmarkPose(faceId, out poseData))
                        FPoseData[i] = poseData;

                    //print("landmark left-eye (id=" + faceId + ", x=" + landmarkData.position.x + ", y=" + landmarkData.position.y + ")");
                }

                FFaceID[i] = faceId;
                FTimestamp[i] = (double)timeStamp;
            }
        }
    }

    #region PluginInfo
    [PluginInfo(Name = "Face (Landmark Data Split)", Category = "Devices", Help = "Basic template with one value in/out", Tags = "", Version = "DS325", Author = "herbst")]
    #endregion PluginInfo
    public class DeviceDS325FaceLandmarkDataSplit : IPluginEvaluate
    {
        [Input("Landmark Data")]
        public ISpread<PXCMFaceAnalysis.Landmark.LandmarkData> FLandmarkData;

        [Output("Position")]
        public ISpread<Vector3D> position;
        [Output("FID")]
        public ISpread<float> fid;
        [Output("Label")]
        // public ISpread<PXCMFaceAnalysis.Landmark.Label> label;
        public ISpread<string> label;
        [Output("Lidx")]
        public ISpread<float> lidx;

        public void Evaluate(int SpreadMax)
        {
            position.SliceCount = SpreadMax;
            fid.SliceCount = SpreadMax;
            label.SliceCount = SpreadMax;
            lidx.SliceCount = SpreadMax;

            for (int i = 0; i < SpreadMax; i++)
            {
                position[i] = FLandmarkData[i].position;
                fid[i] = FLandmarkData[i].fid;
                label[i] = FLandmarkData[i].label.ToString();
                lidx[i] = FLandmarkData[i].lidx;
            }
        }       
    }

    #region PluginInfo
    [PluginInfo(Name = "Face (Detection Data Split)", Category = "Devices", Help = "Basic template with one value in/out", Tags = "", Version = "DS325", Author = "herbst")]
    #endregion PluginInfo
    public class DeviceDS325FaceDetectionDataSplit : IPluginEvaluate
    {
        [Input("Detection Data")]
        public ISpread<PXCMFaceAnalysis.Detection.Data> FDetectionData;

        [Output("Face Rect Position")]
        public ISpread<Vector2D> position;
        [Output("Face Rect Size")]
        public ISpread<Vector2D> size;
        [Output("FID")]
        public ISpread<Int32> fid;
        [Output("Confidence")]
        public ISpread<UInt32> confidence;
        [Output("View Angle")]
        // public ISpread<PXCMFaceAnalysis.Detection.ViewAngle> viewAngle;
        public ISpread<string> viewAngle;

        public void Evaluate(int SpreadMax)
        {
            position.SliceCount = SpreadMax;
            size.SliceCount = SpreadMax;

            fid.SliceCount = SpreadMax;
            confidence.SliceCount = SpreadMax;
            viewAngle.SliceCount = SpreadMax;

            for (int i = 0; i < SpreadMax; i++)
            {
                var rect = FDetectionData[i].rectangle;
                position[i] = new Vector2D(rect.x, rect.y);
                size[i] = new Vector2D(rect.w, rect.h);
                fid[i] = FDetectionData[i].fid;
                confidence[i] = FDetectionData[i].confidence;
                viewAngle[i] = FDetectionData[i].viewAngle.ToString();
            }
        }
    }

    #region PluginInfo
    [PluginInfo(Name = "Face (Pose Data Split)", Category = "Devices", Help = "Basic template with one value in/out", Tags = "", Version = "DS325", Author = "herbst")]
    #endregion PluginInfo
    public class DeviceDS325FacePoseDataSplit : IPluginEvaluate
    {
        [Input("Landmark Pose Data")]
        ISpread<PXCMFaceAnalysis.Landmark.PoseData> FPoseData;

        [Output("FID")]
        public ISpread<Int32> fid;
        [Output("Yaw")]
        public ISpread<float> yaw;
        [Output("Roll")]
        public ISpread<float> roll;
        [Output("Pitch")]
        public ISpread<float> pitch;
        
        public void Evaluate(int SpreadMax)
        {
            fid.SliceCount = SpreadMax;
            yaw.SliceCount = SpreadMax;
            roll.SliceCount = SpreadMax;
            pitch.SliceCount = SpreadMax;

            for (int i = 0; i < SpreadMax; i++)
            {
                fid[i] = FPoseData[i].fid;
                yaw[i] = FPoseData[i].yaw;
                roll[i] = FPoseData[i].roll;
                pitch[i] = FPoseData[i].pitch;
            }
        }
    }
/*
    #region PluginInfo
    [PluginInfo(Name = "DS325 Face (Pose Data Split)", Category = "Devices", Help = "Basic template with one value in/out", Tags = "")]
    #endregion PluginInfo
    public class DeviceDS325FacePoseDataSplit : IPluginEvaluate
    {
        [Input("Landmark Pose Data")]
        ISpread<PXCMFaceAnalysis.Landmark.PoseData> FPoseData;

        public void Evaluate(int SpreadMax)
        {

        }
    }
*/
}