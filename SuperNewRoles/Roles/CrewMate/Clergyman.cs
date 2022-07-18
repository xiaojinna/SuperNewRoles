using System;
using Hazel;
using SuperNewRoles.Buttons;
using SuperNewRoles.CustomRPC;

namespace SuperNewRoles.Roles
{
    class Clergyman
    {
        public static void ResetCoolDown()
        {
            HudManagerStartPatch.ClergymanLightOutButton.MaxTimer = RoleClass.Clergyman.CoolTime;
            RoleClass.Clergyman.ButtonTimer = DateTime.Now;
        }
        public static bool IsClergyman(PlayerControl Player)
        {
            return Player.isRole(RoleId.Clergyman);
        }
        public static void LightOutStart()
        {
            MessageWriter RPCWriter = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId, (byte)CustomRPC.CustomRPC.RPCClergymanLightOut, SendOption.Reliable, -1);
            RPCWriter.Write(true);
            AmongUsClient.Instance.FinishRpcImmediately(RPCWriter);
        }
        public static bool IsLightOutVision()
        {
            if (RoleClass.Clergyman.OldButtonTime <= 0) return false;
            if ((CountChanger.GetRoleType(PlayerControl.LocalPlayer) == TeamRoleType.Impostor)
                || CountChanger.IsChangeMadmate(PlayerControl.LocalPlayer)
                || CountChanger.IsChangeMadMayor(PlayerControl.LocalPlayer)
                || CountChanger.IsChangeMadJester(PlayerControl.LocalPlayer)
                || CountChanger.IsChangeMadStuntMan(PlayerControl.LocalPlayer)
                || CountChanger.IsChangeMadHawk(PlayerControl.LocalPlayer)
                || CountChanger.IsChangeMadSeer(PlayerControl.LocalPlayer)
                || CountChanger.IsChangeMadMaker(PlayerControl.LocalPlayer)
                || CountChanger.IsChangeJackal(PlayerControl.LocalPlayer)
                || CountChanger.IsChangeSidekick(PlayerControl.LocalPlayer)
                || CountChanger.IsChangeJackalFriends(PlayerControl.LocalPlayer)
                || CountChanger.IsChangeSeerFriends(PlayerControl.LocalPlayer)
                || CountChanger.IsChangeJackalSeer(PlayerControl.LocalPlayer))return true;
            return CountChanger.IsChangeSidekickSeer(PlayerControl.LocalPlayer) ? true : CountChanger.IsChangeBlackCat(PlayerControl.LocalPlayer);
        }
        public static bool IsLightOutVisionNoTime()
        {
            return CountChanger.GetRoleType(PlayerControl.LocalPlayer) == TeamRoleType.Impostor;
        }
        public static void LightOutStartRPC()
        {
            if (IsLightOutVisionNoTime())
            {
                new CustomMessage(ModTranslation.getString("ClergymanLightOutMessage"), RoleClass.Clergyman.DurationTime);
            }
            if (EvilEraser.IsOKAndTryUse(EvilEraser.BlockTypes.ClergymanLightOut))
            {
                RoleClass.Clergyman.OldButtonTimer = DateTime.Now;
            }
        }
        public static void EndMeeting()
        {
            HudManagerStartPatch.ClergymanLightOutButton.MaxTimer = RoleClass.Clergyman.CoolTime;
            RoleClass.Clergyman.ButtonTimer = DateTime.Now;
            RoleClass.Clergyman.IsLightOff = false;
        }
    }
}