using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebApiDemo.Common;

namespace WebApiDemo.Controllers
{
    public class TestController : ApiController
    {
        [HttpGet]
        public IHttpActionResult Get()
        {
            ResponseResult rr = new ResponseResult();
            try
            {
                rr.Value = new
                {
                    content = new List<dynamic> {
                        //new {model="任务",subModel=new List<dynamic> { new { subModelName = "语料库" },new { subModelName = "语料库" } } }
                        //,new {model="订单",subModel=new List<dynamic> { new { subModelName = "语料库" } } }
                        new {model="挖掘",subModel=new List<dynamic> { new { subModelName = "语料库",subModelUrl=""}, new { subModelName = "词典", subModelUrl = "/dataproject-war/excavate/dictionary" }, new { subModelName = "网页", subModelUrl = "/dataproject-war/excavate/webpage" }, new { subModelName = "文档", subModelUrl = "" } } }
                        ,new {model="句对齐",subModel=new List<dynamic> { new { subModelName = "人工对齐", subModelUrl = "" }, new { subModelName = "自动对齐", subModelUrl = "" } } }
                        ,new {model="泛术语发现",subModel=new List<dynamic> { new { subModelName = "新词", subModelUrl = "" }, new { subModelName = "热词", subModelUrl = "" }, new { subModelName = "术语提取", subModelUrl = "" }, new { subModelName = "关键词提取", subModelUrl = "" } } }
                        ,new {model="众测",subModel=new List<dynamic> { new { subModelName = "术语审校", subModelUrl = "/dataproject-war/terminoly/addTermTask" }, new { subModelName = "MTPE", subModelUrl = "/dataproject-war/pe/addPeTask" }, new { subModelName = "MT专项评测", subModelUrl = "/dataproject-war/evalute/addEvalute" }, new { subModelName = "MT人工评测", subModelUrl = "" }, new { subModelName = "DNT/NE标注", subModelUrl = "" } } }
                        ,new {model="语言特征",subModel=new List<dynamic> { new { subModelName = "词法分析", subModelUrl = "" }, new { subModelName = "句法分析", subModelUrl = "" }, new { subModelName = "语义分析", subModelUrl = "" }, new { subModelName = "文档分析", subModelUrl = "" } } }
                        ,new {model="数据工程",subModel=new List<dynamic> { new { subModelName = "本地化工程", subModelUrl = "" }, new { subModelName = "语言数据工程", subModelUrl = "" } } }
                        ,new {model="LQA",subModel=new List<dynamic> { new { subModelName = "定性检查", subModelUrl = "" }, new { subModelName = "定量检查", subModelUrl = "" } } }
                        ,new {model="iSearch",subModel=new List<dynamic> { new { subModelName = "句对", subModelUrl = "" }, new { subModelName = "术语", subModelUrl = "" }, new { subModelName = "TSCWiki", subModelUrl = "" }, new { subModelName = "友商资料", subModelUrl = "" } } }
                    }
                };
                rr.Status = 1;
            }
            catch (Exception ex)
            {
                rr.Status = -1;
                rr.Message = ex.Message;
            }
            return Json<dynamic>(rr.Value);
        }

        [HttpPost]
        public IHttpActionResult Get(dynamic id)
        {
            return Json<string>("value");
        }

       [HttpPost]
        public void Post([FromBody]string value)
        {
        }

        // PUT: api/Test/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/Test/5
        public void Delete(int id)
        {
        }
    }
}
