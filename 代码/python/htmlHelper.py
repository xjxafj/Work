# -*- coding:utf-8 -*-
import urllib.request
import urllib.parse
import urllib
import random
import chardet

class htmlHelper:
    crawlerUrl1 = "https://news.qq.com/a/20180312/035287.htm"
    crawlerUrl2 = "http://www.baidu.com/"
    crawlerUrl3 = "http://10.137.55.248:8090/"
    crawlerUrl4 = "https://hanyu.baidu.com/zici/s?wd=%E4%B8%AD"

    # 页面编码
    encoding = "ut-8"
    # 对数据编码
    postBody = urllib.parse.urlencode({
        "name": "name",
        "pass": "pass"
    }).encode("utf-8")

    proxy_list = [
        {"http": "账号:密码@proxy.gg.com:8080"}
        # {"http": "http:/proxy.gg.com:8080"}
    ]

    USER_AGENTS = [
        "Mozilla/5.0 (compatible; MSIE 9.0; Windows NT 6.1; Win64; x64; Trident/5.0; .NET CLR 3.5.30729; .NET CLR 3.0.30729; .NET CLR 2.0.50727; Media Center PC 6.0)",
        "Mozilla/5.0 (compatible; MSIE 8.0; Windows NT 6.0; Trident/4.0; WOW64; Trident/4.0; SLCC2; .NET CLR 2.0.50727; .NET CLR 3.5.30729; .NET CLR 3.0.30729; .NET CLR 1.0.3705; .NET CLR 1.1.4322)",
        "Mozilla/4.0 (compatible; MSIE 7.0b; Windows NT 5.2; .NET CLR 1.1.4322; .NET CLR 2.0.50727; InfoPath.2; .NET CLR 3.0.04506.30)",
        "Mozilla/5.0 (Windows; U; Windows NT 5.1; zh-CN) AppleWebKit/523.15 (KHTML, like Gecko, Safari/419.3) Arora/0.3 (Change: 287 c9dfb30)",
        "Mozilla/5.0 (X11; U; Linux; en-US) AppleWebKit/527+ (KHTML, like Gecko, Safari/419.3) Arora/0.6",
        "Mozilla/5.0 (Windows; U; Windows NT 5.1; en-US; rv:1.8.1.2pre) Gecko/20070215 K-Ninja/2.1.1",
        "Mozilla/5.0 (Windows; U; Windows NT 5.1; zh-CN; rv:1.9) Gecko/20080705 Firefox/3.0 Kapiko/3.0",
        "Mozilla/5.0 (X11; Linux i686; U;) Gecko/20070322 Kazehakase/0.4.5"
    ]

    # 随机选择一个代理并设置
    proxyDic = random.choice(proxy_list)
    # 获取User_Agent设置请求头
    user_agent = random.choice(USER_AGENTS)



    def __init__(self):
        pass
    # 获取指定Url的源字符串
    def getHtmlSourceStr(self,url):
        try:
            # 随机选择一个代理并设置
            # proxyDic = random.choice(self.proxy_list)
            # 获取User_Agent设置请求头
            user_agent = random.choice(self.USER_AGENTS)

            # 设置代理
            # proxy=urllib.request.ProxyHandler(proxyDic)
            # opener=urllib.request.build_opener(proxy,urllib.request.HTTPHandler)
            # urllib.request.install_opener(opener)

            # req.add_header("Cookie","BAIDUID=D8F571E0C0FBBF8578920341274B5E49:FG=1; BIDUPSID=D8F571E0C0FBBF8578920341274B5E49; PSTM=1515860626; BD_UPN=12314753; H_PS_PSSID=1456_21106_18559_22157; BDORZ=B490B5EBF6F3CD402E515D22BCDA1598; BD_CK_SAM=1; PSINO=3; BD_HOME=0; H_PS_645EC=f7edPjbi2a1VIdyzehiUXlUwqqlF3aVCNaM8ezO4SHP1rMP3AiJCESfeVoE")
            # req.add_header("Accept","text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,image/apng,*/*;q=0.8")
            # req.add_header("Accept-Encoding","gzip, deflate, br")
            # req.add_header("Accept-Language","zh-CN,zh;q=0.9")
            # req.add_header("Cache-Control","max-age=0")
            # req.add_header("Connection","keep-alive")
            # req.data=postBody     #添加请求参数

            # 构建请求
            req = urllib.request.Request(url)
            req.add_header("User-Agent", user_agent)


            TestData = urllib.request.urlopen(req).read()
            # 获取数据的编码
            encodeDic = chardet.detect(TestData)
            for key, value in encodeDic.items():
                if (key == "encoding"):
                    encoding = value
            # 获取最终数据
            sourceHtml = urllib.request.urlopen(req).read().decode(encoding, "ignore")
            if (sourceHtml != ""):
                return sourceHtml
            print(len(sourceHtml))
        except Exception as err:
            return "错误信息:{0}".format(err)

    '''
      *Function:  jsonData2DataObj
      *Description：  json对象转换成网络URL请求参数对象，一般Post请求需要  
      *Input:  jsonObj(json格式的对象) : {"name": "name","pass": "pass"}
               encoding(数据格式后的编码设置) : "utf-8"
      *Return: data(网络传输)
      *Others: 异常返回 null
    '''
    @classmethod
    def jsonData2DataObj(self, jsonObj,encoding="utf-8"):
            try:
                postBody = urllib.parse.urlencode(jsonObj).encode(encoding)
                return  postBody
            except Exception as error:
                pass
    #

    '''
         *Function:   urlencode
         *Description：对字符进行编码 
         *Input:  需要转换的字符   如：中
         *Return: 输出转换后的字符 如：%E4%B8%AD
         *Others: 异常返回 空
    '''
    @classmethod
    def urlencode(self,str):
        result=""
        try:
            result=urllib.parse.quote(str)
        except Exception as error:
            pass
        return result

    '''
             *Function:  urlencode
             *Description：  对字符进行解码
             *Input:  需要转换的字符   如：%E4%B8%AD
             *Return: 输出转换后的字符 如：中
             *Others: 异常返回 空
    '''
    @classmethod
    def urldecode(self, str):
        result = ""
        try:
            result=urllib.parse.unquote(str)
        except Exception as error:
            pass
        return result

