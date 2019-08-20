using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Caching;

namespace Utility.Components
{
    public class CacheHelper
    {

        public static CacheHelper Single
        {
            get
            {
                return new CacheHelper() { };
            }
        }

        /// <summary>
        /// 获取缓存
        /// </summary>
        /// <typeparam name="T">存储及返回类型</typeparam>
        /// <param name="key">缓存键</param>
        /// <param name="timeout">超时时间(分钟)，0:无限制</param>
        /// <param name="callData">返回数据的方法</param>
        /// <param name="AbsoluteExpiration">是否绝对过期</param>
        /// <returns></returns>
        public T TryGet<T>(string key, int timeout, Func<T> callData, bool AbsoluteExpiration = true) where T : class
        {
            T value = default(T);
            if (ContainsKey(key) && timeout >= 0)
            {
                return HttpRuntime.Cache.Get(key) as T;
            }
            value = callData();
            if (value == null)
            {
                return null;
            }
            // -1 实时执行，不启用缓存
            if (timeout < 0)
            {
                return value;
            }
            Insert<T>(key, value, timeout, AbsoluteExpiration);
            value = HttpRuntime.Cache[key] as T;
            return value;
        }

        /// <summary>
        /// 获取缓存
        /// </summary>
        /// <typeparam name="T">存储及返回类型</typeparam>
        /// <param name="key">缓存键</param>
        /// <param name="timeout">超时时间(分钟)，0:无限制</param>
        /// <param name="callData">返回数据的方法</param>
        /// <param name="AbsoluteExpiration">是否绝对过期</param>
        /// <returns></returns>
        public object Get(string key, int timeout, Func<object> callData, bool AbsoluteExpiration = true)
        {
            if (ContainsKey(key) && timeout >= 0 && HttpRuntime.Cache[key] != null)
            {
                return HttpRuntime.Cache.Get(key);
            }
            var value = callData();
            if (value == null)
            {
                return null;
            }
            // -1 实时执行，不启用缓存
            if (timeout < 0)
            {
                return value;
            }
            Insert(key, value, timeout, AbsoluteExpiration);
            return HttpRuntime.Cache[key];

        }

        /// <summary>
        /// 获取当前应用程序指定CacheKey的Cache值
        /// </summary>
        /// <param name="CacheKey"></param>
        /// <returns></returns>
        public static object GetCache(string CacheKey)
        {
            System.Web.Caching.Cache objCache = HttpRuntime.Cache;
            return objCache[CacheKey];

        }


        /// <summary>
        /// 将指定key保存到缓存并返回，存在则设置，不存在则新增
        /// </summary>
        /// <param name="key">用于引用该对象的缓存键。</param>
        /// <param name="value">要插入缓存中的对象。</param>
        /// <param name="timeOut">分钟</param>
        public T TrySave<T>(string key, T value, int timeout = 0, bool AbsoluteExpiration = true) where T : class
        {
            if (ContainsKey(key))
            {
                HttpRuntime.Cache[key] = value;
            }
            else
            {
                Insert<T>(key, value, timeout, AbsoluteExpiration);
            }
            return value;
        }

        /// <summary>
        /// 插入缓存
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="timeout"></param>
        /// <param name="AbsoluteExpiration"></param>
        public void Insert<T>(string key, T value, int timeout, bool AbsoluteExpiration) where T : class
        {
            if (timeout == 0)
            {
                // 0 无限期，不会自动清除缓存，重启站点时清空
                HttpRuntime.Cache.Insert(key, value, null, Cache.NoAbsoluteExpiration, Cache.NoSlidingExpiration, CacheItemPriority.NotRemovable, null);
            }
            else
            {
                if (AbsoluteExpiration)
                {
                    //绝对过期 当前时间+timeout 分钟后过期(默认)
                    HttpRuntime.Cache.Insert(key, value, null, DateTime.UtcNow.AddMinutes(timeout), Cache.NoSlidingExpiration, CacheItemPriority.Normal, null);
                }
                else
                {
                    //延时过期 timeout分钟内使用则延时，未使用则过期
                    HttpRuntime.Cache.Insert(key, value, null, Cache.NoAbsoluteExpiration, TimeSpan.FromMinutes(timeout), CacheItemPriority.Low, null);
                }
            }
        }

        /// <summary>
        /// 插入缓存
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="timeout"></param>
        /// <param name="AbsoluteExpiration"></param>
        public void Insert(string key, object value, int timeout, bool AbsoluteExpiration)
        {
            if (timeout == 0)
            {
                // 0 无限期，不会自动清除缓存，重启站点时清空
                HttpRuntime.Cache.Insert(key, value, null, Cache.NoAbsoluteExpiration, Cache.NoSlidingExpiration, CacheItemPriority.NotRemovable, null);
            }
            else
            {
                if (AbsoluteExpiration)
                {
                    //绝对过期 当前时间+timeout 分钟后过期(默认)
                    HttpRuntime.Cache.Insert(key, value, null, DateTime.UtcNow.AddMinutes(timeout), Cache.NoSlidingExpiration, CacheItemPriority.Normal, null);
                }
                else
                {
                    //延时过期 timeout分钟内使用则延时，未使用则过期
                    HttpRuntime.Cache.Insert(key, value, null, Cache.NoAbsoluteExpiration, TimeSpan.FromMinutes(timeout), CacheItemPriority.Low, null);
                }
            }
        }

        /// <summary>
        /// 判断Cache是否存在
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public bool ContainsKey(string key)
        {
            return HttpContext.Current.Cache[key] != null;
        }

        /// <summary>
        /// 删除指定的Cache
        /// </summary>
        /// <param name="key">Cache的key</param>
        public void Remove(string key)
        {
            if (ContainsKey(key))
            {
                HttpRuntime.Cache.Remove(key);
            }
        }

        /// <summary>
        /// 删除指定的数据的缓存
        /// </summary>
        /// <param name="key">Cache的key</param>
        public void Remove(int id)
        {
            if (ContainsKey("PageData-" + id))
            {
                HttpRuntime.Cache.Remove("PageData-" + id);
            }
        }


        /// <summary>
        /// 有时可能需要立即更新,这里就必须手工清除一下Cache
        /// Cache类有一个Remove方法,但该方法需要提供一个CacheKey
        /// 但整个网站的CacheKey我们是无法得知的,只能经过遍历
        /// </summary>
        public void RemoveAll()
        {
            var cacheEnum = HttpRuntime.Cache.GetEnumerator();
            var list = new List<string>();

            while (cacheEnum.MoveNext())
            {
                list.Add(cacheEnum.Key.ToString());
            }

            foreach (var key in list)
            {
                HttpRuntime.Cache.Remove(key);
            }
        }

        /// <summary>
        /// 显示所有缓存
        /// </summary>
        /// <returns></returns>
        public List<string> ShowAll()
        {
            var list = new List<string>();
            //var str = "当前程序总缓存数为：" + HttpRuntime.Cache.Count;
            var cacheEnum = HttpRuntime.Cache.GetEnumerator();

            while (cacheEnum.MoveNext())
            {
                list.Add(HttpUtility.HtmlDecode(cacheEnum.Key.ToString()));
            }
            return list;
        }
    }
}
