using System;
using System.Collections.Generic;
using HarmonyLib;
using SuperNewRoles.Mode;
using SuperNewRoles.Mode.SuperHostRoles;
using UnityEngine;
using static SuperNewRoles.Modules.CustomOption;
using static SuperNewRoles.Modules.CustomOptionHolder;

namespace SuperNewRoles.MapOption;

[HarmonyPatch]
public class MapOption
{
    // |:========== マップの設定 ==========:|
    public static CustomOption MapOptionSetting;

    // |:========== 情報機器制限の設定 ==========:|
    public static CustomOption DeviceOptions;

    // |:========== ON/OFFの設定 ==========:|
    public static CustomOption CanUseDeviceSetting;
    public static CustomOption DeviceUseAdmin;
    public static CustomOption DeviceUseVitalOrDoorLog;
    public static CustomOption DeviceUseCamera;

    // |:========== 時間制限の設定 ==========:|
    public static CustomOption RestrictDevicesTimeOption;
    public static CustomOption RestrictAdmin;
    public static CustomOption DeviceUseAdminTime;
    public static CustomOption RestrictVital;
    public static CustomOption DeviceUseVitalOrDoorLogTime;
    public static CustomOption RestrictCamera;
    public static CustomOption DeviceUseCameraTime;

    // |:========== リアクター継続時間の設定 ==========:|
    public static CustomOption ReactorDurationOption;
    public static CustomOption MiraReactorTimeLimit;
    public static CustomOption PolusReactorTimeLimit;
    public static CustomOption AirshipReactorTimeLimit;

    // |:========== ベントアニメーション有効化の設定 ==========:|
    public static CustomOption VentAnimationPlaySetting;

    // |:========== 配線タスクランダムの設定 ==========:|
    public static CustomOption WireTaskIsRandomOption;
    public static CustomOption WireTaskNumOption;

    // |:========== ランダムマップの設定 ==========:|
    public static CustomOption RandomMapOption;
    public static CustomOption RandomMapSkeld;
    public static CustomOption RandomMapMira;
    public static CustomOption RandomMapPolus;
    public static CustomOption RandomMapAirship;

    // |:========== 反転マップを有効化の設定 ==========:|
    public static CustomOption enableMirrorMap;

    // |:========== その他 ==========:|
    public static Dictionary<byte, PoolablePlayer> playerIcons = new();


    public static void LoadOption()
    {
        // |:========== マップの設定 ==========:|

        MapOptionSetting = Create(527, true, CustomOptionType.Generic, Cs(new Color(170f / 255f, 76f / 255f, 143f / 255f, 1), "MapOptionSetting"), false, null, isHeader: true);

        // |:========== 情報機器制限の設定 ==========:|
        DeviceOptions = Create(528, true, CustomOptionType.Generic, "DeviceOptionsSetting", false, MapOptionSetting, isHeader: true);

        // |:========== ON/OFFの設定 ==========:|
        CanUseDeviceSetting = Create(446, true, CustomOptionType.Generic, "CanUseDeviceSetting", true, DeviceOptions);
        DeviceUseAdmin = Create(1247, true, CustomOptionType.Generic, "DeviceUseAdminSetting", true, CanUseDeviceSetting);
        DeviceUseVitalOrDoorLog = Create(448, true, CustomOptionType.Generic, "DeviceUseVitalOrDoorLogSetting", true, CanUseDeviceSetting);
        DeviceUseCamera = Create(450, true, CustomOptionType.Generic, "DeviceUseCameraSetting", true, CanUseDeviceSetting);

        // |:========== 時間制限の設定 ==========:|
        RestrictDevicesTimeOption = Create(1105, false, CustomOptionType.Generic, "RestrictDevicesTimeOption", false, DeviceOptions);
        RestrictAdmin = Create(1102, false, CustomOptionType.Generic, "RestrictAdmin", false, RestrictDevicesTimeOption);
        DeviceUseAdminTime = Create(447, false, CustomOptionType.Generic, "DeviceTimeSetting", 10f, 0f, 120f, 1f, RestrictAdmin);
        RestrictVital = Create(1103, false, CustomOptionType.Generic, "RestrictVital", false, RestrictDevicesTimeOption);
        DeviceUseVitalOrDoorLogTime = Create(449, false, CustomOptionType.Generic, "DeviceTimeSetting", 10f, 0f, 120f, 1f, RestrictVital);
        RestrictCamera = Create(1104, false, CustomOptionType.Generic, "RestrictCamera", false, RestrictDevicesTimeOption);
        DeviceUseCameraTime = Create(451, false, CustomOptionType.Generic, "DeviceTimeSetting", 10f, 0f, 120f, 1f, RestrictCamera);

        // |:========== リアクター継続時間の設定 ==========:|
        ReactorDurationOption = Create(468, true, CustomOptionType.Generic, "ReactorDurationSetting", false, MapOptionSetting, isHeader: true);
        MiraReactorTimeLimit = Create(470, true, CustomOptionType.Generic, "MiraReactorTime", 30f, 0f, 100f, 1f, ReactorDurationOption);
        PolusReactorTimeLimit = Create(469, true, CustomOptionType.Generic, "PolusReactorTime", 30f, 0f, 100f, 1f, ReactorDurationOption);
        AirshipReactorTimeLimit = Create(471, true, CustomOptionType.Generic, "AirshipReactorTime", 30f, 0f, 100f, 1f, ReactorDurationOption);

        // |:========== ベントアニメーション有効化の設定 ==========:|
        VentAnimationPlaySetting = Create(600, false, CustomOptionType.Generic, "VentAnimationPlaySetting", true, MapOptionSetting, isHeader: true);

        // |:========== 配線タスクランダムの設定 ==========:|
        WireTaskIsRandomOption = Create(956, false, CustomOptionType.Generic, "WireTaskIsRandom", false, MapOptionSetting, isHeader: true);
        WireTaskNumOption = Create(957, false, CustomOptionType.Generic, "WireTaskNum", 5f, 1f, 8f, 1f, WireTaskIsRandomOption);

        // |:========== ランダムマップの設定 ==========:|
        RandomMapOption = Create(454, true, CustomOptionType.Generic, "RamdomMapSetting", false, MapOptionSetting, isHeader: true);
        RandomMapSkeld = Create(455, true, CustomOptionType.Generic, "RMSkeldSetting", true, RandomMapOption);
        RandomMapMira = Create(456, true, CustomOptionType.Generic, "RMMiraSetting", true, RandomMapOption);
        RandomMapPolus = Create(457, true, CustomOptionType.Generic, "RMPolusSetting", true, RandomMapOption);
        RandomMapAirship = Create(458, true, CustomOptionType.Generic, "RMAirshipSetting", true, RandomMapOption);

        // |:========== 反転マップ有効化の設定 ==========:|
        enableMirrorMap = Create(9, false, CustomOptionType.Generic, "enableMirrorMap", false, MapOptionSetting, isHeader: true);
    }

    #region 設定取得に使用している変数の定義

    // |:========== 変数:ON/OFFの設定 ==========:|
    public static bool CanUseAdmin;
    public static bool CanUseVitalOrDoorLog;
    public static bool CanUseCamera;
    // |:========== 変数:時間制限の設定 ==========:|
    public static bool IsUsingRestrictDevicesTime;
    // その他はDeviceClassに存在

    // |:========== 変数:リアクター継続時間の設定 ==========:|
    public static bool IsReactorDurationSetting;

    // |:========== 変数:ベントアニメーションを有効にする ==========:|
    public static bool CanPlayVentAnimation;

    // |:========== 変数:配線タスクランダム ==========:|
    public static bool WireTaskIsRandom;
    public static int WireTaskNum;

    // |:========== 変数:ランダムマップ ==========:|
    public static bool IsRandomMap;
    public static bool ValidationSkeld;
    public static bool ValidationMira;
    public static bool ValidationPolus;
    public static bool ValidationAirship;

    // |:========== 変数:反転マップを有効にする ==========:|
    public static bool IsenableMirrorMap;

    // |:========== 変数:その他 ==========:|
    public static float Default;
    public static float CameraDefault;

    #endregion

    public static void ClearAndReload()
    {
        #region マップの設定全てを初期値で初期化

        CanUseAdmin = true;
        CanUseVitalOrDoorLog = true;
        CanUseCamera = true;

        IsUsingRestrictDevicesTime = false;

        IsReactorDurationSetting = false;

        CanPlayVentAnimation = true;

        WireTaskIsRandom = false;

        IsRandomMap = false;
        ValidationSkeld = true;
        ValidationMira = true;
        ValidationPolus = true;
        ValidationAirship = true;

        IsenableMirrorMap = false;

        #endregion

        #region マップの設定のon/offに関わらない初期化

        BlockTool.OldDesyncCommsPlayers = new();
        BlockTool.CameraPlayers = new();

        PolusReactorTimeLimit.GetFloat();
        MiraReactorTimeLimit.GetFloat();
        AirshipReactorTimeLimit.GetFloat();

        WireTaskNum = WireTaskNumOption.GetInt();

        CameraDefault = Camera.main.orthographicSize;
        Default = FastDestroyableSingleton<HudManager>.Instance.UICamera.orthographicSize;
        playerIcons = new();

        #endregion

        if (MapOptionSetting.GetBool()) // 早期return使用していない理由有。閉じ括弧の下に理由を記載しています。
        {
            if (DeviceOptions.GetBool())
            {
                if (CanUseDeviceSetting.GetBool())
                {
                    CanUseAdmin = DeviceUseAdmin.GetBool();
                    CanUseVitalOrDoorLog = DeviceUseVitalOrDoorLog.GetBool();
                    CanUseCamera = DeviceUseCamera.GetBool();
                }

                if (!ModeHandler.IsMode(ModeId.SuperHostRoles) && RestrictDevicesTimeOption.GetBool())
                {
                    IsUsingRestrictDevicesTime = true;
                    // 他の変数は[DeviceClass.ClearAndReload();]で初期化を行っている。
                }
            }

            IsReactorDurationSetting = ReactorDurationOption.GetBool();

            if (!ModeHandler.IsMode(ModeId.SuperHostRoles)) CanPlayVentAnimation = VentAnimationPlaySetting.GetBool();

            WireTaskIsRandom = WireTaskIsRandomOption.GetBool();

            if (RandomMapOption.GetBool())
            {
                IsRandomMap = true;

                ValidationSkeld = RandomMapSkeld.GetBool();
                ValidationMira = RandomMapMira.GetBool();
                ValidationPolus = RandomMapPolus.GetBool();
                ValidationAirship = RandomMapAirship.GetBool();
            }

            IsenableMirrorMap = enableMirrorMap.GetBool();
        }
        /*
            [MapOptionSetting.GetBool()]に早期returnを使用していない理由。
            [DeviceClass.ClearAndReload();]を[IsUsingRestrictDevicesTime]の初期化の後に行う必要がある為。
            又　後に設定が増えた時に区切りをわかりやすくして追加しやすくする為
        */
        DeviceClass.ClearAndReload();
    }
}