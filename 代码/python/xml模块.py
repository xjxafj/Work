
#_*_coding:utf-8_*_
# created by fujia on 8/19/18
import  xml.etree.cElementTree as ET
'''
    root=ET.Element("rootName") :创建根节点
    child_node=ET.SubElement(root,"book",{"id":"1","name":"name1"}):创建子节点,并添加属性
    child_node.text="text1"   :为节点赋值
    root.findall("book") :寻找root节点下book节点
    --root.findtext("book") :寻找root节点下book节点
'''

# 1,-------------------------------------------------------构建xml tree--------------------------------------------------------

# 根节点
root=ET.Element("bookstore")
# 子节点,并添加属性
book1_node=ET.SubElement(root,"book",{"id":"1","name":"name1"})
# 子节点增加子节点,并添加属性
name_node=ET.SubElement(book1_node,"name")
name_node.text="冰与火之歌"
author_node=ET.SubElement(book1_node,"author")
author_node.text="乔治马丁"
year_node=ET.SubElement(book1_node,"author")
year_node.text="2014"
price_node=ET.SubElement(book1_node,"author")
price_node.text="60"

book2_node=ET.SubElement(root,"book",{"id":"2","name":"name2"})
# 子节点增加子节点,并添加属性
name_node=ET.SubElement(book2_node,"name")
name_node.text="冰与火之歌1"
author_node=ET.SubElement(book2_node,"author")
author_node.text="乔治马丁1"
year_node=ET.SubElement(book2_node,"author")
year_node.text="2014"
price_node=ET.SubElement(book2_node,"author")
price_node.text="80"

# 2,-------------------------------------------------------构建的tree转化成xml tree对象,并保存--------------------------------------------------------
tree=ET.ElementTree(root)
# xml_declaration 为添加声明
tree.write("xml_test.xml",encoding="utf-8",xml_declaration=True)




# 获取xml文件的Tree对象
tree=ET.parse("xml_test.xml")
# 获取root节点
root=tree.getroot()
# 修改
# for node in root:
#     for childNode in node.iter("year"):
#         # 修改节点内容
#         childNode.text=str(int(childNode.text)+1)
#         # 修改节点属性(没有就创建,有则更新)
#         childNode.set("age","12")

# 删除节点
for node in root:
    for childNode in node.iter("price"):
       if int(childNode.text)==60:
           # 删除节点
           root.remove(node)




print('============={0}=============='.format(root.tag))
# 遍历所有节点
for node in root:
    print('--------------------------tag:{0} id:{1} name:{2}-----------------------'.format(node.tag,node.attrib['id'],node.attrib["name"]))
    for childNode in node:
        print('{0}:{1}'.format(childNode.tag,childNode.text))


# 遍历指定的节点
# for node in root.iter("book"):
#     print(node.tag,node.attrib["id"],node.attrib["name"])




# 保存文件
tree.write("xml_test.xml",encoding="utf-8")










