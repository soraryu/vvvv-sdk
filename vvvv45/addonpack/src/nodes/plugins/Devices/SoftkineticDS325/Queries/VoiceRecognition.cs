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
    /// Queries camera and device properties.
    /// </summary>
    /// <remarks>
    /// There is an accelerometer sensor built in, called PROPERTY_ACCELEROMETER_READING.
    /// </remarks>
    [PluginInfo(Name = "VoiceRecognition", Category = "Devices", Help = "Basic template with one value in/out", Tags = "", Version = "DS325", Author = "herbst")]
    public class DS325DeviceVoiceRecognition : IPluginEvaluate
    {
        [Input("Device Handle", IsSingle = true)]
        public IDiffSpread<DS325Device> FDeviceHandle;
        
        [InputAttribute("Possible Commands")]
        public IDiffSpread<string> FCommands;
        
        //[Output("Confidence")]
        //ISpread<int> FConfidence;
        
		[Output("Dictation")]
		ISpread<string> FDictation;
        
		//[Output("Duration")]
        //ISpread<int> FDuration;
        
		//[Output("Label")]
        //ISpread<int> FLabel;
        
        //[Output("Timestamp")]
        //ISpread<int> FTimestamp;
        
        [Output("OnRecognition")]
        ISpread<bool> FOnRecognition;

        DS325Device device = null;

        public void Evaluate(int SpreadMax)
        {
            // 
            DS325Node.code.OnFrame -= Update;
            DS325Node.code.OnFrame += Update;
        }
        
        PXCMVoiceRecognition.Recognition data;
        public void Update()
        {
            if (FDeviceHandle[0] == null) return;
            device = FDeviceHandle[0];

            /*
            if(FDeviceHandle.IsChanged || FCommands.IsChanged)
            {
            	if(FCommands.SliceCount > 0)
            	{
            		int count = FCommands.SliceCount;
            		string[] cmds = new string[count];
            		for(int i = 0; i < count; i++) {
            			cmds[i] = FCommands[i];
            		}
            		device.Pipeline.SetVoiceCommands(cmds);
            	}
            	else {
            		device.Pipeline.SetVoiceDictation();
            	}
            }
            */
           
            //FConfidence.SliceCount = 1;
            FDictation.SliceCount = 1;
            //FDuration.SliceCount = 1;
            //FLabel.SliceCount = 1;
            //FTimestamp.SliceCount = 1;
            FOnRecognition.SliceCount = 1;
            
            if (device.Pipeline.QueryVoiceRecognized(out data))
            {
            	//FConfidence[0] = data.confidence;
            	FDictation[0] = data.dictation;
            	//FDuration[0] = (int)data.duration;
            	//FLabel[0] = data.label;
            	//FTimestamp[0] = (int)data.timeStamp;
            	FOnRecognition[0] = true;
            }
            else {
            	FOnRecognition[0] = false;
            }
        }
    }

}