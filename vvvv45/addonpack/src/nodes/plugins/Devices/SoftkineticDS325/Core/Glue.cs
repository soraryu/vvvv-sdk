/*
 * Created by SharpDevelop.
 * User: felix
 * Date: 06.05.2013
 * Time: 23:13
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;

namespace VVVV.Nodes.DS325
{
	/// <summary>
	/// Description of Class1.
	/// </summary>
	public static class Glue
	{
		public static VVVV.Utils.VMath.Vector3D ToVector3(this PXCMPoint3DF32 point) {
			return new VVVV.Utils.VMath.Vector3D(point.x, point.y, point.z);
		}
	}
}
