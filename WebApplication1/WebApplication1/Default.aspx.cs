using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using WebApplication1.cs;

namespace WebApplication1
{
    public partial class _Default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //string a = Request["a"].ToString();
            //if (a != null)
            //{
            //    this.Label1.Text = a;
            //}
            //else
            //{
            //    this.Label1.Text = DateTime.Now.ToString();
            //}
            //    StartClientCaching();
            this.Label1.Text = DateTime.Now.ToString();
            this.Label2.Text = DateTime.Now.ToString();
        }
        private void StartClientCaching()
        {
            //指定缓存时间  
            int secondsTime = 30;

            string headers = Request.Headers["If-Modified-Since"];
            long nowTicks = DateTime.Now.Ticks;
           
            if (headers != null)
            {
                long modifiedTick = DateTime.Parse(headers).Ticks;
                long ticks = nowTicks - modifiedTick;
                if (TimeSpan.FromTicks(ticks).Seconds < secondsTime)
                {

                    Response.Write(DateTime.Now);
                    Response.StatusCode = 304;
                    // 这种方式会产生“此操作要求使用 IIS 集成管线模式。 ”的异常  
                    //Response.Headers.Add("Content-Encoding", "gzip");  
                    // Response.Headers.Add 和 Response.AddHeader 但是在MSDN中明确写出，这些都是为了兼容ASP，在.NET 3.5要求使用下面这种方式。  
                    //这种方式不会出现异常  
                    Response.AppendHeader("Content-Encoding", "gzip");
                    Response.StatusDescription = "Not Madified";
                }
            }
            else
            {
                Response.Write(DateTime.Now);
                SetClientCaching(Response, DateTime.Now);
            }
        }
        private void SetClientCaching(HttpResponse response, DateTime lastModified)
            {
                //将ETag Http标头设置为制定字符串  
                response.Cache.SetETag(lastModified.Ticks.ToString());
                //将Last_Modified Http标头设置为提供的System.DateTime值  
                response.Cache.SetLastModified(lastModified);
                //将Cache-Control 标头设置为System.Web.HttpCacheability值之一  
                response.Cache.SetCacheability(System.Web.HttpCacheability.Public);
                //基于制定的时间跨度设置Cache-Control: max-age HTTP标头  
                response.Cache.SetMaxAge(new TimeSpan(0, 0, 0, 3));
                //将缓存过期从绝对时间设置为可调时间  
                response.Cache.SetSlidingExpiration(true);
            }

        protected void Button1_Click(object sender, EventArgs e)
        {
            this.Label1.Text = "实时";
        }

    }
}
