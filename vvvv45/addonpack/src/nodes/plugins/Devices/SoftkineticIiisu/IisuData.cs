using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Iisu;
using Iisu.Data;

namespace VVVV.Nodes
{
    public class IisuData
    {

        #region ci attributes

        private IImageData ciSceneLabelImage;
        private IImageData ciDepthImage;
        private Int32 ciSceneLabelImageBackgroundLabel;
        private Int32 ciHand1Status;
        private Int32 ciHand2Status;
        private bool ciHand1IsOpen;
        private bool ciHand2IsOpen;
        private float ciHand1Openness;
        private float ciHand2Openness;
        private Int32 ciHand1Label;
        private Int32 ciHand2Label;
        private bool ciHand1IsIntersectingUpperImageBoundary;
        private bool ciHand2IsIntersectingUpperImageBoundary;
        private bool ciHand1IsIntersectingLowerImageBoundary;
        private bool ciHand2IsIntersectingLowerImageBoundary;
        private bool ciHand1IsIntersectingLeftImageBoundary;
        private bool ciHand2IsIntersectingLeftImageBoundary;
        private bool ciHand1IsIntersectingRightImageBoundary;
        private bool ciHand2IsIntersectingRightImageBoundary;
        private Vector2 ciHand1PalmPosition2D;
        private Vector2 ciHand2PalmPosition2D;
        private Vector3 ciHand1PalmPosition3D;
        private Vector3 ciHand2PalmPosition3D;
        private Vector2 ciHand1TipPosition2D;
        private Vector2 ciHand2TipPosition2D;
        private Vector3 ciHand1TipPosition3D;
        private Vector3 ciHand2TipPosition3D;
        private Vector2 ciHand1ForearmPosition2D;
        private Vector2 ciHand2ForearmPosition2D;
        private Vector3 ciHand1ForearmPosition3D;
        private Vector3 ciHand2ForearmPosition3D;
        private Vector3 ciHand1PalmNormal3D;
        private Vector3 ciHand2PalmNormal3D;
        private Int32[] ciHand1FingerStatus;
        private Int32[] ciHand2FingerStatus;
        private Vector2[] ciHand1FingerTipPositions2D;
        private Vector2[] ciHand2FingerTipPositions2D;
        private Vector3[] ciHand1FingerTipPositions3D;
        private Vector3[] ciHand2FingerTipPositions3D;
        private int ciHand1PosingGestureId;
        private int ciHand2PosingGestureId;

        #endregion attributes

        #region scene attributes
        private Vector3 sourceAcceleration;
        #endregion

        #region get / set

        public Vector3 SourceAcceleration
        {
            get { return sourceAcceleration; }
            set { sourceAcceleration = value; }
        }

        public IImageData CiSceneLabelImage
        {
            get { return ciSceneLabelImage; }
            set { ciSceneLabelImage = value; }
        }
        public IImageData CiDepthImage
        {
            get { return ciDepthImage; }
            set { ciDepthImage = value; }
        }
        public Int32 CiSceneLabelImageBackgroundLabel
        {
            get { return ciSceneLabelImageBackgroundLabel; }
            set { ciSceneLabelImageBackgroundLabel = value; }
        }
        public Int32 CiHand1Status
        {
            get { return ciHand1Status; }
            set { ciHand1Status = value; }
        }
        public Int32 CiHand2Status
        {
            get { return ciHand2Status; }
            set { ciHand2Status = value; }
        }
        public bool CiHand1IsOpen
        {
            get { return ciHand1IsOpen; }
            set { ciHand1IsOpen = value; }
        }
        public bool CiHand2IsOpen
        {
            get { return ciHand2IsOpen; }
            set { ciHand2IsOpen = value; }
        }
        public float CiHand1Openness
        {
            get { return ciHand1Openness; }
            set { ciHand1Openness = value; }
        }
        public float CiHand2Openness
        {
            get { return ciHand2Openness; }
            set { ciHand2Openness = value; }
        }
        public Int32 CiHand1Label
        {
            get { return ciHand1Label; }
            set { ciHand1Label = value; }
        }
        public Int32 CiHand2Label
        {
            get { return ciHand2Label; }
            set { ciHand2Label = value; }
        }
        public bool CiHand1IsIntersectingUpperImageBoundary
        {
            get { return ciHand1IsIntersectingUpperImageBoundary; }
            set { ciHand1IsIntersectingUpperImageBoundary = value; }
        }
        public bool CiHand2IsIntersectingUpperImageBoundary
        {
            get { return ciHand2IsIntersectingUpperImageBoundary; }
            set { ciHand2IsIntersectingUpperImageBoundary = value; }
        }
        public bool CiHand1IsIntersectingLowerImageBoundary
        {
            get { return ciHand1IsIntersectingLowerImageBoundary; }
            set { ciHand1IsIntersectingLowerImageBoundary = value; }
        }
        public bool CiHand2IsIntersectingLowerImageBoundary
        {
            get { return ciHand2IsIntersectingLowerImageBoundary; }
            set { ciHand2IsIntersectingLowerImageBoundary = value; }
        }
        public bool CiHand1IsIntersectingLeftImageBoundary
        {
            get { return ciHand1IsIntersectingLeftImageBoundary; }
            set { ciHand1IsIntersectingLeftImageBoundary = value; }
        }
        public bool CiHand2IsIntersectingLeftImageBoundary
        {
            get { return ciHand2IsIntersectingLeftImageBoundary; }
            set { ciHand2IsIntersectingLeftImageBoundary = value; }
        }
        public bool CiHand1IsIntersectingRightImageBoundary
        {
            get { return ciHand1IsIntersectingRightImageBoundary; }
            set { ciHand1IsIntersectingRightImageBoundary = value; }
        }
        public bool CiHand2IsIntersectingRightImageBoundary
        {
            get { return ciHand2IsIntersectingRightImageBoundary; }
            set { ciHand2IsIntersectingRightImageBoundary = value; }
        }
        public Vector2 CiHand1PalmPosition2D
        {
            get { return ciHand1PalmPosition2D; }
            set { ciHand1PalmPosition2D = value; }
        }
        public Vector2 CiHand2PalmPosition2D
        {
            get { return ciHand2PalmPosition2D; }
            set { ciHand2PalmPosition2D = value; }
        }
        public Vector3 CiHand1PalmPosition3D
        {
            get { return ciHand1PalmPosition3D; }
            set { ciHand1PalmPosition3D = value; }
        }
        public Vector3 CiHand2PalmPosition3D
        {
            get { return ciHand2PalmPosition3D; }
            set { ciHand2PalmPosition3D = value; }

        }
        public Vector2 CiHand1TipPosition2D
        {
            get { return ciHand1TipPosition2D; }
            set { ciHand1TipPosition2D = value; }

        }
        public Vector2 CiHand2TipPosition2D
        {
            get { return ciHand2TipPosition2D; }
            set { ciHand2TipPosition2D = value; }

        }
        public Vector3 CiHand1TipPosition3D
        {
            get { return ciHand1TipPosition3D; }
            set { ciHand1TipPosition3D = value; }

        }
        public Vector3 CiHand2TipPosition3D
        {
            get { return ciHand2TipPosition3D; }
            set { ciHand2TipPosition3D = value; }

        }
        public Vector2 CiHand1ForearmPosition2D
        {
            get { return ciHand1ForearmPosition2D; }
            set { ciHand1ForearmPosition2D = value; }

        }
        public Vector2 CiHand2ForearmPosition2D
        {
            get { return ciHand2ForearmPosition2D; }
            set { ciHand2ForearmPosition2D = value; }

        }
        public Vector3 CiHand1ForearmPosition3D
        {
            get { return ciHand1ForearmPosition3D; }
            set { ciHand1ForearmPosition3D = value; }

        }
        public Vector3 CiHand2ForearmPosition3D
        {
            get { return ciHand2ForearmPosition3D; }
            set { ciHand2ForearmPosition3D = value; }

        }
        public Vector3 CiHand1PalmNormal3D
        {
            get { return ciHand1PalmNormal3D; }
            set { ciHand1PalmNormal3D = value; }

        }
        public Vector3 CiHand2PalmNormal3D
        {
            get { return ciHand2PalmNormal3D; }
            set { ciHand2PalmNormal3D = value; }

        }
        public Int32[] CiHand1FingerStatus
        {
            get { return ciHand1FingerStatus; }
            set { ciHand1FingerStatus = value; }

        }
        public Int32[] CiHand2FingerStatus
        {
            get { return ciHand2FingerStatus; }
            set { ciHand2FingerStatus = value; }

        }
        public Vector2[] CiHand1FingerTipPositions2D
        {
            get { return ciHand1FingerTipPositions2D; }
            set { ciHand1FingerTipPositions2D = value; }

        }
        public Vector2[] CiHand2FingerTipPositions2D
        {
            get { return ciHand2FingerTipPositions2D; }
            set { ciHand2FingerTipPositions2D = value; }

        }
        public Vector3[] CiHand1FingerTipPositions3D
        {
            get { return ciHand1FingerTipPositions3D; }
            set { ciHand1FingerTipPositions3D = value; }

        }
        public Vector3[] CiHand2FingerTipPositions3D
        {
            get { return ciHand2FingerTipPositions3D; }
            set { ciHand2FingerTipPositions3D = value; }

        }
        public int CiHand1PosingGestureId
        {
            get { return ciHand1PosingGestureId; }
            set { ciHand1PosingGestureId = value; }
        }
        public int CiHand2PosingGestureId
        {
            get { return ciHand2PosingGestureId; }
            set { ciHand2PosingGestureId = value; }
        }

        #endregion

    }
}
