using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.Composition;

using VVVV.PluginInterfaces.V1;
using VVVV.PluginInterfaces.V2;
using VVVV.Utils.VColor;
using VVVV.Utils.VMath;

using VVVV.Core.Logging;


using System.Collections;
using Iisu;
// using Iisu.Data;

namespace VVVV.Nodes
{
    
    public class HandIisuInput
    {


        private IDevice device;
        public IDevice Device { get { return device; } }
        private int handNumber;
        public int HandNumber { get { return handNumber; } }

        private IDataHandle<Iisu.Data.Vector3> _tipPosition;
        private IDataHandle<Iisu.Data.Vector3> _thumbDirection;
        private IDataHandle<Iisu.Data.Vector3> _palmDirection;

        private IDataHandle<Iisu.Data.Vector3> _palmPosition;
        private IDataHandle<float> _palmRadius;

        private IDataHandle<Iisu.Data.Vector3[]> _fingerTipPositions;
        private IDataHandle<int[]> _fingersStatus;

        private IDataHandle<int> _handStatus;

        public HandIisuInput(IDevice device, int handNumber)
        {
            this.device = device;
            this.handNumber = handNumber;

            //Registering data for the specific hand
            _tipPosition = device.RegisterDataHandle<Iisu.Data.Vector3>("CI.HAND" + HandNumber + ".TipPosition3D");
            _palmDirection = device.RegisterDataHandle<Iisu.Data.Vector3>("CI.HAND" + HandNumber + ".PalmNormal3D");

            _palmPosition = device.RegisterDataHandle<Iisu.Data.Vector3>("CI.HAND" + HandNumber + ".PalmPosition3D");

            _fingerTipPositions = device.RegisterDataHandle<Iisu.Data.Vector3[]>("CI.HAND" + HandNumber + ".FingerTipPositions3D");

            _fingersStatus = device.RegisterDataHandle<int[]>("CI.HAND" + HandNumber + ".FingerStatus");
            _handStatus = device.RegisterDataHandle<int>("CI.HAND" + HandNumber + ".Status");
        }

        //Meaning of the status values, taken from the developer guide:
        //0	Inactive
        //1	Just detected at current frame. No temporal coherence with the same data at previous frame.
        //2	Tracked. Temporal coherence with the same data at previous frame.
        //3	Extrapolated. Temporal coherence with the same data at previous frame however the object (hand or finger) has not been detected at current frame.
        public bool Detected
        {
            get
            {
                return _handStatus.Value >= 1;
            }
        }

        //Get all hand related data, and encapsulate it in a HandData object.
        public HandData HandPositions
        {
            get
            {
                HandData data = new HandData();

                data.PalmPosition = _palmPosition.Value.ToVector3();
                data.PalmDirection = _palmDirection.Value.ToVector3();
                data.PointingDirection = (_tipPosition.Value.ToVector3() - _palmPosition.Value.ToVector3());

                data.FingerTipPositions = _fingerTipPositions.Value.ToVector3Array();

                int[] fingerStatus = _fingersStatus.Value;

                for (int i = 0; i < data.FingerStatus.Length; ++i)
                {
                    data.FingerStatus[i] = (fingerStatus[i] >= 1);
                }

                return data;
            }
        }

    }


}
