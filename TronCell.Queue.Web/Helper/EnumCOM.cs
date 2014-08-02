using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TronCell.Queue.Web.Helper
{
    public class EnumCOM
    {
        public static string GetEnumDispalyName(object e)
        {
            if (e == null) return "";
            //获取字段信息
            System.Reflection.FieldInfo[] ms = e.GetType().GetFields();

            Type t = e.GetType();
            foreach (System.Reflection.FieldInfo f in ms)
            {
                //判断名称是否相等
                if (f.Name != e.ToString()) continue;

                //反射出自定义属性
                foreach (Attribute attr in f.GetCustomAttributes(true))
                {
                    //类型转换找到一个Description，用Description作为成员名称
                    DisplayAttribute dsplay = attr as DisplayAttribute;
                    if (dsplay != null)
                        return dsplay.Name;
                }

            }
            //如果没有检测到合适的注释，则用默认名称
            return e.ToString();
        }
    }
}