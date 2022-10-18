using GTA5OnlineTools.Modules;
using GTA5OnlineTools.Features.SDK;
using GTA5OnlineTools.Features.Core;
using GTA5OnlineTools.Common.Utils;
using GTA5OnlineTools.Common.Helper;

using CommunityToolkit.Mvvm.Input;

namespace GTA5OnlineTools.Views;

/// <summary>
/// ModulesView.xaml 的交互逻辑
/// </summary>
public partial class ModulesView : UserControl
{
    private ExternalMenuWindow ExternalMenuWindow = null;
    private InjectMenuWindow InjectMenuWindow = null;
    private GTAHaxWindow GTAHaxWindow = null;
    private OutfitsEditWindow OutfitsEditWindow = null;
    private HeistCutWindow HeistCutWindow = null;
    private StatScriptsWindow StatScriptsWindow = null;
    private SpeedMeterWindow SpeedMeterWindow = null;

    public RelayCommand<string> ModelsClickCommand { get; private set; }

    /// <summary>
    /// 关闭全部第三方模块窗口委托
    /// </summary>
    public static Action ActionCloseAllModulesWindow;

    public ModulesView()
    {
        InitializeComponent();
        this.DataContext = this;

        ModelsClickCommand = new(ModelsClick);

        ActionCloseAllModulesWindow = CloseAllModulesWindow;
    }

    private void ModelsClick(string modelName)
    {
        AudioUtil.PlayClickSound();

        if (ProcessUtil.IsGTA5Run())
        {
            GTA5MemInit();

            switch (modelName)
            {
                case "ExternalMenu":
                    ExternalMenuClick();
                    break;
                case "InjectMenu":
                    InjectMenuClick();
                    break;
                case "GTAHax":
                    GTAHaxClick();
                    break;
                case "OutfitsEdit":
                    OutfitsEditClick();
                    break;
                case "HeistCut":
                    HeistCutClick();
                    break;
                case "StatScripts":
                    StatScriptsClick();
                    break;
                case "SpeedMeter":
                    SpeedMeterClick();
                    break;
            }
        }
        else
        {
            NotifierHelper.Show(NotifierType.Warning, "未发现《GTA5》进程，请先运行《GTA5》游戏");
        }
    }

    #region 第三方模块按钮点击事件
    private void ExternalMenuClick()
    {
        if (ExternalMenuWindow == null)
        {
            ExternalMenuWindow = new ExternalMenuWindow();
            ExternalMenuWindow.Show();
        }
        else
        {
            if (ExternalMenuWindow.IsVisible)
            {
                if (!ExternalMenuWindow.Topmost)
                {
                    ExternalMenuWindow.Topmost = true;
                    ExternalMenuWindow.Topmost = false;
                }

                ExternalMenuWindow.WindowState = WindowState.Normal;
            }
            else
            {
                ExternalMenuWindow = null;
                ExternalMenuWindow = new ExternalMenuWindow();
                ExternalMenuWindow.Show();
            }
        }
    }

    private void InjectMenuClick()
    {
        if (InjectMenuWindow == null)
        {
            InjectMenuWindow = new InjectMenuWindow();
            InjectMenuWindow.Show();
        }
        else
        {
            if (InjectMenuWindow.IsVisible)
            {
                InjectMenuWindow.Topmost = true;
                InjectMenuWindow.Topmost = false;
                InjectMenuWindow.WindowState = WindowState.Normal;
            }
            else
            {
                InjectMenuWindow = null;
                InjectMenuWindow = new InjectMenuWindow();
                InjectMenuWindow.Show();
            }
        }
    }

    private void GTAHaxClick()
    {
        if (GTAHaxWindow == null)
        {
            GTAHaxWindow = new GTAHaxWindow();
            GTAHaxWindow.Show();
        }
        else
        {
            if (GTAHaxWindow.IsVisible)
            {
                GTAHaxWindow.Topmost = true;
                GTAHaxWindow.Topmost = false;
                GTAHaxWindow.WindowState = WindowState.Normal;
            }
            else
            {
                GTAHaxWindow = null;
                GTAHaxWindow = new GTAHaxWindow();
                GTAHaxWindow.Show();
            }
        }
    }

    private void OutfitsEditClick()
    {
        if (OutfitsEditWindow == null)
        {
            OutfitsEditWindow = new OutfitsEditWindow();
            OutfitsEditWindow.Show();
        }
        else
        {
            if (OutfitsEditWindow.IsVisible)
            {
                OutfitsEditWindow.Topmost = true;
                OutfitsEditWindow.Topmost = false;
                OutfitsEditWindow.WindowState = WindowState.Normal;
            }
            else
            {
                OutfitsEditWindow = null;
                OutfitsEditWindow = new OutfitsEditWindow();
                OutfitsEditWindow.Show();
            }
        }
    }

    private void HeistCutClick()
    {
        if (HeistCutWindow == null)
        {
            HeistCutWindow = new HeistCutWindow();
            HeistCutWindow.Show();
        }
        else
        {
            if (HeistCutWindow.IsVisible)
            {
                HeistCutWindow.Topmost = true;
                HeistCutWindow.Topmost = false;
                HeistCutWindow.WindowState = WindowState.Normal;
            }
            else
            {
                HeistCutWindow = null;
                HeistCutWindow = new HeistCutWindow();
                HeistCutWindow.Show();
            }
        }
    }

    private void StatScriptsClick()
    {
        if (StatScriptsWindow == null)
        {
            StatScriptsWindow = new StatScriptsWindow();
            StatScriptsWindow.Show();
        }
        else
        {
            if (StatScriptsWindow.IsVisible)
            {
                StatScriptsWindow.Topmost = true;
                StatScriptsWindow.Topmost = false;
                StatScriptsWindow.WindowState = WindowState.Normal;
            }
            else
            {
                StatScriptsWindow = null;
                StatScriptsWindow = new StatScriptsWindow();
                StatScriptsWindow.Show();
            }
        }
    }

    private void SpeedMeterClick()
    {
        if (SpeedMeterWindow == null)
        {
            SpeedMeterWindow = new SpeedMeterWindow();
            SpeedMeterWindow.Show();
        }
        else
        {
            if (SpeedMeterWindow.IsVisible)
            {
                SpeedMeterWindow.Topmost = true;
                SpeedMeterWindow.Topmost = false;
                SpeedMeterWindow.WindowState = WindowState.Normal;
            }
            else
            {
                SpeedMeterWindow = null;
                SpeedMeterWindow = new SpeedMeterWindow();
                SpeedMeterWindow.Show();
            }
        }
    }
    #endregion

    ///////////////////////////////////////////////////////////////////

    /// <summary>
    /// 关闭全部模块窗口
    /// </summary>
    private void CloseAllModulesWindow()
    {
        if (ExternalMenuWindow != null)
        {
            ExternalMenuWindow.Close();
            ExternalMenuWindow = null;
        }

        if (InjectMenuWindow != null)
        {
            InjectMenuWindow.Close();
            InjectMenuWindow = null;
        }

        if (GTAHaxWindow != null)
        {
            GTAHaxWindow.Close();
            GTAHaxWindow = null;
        }

        if (OutfitsEditWindow != null)
        {
            OutfitsEditWindow.Close();
            OutfitsEditWindow = null;
        }

        if (HeistCutWindow != null)
        {
            HeistCutWindow.Close();
            HeistCutWindow = null;
        }

        if (StatScriptsWindow != null)
        {
            StatScriptsWindow.Close();
            StatScriptsWindow = null;
        }

        if (SpeedMeterWindow != null)
        {
            SpeedMeterWindow.Close();
            SpeedMeterWindow = null;
        }
    }

    /// <summary>
    /// GTA5内存模块初始化
    /// </summary>
    private void GTA5MemInit()
    {
        if (!GTA5Mem.Initialize())
        {
            NotifierHelper.Show(NotifierType.Error, "《GTA5》内存模块初始化失败！程序可能无法正常运行");
        }
        else
        {
            General.WorldPTR = GTA5Mem.FindPattern(Offsets.Mask.WorldMask);
            General.WorldPTR = GTA5Mem.Rip_37(General.WorldPTR);

            General.BlipPTR = GTA5Mem.FindPattern(Offsets.Mask.BlipMask);
            General.BlipPTR = GTA5Mem.Rip_37(General.BlipPTR);

            General.GlobalPTR = GTA5Mem.FindPattern(Offsets.Mask.GlobalMask);
            General.GlobalPTR = GTA5Mem.Rip_37(General.GlobalPTR);

            General.PlayerChatterNamePTR = GTA5Mem.FindPattern(Offsets.Mask.PlayerchatterNameMask);
            General.PlayerChatterNamePTR = GTA5Mem.Rip_37(General.PlayerChatterNamePTR);

            General.PlayerExternalDisplayNamePTR = GTA5Mem.FindPattern(Offsets.Mask.PlayerExternalDisplayNameMask);
            General.PlayerExternalDisplayNamePTR = GTA5Mem.Rip_37(General.PlayerExternalDisplayNamePTR);

            General.NetworkPlayerMgrPTR = GTA5Mem.FindPattern(Offsets.Mask.NetworkPlayerMgrMask);
            General.NetworkPlayerMgrPTR = GTA5Mem.Rip_37(General.NetworkPlayerMgrPTR);

            General.ReplayInterfacePTR = GTA5Mem.FindPattern(Offsets.Mask.ReplayInterfaceMask);
            General.ReplayInterfacePTR = GTA5Mem.Rip_37(General.ReplayInterfacePTR);

            General.WeatherPTR = GTA5Mem.FindPattern(Offsets.Mask.WeatherMask);
            General.WeatherPTR = GTA5Mem.Rip_6A(General.WeatherPTR);

            General.UnkModelPTR = GTA5Mem.FindPattern(Offsets.Mask.UnkModelMask);
            General.UnkModelPTR = GTA5Mem.Rip_37(General.UnkModelPTR);

            General.PickupDataPTR = GTA5Mem.FindPattern(Offsets.Mask.PickupDataMask);
            General.PickupDataPTR = GTA5Mem.Rip_37(General.PickupDataPTR);

            General.ViewPortPTR = GTA5Mem.FindPattern(Offsets.Mask.ViewPortMask);
            General.ViewPortPTR = GTA5Mem.Rip_37(General.ViewPortPTR);

            General.AimingPedPTR = GTA5Mem.FindPattern(Offsets.Mask.AimingPedMask);
            General.AimingPedPTR = GTA5Mem.Rip_37(General.AimingPedPTR);

            General.CCameraPTR = GTA5Mem.FindPattern(Offsets.Mask.CCameraMask);
            General.CCameraPTR = GTA5Mem.Rip_37(General.CCameraPTR);

            General.UnkPTR = GTA5Mem.FindPattern(Offsets.Mask.UnkMask);
            General.UnkPTR = GTA5Mem.Rip_37(General.UnkPTR);

            General.LocalScriptsPTR = GTA5Mem.FindPattern(Offsets.Mask.LocalScriptsMask);
            General.LocalScriptsPTR = GTA5Mem.Rip_37(General.LocalScriptsPTR);
        }
    }
}
