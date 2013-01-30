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
using System.Linq;

#endregion usings

namespace VVVV.Nodes.DS325
{
    /// <summary>
    /// Queries image data from camera. Currently NOT IMPLEMENTED.
    /// </summary>
    [PluginInfo(Name = "Image", Category = "Devices", Help = "Basic template with one value in/out", Tags = "", Version = "DS325", Author = "herbst")]
    public class DS325ImageMap : IPluginEvaluate
    {
        [Input("Device Handle", IsSingle = true)]
        public ISpread<DS325Device> FDeviceHandle;
        
        public enum MapType
        {
            Depth,
            IR,
            Label,
            UV
        }

        [Input("Map Type", IsSingle = true)]
        IDiffSpread<MapType> FMapType;

        [Output("Size")]
        ISpread<Vector2D> FSize;

        [Output("Map Data")]
        ISpread<double> FMapData;

        [Import()]
        ILogger FLogger;
        
        delegate bool QuerySizeMethod(out int width, out int height);
        QuerySizeMethod QuerySize;

        delegate short[] QueryMapMethod();
        QueryMapMethod QueryMap;

        int width, height;
        bool addedEvent = false;

        DS325Device device = null;

        public void Evaluate(int SpreadMax)
        {
            if (FDeviceHandle[0] == null) return;
            device = FDeviceHandle[0];

            ////if (FMapType.IsChanged || FDeviceHandle.IsChanged)
            ////{
            if (device.IsInitialized)
            {
                if (FMapType.IsChanged || FDeviceHandle.IsChanged || !addedEvent)
                {
                    switch (FMapType[0])
                    {
                        case MapType.Depth:
                            QuerySize = device.Pipeline.QueryDepthMapSize;
                            QueryMap = QueryDepthMap;
                            break;
                        case MapType.IR:
                            QuerySize = device.Pipeline.QueryIRMapSize;
                            QueryMap = QueryIRMap;
                            break;
                        case MapType.Label:
                            QuerySize = device.Pipeline.QueryLabelMapSize;
                            QueryMap = QueryLabelMap;
                            break;
                        case MapType.UV:
                            QuerySize = device.Pipeline.QueryUVMapSize;
                            // QueryMap = Query
                            break;
                    }

                    //FLogger.Log(LogType.Debug, "was here");
                    FSize.SliceCount = 1;


                    // query map size
                    QuerySize(out width, out height);
                    FSize[0] = new Vector2D(width, height);

                    if (!addedEvent)
                    {
                        device.OnDispose += delegate
                        {
                            addedEvent = false;
                            device.OnFrame -= Do;
                        };

                        device.OnFrame += Do;
                        addedEvent = true;
                    }
                }

                if (addedEvent && valuesInternal != null && valuesInternal.Length == width * height)
                {
                    FMapData.SliceCount = width * height;
                    for (int i = 0; i < valuesInternal.Length; i++)
                    {
                        FMapData[i] = valuesInternal[i];
                    }
                }
            }
        }

        void Do()
        {
            QueryMap();
        }

        void CheckArray()
        {
            if (valuesInternal == null || valuesInternal.Length != width * height)
            {
                valuesInternal = new short[width * height];
            }
        }

        short[] valuesInternal;
        public short[] QueryIRMap()
        {
            // create array if needed
            CheckArray();

            device.Pipeline.QueryIRMap(ref valuesInternal);

           return valuesInternal;
        }

        public short[] QueryDepthMap()
        {
            // create array if needed
            CheckArray();

            device.Pipeline.QueryDepthMap(ref valuesInternal);

            return valuesInternal;
        }

        byte[] labelMap;
        int[] labels;
        public short[] QueryLabelMap()
        {
            CheckArray();

            if (labelMap == null)
            {
                labelMap = new byte[width * height];
            }
            device.Pipeline.QueryLabelMap(ref labelMap, out labels);

            for (int i = 0; i < labelMap.Length; i++)
            {
                valuesInternal[i] = labelMap[i];
            }
            return null;
        }
    }
}
