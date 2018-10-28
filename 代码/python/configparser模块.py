#_*_coding:utf-8_*_
# created by fujia on 8/19/18
'''
add_section
options
remove_option
remove_section
has_option
has_section
setdefault
write
'''
import configparser
#
conf=configparser.ConfigParser()

# ---------------------------------创建config配置文件---------------------------------------------
# 添加section
conf.add_section("grouptest")
conf.add_section("group3")
conf.add_section("group2")

# 添加键值
conf['group3']['g3_name']="groupNmae3"
conf['group3']['name']="name"
conf.set("group2","g2_name","groupname2")

# 删除
conf.remove_option("group3","name")
if "grouptest" in conf.sections():
    conf.remove_section("grouptest")
# 写入保存
conf.write(open("config_test.ini",'w'))
