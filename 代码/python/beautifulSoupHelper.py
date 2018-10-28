# -*- coding:utf-8 -*-
from bs4 import BeautifulSoup
# sourceHtml=html_Helper.getHtmlSourceStr(crawlerUrl4)
# if(sourceHtml!=""):
#         soup = BeautifulSoup(sourceHtml, "lxml")
#         result= soup.select_one("#pinyin").find_all("b")
#         # result=soup.title.get_text()
#         print(result)

class beautifulSoupHelper:
    def __init__(self):
        pass
    @classmethod
    def getPinYin(self,str):
        try:
            soup = BeautifulSoup(str, "lxml")
            result = soup.select_one("#pinyin").find_all("b")
            return result
        except Exception as err:
            return "错误信息beautifulSoupHelper:{0}".format(err)

