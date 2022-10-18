﻿using GTA5OnlineTools.Common.Utils;
using GTA5OnlineTools.Features.Core;

using GTA5OnlineTools.Modules.SpeedMeter;

namespace GTA5OnlineTools.Modules;

/// <summary>
/// SpeedMeterWindow.xaml 的交互逻辑
/// </summary>
public partial class SpeedMeterWindow
{
    private DrawWindow DrawWindow = null;

    public SpeedMeterWindow()
    {
        InitializeComponent();

        Task.Run(() =>
        {
            this.Dispatcher.Invoke((Delegate)(() =>
            {
                var windowData = GTA5Mem.GetGameWindowData();

                TextBlock_ScreenResolution.Text = $"屏幕分辨率 {SystemParameters.PrimaryScreenWidth} x {SystemParameters.PrimaryScreenHeight}";
                TextBlock_GameResolution.Text = $"游戏分辨率 {windowData.Width} x {windowData.Height}";
                TextBlock_ScreenScale.Text = $"缩放比例 {ScreenMgr.GetScalingRatio()}";
            }));
        });

        if (DrawWindow != null)
        {
            DrawWindow.Close();
            DrawWindow = null;
        }
    }

    private void Button_RunDraw_Click(object sender, RoutedEventArgs e)
    {
        AudioUtil.PlayClickSound();

        GTA5Mem.SetForegroundWindow();

        if (DrawWindow == null)
        {
            DrawWindow = new DrawWindow();
            DrawWindow.Show();
        }

        var windowData = GTA5Mem.GetGameWindowData();

        TextBlock_ScreenResolution.Text = $"屏幕分辨率 {SystemParameters.PrimaryScreenWidth} x {SystemParameters.PrimaryScreenHeight}";
        TextBlock_GameResolution.Text = $"游戏分辨率 {windowData.Width} x {windowData.Height}";
        TextBlock_ScreenScale.Text = $"缩放比例 {ScreenMgr.GetScalingRatio()}";
    }

    private void Button_StopDraw_Click(object sender, RoutedEventArgs e)
    {
        AudioUtil.PlayClickSound();

        if (DrawWindow != null)
        {
            DrawWindow.Close();
            DrawWindow = null;
        }

        var windowData = GTA5Mem.GetGameWindowData();

        TextBlock_ScreenResolution.Text = $"屏幕分辨率 {SystemParameters.PrimaryScreenWidth} x {SystemParameters.PrimaryScreenHeight}";
        TextBlock_GameResolution.Text = $"游戏分辨率 {windowData.Width} x {windowData.Height}";
        TextBlock_ScreenScale.Text = $"缩放比例 {ScreenMgr.GetScalingRatio()}";
    }

    private void RadioButton_SpeedMeterPos_Center_Click(object sender, RoutedEventArgs e)
    {
        if (RadioButton_SpeedMeterPos_Center.IsChecked == true)
        {
            DrawData.IsDrawCenter = true;
        }
        else
        {
            DrawData.IsDrawCenter = false;
        }
    }

    private void RadioButton_SpeedMeterUnit_MPH_Click(object sender, RoutedEventArgs e)
    {
        if (RadioButton_SpeedMeterUnit_MPH.IsChecked == true)
        {
            DrawData.IsShowMPH = true;
        }
        else
        {
            DrawData.IsShowMPH = false;
        }
    }
}
