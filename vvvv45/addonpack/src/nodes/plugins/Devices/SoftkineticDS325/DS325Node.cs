#region usings
using System;
using System.ComponentModel.Composition;

using VVVV.PluginInterfaces.V1;
using VVVV.PluginInterfaces.V2;
using VVVV.Utils.VColor;
using VVVV.Utils.VMath;

using VVVV.Core.Logging;
using System.Threading;
using System.Collections;
using System.Collections.Generic;

using VVVV.Nodes.DS325;

#endregion usings

namespace VVVV.Nodes
{

    #region PluginInfo
    [PluginInfo(Name = "DS325", Category = "Devices", Help = "Basic template with one value in/out", Tags = "", Version = "DS325", Author = "herbst")]
    #endregion PluginInfo
    public class DS325Node : IPluginEvaluate, IDisposable
    {
        #region fields & pins
        [Input("Enabled", DefaultValue = 0, IsToggle = true, IsSingle = true, Order = 100)]
        public IDiffSpread<bool> FInputEnable;

        //[Input("Update", DefaultValue = 1, IsToggle = true, IsSingle = true)]
        //public IDiffSpread<bool> FUpdate;

        //[Input("CAPTURE Mode", IsSingle = true)]
        //public IDiffSpread<bool> FMode_CAPTURE;
        [Input("FACE_LOCATION Mode", IsSingle = true)]
        public IDiffSpread<bool> FMode_FACE_LOCATION;
        [Input("FACE_LANDMARK Mode", IsSingle = true)]
        public IDiffSpread<bool> FMode_FACE_LANDMARK;
        [Input("GESTURE Mode", IsSingle = true)]
        public IDiffSpread<bool> FMode_GESTURE;
		[Input("VOICE RECOGNITION Mode", IsSingle = true)]
        public IDiffSpread<bool> FMode_VOICE_RECOGNITION;

        [Output("Enabled", IsToggle = true, IsSingle = true)]
        public ISpread<bool> FOutputEnabled;

        [Output("Message")]
        public ISpread<String> FOutputErrorMessage;

        [Output("Device Handle", IsSingle = true)]
        public ISpread<DS325Device> FDeviceHandle;

        [Import()]
        ILogger FLogger;
        #endregion fields & pins

        static DS325Device _code = new DS325Device();
        public static DS325Device code
        {
            get {
                return _code;
            }
        }

        
        public void Dispose()
        {
            if (code != null)
                code.Dispose();
        }

        //called when data for any output pin is requested
        public void Evaluate(int SpreadMax)
        {
            bool initializationNeeded = false;

            FOutputErrorMessage.SliceCount = 1;
            FOutputEnabled.SliceCount = 1;
            FDeviceHandle.SliceCount = 1;
            
            if (FMode_FACE_LOCATION.IsChanged || FMode_GESTURE.IsChanged || FInputEnable.IsChanged || FMode_VOICE_RECOGNITION.IsChanged)
            {
                // release device (not good if singleton...)
                if(code != null)
                    code.Dispose();

                // needs initialization
                initializationNeeded = true;
            }

            if (initializationNeeded && FInputEnable[0])
            {
                code.Logger = FLogger;
                //code.OnInit = delegate(bool success) { FOutputEnabled[0] = success; };
                //code.OnDispose = delegate(bool success) { if(success) FOutputEnabled[0] = false; };

                PXCUPipelineOT.Mode mode = (PXCUPipelineOT.Mode) 0;
                // if (FMode_CAPTURE[0])       mode |= PXCUPipelineOT.Mode.CAPTURE;
                if (FMode_FACE_LANDMARK[0]) 	mode |= PXCUPipelineOT.Mode.FACE_LANDMARK;
                if (FMode_FACE_LOCATION[0]) 	mode |= PXCUPipelineOT.Mode.FACE_LOCATION;
                if (FMode_GESTURE[0])       	mode |= PXCUPipelineOT.Mode.GESTURE;
                if (FMode_VOICE_RECOGNITION[0])	mode |= PXCUPipelineOT.Mode.VOICE_RECOGNITION;
                

                // start Update thread
                code.Init(mode);
                FDeviceHandle[0] = code;
            }
            
            // run update on cameras
            
            
            if(!initializationNeeded && FInputEnable[0]) {
            	code.Update();
            }

            FOutputEnabled[0] = code.IsInitialized;
            FOutputErrorMessage[0] = code.ErrorMessage;
                
            if (FInputEnable.IsChanged && !FInputEnable[0])
            {
                // Dispose interface
                if(code != null)
                    code.Dispose();
            }
        }

        
        

        
    }
}
