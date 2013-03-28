using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using VVVV.Utils.VMath;

namespace VVVV.Nodes
{
    //Encapsulate all hand related data
    public class HandData
    {
        public Vector3D PalmPosition;

        public Vector3D PalmDirection;
        public Vector3D PointingDirection;

        public Vector3D[] FingerTipPositions;

        public bool[] FingerStatus;
        public bool HandStatus;

        public HandData()
        {
            FingerTipPositions = new Vector3D[5];
            FingerStatus = new bool[5];
        }

    }

}
