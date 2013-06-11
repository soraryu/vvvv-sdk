#region usings
using System;
using System.ComponentModel.Composition;

using VVVV.PluginInterfaces.V1;
using VVVV.PluginInterfaces.V2;
using VVVV.Utils.VColor;
using VVVV.Utils.VMath;

using VVVV.Core.Logging;

#endregion usings

namespace VVVV.Nodes
{
	#region PluginInfo
	[PluginInfo(Name = "RazerHydra", Category = "Devices", Help = "Access Razer Hydra data", Tags = "", Author = "herbst")]
	#endregion PluginInfo
	public class DevicesRazerHydraNode : IPluginEvaluate, IPartImportsSatisfiedNotification
	{
		#region fields & pins
		[Input("Enabled")]
		IDiffSpread<bool> FEnabled;

		[Output("Controller")]
		ISpread<SixenseInput.Controller> FController;

		[Output("IsBaseConnected")]
		ISpread<bool> FIsBaseConnected;

		#endregion fields & pins
		
		SixenseInput sixense = null;
		bool lastState = false;
		public void CheckForChanges() {
			FController.SliceCount = 0;
			for(int i = 0; i < SixenseInput.Controllers.Length; i++) {
				SixenseInput.Controller c = SixenseInput.Controllers[i];
				if(c != null && c.Enabled)
					FController.Add(SixenseInput.Controllers[i]);
			}
		}
		
		public void OnImportsSatisfied() {
			FIsBaseConnected.SliceCount = 1;
			
			FEnabled.Changed += s => {
				if(s[0]) {
					if(sixense != null) {
						sixense.Dispose();
						sixense = null;
					}
					sixense = new SixenseInput();
					sixense.Start();
					
					FController.SliceCount = 0;
					for(int i = 0; i < SixenseInput.Controllers.Length; i++) {
						SixenseInput.Controller c = SixenseInput.Controllers[i];
						if(c != null && c.Enabled)
							FController.Add(SixenseInput.Controllers[i]);
					}
				}
				else {
					if(sixense != null) {
						sixense.Dispose();
						sixense = null;
						
						FController.SliceCount = 0;
					}
				}
			};
		}
		
		//called when data for any output pin is requested
		public void Evaluate(int SpreadMax)
		{
			if(sixense != null)
			{
				sixense.Update();
				FIsBaseConnected[0] = SixenseInput.IsBaseConnected(0);
				
				CheckForChanges();
			}
			else
				FIsBaseConnected[0] = false;			
			
		}
	}
	
	#region PluginInfo
	[PluginInfo(Name = "RazerHydra Controller", Category = "Devices", Help = "Access Razer Hydra data", Tags = "", Author = "herbst")]
	#endregion PluginInfo
	public class DevicesRazerHydraControllerNode : IPluginEvaluate
	{
		#region fields & pins
		[Input("Controller")]
		ISpread<SixenseInput.Controller> FController;
		
		[Input("TriggerButtonThreshold", DefaultValue = 0.9)]
		public ISpread<double> TriggerButtonThreshold;
		
		[Output("Enabled")]
		ISpread<bool> Enabled;
		[Output("Docked")]
		ISpread<bool> Docked;
		[Output("Hand")]
		ISpread<SixenseHands> Hand;
		[Output("Trigger")]
		ISpread<double> Trigger;
		[Output("Joystick")]
		ISpread<Vector2D>  Joystick;
		[Output("Position")]
		ISpread<Vector3D>  Position;
		[Output("Rotation")]
		ISpread<Vector4D>  Rotation;
		
		// Buttons
		[Output("Button Start")]
		ISpread<bool> Start;
		[Output("Button One")]
		ISpread<bool> One;
		[Output("Button Two")]
		ISpread<bool> Two;
		[Output("Button Three")]
		ISpread<bool> Three;
		[Output("Button Four")]
		ISpread<bool> Four;
		[Output("Button Bumper")]
		ISpread<bool> Bumper;
		//[Output("Button Joystick")] // no idea what "Button Joystick" is
		//ISpread<bool> BJoystick;
		[Output("Button Trigger")]
		ISpread<bool> BTrigger;
		
		[Import()]
		ILogger FLogger;
		#endregion fields & pins
		
		//called when data for any output pin is requested
		public void Evaluate(int SpreadMax)
		{
			int slicecount = (FController.SliceCount != 0 && FController[0] != null) ? FController.SliceCount : 0;

			Enabled.SliceCount = slicecount;
			Docked.SliceCount = slicecount;
			Hand.SliceCount = slicecount;
			Trigger.SliceCount = slicecount;
			Joystick.SliceCount = slicecount;
			Position.SliceCount = slicecount;
			Rotation.SliceCount = slicecount;
			
			Start.SliceCount = slicecount;
			One.SliceCount = slicecount;
			Two.SliceCount = slicecount;
			Three.SliceCount = slicecount;
			Four.SliceCount = slicecount;
			Bumper.SliceCount = slicecount;
			//BJoystick.SliceCount = slicecount;
			BTrigger.SliceCount = slicecount;
			
			if(slicecount == 0) return;
			
			for(int i = 0; i < slicecount; i++) {
				SixenseInput.Controller controller = FController[i];
				if(controller == null) continue;
				
				controller.TriggerButtonThreshold = TriggerButtonThreshold[i];
				
				Enabled[i] = controller.Enabled;
				Docked[i] = controller.Docked;
				Hand[i] = controller.Hand;
				Trigger[i] = controller.Trigger;
				Joystick[i] = new Vector2D(controller.JoystickX, controller.JoystickY);
				Position[i] = controller.Position;
				Rotation[i] = controller.Rotation;
				
				Start[i] = controller.GetButton(SixenseButtons.START);
				One[i] = controller.GetButton(SixenseButtons.ONE);
				Two[i] = controller.GetButton(SixenseButtons.TWO);
				Three[i] = controller.GetButton(SixenseButtons.THREE);
				Four[i] = controller.GetButton(SixenseButtons.FOUR);
				Bumper[i] = controller.GetButton(SixenseButtons.BUMPER);
				//BJoystick[i] = controller.GetButton(SixenseButtons.JOYSTICK);
				BTrigger[i] = controller.GetButton(SixenseButtons.TRIGGER);	
			}
			
		}
	}
}
