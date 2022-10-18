using CommunityToolkit.Mvvm.ComponentModel;

namespace GTA5OnlineTools.Models;

public class MainModel : ObservableObject
{
    private bool _isGTA5Run;
    /// <summary>
    /// GTA5是否运行
    /// </summary>
    public bool IsGTA5Run
    {
        get => _isGTA5Run;
        set => SetProperty(ref _isGTA5Run, value);
    }

    private string _appRunTime;
    /// <summary>
    /// 程序运行时间
    /// </summary>
    public string AppRunTime
    {
        get => _appRunTime;
        set => SetProperty(ref _appRunTime, value);
    }

    private string _appVersion;
    /// <summary>
    /// 程序版本
    /// </summary>
    public string AppVersion
    {
        get => _appVersion;
        set => SetProperty(ref _appVersion, value);
    }
}
