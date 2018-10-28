# -*- coding: utf-8 -*-
import  chardet
import cx_Oracle
import  sys
import io
import os
try:

    #字符编码设置
    os.environ['NLS_LANG'] = 'SIMPLIFIED CHINESE_CHINA.UTF8'



    sys.stdout = io.TextIOWrapper(sys.stdout.buffer, encoding='utf-8')  # 改变标准输出的默认编码
    tt = "tt"
    v = chardet.detect(bytes(tt, encoding='utf-8'))
    rrr = bytes(tt, encoding='utf-8').decode("utf-8")

    title = "【狂暑季 满1件立享4折】美特斯邦威 条纹短袖T恤女夏装基础款简约纯棉舒适合体打底上衣".replace("【","").replace("】","")
    price = "¥49.00".replace("¥","")
    param = [("1",title,price), ("2",title,price)]

    # 获取数据库连接方式
    # oracle_tns = cx_Oracle.makedsn('XXX.XXX.XXX.XXX', 1521, 'oracleName')
    # connectObj = cx_Oracle.connect('oracleUserName', 'password', oracle_tns)
    # db = cx_Oracle.connect('hr', 'hrpwd', 'localhost:1521/XE')
    # db1 = cx_Oracle.connect('hr/hrpwd@localhost:1521/XE')
    conn = cx_Oracle.connect('c##test/tsc++2012@127.0.0.1:1521/orcl')
    # 获取游标操作对象(增\删\改\查)
    cr = conn.cursor()

    # 单条插入
    # cr.execute("  insert into test(id,title,price) values(:id,:title,:price)",[str(1), title.replace("【", "").replace("】", ""), price.replace("¥", "")])
    # coun = conn.commit()

    # sql = "INSERT INTO T_AUTOMONITOR_TMP(point_id) VALUES(:pointId)"
    # cr.prepare(sql)
    # rown = cr.execute(None, {'pointId': pointId})
    # cr.commit()


    # 批量插入
    # cr.executemany("  insert into test(id,title,price) values(:id,:title,:price)",param)
    # coun = conn.commit()

    # 删除
    # cr.execute("  delete from test t where t.id=:id ",[str(1)])
    # coun = conn.commit()

    # 改
    # cr.execute("  update test t set t.title=:title where t.id=:id ", ["fffffff",str(2)])
    # coun = conn.commit()

    # 条件查询
    cr.execute("  select * from test t where t.id=:id ", [str(2)])
    tabb=cr.fetchone()
    print(tabb)

    # 查询指定条数
    # cr.execute(" select * from test t ")
    # tabb = cr.fetchmany(2)
    # print(tabb)

    # 全查询
    # cr.execute(" select * from test t ")
    # tabb = cr.fetchall()
    # print(tabb)


    # sql=" select column_name from all_tab_columns where table_name=:t"
    # cr.prepare(sql)
    # cr.execute(" select column_name from all_tab_columns where table_name='test'",{'t': "test"})
    # tabb=cr.fetchall()
    # print(tabb)





    cr.close()
    conn.close()
    # print(rrr)
except Exception as err:
    print("开启数据库失败{0}".format(err))
