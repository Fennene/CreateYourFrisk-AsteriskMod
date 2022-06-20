using AsteriskMod.FakeIniLoader;
using System;
using System.Collections;
using System.Collections.Generic;

namespace AsteriskMod
{
    public enum Languages
    {
        English,
        Japanese,
        Unknown
    }

    internal static class EngineLang
    {
        private static readonly FakeIni English;
        private static readonly FakeIni Japanese;

        internal static string Get(string sectionName, string parameterName)
        {
            if (Asterisk.language == Languages.Japanese) return Japanese[sectionName][parameterName].String;
            else                                         return  English[sectionName][parameterName].String;
        }

        internal static bool Exists(string sectionName, string parameterName)
        {
            if (Asterisk.language == Languages.Japanese) return Japanese.ParameterExists(sectionName, parameterName);
            else                                         return  English.ParameterExists(sectionName, parameterName);
        }

        static EngineLang()
        {
            English = new FakeIni();
            Japanese = new FakeIni();
            InitializeEnglish();
            InitializeJapanese();
        }

        private static void InitializeEnglish()
        {
            //English.SetParameter("", "", new FakeIniParameter(""));

            English.SetParameter("Disclaimer", "Text3", new FakeIniParameter("<b>Lua experience is <color='red'>REQUIRED</color> to make your own mods!!</b>"));
            English.SetParameter("Disclaimer", "Text4", new FakeIniParameter("For support and newest versions, go to <b>/r/Unitale</b>."));
            English.SetParameter("Disclaimer", "Text5", new FakeIniParameter("For support and newest AsteriskMod versions, go to <b>Github</b>."));
            English.SetParameter("Disclaimer", "Text6", new FakeIniParameter("Create Your Frisk is not owned by Toby Fox. Selling this engine or its mods is <b>illegal</b>."));
            //English.SetParameter("Disclaimer", "", new FakeIniParameter(""));

            English.SetParameter("ModSelect", "DescVisible", new FakeIniParameter(" Press V to show/hide the description"));
            English.SetParameter("ModSelect", "NoEncounter", new FakeIniParameter("No Encounters Available"));
            English.SetParameter("ModSelect", "OptionHover", new FakeIniParameter("Hover over an option and its description will appear here!"));
            English.SetParameter("ModSelect", "OptionNewMod", new FakeIniParameter("Creates your new mod.\n\nGenerates skeleton of a mod\nin this option."));
            //English.SetParameter("ModSelect", "OptionHelper", new FakeIniParameter("Opens Create Your Frisk's Modding Helper Tools."));
            English.SetParameter("ModSelect", "OptionHelper", new FakeIniParameter("Opens Modding Helper Tools."));
            English.SetParameter("ModSelect", "OptionCYF", new FakeIniParameter("Goes to the normal option."));
            English.SetParameter("ModSelect", "OptionAsterisk", new FakeIniParameter("Goes to the option that AsteriskMod adds."));
            //English.SetParameter("ModSelect", "", new FakeIniParameter(""));

            English.SetParameter("CYFOption", "Hover", new FakeIniParameter("Hover over an option and its description will appear here!"));
            English.SetParameter("CYFOption", "ResetRG", new FakeIniParameter("Resets all Real Globals.\n\nReal Globals are variables that persist through battles, but are deleted when CYF is closed."));
            English.SetParameter("CYFOption", "ResetAG", new FakeIniParameter("Resets all AlMighty Globals.\n\nAlMighty Globals are variables that are saved to a file, and stay even when you close CYF.\n\nThe options on this screen are stored as AlMighties."));
            English.SetParameter("CYFOption", "ClearSave", new FakeIniParameter("Clears your save file.\n\nThis is the save file used for CYF's Overworld.\n\nYour save file is located at:\n\n"));
            English.SetParameter("CYFOption", "Safe", new FakeIniParameter("Toggles safe mode.\n\nThis does nothing on its own, but mod authors can detect if you have this enabled, and use it to filter unsafe content, such as blood, gore, and swear words."));
            English.SetParameter("CYFOption", "Retro", new FakeIniParameter("Toggles retrocompatibility mode.\n\nThis mode is designed specifically to make encounters imported from Unitale v0.2.1a act as they did on the old engine.\n\n\n\n<b>CAUTION!\nDISABLE</b> this for mods made for CYF. This feature should only be used with Mods made for\n<b>Unitale v0.2.1a</b>."));
            English.SetParameter("CYFOption", "Fullscreen", new FakeIniParameter("Toggles blurless Fullscreen mode.\n\nThis controls whether fullscreen mode will appear \"blurry\" or not.\n\n\nPress <b>F4</b> or <b>Alt+Enter</b> to toggle Fullscreen."));
            English.SetParameter("CYFOption", "Scale", new FakeIniParameter("Scales the window in Windowed mode.\n\nThis is useful for especially large screens (such as 4k monitors).\n\nHas no effect in Fullscreen mode."));
            English.SetParameter("CYFOption", "Discord", new FakeIniParameter("Changes how much Discord Rich Presence should display on your profile regarding you playing Create Your Frisk.\n\n<b>Everything</b>: Everything is displayed: the mod you're playing, a timestamp and a description.\n\n<b>Game Only</b>: Only shows that you're playing Create Your Frisk.\n\n<b>Nothing</b>: Disables Discord Rich Presence entirely.\n\nIf CYF's connection to Discord is lost, you will have to restart CYF if you want your rich presence back."));
            English.SetParameter("CYFOption", "Exit", new FakeIniParameter(""));
            English.SetParameter("CYFOption", "Git", new FakeIniParameter(""));
            //English.SetParameter("CYFOption", "", new FakeIniParameter(""));
            
            English.SetParameter("AsteriskOption", "", new FakeIniParameter(""));
            English.SetParameter("MHTMenu", "", new FakeIniParameter(""));
            English.SetParameter("MHTSim", "", new FakeIniParameter(""));
        }

        private static void InitializeJapanese()
        {
            //Japanese.SetParameter("", "", new FakeIniParameter(""));

            Japanese.SetParameter("Disclaimer", "Text3", new FakeIniParameter("<b>Mod製作にはLuaコーディング経験が必要です！</b>"));
            Japanese.SetParameter("Disclaimer", "Text4", new FakeIniParameter("最新バージョンの情報は、<b>/r/Unitale</b>にあります。"));
            Japanese.SetParameter("Disclaimer", "Text5", new FakeIniParameter("AsteriskModの最新バージョンの情報は、<b>Github</b>にあります。"));
            Japanese.SetParameter("Disclaimer", "Text6", new FakeIniParameter("CreateYourFriskはToby Fox氏のものではありません。このエンジンやModを販売すること<b>違法</b>です。"));
            //Japanese.SetParameter("Disclaimer", "", new FakeIniParameter(""));

            Japanese.SetParameter("ModSelect", "DescVisible", new FakeIniParameter(" Vキーで説明の表示を切り替えます。"));
            Japanese.SetParameter("ModSelect", "NoEncounter", new FakeIniParameter("利用可能なエンカウンターがありません"));
            Japanese.SetParameter("ModSelect", "OptionHover", new FakeIniParameter("オプションにカーソルを乗せると\nここに説明が表示されます。"));
            Japanese.SetParameter("ModSelect", "OptionNewMod", new FakeIniParameter("Modの作成。\n\nModのベース(骨組み)を\n生成します。"));
            Japanese.SetParameter("ModSelect", "OptionHelper", new FakeIniParameter("Mod製作支援ツールを開きます。"));
            Japanese.SetParameter("ModSelect", "OptionCYF", new FakeIniParameter("通常のCYFの\nオプションを開きます。"));
            Japanese.SetParameter("ModSelect", "OptionAsterisk", new FakeIniParameter("AsteriskModが追加した\nオプションを開きます。"));
            //Japanese.SetParameter("ModSelect", "", new FakeIniParameter(""));

            Japanese.SetParameter("CYFOption", "Hover", new FakeIniParameter("オプションにカーソルを乗せると\nここに説明が表示されます。"));
            Japanese.SetParameter("CYFOption", "ResetRG", new FakeIniParameter("Real Globalをリセットします。\n\nReal Globalは、戦闘を通じて保持される値です。\nしかし、CYFを閉じるとそのデータは消えます。"));
            Japanese.SetParameter("CYFOption", "ResetAG", new FakeIniParameter("AlMighty Globalをリセットします。\n\nAlMighty Globalはファイルに保存される値です。\nCYFを閉じてもそのデータが保持されます。\n\nこのオプションもAlMighty Globalを使用して保存されています。"));
            Japanese.SetParameter("CYFOption", "ClearSave", new FakeIniParameter("セーブファイルを削除します。\n\nこれはCYFのOverworldに使用されているファイルを表します。\n\nセーブファイルのパスは以下の通り:\n\n"));
            Japanese.SetParameter("CYFOption", "Safe", new FakeIniParameter("Safeモードの切り替え。\n\nこのオプション自体は何もしませんが、\nModの製作者がこのオプションを検出し、\n流血表現やセンシティブなコンテンツなどをフィルタリングできます。"));
            Japanese.SetParameter("CYFOption", "Retro", new FakeIniParameter("Retrocompatibility(下位互換性)モードの切り替え。\n\nこれはUnitale v0.2.1a で製作されたModを\nCYFで動かすための特殊なモードです。\n\n\n\n<b>警告！</b>\nCYF用に製作されたModでは<b>無効(Off)にしてください！</b>\nこの機能は<b>Unitale v0.2.1a</b>用に作成されたModに対してのみ有効です。"));
            Japanese.SetParameter("CYFOption", "Fullscreen", new FakeIniParameter("Blurless Fullscreen(鮮明なフルスクリーン)モードの切り替え。\n\nこれはフルスクリーンを「ぼやけて(\"blurry\")」表示するかどうかを設定します。\n\n\n<b>F4キー</b>または<b>Altキー + Enterキー</b>でフルスクリーン表示になります。"));
            Japanese.SetParameter("CYFOption", "Scale", new FakeIniParameter("(ウインドウ表示での)ウインドウの拡大縮小。\n\nこれは特に、大画面(4Kモニターなど)での表示で役に立ちます。\n\nフルスクリーン表示では特に効果はありません。"));
            Japanese.SetParameter("CYFOption", "Discord", new FakeIniParameter("CYFプレイ中において、Discord Rich Presenceにどのくらい表示するかを設定をします。\n\n<b>Everything</b>: 全て表示 - プレイ中のMod名、プレイ時間、説明を表示します。\n\n<b>Game Only</b>: ゲーム名のみ - CYFで遊んでいることのみを表示します。\n\n<b>Nothing</b>: 無効 - Discord Rich Presenceを無効にします。\n\nCYFとDiscordとの接続が切れた場合は、CYFを再起動する必要があります。"));
            Japanese.SetParameter("CYFOption", "Exit", new FakeIniParameter(""));
            Japanese.SetParameter("CYFOption", "Git", new FakeIniParameter(""));
            //Japanese.SetParameter("CYFOption", "", new FakeIniParameter(""));

            Japanese.SetParameter("AsteriskOption", "", new FakeIniParameter(""));
            Japanese.SetParameter("MHTMenu", "", new FakeIniParameter(""));
            Japanese.SetParameter("MHTSim", "", new FakeIniParameter(""));
        }
    }
}
