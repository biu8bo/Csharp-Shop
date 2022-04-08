using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Commons.Constant
{
    public static class ShopConstants
    {
        public static String NOTIFY = "NOTIFY";
        public static   long ORDER_OUTTIME_UNPAY = 30L;
        public static long ORDER_OUTTIME_UNCONFIRM = 7L;
        public static String REDIS_ORDER_OUTTIME_UNPAY = "order:unpay:";
        public static String REDIS_ORDER_OUTTIME_UNCONFIRM = "order:unconfirm:";
        public static String REDIS_PINK_CANCEL_KEY = "pink:cancel:";
        public static String YSHOP_WEIXIN_PAY_SERVICE = "yshop_weixin_pay_service";
        public static String YSHOP_WEIXIN_MINI_PAY_SERVICE = "yshop_weixin_mini_pay_service";
        public static String YSHOP_WEIXIN_APP_PAY_SERVICE = "yshop_weixin_app_pay_service";
        public static String YSHOP_WEIXIN_MP_SERVICE = "yshop_weixin_mp_service";
        public static String YSHOP_WEIXIN_MA_SERVICE = "yshop_weixin_ma_service";
        public static String YSHOP_DEFAULT_PWD = "123456";
        public static String YSHOP_DEFAULT_AVATAR = "default_avatar.png";
        public static String QQ_MAP_URL = "https://apis.map.qq.com/ws/geocoder/v1/";
        public static String YSHOP_REDIS_INDEX_KEY = "yshop:index_data";
        public static String YSHOP_REDIS_CONFIG_DATAS = "yshop:config_datas";
        public static String YSHOP_RECHARGE_PRICE_WAYS = "yshop_recharge_price_ways";
        public static String YSHOP_HOME_BANNER = "yshop_home_banner";
        public static String YSHOP_HOME_MENUS = "yshop_home_menus";
        public static String YSHOP_HOME_ROLL_NEWS = "yshop_home_roll_news";
        public static String YSHOP_HOT_SEARCH = "yshop_hot_search";
        public static String YSHOP_MY_MENUES = "yshop_my_menus";
        public static String YSHOP_SECKILL_TIME = "yshop_seckill_time";
        public static String YSHOP_SIGN_DAY_NUM = "yshop_sign_day_num";
        public static String YSHOP_ORDER_PRINT_COUNT = "order_print_count";
        public static String YSHOP_FEI_E_USER = "fei_e_user";
        public static String YSHOP_FEI_E_UKEY = "fei_e_ukey";
        public static String YSHOP_ORDER_PRINT_COUNT_DETAIL = "order_print_count_detail";
        public static int YSHOP_SMS_SIZE = 6;
        public static long YSHOP_SMS_REDIS_TIME = 600L;
        public static String YSHOP_ZERO = "0";
        public static String YSHOP_ONE = "1";
        public static int TASK_FINISH_COUNT = 3;
        public static int YSHOP_ONE_NUM = 1;
        public static String YSHOP_ORDER_CACHE_KEY = "yshop:order";
        public static long YSHOP_ORDER_CACHE_TIME = 600L;
        public static String WECHAT_MENUS = "wechat_menus";
        public static String YSHOP_EXPRESS_SERVICE = "yshop_express_service";
        public static String YSHOP_REDIS_SYS_CITY_KEY = "yshop:city_list";
        public static String YSHOP_REDIS_CITY_KEY = "yshop:city";
        public static String YSHOP_APP_LOGIN_USER = "app-online-token:";
        public static String YSHOP_WECHAT_PUSH_REMARK = "eshop为您服务！";
        public static String DEFAULT_UNI_H5_URL = "https://h5.yixiang.co";
        public static String YSHOP_MINI_SESSION_KET = "yshop:session_key:";
        public static String WECHAT_FOLLOW_IMG = "wechat_follow_img";
        public static String ADMIN_API_URL = "admin_api_url";
    }

}
