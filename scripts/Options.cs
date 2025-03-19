using Godot;
using System;

public partial class Options : Control
{
	private int MasterBus = AudioServer.GetBusIndex("Master");

	private CheckButton fullscreenBtn;
	private HSlider volumeSlider;

	public override void _Ready()
	{
		fullscreenBtn = GetNode<CheckButton>("MenuOptions/MarginContainer/VBoxContainer/Fullscreen");
		volumeSlider = GetNode<HSlider>("MenuOptions/MarginContainer/VBoxContainer/VolumeSlider");

		fullscreenBtn.ButtonPressed = DisplayServer.WindowGetMode() == DisplayServer.WindowMode.Fullscreen;

		float currentVolumeDb = AudioServer.GetBusVolumeDb(MasterBus);
		volumeSlider.Value = Mathf.Clamp((float)currentVolumeDb, (float)volumeSlider.MinValue, (float)volumeSlider.MaxValue);
	}

	private void _OnBackPressed()
	{
		GetTree().ChangeSceneToFile("res://scenes/menu.tscn");
	}

	private void _OnFullscreenToggled(bool buttonPressed)
	{
		if (buttonPressed)
		{
			DisplayServer.WindowSetMode(DisplayServer.WindowMode.Fullscreen);
		}
		else if (!buttonPressed)
		{
			DisplayServer.WindowSetMode(DisplayServer.WindowMode.Windowed);
		}
	}

	private void _OnVolumeSliderValueChanged(int value)
	{
		AudioServer.SetBusVolumeDb(MasterBus, value);

		if (value == -30)
		{
			AudioServer.SetBusMute(MasterBus, true);
		}
		else
		{
			AudioServer.SetBusMute(MasterBus, false);
		}
	}
}
