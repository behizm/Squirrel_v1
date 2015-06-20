using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web;
using BotDetect;
using BotDetect.Web.UI.Mvc;

namespace Squirrel.Web.Models
{
    public static class CaptchaHelper
    {
        public static MvcCaptcha GetCaptcha()
        {
            var rcaptcha = new MvcCaptcha("CaptchaId")
            {
                ImageFormat = ImageFormat.Jpeg,
                ImageStyle = ImageStyle.SunAndWarmAir,
                CodeLength = 4,
                CaptchaImageTooltip = "عبارت امنیتی را به درستی وارد کنید.",
                CodeStyle = CodeStyle.Numeric,
                ImageSize = new Size(130, 47),
                UserInputClientID = "CaptchaText"
            };

            return rcaptcha;
        }
    }
}