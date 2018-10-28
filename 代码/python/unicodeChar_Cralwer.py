# -*- coding: utf-8 -*-
from htmlHelper import htmlHelper
from beautifulSoupHelper import beautifulSoupHelper
from unicodeHelper import unicodeHelper

count=0
# 中文字符范围
start=0x4E00
end=0x9FBF

# start=0x3040
# end=0x309F

crawlerUrl4="https://hanyu.baidu.com/zici/s?wd=%E9%BE%9C"
beautifulSoup_helper=beautifulSoupHelper()
html_Helper=htmlHelper()
for i in range(start,end):
    count+=1
    unicode_char= unicodeHelper.HexData2Unicode(i)
    url= "https://hanyu.baidu.com/zici/s?wd={0}".format(htmlHelper.urlencode(unicode_char))
    htmlSouce= html_Helper.getHtmlSourceStr(url)
    # print(htmlSouce)
    pinyin= beautifulSoup_helper.getPinYin(htmlSouce)
    print("{0}:{1}____{2}pinyin:{3}".format(count,url,unicode_char,pinyin))
print(count)
