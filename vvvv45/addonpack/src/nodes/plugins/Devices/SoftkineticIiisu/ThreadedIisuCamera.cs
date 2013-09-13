using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

using Iisu;
using Iisu.Data;

namespace VVVV.Nodes
{
    public class ThreadedIisuCamera
    {
        // iisu handle
        private IHandle m_iisuHandle = null;

        // iisu device
        private IDevice m_device = null;
        public IDevice Device
        {
            get { return m_device; }
        }

        // iisu thread
        private Thread thread = null;

        // thread run flag
        private bool FDoProcessing = true;
        public bool DoProcessing
        {
            get { return FDoProcessing; }
            set { FDoProcessing = value; }
        }
        
        // delegates
        public delegate void OnSystemErrorDelegate(string name, Iisu.Error e, bool isFatal);
        public delegate void OnDeviceDataFrameDelegate(string name, System.UInt32 e);
        public delegate void OnDeviceStatusChangedDelegate(string name, DeviceStatus status);
        public delegate void OnCIHandActivatedDelegate(string name, Int32 e);
        public delegate void OnCIHandDeactivatedDelegate(string name, Int32 e);
        public delegate void OnCIHandPosingGestureDelegate(string name, Int32 handId1, Int32 handId2, UInt32 posingGestureId);
        
        public delegate void OnDataProcessedDelegate(IisuData iisuData);
        public delegate void OnCustomErrorDelegate(string message);


        // events
        public event OnSystemErrorDelegate OnSystemError;
        public event OnDeviceDataFrameDelegate OnDeviceDataFrame;
        public event OnDeviceStatusChangedDelegate OnDeviceStatusChanged;
        public event OnCIHandActivatedDelegate OnCIHandActivated;
        public event OnCIHandDeactivatedDelegate OnCIHandDeactivated;
        public event OnCIHandPosingGestureDelegate OnCIHandPosingGesture;

        public event OnDataProcessedDelegate OnDataProcessed;
        public event OnCustomErrorDelegate OnCustomError;

        
        // iisuData instance
        private IisuData iisuData = new IisuData();
        public IisuData IisuData
        {
            get { return iisuData; }
        }

        // iisu data handles
        #region data handles
        IDataHandle<Iisu.Data.Vector3> SOURCE_Acceleration;
        IDataHandle<Iisu.Data.IImageData> CI_SceneLabelImage;
        IDataHandle<Iisu.Data.IImageData> CI_DepthImage;
        IDataHandle<Int32> CI_SceneLabelImage_BackgroundLabel;
        IDataHandle<Int32> CI_Hand1_Status;
        IDataHandle<Int32> CI_Hand2_Status;
        IDataHandle<bool> CI_Hand1_IsOpen;
        IDataHandle<bool> CI_Hand2_IsOpen;
        IDataHandle<float> CI_Hand1_Openness;
        IDataHandle<float> CI_Hand2_Openness;
        IDataHandle<Int32> CI_Hand1_Label;
        IDataHandle<Int32> CI_Hand2_Label;
        IDataHandle<bool> CI_Hand1_IsIntersectingUpperImageBoundary;
        IDataHandle<bool> CI_Hand2_IsIntersectingUpperImageBoundary;
        IDataHandle<bool> CI_Hand1_IsIntersectingLowerImageBoundary;
        IDataHandle<bool> CI_Hand2_IsIntersectingLowerImageBoundary;
        IDataHandle<bool> CI_Hand1_IsIntersectingLeftImageBoundary;
        IDataHandle<bool> CI_Hand2_IsIntersectingLeftImageBoundary;
        IDataHandle<bool> CI_Hand1_IsIntersectingRightImageBoundary;
        IDataHandle<bool> CI_Hand2_IsIntersectingRightImageBoundary;
        IDataHandle<Iisu.Data.Vector2> CI_Hand1_PalmPosition2D;
        IDataHandle<Iisu.Data.Vector2> CI_Hand2_PalmPosition2D;
        IDataHandle<Iisu.Data.Vector3> CI_Hand1_PalmPosition3D;
        IDataHandle<Iisu.Data.Vector3> CI_Hand2_PalmPosition3D;
        IDataHandle<Iisu.Data.Vector2> CI_HAND1_TipPosition2D;
        IDataHandle<Iisu.Data.Vector2> CI_HAND2_TipPosition2D;
        IDataHandle<Iisu.Data.Vector3> CI_HAND1_TipPosition3D;
        IDataHandle<Iisu.Data.Vector3> CI_HAND2_TipPosition3D;
        IDataHandle<Iisu.Data.Vector2> CI_HAND1_ForearmPosition2D;
        IDataHandle<Iisu.Data.Vector2> CI_HAND2_ForearmPosition2D;
        IDataHandle<Iisu.Data.Vector3> CI_HAND1_ForearmPosition3D;
        IDataHandle<Iisu.Data.Vector3> CI_HAND2_ForearmPosition3D;
        IDataHandle<Iisu.Data.Vector3> CI_HAND1_PalmNormal3D;
        IDataHandle<Iisu.Data.Vector3> CI_HAND2_PalmNormal3D;
        IDataHandle<Int32[]> CI_HAND1_FingerStatus;
        IDataHandle<Int32[]> CI_HAND2_FingerStatus;
        IDataHandle<Iisu.Data.Vector2[]> CI_HAND1_FingerTipPositions2D;
        IDataHandle<Iisu.Data.Vector2[]> CI_HAND2_FingerTipPositions2D;
        IDataHandle<Iisu.Data.Vector3[]> CI_HAND1_FingerTipPositions3D;
        IDataHandle<Iisu.Data.Vector3[]> CI_HAND2_FingerTipPositions3D;
        IDataHandle<int> CI_HAND1_PosingGestureId;
        IDataHandle<int> CI_HAND2_PosingGestureId;

        #endregion data handles

        // iisu parameter handles
        #region parameter handles
        // ReadOnly
        IParameterHandle<float> Source_FrameRate;
        IParameterHandle<String> Source_Camera_Name;
        IParameterHandle<String> Source_Camera_Model;
        IParameterHandle<float> Source_Camera_Depth_HFOV;
        IParameterHandle<float> Source_Camera_Depth_VFOV;
        IParameterHandle<int> Source_Camera_Depth_Width;
        IParameterHandle<int> Source_Camera_Depth_Height;
        IParameterHandle<float> Source_Camera_Confidence_HFOV;
        IParameterHandle<float> Source_Camera_Confidence_VFOV;
        IParameterHandle<int> Source_Camera_Confidence_Width;
        IParameterHandle<int> Source_Camera_Confidence_Height;
        IParameterHandle<float> Source_Camera_Color_HFOV;
        IParameterHandle<float> Source_Camera_Color_VFOV;
        IParameterHandle<int> Source_Camera_Color_Width;
        IParameterHandle<int> Source_Camera_Color_Height;
        IParameterHandle<int> Source_Camera_Color_Registration_Mode;

        // ReadWrite
        IParameterHandle<int> Source_DepthSense_AmplitudeThreshold;
        IParameterHandle<int> Source_DepthSense_LightIntensity;
        IParameterHandle<bool> CI_Big5ActivationEnabled;
        IParameterHandle<float> CI_ActivationMaxDistance;
        IParameterHandle<bool> CI_Enabled;
        IParameterHandle<bool> CI_Stabilization_Enabled;
        IParameterHandle<float> CI_Stabilization_Strength;

        #endregion parameter handles


        #region useful information
        /* All 2D data (positions, directions, lengths) are in 
         * depth map pixels coordinates.
         * 
         * All 3D data (positions, directions, lengths) are in 
         * camera reference system coordinates, in meters. 
         * 
         * CI starts to track a hand as soon as it enters the activation zone, 
         * which is in fact a maximal distance from the camera (default is 60cm).
         * 
         * 
        */
        #endregion






        // start the thread, but how to stop it correctly by - for example - an InputPin Bang coming from vvvv thread? (in case of reinitialization oder so ..)
        public void StartThread()
        {
            this.thread = new Thread(this.InitializeIisuCam);
            thread.Start();
        }


        // iisu device: initialize, register event listeners, start processing data
        private void InitializeIisuCam()
        {
            // We need to specify where is located the iisu dll and its configuration file.
            // in this sample we'll use the SDK's environment variable as resource to locate them
            // but you can use any mean you need.
            //string libraryLocation = System.Environment.GetEnvironmentVariable("IISU_SDK_DIR");
            IHandleConfiguration config = Iisu.Iisu.Context.CreateHandleConfiguration();
            // config.IisuBinDir = (libraryLocation + "/bin");
            // config.ConfigFileName = "iisu_config.xml";

            try
            {
                // get iisu handle

                // m_iisuHandle = Iisu.Iisu.Context.CreateHandle(config);
                m_iisuHandle = Iisu.Iisu.Context.CreateHandle();

                // Force specific configuration (disable all layers except close interaction)
                // overwrite loaded settings:

                // einzelne Funktionen zum Setzen einzelner Config-Params erstellen, sodass einzeln an-/ abgeschaltet werden kann

                m_iisuHandle.SetConfigString("//CONFIG/CAMERA", "Softkinetic", false);
                m_iisuHandle.SetConfigString("//CONFIG/PROCESSING/SCENE", "0", false);
                m_iisuHandle.SetConfigString("//CONFIG/PROCESSING/SKELETON", "0", false);
                m_iisuHandle.SetConfigString("//CONFIG/PROCESSING/RECORDING", "0", false);
                m_iisuHandle.SetConfigString("//CONFIG/PROCESSING/INFO", "0", false);
                m_iisuHandle.SetConfigString("//CONFIG/PROCESSING/UI", "0", false);
                m_iisuHandle.SetConfigString("//CONFIG/PROCESSING/UM", "0", false);              
                m_iisuHandle.SetConfigString("//CONFIG/PROCESSING/CI", "1", false);
                m_iisuHandle.SetConfigString("//CONFIG/PROCESSING/CI_GESTURES", "1", false);                
                m_iisuHandle.SetConfigString("//CONFIG/PROCESSING/CI_SHAPE", "0", false);                
                m_iisuHandle.SetConfigString("//CONFIG/PROCESSING/CI_MESH", "0", false);               
                m_iisuHandle.SetConfigString("//CONFIG/PROCESSING/CI_CENTROIDS", "0", false);
                m_iisuHandle.SetConfigString("//CONFIG/PROCESSING/SHAPE", "0", false);                
                m_iisuHandle.SetConfigString("//CONFIG/PROCESSING/CENTROIDS", "0", false);                
                m_iisuHandle.SetConfigString("//CONFIG/OPTIONS/AUTOCONFIG", "1", false);               
                m_iisuHandle.SetConfigString("//CONFIG/OPTIONS/MAX_USERS", "1", false);                
                m_iisuHandle.SetConfigString("//CONFIG/OPTIONS/MAX_CONTROLLERS", "1", false);

                // initialize iisu device
                
                m_device = m_iisuHandle.InitializeDevice();


                // register event listener
               
                m_device.EventManager.RegisterEventListener("SYSTEM.Error", new OnSystemErrorDelegate(OnSystemError));               
                m_device.EventManager.RegisterEventListener("CI.HandActivated", new OnCIHandActivatedDelegate(OnCIHandActivated));               
                m_device.EventManager.RegisterEventListener("CI.HandDeactivated", new OnCIHandDeactivatedDelegate(OnCIHandDeactivated));
                m_device.EventManager.RegisterEventListener("CI.HandPosingGesture", new OnCIHandPosingGestureDelegate(OnCIHandPosingGesture));               
                m_device.EventManager.RegisterEventListener("DEVICE.DataFrame", new OnDeviceDataFrameDelegate(OnDeviceDataFrame));
                m_device.EventManager.RegisterEventListener("DEVICE.Status", new OnDeviceStatusChangedDelegate(OnDeviceStatusChanged));

                // register parameter handles for given device
                RegisterParameterHandles(m_device);


                // register data handles for given device
                RegisterDataHandles(m_device);


                // start the device
                m_device.Start();


                // start processing data from iisu cam
                ProcessData(m_device);
            }
            catch (NullReferenceException nre)
            {
                OnCustomError(nre.Message);
            }
            catch (NativeIisuException nie)
            {
                OnCustomError(nie.Message);
            }
            catch (DotNetIisuException die)
            {
                OnCustomError(die.Message);
            }
            catch (Exception e)
            {
                OnCustomError(e.Message);
            }
        }
        
        // process data in loop by firing events for the vvvv thread (??)
        private void ProcessData(IDevice device)
        {
            while (FDoProcessing)
            {
                // process frame only if acquiring frame data from device was successful
                bool resAcquire = device.UpdateFrame(true);
                if (resAcquire)
                {
                    // lock frame to preserve data consistency
                    device.LockFrame();

                    // iisuData.CiDepthImage = CI_DepthImage.Value;

                    // write complete IisuData structure




                    /*
                    iisuData.CiHand1Status = CI_Hand1_Status.Value;
                    
                    iisuData.CiHand1IsOpen = CI_Hand1_IsOpen.Value;
                    
                    iisuData.CiHand1PalmNormal3D = CI_HAND1_PalmNormal3D.Value;
                    iisuData.CiHand1TipPosition3D = CI_HAND1_TipPosition3D.Value;
                    iisuData.CiHand1PalmPosition3D = CI_Hand1_PalmPosition3D.Value;
                    iisuData.CiHand1FingerStatus = CI_HAND1_FingerStatus.Value;
                    iisuData.CiHand1FingerTipPositions2D = CI_HAND1_FingerTipPositions2D.Value;
                    iisuData.CiHand1FingerTipPositions3D = CI_HAND1_FingerTipPositions3D.Value;   
                    iisuData.CiHand1IsIntersectingLeftImageBoundary = CI_Hand1_IsIntersectingLeftImageBoundary.Value;
                    */


                    iisuData.SourceAcceleration = SOURCE_Acceleration.Value;
                    iisuData.CiSceneLabelImage = CI_SceneLabelImage.Value;
                    iisuData.CiDepthImage = CI_DepthImage.Value;
                    iisuData.CiSceneLabelImageBackgroundLabel = CI_SceneLabelImage_BackgroundLabel.Value;
                    iisuData.CiHand1Status = CI_Hand1_Status.Value;
                    iisuData.CiHand2Status = CI_Hand2_Status.Value;
                    iisuData.CiHand1IsOpen = CI_Hand1_IsOpen.Value;
                    iisuData.CiHand2IsOpen = CI_Hand2_IsOpen.Value;
                    iisuData.CiHand1Openness = CI_Hand1_Openness.Value;
                    iisuData.CiHand2Openness = CI_Hand2_Openness.Value;
                    iisuData.CiHand1Label = CI_Hand1_Label.Value;
                    iisuData.CiHand2Label = CI_Hand2_Label.Value;
                    iisuData.CiHand1IsIntersectingUpperImageBoundary = CI_Hand1_IsIntersectingUpperImageBoundary.Value;
                    iisuData.CiHand2IsIntersectingUpperImageBoundary = CI_Hand2_IsIntersectingUpperImageBoundary.Value;
                    iisuData.CiHand1IsIntersectingLowerImageBoundary = CI_Hand1_IsIntersectingLowerImageBoundary.Value;
                    iisuData.CiHand2IsIntersectingLowerImageBoundary = CI_Hand2_IsIntersectingLowerImageBoundary.Value;
                    iisuData.CiHand1IsIntersectingLeftImageBoundary = CI_Hand1_IsIntersectingLeftImageBoundary.Value;
                    iisuData.CiHand2IsIntersectingLeftImageBoundary = CI_Hand2_IsIntersectingLeftImageBoundary.Value;
                    iisuData.CiHand1IsIntersectingRightImageBoundary = CI_Hand1_IsIntersectingRightImageBoundary.Value;
                    iisuData.CiHand2IsIntersectingRightImageBoundary = CI_Hand2_IsIntersectingRightImageBoundary.Value;
                    iisuData.CiHand1PalmPosition2D = CI_Hand1_PalmPosition2D.Value;
                    iisuData.CiHand2PalmPosition2D = CI_Hand2_PalmPosition2D.Value;
                    iisuData.CiHand1PalmPosition3D = CI_Hand1_PalmPosition3D.Value;
                    iisuData.CiHand2PalmPosition3D = CI_Hand2_PalmPosition3D.Value;
                    iisuData.CiHand1TipPosition2D = CI_HAND1_TipPosition2D.Value;
                    iisuData.CiHand2TipPosition2D = CI_HAND2_TipPosition2D.Value;
                    iisuData.CiHand1TipPosition3D = CI_HAND1_TipPosition3D.Value;
                    iisuData.CiHand2TipPosition3D = CI_HAND2_TipPosition3D.Value;
                    iisuData.CiHand1ForearmPosition2D = CI_HAND1_ForearmPosition2D.Value;
                    iisuData.CiHand2ForearmPosition2D = CI_HAND2_ForearmPosition2D.Value;
                    iisuData.CiHand1ForearmPosition3D = CI_HAND1_ForearmPosition3D.Value;
                    iisuData.CiHand2ForearmPosition3D = CI_HAND2_ForearmPosition3D.Value;
                    iisuData.CiHand1PalmNormal3D = CI_HAND1_PalmNormal3D.Value;
                    iisuData.CiHand2PalmNormal3D = CI_HAND2_PalmNormal3D.Value;
                    iisuData.CiHand1FingerStatus = CI_HAND1_FingerStatus.Value;
                    iisuData.CiHand2FingerStatus = CI_HAND2_FingerStatus.Value;
                    iisuData.CiHand1FingerTipPositions2D = CI_HAND1_FingerTipPositions2D.Value;
                    iisuData.CiHand2FingerTipPositions2D = CI_HAND2_FingerTipPositions2D.Value;
                    iisuData.CiHand1FingerTipPositions3D = CI_HAND1_FingerTipPositions3D.Value;
                    iisuData.CiHand2FingerTipPositions3D = CI_HAND2_FingerTipPositions3D.Value;
                    iisuData.CiHand1PosingGestureId = CI_HAND1_PosingGestureId.Value;
                    iisuData.CiHand2PosingGestureId = CI_HAND2_PosingGestureId.Value;






                    // release frame again
                    device.ReleaseFrame();

                    // throw event to be processed in vvvv thread (??)
                    OnDataProcessed(iisuData);
                }
                
                // need this ??
                Thread.Sleep(1);
            }
        }


        // register (a selection of) parameter handles to the device
        private void RegisterParameterHandles(IDevice dev)
        {
            // General            
            Source_FrameRate = dev.RegisterParameterHandle<float>(Constants.SOURCE_FrameRate);
            Source_Camera_Name = dev.RegisterParameterHandle<String>(Constants.SOURCE_CAMERA_Name);            
            Source_Camera_Model = dev.RegisterParameterHandle<String>(Constants.SOURCE_CAMERA_Model);            

            // Depth            
            Source_Camera_Depth_HFOV = dev.RegisterParameterHandle<float>(Constants.SOURCE_CAMERA_DEPTH_HFOV);            
            Source_Camera_Depth_VFOV = dev.RegisterParameterHandle<float>(Constants.SOURCE_CAMERA_DEPTH_VFOV);            
            Source_Camera_Depth_Width = dev.RegisterParameterHandle<int>(Constants.SOURCE_CAMERA_DEPTH_Width);            
            Source_Camera_Depth_Height = dev.RegisterParameterHandle<int>(Constants.SOURCE_CAMERA_DEPTH_Height);

            // Confidence            
            Source_Camera_Confidence_HFOV = dev.RegisterParameterHandle<float>(Constants.SOURCE_CAMERA_CONFIDENCE_HFOV);            
            Source_Camera_Confidence_VFOV = dev.RegisterParameterHandle<float>(Constants.SOURCE_CAMERA_CONFIDENCE_VFOV);            
            Source_Camera_Confidence_Width = dev.RegisterParameterHandle<int>(Constants.SOURCE_CAMERA_CONFIDENCE_Width);            
            Source_Camera_Confidence_Height = dev.RegisterParameterHandle<int>(Constants.SOURCE_CAMERA_CONFIDENCE_Height);

            // Color            
            Source_Camera_Color_HFOV = dev.RegisterParameterHandle<float>(Constants.SOURCE_CAMERA_COLOR_HFOV);            
            Source_Camera_Color_VFOV = dev.RegisterParameterHandle<float>(Constants.SOURCE_CAMERA_COLOR_VFOV);            
            Source_Camera_Color_Width = dev.RegisterParameterHandle<int>(Constants.SOURCE_CAMERA_COLOR_Width);            
            Source_Camera_Color_Height = dev.RegisterParameterHandle<int>(Constants.SOURCE_CAMERA_COLOR_Height);

            // Color Registration Mode            
            Source_Camera_Color_Registration_Mode = dev.RegisterParameterHandle<int>(Constants.SOURCE_CAMERA_COLOR_REGISTRATION_Mode);


            // ****************************************************************************************************************************

            // Read/Write parameters

            // DepthSense specific            
            Source_DepthSense_AmplitudeThreshold = dev.RegisterParameterHandle<int>(Constants.SOURCE_DEPTHSENSE_AmplitudeThreshold);            
            Source_DepthSense_LightIntensity = dev.RegisterParameterHandle<int>(Constants.SOURCE_DEPTHSENSE_LightIntensity);
            
            // CI specific            
            // BUG - not there anymore in iisu 3.5.1
            // CI_Big5ActivationEnabled = dev.RegisterParameterHandle<bool>(Constants.CI_Big5ActivationEnabled);            

            CI_ActivationMaxDistance = dev.RegisterParameterHandle<float>(Constants.CI_ActivationMaxDistance);            
            CI_Enabled = dev.RegisterParameterHandle<bool>(Constants.CI_Enabled);            
            CI_Stabilization_Enabled = dev.RegisterParameterHandle<bool>(Constants.CI_STABILIZATION_Enabled);            
            CI_Stabilization_Strength = dev.RegisterParameterHandle<float>(Constants.CI_STABILIZATION_Strength);
        }


        // register (a selection of) data handles to the device
        private void RegisterDataHandles(IDevice dev)
        {
            SOURCE_Acceleration = dev.RegisterDataHandle<Iisu.Data.Vector3>(Constants.SOURCE_Acceleration);

            CI_SceneLabelImage = dev.RegisterDataHandle<Iisu.Data.IImageData>(Constants.CI_SceneLabelImage);
            CI_DepthImage = dev.RegisterDataHandle<Iisu.Data.IImageData>(Constants.SOURCE_CAMERA_DEPTH_Image);
            CI_SceneLabelImage_BackgroundLabel = dev.RegisterDataHandle<Int32>(Constants.CI_SceneLabelImage_BackgroundLabel);
            CI_Hand1_Status = dev.RegisterDataHandle<Int32>(Constants.CI_HAND1_Status);
            CI_Hand2_Status = dev.RegisterDataHandle<Int32>(Constants.CI_HAND2_Status);
            CI_Hand1_IsOpen = dev.RegisterDataHandle<bool>(Constants.CI_HAND1_IsOpen);
            CI_Hand2_IsOpen = dev.RegisterDataHandle<bool>(Constants.CI_HAND2_IsOpen);
            CI_Hand1_Openness = dev.RegisterDataHandle<float>(Constants.CI_HAND1_Openness);
            CI_Hand2_Openness = dev.RegisterDataHandle<float>(Constants.CI_HAND2_Openness);
            CI_Hand1_Label = dev.RegisterDataHandle<Int32>(Constants.CI_HAND1_Label);
            CI_Hand2_Label = dev.RegisterDataHandle<Int32>(Constants.CI_HAND2_Label);

            CI_Hand1_IsIntersectingUpperImageBoundary = dev.RegisterDataHandle<bool>(Constants.CI_HAND1_IsIntersectingUpperImageBoundary);
            CI_Hand2_IsIntersectingUpperImageBoundary = dev.RegisterDataHandle<bool>(Constants.CI_HAND2_IsIntersectingUpperImageBoundary);
            CI_Hand1_IsIntersectingLowerImageBoundary = dev.RegisterDataHandle<bool>(Constants.CI_HAND1_IsIntersectingLowerImageBoundary);
            CI_Hand2_IsIntersectingLowerImageBoundary = dev.RegisterDataHandle<bool>(Constants.CI_HAND2_IsIntersectingLowerImageBoundary);
            CI_Hand1_IsIntersectingLeftImageBoundary = dev.RegisterDataHandle<bool>(Constants.CI_HAND1_IsIntersectingLeftImageBoundary);
            CI_Hand2_IsIntersectingLeftImageBoundary = dev.RegisterDataHandle<bool>(Constants.CI_HAND2_IsIntersectingLeftImageBoundary);
            CI_Hand1_IsIntersectingRightImageBoundary = dev.RegisterDataHandle<bool>(Constants.CI_HAND1_IsIntersectingRightImageBoundary);
            CI_Hand2_IsIntersectingRightImageBoundary = dev.RegisterDataHandle<bool>(Constants.CI_HAND2_IsIntersectingRightImageBoundary);

            CI_Hand1_PalmPosition2D = dev.RegisterDataHandle<Iisu.Data.Vector2>(Constants.CI_HAND1_PalmPosition2D);
            CI_Hand2_PalmPosition2D = dev.RegisterDataHandle<Iisu.Data.Vector2>(Constants.CI_HAND2_PalmPosition2D);
            CI_Hand1_PalmPosition3D = dev.RegisterDataHandle<Iisu.Data.Vector3>(Constants.CI_HAND1_PalmPosition3D);
            CI_Hand2_PalmPosition3D = dev.RegisterDataHandle<Iisu.Data.Vector3>(Constants.CI_HAND2_PalmPosition3D);

            CI_HAND1_TipPosition2D = dev.RegisterDataHandle<Iisu.Data.Vector2>(Constants.CI_HAND1_TipPosition2D);
            CI_HAND2_TipPosition2D = dev.RegisterDataHandle<Iisu.Data.Vector2>(Constants.CI_HAND2_TipPosition2D);
            CI_HAND1_TipPosition3D = dev.RegisterDataHandle<Iisu.Data.Vector3>(Constants.CI_HAND1_TipPosition3D);
            CI_HAND2_TipPosition3D = dev.RegisterDataHandle<Iisu.Data.Vector3>(Constants.CI_HAND2_TipPosition3D);

            CI_HAND1_ForearmPosition2D = dev.RegisterDataHandle<Iisu.Data.Vector2>(Constants.CI_HAND1_ForearmPosition2D);
            CI_HAND2_ForearmPosition2D = dev.RegisterDataHandle<Iisu.Data.Vector2>(Constants.CI_HAND2_ForearmPosition2D);
            CI_HAND1_ForearmPosition3D = dev.RegisterDataHandle<Iisu.Data.Vector3>(Constants.CI_HAND1_ForearmPosition3D);
            CI_HAND2_ForearmPosition3D = dev.RegisterDataHandle<Iisu.Data.Vector3>(Constants.CI_HAND2_ForearmPosition3D);

            CI_HAND1_PalmNormal3D = dev.RegisterDataHandle<Iisu.Data.Vector3>(Constants.CI_HAND1_PalmNormal3D);
            CI_HAND2_PalmNormal3D = dev.RegisterDataHandle<Iisu.Data.Vector3>(Constants.CI_HAND2_PalmNormal3D);
            CI_HAND1_FingerStatus = dev.RegisterDataHandle<Int32[]>(Constants.CI_HAND1_FingerStatus);
            CI_HAND2_FingerStatus = dev.RegisterDataHandle<Int32[]>(Constants.CI_HAND2_FingerStatus);
            CI_HAND1_FingerTipPositions2D = dev.RegisterDataHandle<Iisu.Data.Vector2[]>(Constants.CI_HAND1_FingerTipPositions2D);
            CI_HAND2_FingerTipPositions2D = dev.RegisterDataHandle<Iisu.Data.Vector2[]>(Constants.CI_HAND2_FingerTipPositions2D);
            CI_HAND1_FingerTipPositions3D = dev.RegisterDataHandle<Iisu.Data.Vector3[]>(Constants.CI_HAND1_FingerTipPositions3D);
            CI_HAND2_FingerTipPositions3D = dev.RegisterDataHandle<Iisu.Data.Vector3[]>(Constants.CI_HAND2_FingerTipPositions3D);
            CI_HAND1_PosingGestureId = dev.RegisterDataHandle<int>(Constants.CI_HAND1_PosingGestureId);
            CI_HAND2_PosingGestureId = dev.RegisterDataHandle<int>(Constants.CI_HAND2_PosingGestureId);
        }


    }
}
