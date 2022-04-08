using Fleck;
using Mapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Commons.BaseModels
{
    /// <summary>
    /// web套接字模块
    /// </summary>
   public class WebSocketService
    {
        //存储所有连接状态
    public static List<IWebSocketConnection> Sockets = new List<IWebSocketConnection>();
        
        //开启套接字连接
        public static void WebSocketStart()
        {

            var server = new WebSocketServer("ws://127.0.0.1:8008");
            server.Start(socket =>
            {
                socket.OnOpen = () =>
                {
                    WebSocketService.Sockets.Add(socket);
                    int count;
                    using (var db = new eshoppingEntities())
                    {
                    count =  db.Set<store_order>().Include("store_order_cart_info").Where(e => e.is_del == false).Where(e => e.paid == true && e.status == 0).Count();
                    }
                    socket.Send("即时通讯服务开启成功！");
                    socket.Send($"您有{count}条订单信息待处理");
                };
                socket.OnClose = () =>
                {
                    WebSocketService.Sockets.Add(socket);
                };
                socket.OnMessage = message =>
                {
                    WebSocketService.Sockets.ToList().ForEach(s => s.Send(message));
                };
            });
        }

        //发送消息
        public static void SendMessage(string message) {
                foreach (var socket in Sockets.ToList())
                {
                    socket.Send(message);
                }
            }

    }
}
