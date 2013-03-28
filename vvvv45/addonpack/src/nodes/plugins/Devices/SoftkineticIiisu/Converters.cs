using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

using VVVV.Utils.VMath;
using Iisu;
using Iisu.Data;

namespace VVVV.Nodes
{
    public static class Converters
    {
        public static Vector3D ToVector3(this Iisu.Data.Vector3 vector)
        {
            return new Vector3D(vector.X, vector.Z, vector.Y);
        }

        public static Vector2D ToVector2(this Iisu.Data.Vector2 vector)
        {
            return new Vector2D(vector.X, vector.Y);
        }

        public static Vector3D[] ToVector3Array(this Iisu.Data.Vector3[] vectorArray)
        {
            if (vectorArray == null) return null;

            Vector3D[] res = new Vector3D[vectorArray.Length];
            for (int i = 0; i < res.Length; i++)
                res[i] = vectorArray[i].ToVector3();
            return res;
        }

        public static Vector2D[] ToVector2Array(this Iisu.Data.Vector2[] vectorArray)
        {
            if (vectorArray == null) return null;

            Vector2D[] res = new Vector2D[vectorArray.Length];
            for (int i = 0; i < res.Length; i++)
                res[i] = vectorArray[i].ToVector2();
            return res;
        }

        public static ImageConvertor convertor = new ImageConvertor();
        public static void ToDepthValues(this IImageData imageRaw, ref double[] depthArray)
        {
            convertor.generateDepthArray(imageRaw, ref depthArray);
        }

    }



    /// <summary>
    /// Helper class to convert iisu images to Unity images. 
    /// </summary>
    public class ImageConvertor
    {
        private int _width;
        private int _height;

        private byte[] imageRaw;
        private byte[] idImageRaw;

        //these two values are used to map the depth of a hand pixel to a color
        private float minDepth = 10;
        private float maxDepth = 40;

        private float floatConvertor = 1f / 255f;

        public ImageConvertor(int width, int height)
        {
            _width = width;
            _height = height;
        }

        public ImageConvertor()
        {
            _width = 160;
            _height = 120;
        }

        private void getUVEquivalent(int fromWidth, int fromHeight, int fromU, int fromV, int toWidth, int toHeight, out int toU, out int toV, out int toIndex)
        {
            float uNorm = (float)fromU / (float)fromWidth;
            float vNorm = (float)fromV / (float)fromHeight;

            toU = (int)(uNorm * toWidth);
            toV = (int)(vNorm * toHeight);

            toIndex = toU + toV * toWidth;
        }

        private void getUV(int index, int width, int height, out int u, out int v)
        {
            u = index % width;
            v = index / width;
        }

        private double[] depthArray;

        public bool generateDepthArray(IImageData image, ref double[] depth)
        {
            try
            {

                if (image == null)
                    return false;

                if (depthArray == null || depthArray.Length != image.ImageInfos.BytesRaw / 2)
                {
                    depthArray = new double[_width * _height];
                    imageRaw = new byte[image.ImageInfos.BytesRaw];
                }

                uint byte_size = (uint)image.ImageInfos.BytesRaw;
                Marshal.Copy(image.Raw, imageRaw, 0, (int)byte_size);

                int destinationU, destinationV;
                int sourceU, sourceV;
                int sourceIndex;

                int imageWidth = (int)image.ImageInfos.Width;
                int imageHeight = (int)image.ImageInfos.Height;

                //build up the user mask
                for (int destinationIndex = 0; destinationIndex < depthArray.Length; ++destinationIndex)
                {
                    //get the UV coordinates from the final texture that will be displayed
                    getUV(destinationIndex, _width, _height, out destinationU, out destinationV);

                    //the resolutions of the depth and label image can differ from the final texture, 
                    //so we have to apply some remapping to get the equivalent UV coordinates in the depth and label image.
                    getUVEquivalent(_width, _height, destinationU, destinationV, imageWidth, imageHeight, out sourceU, out sourceV, out sourceIndex);

                    // reconstruct ushort value from 2 bytes (low indian)
                    ushort value = (ushort)(imageRaw[sourceIndex * 2] + (imageRaw[sourceIndex * 2 + 1] << 8));

                    //normalize depth value in millimeter so that 5m <-> color 255
                    value = (ushort)(value * 255 / (5000));
                    if (value > 255) value = 255;

                    depthArray[destinationIndex] = value;
                }

                depth = depthArray;
                return true;
            }
            // HACK - manchmal sind die Bilddaten-Arrays am Anfang falsch befüllt
            catch (Exception)
            {
                return false;
            }
        }

        /*
        public bool generateHandMask(IImageData image, IImageData idImage, ref Texture2D destinationImage, int hand1ID, int hand2ID)
        {
            if (image == null || idImage == null)
                return false;

            if (image.Raw == IntPtr.Zero || idImage.Raw == IntPtr.Zero)
                return false;

            if (_colored_image == null || _colored_image.Length != image.ImageInfos.BytesRaw / 2)
            {
                _colored_image = new Color[_width * _height];
                imageRaw = new byte[image.ImageInfos.BytesRaw];
            }

            if (idImageRaw == null || idImageRaw.Length != idImage.ImageInfos.BytesRaw)
            {
                idImageRaw = new byte[idImage.ImageInfos.BytesRaw];
            }

            uint byte_size = (uint)image.ImageInfos.BytesRaw;
            uint labelImageSize = (uint)idImage.ImageInfos.BytesRaw;

            // copy image content into managed arrays
            Marshal.Copy(image.Raw, imageRaw, 0, (int)byte_size);
            Marshal.Copy(idImage.Raw, idImageRaw, 0, (int)labelImageSize);

            int destinationU, destinationV;
            int sourceU, sourceV;
            int sourceIndex;
            int labelU, labelV;
            int labelIndex;

            int imageWidth = (int)image.ImageInfos.Width;
            int imageHeight = (int)image.ImageInfos.Height;
            int idImageWidth = (int)idImage.ImageInfos.Width;
            int idImageHeight = (int)idImage.ImageInfos.Height;

            //build up the user mask
            for (int destinationIndex = 0; destinationIndex < _colored_image.Length; ++destinationIndex)
            {
                //get the UV coordinates from the final texture that will be displayed
                getUV(destinationIndex, _width, _height, out destinationU, out destinationV);

                //the resolutions of the depth and label image can differ from the final texture, 
                //so we have to apply some remapping to get the equivalent UV coordinates in the depth and label image.
                getUVEquivalent(_width, _height, destinationU, destinationV, imageWidth, imageHeight, out sourceU, out sourceV, out sourceIndex);
                getUVEquivalent(_width, _height, destinationU, destinationV, idImageWidth, idImageHeight, out labelU, out labelV, out labelIndex);

                // reconstruct ushort value from 2 bytes (low indian)
                ushort value = (ushort)(imageRaw[sourceIndex * 2] + (imageRaw[sourceIndex * 2 + 1] << 8));

                //normalize depth value in millimeter so that 5m <-> color 255
                value = (ushort)(value * 255 / (5000));
                if (value > 255) value = 255;

                _colored_image[destinationIndex] = getColor(idImageRaw[labelIndex], hand1ID, hand2ID, value);
            }

            destinationImage.SetPixels(_colored_image);
            destinationImage.Apply();

            return true;

        }
        */
    }
}
