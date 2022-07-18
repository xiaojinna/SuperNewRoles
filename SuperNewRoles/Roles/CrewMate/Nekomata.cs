using System.Collections.Generic;
using Hazel;
using SuperNewRoles.CustomRPC;
using SuperNewRoles.EndGame;
using SuperNewRoles.Mode;

namespace SuperNewRoles.Roles
{
    class Nekomata
    {
        public static void NekomataEnd(GameData.PlayerInfo __instance)
        {
            if (!ModeHandler.IsMode(ModeId.Default)) return;
            if (__instance == null) return;
            if (AmongUsClient.Instance.AmHost)
            {
                //もし 追放された役職が猫なら
                if ((__instance != null && __instance.Object.isRole(RoleId.NiceNekomata)) || __instance.Object.isRole(RoleId.EvilNekomata) || __instance.Object.isRole(RoleId.BlackCat))
                {
                    List<PlayerControl> p = new();//道連れにするプレイヤーの抽選リスト
                    foreach (PlayerControl p1 in CachedPlayer.AllPlayers)
                    {
                        //もし 黒猫・イビル猫又が追放Impostorを道連れしないがオンるにゃら
                        if ((__instance.Object.isRole(RoleId.NiceNekomata) && RoleClass.EvilNekomata.NotImpostorExiled) || (__instance.Object.isRole(RoleId.BlackCat) && RoleClass.BlackCat.NotImpostorExiled))
                        {
                            //もし 抜き出されたプレイヤーが　追放されたプレイヤーではない  生きている  インポスターでないにゃら
                            if (p1.Data != __instance && p1.isAlive() && !p1.isImpostor())
                            {
                                p.Add(p1);//道連れにするプレイヤーの抽選リストに追加する
                                //Logへの記載
                                if (__instance.Object.isRole(RoleId.BlackCat))
                                    SuperNewRolesPlugin.Logger.LogInfo("[SNR:黒猫Info]Impostorを道連れ対象から除外しました");
                                else if (__instance.Object.isRole(RoleId.NiceNekomata))
                                    SuperNewRolesPlugin.Logger.LogInfo("[SNR:イビル猫又Info]Impostorを道連れ対象から除外しました");
                                else
                                    SuperNewRolesPlugin.Logger.LogError("[SNR:猫又Error][NotImpostorExiled == true] 異常な抽選リストです");
                            }
                        }
                        //ナイス・設定オフ
                        else
                        {
                            //もし 抜き出されたプレイヤーが　追放されたプレイヤーではない 且つ 生きているにゃら
                            if (p1.Data != __instance && p1.isAlive())
                            {
                                p.Add(p1);//道連れにするプレイヤーの抽選リストに追加する
                                //Logへの記載
                                if (__instance.Object.isRole(RoleId.BlackCat))
                                    SuperNewRolesPlugin.Logger.LogInfo("[SNR:黒猫Info]Impostorを道連れ対象から除外しませんでした");
                                else if (__instance.Object.isRole(RoleId.NiceNekomata))
                                    SuperNewRolesPlugin.Logger.LogInfo("[SNR:イビル猫又Info]Impostorを道連れ対象から除外しませんでした");
                                else
                                    SuperNewRolesPlugin.Logger.LogError("[SNR:猫又Error][NotImpostorExiled != true] 異常な抽選リストです");
                            }
                        }
                    }
                    MessageWriter RPCWriter = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId, (byte)CustomRPC.CustomRPC.ExiledRPC, SendOption.Reliable, -1);
                    RPCWriter.Write(__instance.PlayerId);
                    AmongUsClient.Instance.FinishRpcImmediately(RPCWriter);
                    RPCProcedure.ExiledRPC(__instance.PlayerId);
                    NekomataProc(p);
                }

            }
        }
        public static void NekomataProc(List<PlayerControl> p)
        {
            var rdm = ModHelpers.GetRandomIndex(p);
            var random = p[rdm];
            SuperNewRolesPlugin.Logger.LogInfo(random.nameText().text);
            if (EvilEraser.IsOKAndTryUse(EvilEraser.BlockTypes.NekomataExiled, random))
            {
                MessageWriter RPCWriter = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId, (byte)CustomRPC.CustomRPC.NekomataExiledRPC, SendOption.Reliable, -1);
                RPCWriter.Write(random.PlayerId);
                AmongUsClient.Instance.FinishRpcImmediately(RPCWriter);
                RPCProcedure.ExiledRPC(random.PlayerId);
                if (random.isRole(RoleId.NiceNekomata) || random.isRole(RoleId.EvilNekomata) || (random.isRole(RoleId.BlackCat) && RoleClass.NiceNekomata.IsChain))
                {
                    p.RemoveAt(rdm);
                    NekomataProc(p);
                }
                if (random.isRole(RoleId.Jester) || random.isRole(RoleId.MadJester))
                {
                    if (!RoleClass.Jester.IsJesterTaskClearWin || (RoleClass.Jester.IsJesterTaskClearWin && Patch.TaskCount.TaskDateNoClearCheck(random.Data).Item2 - Patch.TaskCount.TaskDateNoClearCheck(random.Data).Item1 == 0))
                    {
                        RPCProcedure.ShareWinner(random.PlayerId);
                        MessageWriter Writer = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId, (byte)CustomRPC.CustomRPC.ShareWinner, SendOption.Reliable, -1);
                        Writer.Write(random.PlayerId);
                        AmongUsClient.Instance.FinishRpcImmediately(Writer);
                        RoleClass.Jester.IsJesterWin = true;
                        ShipStatus.RpcEndGame((GameOverReason)CustomGameOverReason.JesterWin, false);
                    }
                    if (!RoleClass.MadJester.IsMadJesterTaskClearWin || (RoleClass.MadJester.IsMadJesterTaskClearWin && Patch.TaskCount.TaskDateNoClearCheck(random.Data).Item2 - Patch.TaskCount.TaskDateNoClearCheck(random.Data).Item1 == 0))
                    {
                        RPCProcedure.ShareWinner(random.PlayerId);
                        MessageWriter Writer = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId, (byte)CustomRPC.CustomRPC.ShareWinner, SendOption.Reliable, -1);
                        Writer.Write(random.PlayerId);
                        AmongUsClient.Instance.FinishRpcImmediately(Writer);
                        RoleClass.MadJester.IsMadJesterWin = true;
                        ShipStatus.RpcEndGame((GameOverReason)CustomGameOverReason.MadJesterWin, false);
                    }
                }
            }
        }
    }
}