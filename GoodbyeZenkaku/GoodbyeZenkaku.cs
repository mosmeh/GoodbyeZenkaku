using System;
using System.ComponentModel.Composition;
using System.Text.RegularExpressions;
using Acuerdo.Plugin;
using Dulcet.Twitter;
using Inscribe.Storage;
using Microsoft.VisualBasic;

namespace GoodbyeZenkaku
{
    [Export(typeof(IPlugin))]
    class GoodbyeZenkaku : IPlugin
    {
        public string Name
        {
            get
            {
                return "GoodbyeZenkaku";
            }
        }

        public Version Version
        {
            get
            {
                return new Version(1, 0);
            }
        }

        public void Loaded()
        {
            var reg = new Regex("[０-９Ａ-Ｚａ-ｚ：，－　]+");
            TweetStorage.TweetStorageChanged += (sender, args) =>
                {
                    TwitterStatusBase status = args.Tweet.Status;
                    status.Text = reg.Replace(status.Text, ZenkakuToHankakuReplacer);
                    args.Tweet.SetStatus(status);
                };
        }

        public IConfigurator ConfigurationInterface
        {
            get
            {
                return null;
            }
        }

        static string ZenkakuToHankakuReplacer(Match m)
        {
            return Strings.StrConv(m.Value, VbStrConv.Narrow, 0);
        }
    }
}