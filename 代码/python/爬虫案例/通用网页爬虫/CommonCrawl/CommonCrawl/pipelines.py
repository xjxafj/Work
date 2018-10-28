# -*- coding: utf-8 -*-

# Define your item pipelines here
#
# Don't forget to add your pipeline to the ITEM_PIPELINES setting
# See: https://doc.scrapy.org/en/latest/topics/item-pipeline.html

import cx_Oracle
import chardet
import  sys
import  io
class CommoncrawlPipeline(object):
    def process_item(self, item, spider):
        # print("进入了")
        try:
            # sys.stdout = io.TextIOWrapper(sys.stdout.buffer, encoding='utf8')

            conn = cx_Oracle.connect('c##test/tsc++2012@127.0.0.1/orcl')
            cr = conn.cursor()
            for i in range(0, len(item['title'])):
                title = item['title'][i]
                price = item['price'][i]
                # chardet.detect()
                print("{0}=={1}".format(title, price))
                # cr.execute("  insert into test(id,title,price) values(:id,:title,:price)",[str(i),"ts",10])
                # conn.commit();
                # cr.close()
                # conn.close()
                print("成功开启数据库")
        except Exception as err:
            print("开启数据库失败{0}".format(err))
        return item
