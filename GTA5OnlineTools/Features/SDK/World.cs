﻿using GTA5OnlineTools.Features.Core;
using GTA5OnlineTools.Features.Data;

namespace GTA5OnlineTools.Features.SDK;

public static class World
{
    /// <summary>
    /// 设置本地天气
    /// 
    /// -1, Default
    ///  0, Extra Sunny
    ///  1, Clear
    ///  2, Clouds
    ///  3, Smog
    ///  4, Foggy
    ///  5, Overcast
    ///  6, Rain
    ///  7, Thunder
    ///  8, Light Rain
    ///  9, Smoggy Light Rain
    /// 10, Very Light Snow
    /// 11, Windy Snow
    /// 12, Light Snow
    /// 14, Halloween
    /// </summary>
    /// <param name="weatherID">天气ID</param>
    public static void Set_Local_Weather(int weatherID)
    {
        if (weatherID == -1)
        {
            GTA5Mem.Write(General.WeatherPTR + 0x24, -1);
            GTA5Mem.Write(General.WeatherPTR + 0x104, 13);
        }
        if (weatherID == 13)
        {
            GTA5Mem.Write(General.WeatherPTR + 0x24, 13);
            GTA5Mem.Write(General.WeatherPTR + 0x104, 13);
        }

        GTA5Mem.Write(General.WeatherPTR + 0x104, weatherID);
    }

    /// <summary>
    /// 杀死NPC，仅敌对？
    /// </summary>
    public static void KillNPC(bool isOnlyHostility)
    {
        // Ped实体
        long m_replay = GTA5Mem.Read<long>(General.ReplayInterfacePTR);
        long m_ped_interface = GTA5Mem.Read<long>(m_replay + 0x18);
        int m_max_peds = GTA5Mem.Read<int>(m_ped_interface + 0x108);

        for (int i = 0; i < m_max_peds; i++)
        {
            long m_ped_list = GTA5Mem.Read<long>(m_ped_interface + 0x100);
            m_ped_list = GTA5Mem.Read<long>(m_ped_list + i * 0x10);
            if (!GTA5Mem.IsValid(m_ped_list))
                continue;

            // 跳过玩家
            long m_player_info = GTA5Mem.Read<long>(m_ped_list + 0x10C8);
            if (GTA5Mem.IsValid(m_player_info))
                continue;

            if (isOnlyHostility)
            {
                byte oHostility = GTA5Mem.Read<byte>(m_ped_list + 0x18C);

                if (oHostility > 0x01)
                {
                    GTA5Mem.Write<float>(m_ped_list + 0x280, 0.0f);
                }
            }
            else
            {
                GTA5Mem.Write<float>(m_ped_list + 0x280, 0.0f);
            }
        }
    }

    /// <summary>
    /// 杀死警察
    /// </summary>
    public static void KillPolice()
    {
        // Ped实体
        long m_replay = GTA5Mem.Read<long>(General.ReplayInterfacePTR);
        long m_ped_interface = GTA5Mem.Read<long>(m_replay + 0x18);
        int m_max_peds = GTA5Mem.Read<int>(m_ped_interface + 0x108);

        for (int i = 0; i < m_max_peds; i++)
        {
            long m_ped_list = GTA5Mem.Read<long>(m_ped_interface + 0x100);
            m_ped_list = GTA5Mem.Read<long>(m_ped_list + i * 0x10);
            if (!GTA5Mem.IsValid(m_ped_list))
                continue;

            // 跳过玩家
            long m_player_info = GTA5Mem.Read<long>(m_ped_list + 0x10C8);
            if (GTA5Mem.IsValid(m_player_info))
                continue;

            int ped_type = GTA5Mem.Read<int>(m_ped_list + 0x10B8);
            ped_type = ped_type << 11 >> 25;

            if (ped_type == (int)EnumData.PedTypes.COP ||
                ped_type == (int)EnumData.PedTypes.SWAT ||
                ped_type == (int)EnumData.PedTypes.ARMY)
            {
                GTA5Mem.Write<float>(m_ped_list + 0x280, 0.0f);
            }
        }
    }

    /// <summary>
    /// 摧毁NPC载具，仅敌对？
    /// </summary>
    public static void DestroyNPCVehicles(bool isOnlyHostility)
    {
        // Ped实体
        long m_replay = GTA5Mem.Read<long>(General.ReplayInterfacePTR);
        long m_ped_interface = GTA5Mem.Read<long>(m_replay + 0x18);
        int m_max_peds = GTA5Mem.Read<int>(m_ped_interface + 0x108);

        for (int i = 0; i < m_max_peds; i++)
        {
            long m_ped_list = GTA5Mem.Read<long>(m_ped_interface + 0x100);
            m_ped_list = GTA5Mem.Read<long>(m_ped_list + i * 0x10);
            if (!GTA5Mem.IsValid(m_ped_list))
                continue;

            // 跳过玩家
            long m_player_info = GTA5Mem.Read<long>(m_ped_list + 0x10C8);
            if (GTA5Mem.IsValid(m_player_info))
                continue;

            long m_vehicle = GTA5Mem.Read<long>(m_ped_list + 0xD30);

            if (isOnlyHostility)
            {
                byte oHostility = GTA5Mem.Read<byte>(m_ped_list + 0x18C);

                if (oHostility > 0x01)
                {
                    GTA5Mem.Write<float>(m_vehicle + 0x280, -1.0f);
                    GTA5Mem.Write<float>(m_vehicle + 0x840, -1.0f);
                    GTA5Mem.Write<float>(m_vehicle + 0x844, -1.0f);
                    GTA5Mem.Write<float>(m_vehicle + 0x908, -1.0f);
                }
            }
            else
            {
                GTA5Mem.Write<float>(m_vehicle + 0x280, -1.0f);
                GTA5Mem.Write<float>(m_vehicle + 0x840, -1.0f);
                GTA5Mem.Write<float>(m_vehicle + 0x844, -1.0f);
                GTA5Mem.Write<float>(m_vehicle + 0x908, -1.0f);
            }
        }
    }

    /// <summary>
    /// 摧毁全部载具，玩家自己的载具也会摧毁
    /// </summary>
    public static void DestroyAllVehicles()
    {
        // Ped实体
        long m_replay = GTA5Mem.Read<long>(General.ReplayInterfacePTR);
        long m_vehicle_interface = GTA5Mem.Read<long>(m_replay + 0x10);
        long m_ped_interface = GTA5Mem.Read<long>(m_replay + 0x18);
        int m_max_peds = GTA5Mem.Read<int>(m_ped_interface + 0x108);

        for (int i = 0; i < m_max_peds; i++)
        {
            long m_vehicle_list = GTA5Mem.Read<long>(m_vehicle_interface + 0x180);
            m_vehicle_list = GTA5Mem.Read<long>(m_vehicle_list + i * 0x10);
            if (!GTA5Mem.IsValid(m_vehicle_list))
                continue;

            GTA5Mem.Write<float>(m_vehicle_list + 0x280, -1.0f);     // m_health
            GTA5Mem.Write<float>(m_vehicle_list + 0x840, -1.0f);     // m_body_health
            GTA5Mem.Write<float>(m_vehicle_list + 0x844, -1.0f);     // m_petrol_tank_health
            GTA5Mem.Write<float>(m_vehicle_list + 0x908, -1.0f);     // m_engine_health
        }
    }

    /// <summary>
    /// 传送NPC到我这里，仅敌对？
    /// </summary>
    public static void TeleportNPCToMe(bool isOnlyHostility)
    {
        Vector3 v3MyPos = GTA5Mem.Read<Vector3>(General.WorldPTR, Offsets.PlayerPositionX);

        // Ped实体
        long m_replay = GTA5Mem.Read<long>(General.ReplayInterfacePTR);
        long m_ped_interface = GTA5Mem.Read<long>(m_replay + 0x18);
        int m_max_peds = GTA5Mem.Read<int>(m_ped_interface + 0x108);

        for (int i = 0; i < m_max_peds; i++)
        {
            long m_ped_list = GTA5Mem.Read<long>(m_ped_interface + 0x100);
            m_ped_list = GTA5Mem.Read<long>(m_ped_list + i * 0x10);
            if (!GTA5Mem.IsValid(m_ped_list))
                continue;

            // 跳过玩家
            long m_player_info = GTA5Mem.Read<long>(m_ped_list + 0x10C8);
            if (GTA5Mem.IsValid(m_player_info))
                continue;

            long m_navigation = GTA5Mem.Read<long>(m_ped_list + 0x30);
            if (!GTA5Mem.IsValid(m_navigation))
                continue;

            if (isOnlyHostility)
            {
                byte oHostility = GTA5Mem.Read<byte>(m_ped_list + 0x18C);

                if (oHostility > 0x01)
                {
                    GTA5Mem.Write<Vector3>(m_navigation + 0x50, v3MyPos);
                    GTA5Mem.Write<Vector3>(m_ped_list + 0x90, v3MyPos);
                }
            }
            else
            {
                GTA5Mem.Write<Vector3>(m_navigation + 0x50, v3MyPos);
                GTA5Mem.Write<Vector3>(m_ped_list + 0x90, v3MyPos);
            }
        }
    }
}
