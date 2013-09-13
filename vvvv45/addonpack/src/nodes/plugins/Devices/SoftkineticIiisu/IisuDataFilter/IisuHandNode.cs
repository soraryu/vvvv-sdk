#region usings
using System;
using System.ComponentModel.Composition;

using VVVV.PluginInterfaces.V1;
using VVVV.PluginInterfaces.V2;
using VVVV.Utils.VColor;
using VVVV.Utils.VMath;

using VVVV.Core.Logging;

using Iisu;

#endregion usings

namespace VVVV.Nodes
{
	[PluginInfo(Name = "Hand",Category = "Device", Help = "Basic template with one value in/out",Tags = "", Version = "SoftkineticIisu")]
    public class IisuHandNode : IPluginEvaluate
    {
        #region fields & pins
        [Input("IisuData", IsSingle = true)]
        IDiffSpread<IisuData> FData;

        enum HandNumber { Hand1 = 1, Hand2 = 2 };

        [Input("Hand", IsSingle = true)]
        IDiffSpread<HandNumber> FHandNumber;


        [Output("PalmPosition2D")]
        ISpread<Vector2D> PalmPosition2D;
        [Output("PalmPosition3D")]
        ISpread<Vector3D> PalmPosition3D;

        [Output("PalmNormal3D")]
        ISpread<Vector3D> PalmNormal3D;
        
        [Output("PointingDirection3D")]
        ISpread<Vector3D> PointingDirection3D;

        [Output("FingerTipPositions2D")]
        ISpread<Vector2D> FingerTipPositions2D;
        [Output("FingerTipPositions3D")]
        ISpread<Vector3D> FingerTipPositions3D;

        [Output("FingerStatus")]
        ISpread<bool> FingerStatus;

        [Output("HandStatus")]
        ISpread<bool> HandStatus;

        [Output("Openness")]
        ISpread<double> Openness;

        [Output("IsOpen")]
        ISpread<bool> IsOpen;

        [Output("PosingGestureID")]
        ISpread<int> PosingGestureID;

        [Import()]
        ILogger FLogger;
        #endregion fields & pins

        //private HandIisuInput handInput = null;


        //called when data for any output pin is requested
        public void Evaluate(int SpreadMax)
        {
            PalmPosition2D.SliceCount = 1;
            PalmPosition3D.SliceCount = 1;

            PalmNormal3D.SliceCount = 1;
            
            PointingDirection3D.SliceCount = 1;
            FingerStatus.SliceCount = 5;
            HandStatus.SliceCount = 1;
            IsOpen.SliceCount = 1;
            Openness.SliceCount = 1;
            PosingGestureID.SliceCount = 1;

            if (FData != null && FData.SliceCount > 0 && FData[0] != null)
            {
                // HandData data = handInput.HandPositions;
                IisuData data = FData[0];

                Vector3D _tipPosition = Vector3D.Zero;
                Vector3D _palmDirection = Vector3D.Zero;

                Vector2D _palmPosition2d = new Vector2D(0,0);
                Vector3D _palmPosition3d = Vector3D.Zero;
                Vector2D[] _fingerTipPositions2d = null;
                Vector3D[] _fingerTipPositions3d = null;

                bool _isOpen = false;
                double _openness = 0.0;
                int _posingGestureId = -1;

                int[] _fingersStatus = null;
                int _handStatus = 0;

                switch(FHandNumber[0]) {
                    case HandNumber.Hand1:
                        _tipPosition = data.CiHand1TipPosition3D.ToVector3();
                        _palmDirection = data.CiHand1PalmNormal3D.ToVector3();
                        _palmPosition2d = data.CiHand1PalmPosition2D.ToVector2();
                        _palmPosition3d = data.CiHand1PalmPosition3D.ToVector3();
                        _fingerTipPositions2d = data.CiHand1FingerTipPositions2D.ToVector2Array();
                        _fingerTipPositions3d = data.CiHand1FingerTipPositions3D.ToVector3Array();
                        _fingersStatus = data.CiHand1FingerStatus;
                        _handStatus = data.CiHand1Status;
                        _isOpen = data.CiHand1IsOpen;
                        _openness = data.CiHand1Openness;
                        _posingGestureId = data.CiHand1PosingGestureId;
                        break;
                    case HandNumber.Hand2:
                        _tipPosition = data.CiHand2TipPosition3D.ToVector3();
                        _palmDirection = data.CiHand2PalmNormal3D.ToVector3();
                        _palmPosition2d = data.CiHand2PalmPosition2D.ToVector2();
                        _palmPosition3d = data.CiHand2PalmPosition3D.ToVector3();
                        _fingerTipPositions2d = data.CiHand2FingerTipPositions2D.ToVector2Array();
                        _fingerTipPositions3d = data.CiHand2FingerTipPositions3D.ToVector3Array();
                        _fingersStatus = data.CiHand2FingerStatus;
                        _handStatus = data.CiHand2Status;
                        _isOpen = data.CiHand2IsOpen;
                        _openness = data.CiHand2Openness;
                        _posingGestureId = data.CiHand2PosingGestureId;
                        break;
                }
                

                PalmPosition2D[0] = _palmPosition2d;
                PalmPosition3D[0] = _palmPosition3d;
                PalmNormal3D[0] = _palmDirection;
                PointingDirection3D[0] = (_tipPosition - _palmPosition3d);

                FingerTipPositions2D.AssignFrom(_fingerTipPositions2d);
                FingerTipPositions3D.AssignFrom(_fingerTipPositions3d);

                for (int i = 0; i < _fingersStatus.Length; ++i)
                {
                    FingerStatus[i] = (_fingersStatus[i] >= 1);
                }

                HandStatus[0] = _handStatus == 0;
                IsOpen[0] = _isOpen;
                Openness[0] = _openness;
                PosingGestureID[0] = _posingGestureId;
            }
        }
    }
}
