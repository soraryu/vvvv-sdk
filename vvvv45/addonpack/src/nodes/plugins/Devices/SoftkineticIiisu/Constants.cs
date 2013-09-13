using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VVVV.Nodes
{
    public class Constants
    {
        public const String SOURCE_FrameRate = "SOURCE.FrameRate";
        public const String SOURCE_CAMERA_Name = "SOURCE.CAMERA.Name";
        public const String SOURCE_CAMERA_Model = "SOURCE.CAMERA.Model";
        
        public const String SOURCE_CAMERA_DEPTH_HFOV = "SOURCE.CAMERA.DEPTH.HFOV"; 
        public const String SOURCE_CAMERA_DEPTH_VFOV = "SOURCE.CAMERA.DEPTH.VFOV";
        public const String SOURCE_CAMERA_DEPTH_Width = "SOURCE.CAMERA.DEPTH.Width";
        public const String SOURCE_CAMERA_DEPTH_Height = "SOURCE.CAMERA.DEPTH.Height";

        public const String SOURCE_CAMERA_CONFIDENCE_HFOV = "SOURCE.CAMERA.CONFIDENCE.HFOV";
        public const String SOURCE_CAMERA_CONFIDENCE_VFOV = "SOURCE.CAMERA.CONFIDENCE.VFOV";
        public const String SOURCE_CAMERA_CONFIDENCE_Width = "SOURCE.CAMERA.CONFIDENCE.Width";
        public const String SOURCE_CAMERA_CONFIDENCE_Height = "SOURCE.CAMERA.CONFIDENCE.Height";

        public const String SOURCE_CAMERA_COLOR_HFOV = "SOURCE.CAMERA.COLOR.HFOV";
        public const String SOURCE_CAMERA_COLOR_VFOV = "SOURCE.CAMERA.COLOR.VFOV";
        public const String SOURCE_CAMERA_COLOR_Width = "SOURCE.CAMERA.COLOR.Width";
        public const String SOURCE_CAMERA_COLOR_Height = "SOURCE.CAMERA.COLOR.Height";

        public const String SOURCE_CAMERA_COLOR_REGISTRATION_Mode = "SOURCE.CAMERA.COLOR.REGISTRATION.Mode";

        public const int COLOR_REGISTRATION_DISABLED = 0;
        public const int COLOR_REGISTRATION_DEPTHCORRECTION = 1;
        public const int COLOR_REGISTRATION_UVMAP = 2;

        public const String SOURCE_FILTER_RECONSTRUCTION_Enabled = "SOURCE.FILTER.RECONSTRUCTION.Enabled";
        public const String SOURCE_FILTER_RECONSTRUCTION_MaxThreshold = "SOURCE.FILTER.RECONSTRUCTION.MaxThreshold";
        public const String SOURCE_FILTER_MinDepth = "SOURCE.FILTER.MinDepth";
        public const String SOURCE_FILTER_MaxDepth = "SOURCE.FILTER.MaxDepth";

        public const String SOURCE_FILTER_CONFIDENCE_Enabled = "SOURCE.FILTER.CONFIDENCE.Enabled";

        public const String SOURCE_FILTER_CONFIDENCE_MinThreshold = "SOURCE.FILTER.CONFIDENCE.MinThreshold";
        public const String SOURCE_FILTER_SMOOTHING_Enabled = "SOURCE.FILTER.SMOOTHING.Enabled";


        // DepthSense specific
        public const String SOURCE_DEPTHSENSE_AmplitudeThreshold = "SOURCE.DEPTHSENSE.AmplitudeThreshold";
        public const String SOURCE_DEPTHSENSE_LightIntensity = "SOURCE.DEPTHSENSE.LightIntensity";


        public const String SOURCE_CAMERA_DEPTH_Image = "SOURCE.CAMERA.DEPTH.Image";
        public const String SOURCE_CAMERA_CONFIDENCE_Image = "SOURCE.CAMERA.CONFIDENCE.Image";

        public const String SOURCE_CAMERA_COLOR_Image = "SOURCE.CAMERA.COLOR.Image";
        public const String SOURCE_CAMERA_COLOR_REGISTRATION_COLOR_Image = "SOURCE.CAMERA.COLOR.REGISTRATION.COLOR.Image";
        public const String SOURCE_CAMERA_COLOR_REGISTRATION_UV_Image = "SOURCE.CAMERA.COLOR.REGISTRATION.UV.Image";


        public const String SOURCE_Acceleration = "SOURCE.Acceleration";

        public const String CI_Enabled = "CI.Enabled";
        public const String CI_STABILIZATION_Enabled = "CI.STABILIZATION.Enabled";
        public const String CI_STABILIZATION_Strength = "CI.STABILIZATION.Strength";

        public const String CI_Big5ActivationEnabled = "CI.Big5ActivationEnabled";
        public const String CI_ActivationMaxDistance = "CI.ActivationMaxDistance";

        public const String CI_SceneLabelImage = "CI.SceneLabelImage";
        public const String CI_SceneLabelImage_BackgroundLabel = "CI.SceneLabelImage.BackgroundLabel";

        public const String CI_HAND1_Status = "CI.HAND1.Status";
        public const String CI_HAND2_Status = "CI.HAND2.Status";

        public const String CI_HAND1_IsOpen = "CI.HAND1.IsOpen";
        public const String CI_HAND2_IsOpen = "CI.HAND2.IsOpen";

        public const String CI_HAND1_Openness = "CI.HAND1.Openness";
        public const String CI_HAND2_Openness = "CI.HAND2.Openness";

        public const String CI_HAND1_Label = "CI.HAND1.Label";
        public const String CI_HAND2_Label = "CI.HAND2.Label";

        public const String CI_HAND1_PosingGestureId = "CI.HAND1.PosingGestureId";
        public const String CI_HAND2_PosingGestureId = "CI.HAND2.PosingGestureId";

        // Intersecting
        public const String CI_HAND1_IsIntersectingUpperImageBoundary = "CI.HAND1.IsIntersectingUpperImageBoundary";
        public const String CI_HAND2_IsIntersectingUpperImageBoundary = "CI.HAND2.IsIntersectingUpperImageBoundary";

        public const String CI_HAND1_IsIntersectingLowerImageBoundary = "CI.HAND1.IsIntersectingLowerImageBoundary";
        public const String CI_HAND2_IsIntersectingLowerImageBoundary = "CI.HAND2.IsIntersectingLowerImageBoundary";

        public const String CI_HAND1_IsIntersectingLeftImageBoundary = "CI.HAND1.IsIntersectingLeftImageBoundary";
        public const String CI_HAND2_IsIntersectingLeftImageBoundary = "CI.HAND2.IsIntersectingLeftImageBoundary";

        public const String CI_HAND1_IsIntersectingRightImageBoundary = "CI.HAND1.IsIntersectingRightImageBoundary";
        public const String CI_HAND2_IsIntersectingRightImageBoundary = "CI.HAND2.IsIntersectingRightImageBoundary";
        
        
        // Tip Positions
        public const String CI_HAND1_TipPosition2D = "CI.HAND1.TipPosition2D";
        public const String CI_HAND2_TipPosition2D = "CI.HAND2.TipPosition2D";

        public const String CI_HAND1_TipPosition3D = "CI.HAND1.TipPosition3D";
        public const String CI_HAND2_TipPosition3D = "CI.HAND2.TipPosition3D";


        // Forearm
        public const String CI_HAND1_ForearmPosition2D = "CI.HAND1.ForearmPosition2D";
        public const String CI_HAND2_ForearmPosition2D = "CI.HAND2.ForearmPosition2D";

        public const String CI_HAND1_ForearmPosition3D = "CI.HAND1.ForearmPosition3D";
        public const String CI_HAND2_ForearmPosition3D = "CI.HAND2.ForearmPosition3D";


        // Palm
        public const String CI_HAND1_PalmNormal3D = "CI.HAND1.PalmNormal3D";
        public const String CI_HAND2_PalmNormal3D = "CI.HAND2.PalmNormal3D";

        public const String CI_HAND1_PalmPosition2D = "CI.HAND1.PalmPosition2D";
        public const String CI_HAND2_PalmPosition2D = "CI.HAND2.PalmPosition2D";

        public const String CI_HAND1_PalmPosition3D = "CI.HAND1.PalmPosition3D";
        public const String CI_HAND2_PalmPosition3D = "CI.HAND2.PalmPosition3D";


        // Fingers
        public const String CI_HAND1_FingerStatus = "CI.HAND1.FingerStatus";
        public const String CI_HAND2_FingerStatus = "CI.HAND2.FingerStatus";

        public const String CI_HAND1_FingerTipPositions2D = "CI.HAND1.FingerTipPositions2D";
        public const String CI_HAND2_FingerTipPositions2D = "CI.HAND2.FingerTipPositions2D";

        public const String CI_HAND1_FingerTipPositions3D = "CI.HAND1.FingerTipPositions3D";
        public const String CI_HAND2_FingerTipPositions3D = "CI.HAND2.FingerTipPositions3D";


        // Hand Centroids
        public const String CI_CENTROIDS_Enabled = "CI.CENTROIDS.Enabled";
        public const String CI_CENTROIDS_Count = "CI.CENTROIDS.Count";

        public const String CI_HAND1_CENTROIDS_Positions = "CI.HAND1.CENTROIDS.Positions";
        public const String CI_HAND2_CENTROIDS_Positions = "CI.HAND2.CENTROIDS.Positions";

        public const String CI_HAND1_CENTROIDS_JumpStatus = "CI.HAND1.CENTROIDS.JumpStatus";
        public const String CI_HAND2_CENTROIDS_JumpStatus = "CI.HAND2.CENTROIDS.JumpStatus";

        public const int CENTROIDS_JUMPSTATUS_TIMECOHERENT = 0;
        public const int CENTROIDS_JUMPSTATUS_JUMP = 1;


        // Hand Mesh

        /* Each hand mesjh is provided as:
         * - two arrays of 2D and 3D coordinates at the points of the mesh 
         *   (respectively CI.HAND#.MESH.Points2D and CI.HAND#.MESH.Points3D)
         * - an array of normal vectors to the 3D points (CI.HAND#.MESH.Normals) , and
         * - an array of intensities at the points (CI.HAND#.MESH.Intensities). 
         *   These intensities are values from 0 to 1 which might be used to color the mesh. 
         *   It is possible to map these intensities to a color map.
        */

        public const String CI_HAND1_MESH_Points2D = "CI.HAND1.MESH.Points2D";
        public const String CI_HAND2_MESH_Points2D = "CI.HAND2.MESH.Points2D";

        public const String CI_HAND1_MESH_Points3D = "CI.HAND1.MESH.Points3D";
        public const String CI_HAND2_MESH_Points3D = "CI.HAND2.MESH.Points3D";

        public const String CI_HAND1_MESH_Normals = "CI.HAND1.MESH.Normals";
        public const String CI_HAND2_MESH_Normals = "CI.HAND2.MESH.Normals";
        
        public const String CI_HAND1_MESH_Intensities = "CI.HAND1.MESH.Intensities";
        public const String CI_HAND2_MESH_Intensities = "CI.HAND2.MESH.Intensities";

        public const String CI_HAND1_MESH_Triangles = "CI.HAND1.MESH.Triangles";
        public const String CI_HAND2_MESH_Triangles = "CI.HAND2.MESH.Triangles";

        public const String CI_HAND1_MESH_Contour = "CI.HAND1.MESH.Contour";
        public const String CI_HAND2_MESH_Contour = "CI.HAND2.MESH.Contour";

        
        // CI Gestures
        public const String CI_GESTURES_Enabled = "CI.GESTURES.Enabled";


        // hand / fingetr states
        
        // inactive: self-explanatory (.. oder wie dat auf englisch heisst ..)
        public const int STATUS_INACTIVE = 0;

        // detected: detected at current frame. 
        // No temporal coherence with the same data at previous frame.
        public const int STATUS_DETECTED = 1;
        
        // tracked: detected at current frame and 
        // temporal coherence with same data at previous frame
        public const int STATUS_TRACKED = 2;


    }
}
