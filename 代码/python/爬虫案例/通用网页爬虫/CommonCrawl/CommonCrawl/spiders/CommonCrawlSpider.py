# -*- coding: utf-8 -*-
import scrapy


class CommoncrawlspiderSpider(scrapy.Spider):
    name = 'CommonCrawlSpider'
    allowed_domains = ['dangdang.com']
    start_urls = ['http://dangdang.com/']

    def parse(self, response):
        pass
