using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// config 的摘要说明
/// </summary>
public class config
{
    public config()
    {
        //
        // TODO: 在此处添加构造函数逻辑
        //
    }
    // 应用ID,您的APPID
    public static string app_id = "2021000118645407";

    // 支付宝网关
    public static string gatewayUrl = "https://openapi.alipaydev.com/gateway.do";

    // 商户私钥，您的原始格式RSA私钥
    public static string private_key = "MIIEpAIBAAKCAQEAx4RF3CsyFIZXXECsMo4nPG1Nw+Pz4jJ+si+j8Mih6ZWhmgrhKqk1ZLmAgZlMjL/GuuzcZPKTOzbyI/AxyhJizWXbygjc5+6k/OWA5trZgGJa7/Xna5vTqY5HO4EP0caSW6O9NuRCm2iBrYkJieZvxbiNYKuULtwcM48kRmDB0Kn5ZUiyN6pJAh3AfwAgpPupuUKHmqU881KwLzg2q+Wl3v9fIMPmMSyF6mAduupUMNYksNcZOOWx+jqSonNXOe1R9QpCmgIX4DHCEyMubuZU9XkEYEQnKzwBg+IVXvKVMFIYe79fm3hQhQbZrz1VxAFNBm/G0oELO9L+Uxikqt4xlwIDAQABAoIBAQCuGXpsmTTmPCRXWAfrRahvTmIhofTbWXy9OF0ya6D+F5ggt4WsmGMDNewxKvhliaN80duwKwzXCydYvOx1jH2zBkM4lWtO6CwIkqxcxnQtV6ZK5qW+fQfCWWlHP2PyhfiJBuTB1YVZ32Ppbj9omjDGtDiKNq7kBSaGx028LAp0bZFmnN0F3mqRz2lD7s6I6h9/NXk0lbIYm9Pu851paRl+FvHUJ+APM2dG+Z9gIzf+Lc72u6eSNKSQnwNRZhL8v0ubu1nZ7Zt7vzMyu6Ec4F3iVaxKuGmtvRiF/58r7sWOtk7+gZAABNCaETx235WdHSS91GxMQZRV0RigNADON/yhAoGBAPxIVFP/2shSvdKO04CLKluIbHvCEHuuGsLCvW7zEY0nenLs82djTLfUONjFwDs02OGMTW8RsLioesBikYPWhq8XJ4EqylFi6M6Mged17Ky1KblRx5WLgcxW6vEUoAZxobitnovCv8RIoGxTSrWrNxY1JIgYAijqndtKWJZm7aUlAoGBAMp05eJC7r8WAJEnKbIY9IY/fyW19MEQJ6F0ZWGL7WJwXAIWXpz2N/HHuMMGYNXQLvtR5kNv1QWdcHHlc4q2tOauOZwc6DacG1mLeEYFcvBq6OVQUsHDXZITRRSpFugSDykCduzxiOXFLLki4a8roeZV2INATDU0xEPr4hcZ1uULAoGBAI4uGVCJ5nx1nUN4eRx90e5qMXGCCpYZpj1TUT2hQagCstDoV7lRzl/f+/W3ZUI4424iC1Xoa2d2lE+ufYIGujsdWodCXWmGy9v0dhXDcRJYu14VB91xFULbTWd3D0Tyb4lMWAk+RaNSpw7F1loSV2ZtptwIY9c5eqOm/8wr0Az9AoGADMLvlqKHcA4P/RZN34cJKskn15WvltfQ17GBjnOGhBT3B76nOhefESN4tvpY8kFMJNVVwVx0PgVdiCBhdxXrMzs6MGf2DzMp2iwbabaANz1V7tZwUPxikomaaiH6aqhwwjIAkxDqgyk5l3XmaedDLkStACD2bMglsu5UcwZS8F8CgYBLdyyR7NdHIu3dyWLTuEb5vmmZwBoWqoWm3W5MzY46WQEiMsYV0TmfMh7kefAzj6EwiC9ldLxAq/SQGE/8Rz5pXxIuGp444e9M4CSbKj0Gd8i3PoiafKIr+ZrCk7Rtf/a1i3ZkvxdInO/TX15NkuDTvqD1GLRFa42Z8mLrUD2I3g==";

    // 支付宝公钥,查看地址：https://openhome.alipay.com/platform/keyManage.htm 对应APPID下的支付宝公钥。
    public static string alipay_public_key = "MIIBIjANBgkqhkiG9w0BAQEFAAOCAQ8AMIIBCgKCAQEAx4RF3CsyFIZXXECsMo4nPG1Nw+Pz4jJ+si+j8Mih6ZWhmgrhKqk1ZLmAgZlMjL/GuuzcZPKTOzbyI/AxyhJizWXbygjc5+6k/OWA5trZgGJa7/Xna5vTqY5HO4EP0caSW6O9NuRCm2iBrYkJieZvxbiNYKuULtwcM48kRmDB0Kn5ZUiyN6pJAh3AfwAgpPupuUKHmqU881KwLzg2q+Wl3v9fIMPmMSyF6mAduupUMNYksNcZOOWx+jqSonNXOe1R9QpCmgIX4DHCEyMubuZU9XkEYEQnKzwBg+IVXvKVMFIYe79fm3hQhQbZrz1VxAFNBm/G0oELO9L+Uxikqt4xlwIDAQAB";

    // 签名方式
    public static string sign_type = "RSA2";

    // 编码格式
    public static string charset = "UTF-8";
}