﻿using GTA5OnlineTools.Views;
using GTA5OnlineTools.Models;
using GTA5OnlineTools.Windows;
using GTA5OnlineTools.Features;
using GTA5OnlineTools.Features.SDK;
using GTA5OnlineTools.Features.Core;
using GTA5OnlineTools.Common.Data;
using GTA5OnlineTools.Common.Utils;
using GTA5OnlineTools.Common.Helper;

using RestSharp;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Hardcodet.Wpf.TaskbarNotification;

namespace GTA5OnlineTools;

/// <summary>
/// MainWindow.xaml 的交互逻辑
/// </summary>
public partial class MainWindow
{
    /// <summary>
    /// 主窗口数据模型
    /// </summary>
    public MainModel MainModel { get; set; } = new();

    /// <summary>
    /// 主窗口页面导航命令
    /// </summary>
    public RelayCommand<string> NavigateCommand { get; private set; }

    /// <summary>
    /// 当前View名称
    /// </summary>
    private string CurrentViewName = string.Empty;

    /// <summary>
    /// 用户控件，用于视图切换
    /// </summary>
    private readonly HomeView HomeView = new();
    private readonly CheatsView ThirdView = new();
    private readonly ModulesView HacksView = new();
    private readonly ToolsView ToolsView = new();
    private readonly OptionView OptionView = new();
    private readonly AboutView AboutView = new();

    ///////////////////////////////////////////////////////////////

    /// <summary>
    /// 主窗口关闭委托
    /// </summary>
    public delegate void WindowClosingDelegate();
    /// <summary>
    /// 主窗口关闭事件
    /// </summary>
    public static event WindowClosingDelegate WindowClosingEvent;

    /// <summary>
    /// 用于向外暴露主窗口实例
    /// </summary>
    public static Window MainWindowInstance { get; private set; } = null;

    /// <summary>
    /// 发送任务栏通知消息委托
    /// </summary>
    public static Action<string> ActionShowNoticeInfo;

    /// <summary>
    /// 存储软件开始运行的时间
    /// </summary>
    private DateTime Origin_DateTime;

    ///////////////////////////////////////////////////////////////

    public MainWindow()
    {
        InitializeComponent();
    }

    /// <summary>
    /// 窗口加载完成事件
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void Window_Main_Loaded(object sender, RoutedEventArgs e)
    {
        // 设置当前上下文数据
        this.DataContext = this;
        // 向外暴露主窗口实例
        MainWindowInstance = this;

        // 初始化主数据模型
        NavigateCommand = new(Navigate);
        // 首页导航
        Navigate("HomeView");

        // 获取当前时间，存储到对于变量中
        Origin_DateTime = DateTime.Now;

        ///////////////////////////////////////////////////////////////

        // 设置主窗口标题
        this.Title = CoreUtil.MainAppWindowName + CoreUtil.ClientVersion;

        MainModel.AppRunTime = "00:00:00";
        MainModel.AppVersion = $"{CoreUtil.ClientVersion}";

        ActionShowNoticeInfo = ShowNoticeInfo;

        // 更新主窗口UI线程
        new Thread(UpdateUiThread)
        {
            Name = "UpdateUiThread",
            IsBackground = true
        }.Start();

        // 检查GTA5是否运行线程
        new Thread(CheckGTA5IsRunThread)
        {
            Name = "CheckGTA5IsRunThread",
            IsBackground = true
        }.Start();

        // 检查更新线程
        new Thread(CheckUpdateThread)
        {
            Name = "CheckUpdateThread",
            IsBackground = true
        }.Start();
    }

    /// <summary>
    /// 窗口关闭事件
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void Window_Main_Closing(object sender, CancelEventArgs e)
    {
        // 终止线程内循环
        Globals.IsAppRunning = false;

        WindowClosingEvent();
        LoggerHelper.Info("调用主窗口关闭事件成功");

        GTA5Mem.CloseHandle();
        LoggerHelper.Info("释放内存模块进程句柄成功");

        ProcessUtil.CloseThirdProcess();
        LoggerHelper.Info("关闭第三方进程成功");
    }

    ///////////////////////////////////////////////////////////////

    /// <summary>
    /// View页面导航
    /// </summary>
    /// <param name="viewName"></param>
    private void Navigate(string viewName)
    {
        if (viewName == null || string.IsNullOrEmpty(viewName))
            return;

        // 防止重复触发
        if (CurrentViewName != viewName)
            CurrentViewName = viewName;
        else
            return;

        switch (viewName)
        {
            case "HomeView":
                ContentControl_Main.Content = HomeView;
                break;
            case "ThirdView":
                ContentControl_Main.Content = ThirdView;
                break;
            case "HacksView":
                ContentControl_Main.Content = HacksView;
                break;
            case "ToolsView":
                ContentControl_Main.Content = ToolsView;
                break;
            case "OptionView":
                ContentControl_Main.Content = OptionView;
                break;
            case "AboutView":
                ContentControl_Main.Content = AboutView;
                break;
        }
    }

    /// <summary>
    /// 更新主窗口UI线程
    /// </summary>
    private void UpdateUiThread()
    {
        while (Globals.IsAppRunning)
        {
            // 获取软件运行时间
            MainModel.AppRunTime = CoreUtil.ExecDateDiff(Origin_DateTime, DateTime.Now);

            Thread.Sleep(1000);
        }
    }

    /// <summary>
    /// 检查GTA5是否运行线程
    /// </summary>
    private void CheckGTA5IsRunThread()
    {
        bool isExecute = true;

        while (Globals.IsAppRunning)
        {
            // 判断 GTA5 是否运行
            MainModel.IsGTA5Run = ProcessUtil.IsGTA5Run();

            if (MainModel.IsGTA5Run)
            {
                isExecute = false;

                if (GTA5Mem.GTA5ProHandle == IntPtr.Zero)
                {
                    if (GTA5Mem.Initialize())
                    {
                        GTA5MaskInit();
                    }
                }
                else
                {
                    GTA5MaskInit();
                }
            }
            else
            {
                // 下列功能只会在GTA5停止运行时执行一次，直到下一次GTA5停止运行
                if (!isExecute)
                {
                    isExecute = true;

                    GTA5Mem.CloseHandle();
                    ModulesView.ActionCloseAllModulesWindow();
                }
            }

            Thread.Sleep(1000);
        }
    }

    /// <summary>
    /// 检查更新线程
    /// </summary>
    private async void CheckUpdateThread()
    {
        try
        {
            // 刷新DNS缓存
            CoreUtil.FlushDNSCache();
            LoggerHelper.Info("刷新DNS缓存成功");

            LoggerHelper.Info("正在检测版本更新...");
            this.Dispatcher.Invoke(() =>
            {
                NotifierHelper.Show(NotifierType.Notification, "正在检测版本更新...");
            });

            // 检测版本更新
            var options = new RestClientOptions("https://api.crazyzhang.cn")
            {
                MaxTimeout = 9000,
                FollowRedirects = true
            };
            var client = new RestClient(options);

            var request = new RestRequest("/update/config.json");
            var response = await client.ExecuteGetAsync(request);
            if (response.StatusCode == HttpStatusCode.OK)
            {
                // 解析web返回的数据
                CoreUtil.UpdateInfo = JsonUtil.JsonDese<UpdateInfo>(response.Content);
                // 获取对应数据
                CoreUtil.ServerVersion = Version.Parse(CoreUtil.UpdateInfo.Version);

                // 获取最新公告
                request = new RestRequest("/update/server/notice.txt");
                response = await client.ExecuteGetAsync(request);
                if (response.StatusCode == HttpStatusCode.OK)
                    WeakReferenceMessenger.Default.Send(response.Content, "Notice");
                else
                    WeakReferenceMessenger.Default.Send("网络异常，获取最新公告内容失败！这并不影响小助手程序使用", "Notice");

                // 获取更新日志
                request = new RestRequest("/update/server/change.txt");
                response = await client.ExecuteGetAsync(request);
                if (response.StatusCode == HttpStatusCode.OK)
                    WeakReferenceMessenger.Default.Send(response.Content, "Change");
                else
                    WeakReferenceMessenger.Default.Send("网络异常，获取更新日志信息失败！这并不影响小助手程序使用", "Change");

                // 如果线上版本号大于本地版本号，则提示更新
                if (CoreUtil.ServerVersion > CoreUtil.ClientVersion)
                {
                    // 打开更新对话框
                    this.Dispatcher.Invoke(() =>
                    {
                        var UpdateWindow = new UpdateWindow
                        {
                            // 设置父窗口
                            Owner = this
                        };
                        // 以对话框形式显示更新窗口
                        UpdateWindow.ShowDialog();
                    });
                }
                else
                {
                    LoggerHelper.Info($"当前已是最新版本 {CoreUtil.ServerVersion}");
                    this.Dispatcher.Invoke(() =>
                    {
                        NotifierHelper.Show(NotifierType.Notification, $"当前已是最新版本 {CoreUtil.ServerVersion}");
                    });
                }
            }
            else
            {
                LoggerHelper.Error("网络异常");
                this.Dispatcher.Invoke(() =>
                {
                    NotifierHelper.Show(NotifierType.Error, "网络异常，这并不影响小助手程序使用");
                });

                WeakReferenceMessenger.Default.Send("网络异常，获取最新公告内容失败！这并不影响小助手程序使用", "Notice");
                WeakReferenceMessenger.Default.Send("网络异常，获取更新日志信息失败！这并不影响小助手程序使用", "Change");
            }
        }
        catch (Exception ex)
        {
            LoggerHelper.Error($"初始化错误", ex);
            this.Dispatcher.Invoke(() =>
            {
                NotifierHelper.Show(NotifierType.Error, $"初始化错误\n{ex.Message}");
            });

            WeakReferenceMessenger.Default.Send("获取最新公告内容失败！", "Notice");
            WeakReferenceMessenger.Default.Send("获取更新日志信息失败！", "Change");
        }
    }

    /// <summary>
    /// 发送通知栏提示信息
    /// </summary>
    /// <param name="msg"></param>
    private void ShowNoticeInfo(string msg)
    {
        TaskbarIcon_Main.ShowBalloonTip("提示", msg, BalloonIcon.Info);
    }

    /// <summary>
    /// GTA5特征码寻址
    /// </summary>
    private void GTA5MaskInit()
    {
        if (General.WorldPTR == 0)
        {
            General.WorldPTR = GTA5Mem.FindPattern(Offsets.Mask.WorldMask);
            General.WorldPTR = GTA5Mem.Rip_37(General.WorldPTR);
        }

        if (General.BlipPTR == 0)
        {
            General.BlipPTR = GTA5Mem.FindPattern(Offsets.Mask.BlipMask);
            General.BlipPTR = GTA5Mem.Rip_37(General.BlipPTR);
        }

        if (General.GlobalPTR == 0)
        {
            General.GlobalPTR = GTA5Mem.FindPattern(Offsets.Mask.GlobalMask);
            General.GlobalPTR = GTA5Mem.Rip_37(General.GlobalPTR);
        }

        if (General.PlayerChatterNamePTR == 0)
        {
            General.PlayerChatterNamePTR = GTA5Mem.FindPattern(Offsets.Mask.PlayerchatterNameMask);
            General.PlayerChatterNamePTR = GTA5Mem.Rip_37(General.PlayerChatterNamePTR);
        }

        if (General.PlayerExternalDisplayNamePTR == 0)
        {
            General.PlayerExternalDisplayNamePTR = GTA5Mem.FindPattern(Offsets.Mask.PlayerExternalDisplayNameMask);
            General.PlayerExternalDisplayNamePTR = GTA5Mem.Rip_37(General.PlayerExternalDisplayNamePTR);
        }

        if (General.NetworkPlayerMgrPTR == 0)
        {
            General.NetworkPlayerMgrPTR = GTA5Mem.FindPattern(Offsets.Mask.NetworkPlayerMgrMask);
            General.NetworkPlayerMgrPTR = GTA5Mem.Rip_37(General.NetworkPlayerMgrPTR);
        }

        if (General.ReplayInterfacePTR == 0)
        {
            General.ReplayInterfacePTR = GTA5Mem.FindPattern(Offsets.Mask.ReplayInterfaceMask);
            General.ReplayInterfacePTR = GTA5Mem.Rip_37(General.ReplayInterfacePTR);
        }

        if (General.WeatherPTR == 0)
        {
            General.WeatherPTR = GTA5Mem.FindPattern(Offsets.Mask.WeatherMask);
            General.WeatherPTR = GTA5Mem.Rip_6A(General.WeatherPTR);
        }

        if (General.UnkModelPTR == 0)
        {
            General.UnkModelPTR = GTA5Mem.FindPattern(Offsets.Mask.UnkModelMask);
            General.UnkModelPTR = GTA5Mem.Rip_37(General.UnkModelPTR);
        }

        if (General.PickupDataPTR == 0)
        {
            General.PickupDataPTR = GTA5Mem.FindPattern(Offsets.Mask.PickupDataMask);
            General.PickupDataPTR = GTA5Mem.Rip_37(General.PickupDataPTR);
        }

        if (General.ViewPortPTR == 0)
        {
            General.ViewPortPTR = GTA5Mem.FindPattern(Offsets.Mask.ViewPortMask);
            General.ViewPortPTR = GTA5Mem.Rip_37(General.ViewPortPTR);
        }

        if (General.AimingPedPTR == 0)
        {
            General.AimingPedPTR = GTA5Mem.FindPattern(Offsets.Mask.AimingPedMask);
            General.AimingPedPTR = GTA5Mem.Rip_37(General.AimingPedPTR);
        }

        if (General.CCameraPTR == 0)
        {
            General.CCameraPTR = GTA5Mem.FindPattern(Offsets.Mask.CCameraMask);
            General.CCameraPTR = GTA5Mem.Rip_37(General.CCameraPTR);
        }

        if (General.UnkPTR == 0)
        {
            General.UnkPTR = GTA5Mem.FindPattern(Offsets.Mask.UnkMask);
            General.UnkPTR = GTA5Mem.Rip_37(General.UnkPTR);
        }

        if (General.LocalScriptsPTR == 0)
        {
            General.LocalScriptsPTR = GTA5Mem.FindPattern(Offsets.Mask.LocalScriptsMask);
            General.LocalScriptsPTR = GTA5Mem.Rip_37(General.LocalScriptsPTR);
        }
    }

    #region 托盘菜单事件
    /// <summary>
    /// 鼠标双击托盘
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void TaskbarIcon_Main_TrayMouseDoubleClick(object sender, RoutedEventArgs e)
    {
        if (WindowState == WindowState.Minimized)
        {
            WindowState = WindowState.Normal;
            Activate();
            ShowInTaskbar = true;
        }
    }

    /// <summary>
    /// 显示主窗口
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void MenuItem_ShowMainWindow_Click(object sender, RoutedEventArgs e)
    {
        if (WindowState == WindowState.Minimized)
        {
            WindowState = WindowState.Normal;
            Activate();
            ShowInTaskbar = true;
        }
    }

    /// <summary>
    /// 退出程序
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void MenuItem_ExitProcess_Click(object sender, RoutedEventArgs e)
    {
        Close();
    }
    #endregion
}
