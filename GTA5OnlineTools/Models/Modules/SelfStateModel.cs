using CommunityToolkit.Mvvm.ComponentModel;

namespace GTA5OnlineTools.Models.Modules;

public class SelfStateModel : ObservableObject
{
    private bool _isHotKeyToWaypoint = false;
    /// <summary>
    /// 热键状态 传送到导航点
    /// </summary>
    public bool IsHotKeyToWaypoint
    {
        get => _isHotKeyToWaypoint;
        set => SetProperty(ref _isHotKeyToWaypoint, value);
    }

    private bool _isHotKeyToObjective = false;
    /// <summary>
    /// 热键状态 传送到目标点
    /// </summary>
    public bool IsHotKeyToObjective
    {
        get => _isHotKeyToObjective;
        set => SetProperty(ref _isHotKeyToObjective, value);
    }

    private bool _isHotKeyFillHealthArmor = false;
    /// <summary>
    /// 热键状态 补满血量和护甲
    /// </summary>
    public bool IsHotKeyFillHealthArmor
    {
        get => _isHotKeyFillHealthArmor;
        set => SetProperty(ref _isHotKeyFillHealthArmor, value);
    }

    private bool _isHotKeyClearWanted = false;
    /// <summary>
    /// 热键状态 消除警星
    /// </summary>
    public bool IsHotKeyClearWanted
    {
        get => _isHotKeyClearWanted;
        set => SetProperty(ref _isHotKeyClearWanted, value);
    }

    private bool _isHotKeyFillAllAmmo = false;
    /// <summary>
    /// 热键状态 补满全部武器弹药
    /// </summary>
    public bool IsHotKeyFillAllAmmo
    {
        get => _isHotKeyFillAllAmmo;
        set => SetProperty(ref _isHotKeyFillAllAmmo, value);
    }

    private bool _isHotKeyMovingFoward = false;
    /// <summary>
    /// 热键状态 坐标向前微调
    /// </summary>
    public bool IsHotKeyMovingFoward
    {
        get => _isHotKeyMovingFoward;
        set => SetProperty(ref _isHotKeyMovingFoward, value);
    }

    private bool _isHotKeyNoCollision = false;
    /// <summary>
    /// 热键状态 玩家无碰撞体积（穿墙）
    /// </summary>
    public bool IsHotKeyNoCollision
    {
        get => _isHotKeyNoCollision;
        set => SetProperty(ref _isHotKeyNoCollision, value);
    }
}
