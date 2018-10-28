# 爬虫页面开始
from scrapy import cmdline
# 执行 指定的爬虫文件
#带日志的执行方式
# cmdline.execute("scrapy crawl dangdang".split())
#不带日志的执行方式
cmdline.execute("scrapy crawl dangdang --nolog".split())