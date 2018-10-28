using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace OneCheck.Common
{
    class RegexHelper
    {
        /// <summary>
        /// 判断是否是0-100的数字
        /// </summary>
        public static bool IsNumberZeroAndHundred(string text)
        {
            Regex re = new Regex(@"^(?:0|[1-9][0-9]?|100)$");
            return re.IsMatch(text);//匹配成功

        }
        /// <summary>
        /// 判断是否是1-100的数字
        /// </summary>
        public static bool IsNumberOneAndHundred(string text)
        {
            Regex re = new Regex(@"^([1-9][0-9]?|100)$");
            return re.IsMatch(text);//匹配成功
        }
        /// <summary>
        /// 判断是字符串中是否含有中文
        /// </summary>
        public static bool MatchedZHCN(string sentence)
        {
            //MatchCollection collection= Regex.Matches(sentence, "[\u4e00-\u9fa5]");
            return Regex.IsMatch(sentence, @"[\u4e00-\u9fa5]");
        }
    }
}
