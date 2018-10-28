# -*- coding: utf-8 -*-
import scrapy
from CommonCrawl.items import CommoncrawlItem
from scrapy.http import Request
class DangdangcrawlspiderSpider(scrapy.Spider):
    name = 'dangdangCrawlSpider'
    allowed_domains = ['dangdang.com']
    start_urls = ['http://category.dangdang.com/cid4003844.html']

    def parse(self, response):
        # 获取全局的定义字段
        item = CommoncrawlItem()

        # 爬取的网页中获取指定的内容
        item["title"] = response.xpath("//a[@name='itemlist-title']/text()").extract()
        item["price"] = response.xpath("//span[@class='price_n']/text()").extract()

        # 提交数据
        yield item
        # Request.meta["proxy"]=''

        # 注意需要开启pipelines
        # pipelines 提交给pipelines处理 需要在settings.py文件开启pipelines的模块
        # 开启pipelines              settings.py 文件中对应的默认注释的模块取消注释
        # ITEM_PIPELINES = {
        #	'dangdangPageCrawler.pipelines.DangdangpagecrawlerPipeline': 300,
        # }