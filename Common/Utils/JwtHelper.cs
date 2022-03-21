 
using JWT;
using JWT.Algorithms;
using JWT.Serializers;
using Mapper;
using MVC卓越项目.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace MVC卓越项目.Commons.Utils
{
    public class JwtHelper<T>
    {

        //私钥  web.config中配置
        private static string secret = ConfigurationManager.AppSettings["JwtKey"].ToString();

        /// <summary>
        /// 生成Token
        /// </summary>
        /// <param name="payload">不敏感的用户数据</param>
        /// <returns></returns>
        public static string getJwtEncode(T eshopUser)
        {
            //SHA256 算法加密
            IJwtAlgorithm algorithm = new HMACSHA256Algorithm();
            //json序列化
            IJsonSerializer serializer = new JsonNetSerializer();
            IBase64UrlEncoder urlEncoder = new JwtBase64UrlEncoder();
            //创建 jwt 加密器
            IJwtEncoder encoder = new JwtEncoder(algorithm, serializer, urlEncoder);
            var token = encoder.Encode(eshopUser, secret);
            return token;
        }

        /// <summary>
        /// 根据Token  获取实体
        /// </summary>
        /// <param name="token">jwtToken</param>
        /// <returns></returns>
        public static T getUserByToken(string token)
        {
            //json序列化
            IJsonSerializer serializer = new JsonNetSerializer();
            IDateTimeProvider provider = new UtcDateTimeProvider();
            IJwtValidator validator = new JwtValidator(serializer, provider);
            IBase64UrlEncoder urlEncoder = new JwtBase64UrlEncoder();
            //SHA256 算法解密
            IJwtAlgorithm algorithm = new HMACSHA256Algorithm();
            //获取jwt解码器
            IJwtDecoder decoder = new JwtDecoder(serializer, validator, urlEncoder, algorithm);
            var userInfo = decoder.DecodeToObject<T>(token, secret, verify: true);//token为之前生成的字符串
            return userInfo;
        }

    }
}