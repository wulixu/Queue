using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Security;

namespace TronCell.Queue.Web.Helper
{
    public class SendMSS
    {
        public SendMSS()
        {
        }

        public string sendmssForUser(string mobile, string strContent)
        {
            string sendurl = System.Configuration.ConfigurationManager.AppSettings["SEND_URL"].ToString();
            string uid = System.Configuration.ConfigurationManager.AppSettings["SEND_UID"].ToString();
            string pwd = System.Configuration.ConfigurationManager.AppSettings["SEND_PWD"].ToString();
            string Pass = FormsAuthentication.HashPasswordForStoringInConfigFile(pwd + uid, "MD5");
            StringBuilder sbTemp = new StringBuilder();

            sbTemp.Append("uid=" + uid + "&pwd=" + Pass + "&mobile=" + mobile + "&content=" + strContent);
            byte[] bTemp = System.Text.Encoding.UTF8.GetBytes(sbTemp.ToString());
            String postReturn = doPostRequest(sendurl, bTemp);
            return postReturn;
        }

        private string doPostRequest(string url, byte[] bData)
        {
            System.Net.HttpWebRequest hwRequest;
            System.Net.HttpWebResponse hwResponse;
            string strResult = string.Empty;
            try
            {
                hwRequest = (System.Net.HttpWebRequest)System.Net.WebRequest.Create(url);
                hwRequest.Timeout = 5000;
                hwRequest.Method = "POST";
                hwRequest.ContentType = "application/x-www-form-urlencoded";
                hwRequest.ContentLength = bData.Length;

                System.IO.Stream smWrite = hwRequest.GetRequestStream();
                smWrite.Write(bData, 0, bData.Length);
                smWrite.Close();
            }
            catch (System.Exception err)
            {
                return strResult;
            }

            //get response
            try
            {
                hwResponse = (HttpWebResponse)hwRequest.GetResponse();
                StreamReader srReader = new StreamReader(hwResponse.GetResponseStream(), Encoding.UTF8);
                strResult = srReader.ReadToEnd();
                srReader.Close();
                hwResponse.Close();
            }
            catch (System.Exception err)
            {
            }
            return strResult;
        }
    }
}