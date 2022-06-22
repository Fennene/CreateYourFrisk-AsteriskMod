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
            English.SetParameter("CYFOption", "Exit", new FakeIniParameter("Returns to the Mod Select screen."));
            English.SetParameter("CYFOption", "Git", new FakeIniParameter("Goes to official CYF's GitHub page."));
            //English.SetParameter("CYFOption", "", new FakeIniParameter(""));

            English.SetParameter("AsteriskOption", "Hover", new FakeIniParameter("Hover over an option and its description will appear here!"));
            English.SetParameter("AsteriskOption", "Lang", new FakeIniParameter("Display Language\n\nSwitches display language\nto English or Japanese."));
            English.SetParameter("AsteriskOption", "Title", new FakeIniParameter("Replace Prepared Mods' Title\n\nWhether replace mods' title and subtitle to prepared from modders.\n\nDefault by On"));
            English.SetParameter("AsteriskOption", "Desc", new FakeIniParameter("Always Description Visible\n\nWhether mods' descriptions are always shown or not.\n\nDefault by On"));
            English.SetParameter("AsteriskOption", "SafeSetAlMightGlobal", new FakeIniParameter("Permission of changing system option's data from mods' code.\n\nSave data of option also uses AlMighty Global, so mod can change that data forcely.\nThis feature will prevent that illegal change.\n\nYou can select option: Error, Ignore, Allow.\n<b>Error</b> shows error message if that change happens.\n<b>Ignore</b> prevents that change and continues mod running.\n<b>Allow</b> allows that change.\n\nDefault by \"Error\""));
            English.SetParameter("AsteriskOption", "Experiment", new FakeIniParameter("Experimental Features\n\nThis feature allows the mod uses experimental features.\nExperimental Features contains the feature that is added in the future update or may be unstable or occur error.\n\nDefault by Off"));
            English.SetParameter("AsteriskOption", "Exit", new FakeIniParameter("Returns to the Mod Select screen."));
            English.SetParameter("AsteriskOption", "Git", new FakeIniParameter("Goes to AsteriskMod's GitHub page."));
            //English.SetParameter("AsteriskOption", "", new FakeIniParameter(""));

            English.SetParameter("MHTMenu", "Hover", new FakeIniParameter("Hover over an option and its description will appear here!"));
            English.SetParameter("MHTMenu", "Document", new FakeIniParameter("<color=#FF0>Open CYF Documentation</color>\n\nOpens the documentation of\nCreate Your Frisk.\nIt is written about all of CYF\n and AsteriskMod.\n\nYou should read document before asking\nany question to CYF Discord."));
            English.SetParameter("MHTMenu", "Sim", new FakeIniParameter("<color=#FF0>Battle Simulator</color>\n\nsimulates sprite(bullet) object,\nstatic text object\nand etc...\n\n<color=#FF0>Recommended to set window scale to 2!</color>"));
            English.SetParameter("MHTMenu", "Exit", new FakeIniParameter("Returns to the Mod Select screen."));
            //English.SetParameter("MHTMenu", "", new FakeIniParameter(""));

            //English.SetParameter("MHTSim", "", new FakeIniParameter(""));
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
            Japanese.SetParameter("CYFOption", "ResetRG", new FakeIniParameter("Real Globalをリセットします。\n\nReal Globalは、戦闘を通じて\n保持される値です。\nしかし、CYFを閉じると\nそのデータは消えます。"));
            Japanese.SetParameter("CYFOption", "ResetAG", new FakeIniParameter("AlMighty Globalをリセットします。\n\nAlMighty Globalはファイルに\n保存される値です。\nCYFを閉じてもそのデータが\n保持されます。\n\nこのオプションも\nAlMighty Globalを使用して\n保存されています。"));
            Japanese.SetParameter("CYFOption", "ClearSave", new FakeIniParameter("セーブファイルを削除します。\n\nこれはCYFのOverworldに使用されているファイルを表します。\n\nセーブファイルのパスは以下の通り:\n\n"));
            Japanese.SetParameter("CYFOption", "Safe", new FakeIniParameter("Safeモードの切り替え。\n\nこのオプション自体は\n何もしませんが、\nModの製作者がこのオプションを\n検出し、\n流血表現やセンシティブな\nコンテンツなどを\nフィルタリングできます。"));
            Japanese.SetParameter("CYFOption", "Retro", new FakeIniParameter("Retrocompatibility(下位互換性)モードの切り替え。\n\nこれはUnitale v0.2.1a で製作されたModを\nCYFで動かすための\n特殊なモードです。\n\n\n\n<b>警告！</b>\nCYF用に製作されたModでは\n<b>無効(Off)にしてください！</b>\nこの機能は<b>Unitale v0.2.1a</b>用に\n作成されたModに対してのみ\n有効です。"));
            Japanese.SetParameter("CYFOption", "Fullscreen", new FakeIniParameter("Blurless Fullscreen(鮮明なフルスクリーン)\nモードの切り替え。\n\nこれはフルスクリーンを\n「ぼやけて(\"blurry\")」表示するか\nどうかを設定します。\n\n\n<b>F4キー</b>または<b>Altキー + Enterキー</b>で\nフルスクリーン表示になります。"));
            Japanese.SetParameter("CYFOption", "Scale", new FakeIniParameter("(ウインドウ表示での)ウインドウの\n拡大縮小。\n\nこれは特に、\n大画面(4Kモニターなど)での表示で\n役に立ちます。\n\nフルスクリーン表示では特に効果はありません。"));
            Japanese.SetParameter("CYFOption", "Discord", new FakeIniParameter("CYFプレイ中において、Discord Rich Presenceにどのくらい表示するかを設定をします。\n\n<b>Everything</b>: 全て表示 - プレイ中のMod名、プレイ時間、\n説明を表示します。\n\n<b>Game Only</b>: ゲーム名のみ - CYFで遊んでいることのみを\n表示します。\n\n<b>Nothing</b>: 無効 - Discord Rich Presenceを無効にします。\n\nCYFとDiscordの接続が切れた場合は\nCYFを再起動する必要が\nあります。"));
            Japanese.SetParameter("CYFOption", "Exit", new FakeIniParameter("Mod選択画面に戻ります。"));
            Japanese.SetParameter("CYFOption", "Git", new FakeIniParameter("公式CYFのGitHubページに移動します。"));
            //Japanese.SetParameter("CYFOption", "", new FakeIniParameter(""));

            Japanese.SetParameter("AsteriskOption", "Hover", new FakeIniParameter("オプションにカーソルを乗せると\nここに説明が表示されます。"));
            Japanese.SetParameter("AsteriskOption", "Lang", new FakeIniParameter("表示言語\n\n英語と日本語を切り替えます。"));
            Japanese.SetParameter("AsteriskOption", "Title", new FakeIniParameter("Modのタイトル表示\n\nModのタイトルやサブタイトルを\nMod製作者が用意したものに\n表示を切り替えるかどうか。\n\nデフォルトでは有効(On)"));
            Japanese.SetParameter("AsteriskOption", "Desc", new FakeIniParameter("常時Mod説明の表示\n\nModの説明文を\n常時表示するかどうか。\n\nデフォルトでは有効(On)"));
            Japanese.SetParameter("AsteriskOption", "SafeSetAlMightGlobal", new FakeIniParameter("Modのコードから\nシステムのオプションの\n変更許可\n\nオプションのデータも\nAlMighty Globalを使用して\n保存されるため\nModからそのデータを\n強制的に変更できます。\nこの機能はその不正な変更を\n防ぎます。\n\n以下のオプションを利用できます。\n<b>Error</b> - 不正な変更を検出し\nエラーを表示します。\n<b>Ignore</b> - 不正な変更を防ぎ\nModの実行を継続します。\n<b>Allow</b> - 不正な変更を許可します。\n\nデフォルトではエラー(Error)"));
            Japanese.SetParameter("AsteriskOption", "Experiment", new FakeIniParameter("実験的機能\n\nこのオプションは\nModに実験的機能を使用することを\n許可します。\n実験的機能は\n将来の更新で追加される要素、\n動作が不安定である要素、\nエラーの可能性のある要素\nを含んできます。\n\nデフォルトでは無効(Off)"));
            Japanese.SetParameter("AsteriskOption", "Exit", new FakeIniParameter("Mod選択画面に戻ります。"));
            Japanese.SetParameter("AsteriskOption", "Git", new FakeIniParameter("AsteriskModの\nGitHubページに移動します。"));
            //Japanese.SetParameter("AsteriskOption", "", new FakeIniParameter(""));

            Japanese.SetParameter("MHTMenu", "Hover", new FakeIniParameter("オプションにカーソルを乗せると\nここに説明が表示されます。"));
            Japanese.SetParameter("MHTMenu", "Document", new FakeIniParameter("<color=#FF0>CYFドキュメントへの移動</color>\n\nCYFのドキュメントを開きます。\nドキュメントには\nCYFとAsteriskModについての\n全てが書かれています。\n\nCYF公式Discordチャンネルで聞く前に\nまずはドキュメントを確認してみましょう。"));
            Japanese.SetParameter("MHTMenu", "Sim", new FakeIniParameter("<color=#FF0>Battle Simulator</color>\n\nスプライト(弾丸)オブジェクトや\n静的テキストオブジェクトなどを\nシミュレートします。\n\n<color=#FF0>ウインドウの拡大倍率を\n２倍にすることをおすすめします。</color>"));
            Japanese.SetParameter("MHTMenu", "Exit", new FakeIniParameter("Mod選択画面に戻ります。"));
            //Japanese.SetParameter("MHTMenu", "", new FakeIniParameter(""));

            //Japanese.SetParameter("MHTSim", "", new FakeIniParameter(""));
        }
    }
}
