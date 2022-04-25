using System;

namespace AsteriskMod
{
    /// <summary>Container of the text for translation<br/>翻訳用テキストのクラス</summary>
    public class TransText
    {
        /// <summary>Normal Text<br/>通常テキスト</summary>
        private string normalText;
        /// <summary>The Text on Crate Your Frisk<br/>Crate Your Frisk時のテキスト</summary>
        private string cratedText;
        /// <summary>Crate Your Frisk化用の関数。</summary>
        private Func<string, string> Crateing;

        public TransText(string text, string crate = null)
        {
            normalText = text;
            cratedText = crate;
            Crateing = null;
        }

        /// <summary>Sets the texts.<br/>テキストを設定します。</summary>
        /// <param name="normal">The text that is normal and base of crateing<br/>通常のテキスト(crate化の基となる文)</param>
        /// <param name="crate">THE TXET TAHT THE EGENE IS CRATE<br/>ｴﾝｼﾞﾝがcrate時のﾃｷｽﾄ</param>
        public void SetTexts(string normal, string crate = null)
        {
            if (string.IsNullOrEmpty(normal)) throw new Exception("The normal text should not be null or empty string.\n通常のテキストはnullや空の文字列であってはいけません。");
            normalText = normal;
            cratedText = crate;
        }

        /// <summary>Sets the function of crateing.<br/>crate化用の関数を設定します。</summary>
        public void SetCrateingFunc(Func<string, string> temmify) { Crateing = temmify; }

        public string Get()
        {
            if (!GlobalControls.crate) return normalText;
            if (cratedText != null)    return cratedText;
            if (Crateing != null)      return Crateing(normalText);
            return normalText;
        }
    }
}
