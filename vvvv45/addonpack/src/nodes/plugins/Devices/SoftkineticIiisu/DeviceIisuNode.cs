#region usings
using System;
using System.ComponentModel.Composition;

using VVVV.PluginInterfaces.V1;
using VVVV.PluginInterfaces.V2;
using VVVV.Utils.VColor;
using VVVV.Utils.VMath;

using VVVV.Core.Logging;
using Iisu;
using Iisu.Data;
#endregion usings

namespace VVVV.Nodes
{
	[PluginInfo(Name = "Device", Category = "Device", Help = "Basic template with one value in/out", Tags = "", Version = "SoftkineticIisu")]
	public class DeviceIisuNode : IPluginEvaluate
	{
        public enum eCameraManufacturer { SoftKinetic, Asus, Kinect };

        // what to do next:
        /*
         * change inputs to not spreadable by using IDiffSpread for the Inputs
         * 
         * create settings inputs for the most important params for CI
         * 
         * clean up the ThreadedIisuCam class 
         * 
         * separate methods for setting params
         * 
         * use a backgroundworker for initialization
         * 
         
         */

		#region fields & pins
		[Input("Enable", DefaultValue = 0, IsToggle=true, IsSingle=true)]
		IDiffSpread<bool> FInputEnable;

        [Input("Enable Stabilization", DefaultValue = 1, IsToggle = true, IsSingle = true)]
        IDiffSpread<bool> FInputStabilizationEnabled;

        [Input("Stabilization Strength", DefaultValue = 0.5, IsSingle = true)]
        IDiffSpread<float> FInputStabilizationStrength;

        [Input("Enable Big5Activation", DefaultValue = 0, IsToggle = true, IsSingle = true)]
        IDiffSpread<bool> FInputEnableBig5Activation;

        [Input("Activation Max Distance", DefaultValue = 0.6, IsSingle = true)]
        IDiffSpread<float> FInputActivationMaxDistance;

        [Input("Camera Manufacturer", IsSingle = true)]
        IDiffSpread<eCameraManufacturer> FInputCameraManufacturer;

        [Input("Update Settings", DefaultValue = 0, IsBang = true)]
        IDiffSpread<bool> FInputUpdateSetings;

        [Output("DeviceActivated", IsToggle = true)]
        ISpread<bool> FOutputDeviceActivated;

        [Output("HandActivated", IsToggle=true)]
		ISpread<bool> FOutputHandActivated;
        
        [Output("ErrorMessage")]
        ISpread<String> FOutputErrorMessage;

        [Output("CamStatusMessage")]
        ISpread<String> FOutputDeviceStatus;
        
        /*
        [Output("TipPositionHand1_3D")]
        ISpread<Vector3D> FOutputVec_TipPosHand1_3D;

        [Output("PalmPositionHand1_3D")]
        ISpread<Vector3D> FOutputVec_PalmPosHand1_3D;

        [Output("FingerTipPositionHand1_3D")]
        ISpread<Vector3D> FOutputVec_FingerTipPosHand1_3D;

        [Output("ImageRaw")]
        ISpread<double> FOutput_ImageRaw = new Spread<double>();
        */

        [Output("Device", IsSingle = true)]
        ISpread<IDevice> FDevice;

        [Output("IisuData", IsSingle = true)]
        ISpread<IisuData> FData;

		[Import()]
		ILogger FLogger;
		#endregion fields & pins

        ThreadedIisuCamera FTic = null;


        // CI writable settings: 
        private bool FIsHandActivated = false;
        private bool FIsStabilizationEnabled = true;
        private bool FBig5ActivationEnabled = false;
        private float FActivationMaxDistance = 0.6f;

        private Vector3D FVecTipPos3D = new Vector3D();
        private Vector3D FVecPalmPos3D = new Vector3D();
        private ISpread<Vector3D> FVecFingerTipPos3D = new Spread<Vector3D>();

        private double[] ImageRaw = new double[0];

        private void SetCIStabilizationEnabled(bool isEnabled)
        {
            // write to iisu config here
        }

        private void SetStabilizationStrength(float strength)
        {
            // range 0 .. 1
            // write to iisu config here
        }

        private void SetActivationMaxDistance(float distance)
        {
            // range 0 .. 1
            // write to config here
        }

        private void SetEnableBig5Activation(bool isActivated)
        {
            // write to config here
        }

        private void initIisuThreaded()
        {
            if (FInputStabilizationEnabled.IsChanged)
            {
            }
            if (FTic == null)
            {
                FTic = new ThreadedIisuCamera();

                // start iisu thread
                FTic.StartThread();

                // register events thrown by iisu thread
                FTic.OnDeviceStatusChanged += new ThreadedIisuCamera.OnDeviceStatusChangedDelegate(tic_OnStatusChanged);
                FTic.OnSystemError += new ThreadedIisuCamera.OnSystemErrorDelegate(tic_OnError);
                FTic.OnCIHandActivated += new ThreadedIisuCamera.OnCIHandActivatedDelegate(tic_OnHandActivated);
                FTic.OnCIHandDeactivated += new ThreadedIisuCamera.OnCIHandDeactivatedDelegate(tic_OnHandDeactivated);
                FTic.OnDeviceDataFrame += new ThreadedIisuCamera.OnDeviceDataFrameDelegate(tic_OnDataFrame);
                FTic.OnCIHandPosingGesture += new ThreadedIisuCamera.OnCIHandPosingGestureDelegate(FTic_OnCIHandPosingGesture);
                FTic.OnDataProcessed += new ThreadedIisuCamera.OnDataProcessedDelegate(tic_OnDataProcessed);
                FTic.OnCustomError += new ThreadedIisuCamera.OnCustomErrorDelegate(tic_OnCustomError);
            }
            else if (FTic != null)
            {
                FTic.DoProcessing = false;
            }
        }

        void FTic_OnCIHandPosingGesture(string name, Int32 handId1, Int32 handId2, UInt32 posingGestureId)
        {
            // throw new NotImplementedException();
            FLogger.Log(LogType.Debug, string.Format("name: {0}, handId1: {1}, handId2: {2}, poseId: {3}", name, handId1, handId2, posingGestureId));

        }

        void tic_OnCustomError(string message)
        {
            FOutputErrorMessage[0] = message;
            FLogger.Log(LogType.Error, message);
        }

        private void tic_OnDataProcessed(IisuData iisuData)
        {
            // pass iisu Data to outside



            // testweise output of some parameters
            /*
            FVecTipPos3D = new Vector3D(iisuData.CiHand1TipPosition3D._X,
                                        iisuData.CiHand1TipPosition3D._Y,
                                        iisuData.CiHand1TipPosition3D._Z);

            FVecPalmPos3D = new Vector3D(iisuData.CiHand1PalmPosition3D._X,
                                         iisuData.CiHand1PalmPosition3D._Y,
                                         iisuData.CiHand1PalmPosition3D._Z);

            
            int fingerTipCount = iisuData.CiHand1FingerTipPositions3D.Length;
            FVecFingerTipPos3D.SliceCount = fingerTipCount;
            for(int i = 0; i < fingerTipCount; i++) {
                Vector3 v = iisuData.CiHand1FingerTipPositions3D[i];
                FVecFingerTipPos3D[i] = new Vector3D(v.X, v.Y, v.Z);
            }
            */
            /*
            iisuData.CiHand1PalmNormal3D = CI_HAND1_PalmNormal3D.Value;
            iisuData.CiHand1TipPosition3D = CI_HAND1_TipPosition3D.Value;
            iisuData.CiHand1PalmPosition3D = CI_Hand1_PalmPosition3D.Value;
            iisuData.CiHand1FingerStatus = CI_HAND1_FingerStatus.Value;
            iisuData.CiHand1FingerTipPositions2D = CI_HAND1_FingerTipPositions2D.Value;
            iisuData.CiHand1FingerTipPositions3D = CI_HAND1_FingerTipPositions3D.Value;
            iisuData.CiHand1IsIntersectingLeftImageBoundary = CI_Hand1_IsIntersectingLeftImageBoundary.Value;
            */

            // make array from depth image
            //iisuData.CiDepthImage.ToDepthValues(ref ImageRaw);

            // nochwas zu tun?
        }

        void tic_OnDataFrame(string name, uint e)
        {
        }

        void tic_OnHandDeactivated(string name, int e)
        {
            FLogger.Log(LogType.Debug, name);
            FOutputHandActivated[0] = false;
        }

        void tic_OnHandActivated(string name, int e)
        {
            FLogger.Log(LogType.Debug, name);
            FOutputHandActivated[0] = true;
        }

        void tic_OnError(string name, Error e, bool isFatal)
        {
            FOutputErrorMessage[0] = e.Message;
            FLogger.Log(LogType.Error, e.Message);
        }

        void tic_OnStatusChanged(string name, DeviceStatus status)
        {
            FOutputDeviceStatus[0] = status.ToString();
            FLogger.Log(LogType.Debug, "New Status: " + status.ToString());

            if (status != DeviceStatus.Error && status != DeviceStatus.Invalid)
            {
                FOutputDeviceActivated[0] = true;

                // send device out
                FData.SliceCount = 1;
                FData[0] = FTic.IisuData;

                FDevice.SliceCount = 1;
                FDevice[0] = FTic.Device;
            }
            else
            {
                FOutputDeviceActivated[0] = false;
            }
        }

        
		//called when data for any output pin is requested
		public void Evaluate(int SpreadMax)
		{
            // register depth image


            FOutputDeviceStatus.SliceCount = SpreadMax;
            FOutputErrorMessage.SliceCount = SpreadMax;
            //FOutputHandActivated.SliceCount = SpreadMax;
            FOutputDeviceActivated.SliceCount = 1;

            
            //for (int i = 0; i < SpreadMax; i++)
            //{
                /*
                FOutputVec_TipPosHand1_3D[i] = FVecTipPos3D;
                FOutputVec_PalmPosHand1_3D[i] = FVecPalmPos3D;
                FOutputVec_FingerTipPosHand1_3D.AssignFrom(FVecFingerTipPos3D);
                FOutput_ImageRaw.AssignFrom(ImageRaw);
                */

                // check enable / disable state 
                if (FInputEnable.IsChanged && FInputEnable[0] == true)
                {
                    FLogger.Log(LogType.Debug, "Initializing iisu");
                    initIisuThreaded();
                }
                if (FInputEnable.IsChanged && FInputEnable[0] == false)
                {
                    FLogger.Log(LogType.Debug, "Stopping iisu ...");
                    if (FTic != null)
                        FTic.DoProcessing = false;
                }
            //}
        }
	}
}
