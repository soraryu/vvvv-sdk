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
    [PluginInfo(Name = "Properties", Category = "Devices", Help = "Basic template with one value in/out", Tags = "", Version = "DS325", Author = "herbst")]
    public class DS325DeviceProperty : IPluginEvaluate
    {
        [Input("Device Handle", IsSingle = true)]
        public ISpread<DS325Device> FDeviceHandle;
        
        [Input("Device Property")]
        ISpread<PXCMCapture.Device.Property> FDeviceProperty;

        [Output("Device Property Value")]
        ISpread<ISpread<float>> FDevicePropertyValue;

        DS325Device device = null;

        public void Evaluate(int SpreadMax)
        {
            // 
            DS325Node.code.OnFrame -= Update;
            DS325Node.code.OnFrame += Update;
        }
        
        public void Update()
        {
            if (FDeviceHandle[0] == null) return;
            device = FDeviceHandle[0];

            FDevicePropertyValue.SliceCount = FDeviceProperty.SliceCount;
            for (int i = 0; i < FDeviceProperty.SliceCount; i++)
            {
                float[] vals = new float[ValueCount(FDeviceProperty[i])];
                if (device.Pipeline.QueryDeviceProperty(FDeviceProperty[i], vals))
                {
                    FDevicePropertyValue[i].AssignFrom(vals);
                }
            }
        }

        
        public int ValueCount(PXCMCapture.Device.Property property)
        {
            switch(property)
            {
                case PXCMCapture.Device.Property.PROPERTY_COLOR_EXPOSURE:
                case PXCMCapture.Device.Property.PROPERTY_COLOR_BRIGHTNESS:
                case PXCMCapture.Device.Property.PROPERTY_COLOR_CONTRAST:
                case PXCMCapture.Device.Property.PROPERTY_COLOR_SATURATION:
                case PXCMCapture.Device.Property.PROPERTY_COLOR_HUE:
                case PXCMCapture.Device.Property.PROPERTY_COLOR_GAMMA:
                case PXCMCapture.Device.Property.PROPERTY_COLOR_WHITE_BALANCE:
                case PXCMCapture.Device.Property.PROPERTY_COLOR_SHARPNESS:
                case PXCMCapture.Device.Property.PROPERTY_COLOR_BACK_LIGHT_COMPENSATION:
                case PXCMCapture.Device.Property.PROPERTY_COLOR_GAIN:
                case PXCMCapture.Device.Property.PROPERTY_AUDIO_MIX_LEVEL:
			    case PXCMCapture.Device.Property.PROPERTY_DEPTH_SATURATION_VALUE:
			    case PXCMCapture.Device.Property.PROPERTY_DEPTH_LOW_CONFIDENCE_VALUE:
                    return 1;
               
                case PXCMCapture.Device.Property.PROPERTY_COLOR_FIELD_OF_VIEW:
                case PXCMCapture.Device.Property.PROPERTY_COLOR_SENSOR_RANGE:
                case PXCMCapture.Device.Property.PROPERTY_COLOR_FOCAL_LENGTH:
                case PXCMCapture.Device.Property.PROPERTY_COLOR_PRINCIPAL_POINT:

                case PXCMCapture.Device.Property.PROPERTY_DEPTH_FIELD_OF_VIEW:
                case PXCMCapture.Device.Property.PROPERTY_DEPTH_SENSOR_RANGE:
                case PXCMCapture.Device.Property.PROPERTY_DEPTH_FOCAL_LENGTH:
                case PXCMCapture.Device.Property.PROPERTY_DEPTH_PRINCIPAL_POINT:
                    return 2;
               
                case PXCMCapture.Device.Property.PROPERTY_ACCELEROMETER_READING:
                    return 3;

                default:
                    return 0;
                /* Customized properties */
                // PROPERTY_CUSTOMIZED=0x04000000,
            }
        }
    }

}