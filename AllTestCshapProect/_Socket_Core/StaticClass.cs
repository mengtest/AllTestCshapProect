using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace _Socket_Core {
    public static class StaticClass {
        public static GIPEndPoint AsGIPEndPoint (this IPEndPoint payment) {
            return new GIPEndPoint() {
                Ip = payment.Address.ToString() ,
                Port = payment.Port
            };
        }
        public static GIPEndPoint AsGIPEndPoint (this EndPoint _payment) {
            IPEndPoint payment = (IPEndPoint)_payment;
            return new GIPEndPoint() {
                Ip = payment.Address.ToString() ,
                Port = payment.Port
            };
        }
        public static IPEndPoint AsIPEndPoint (this GIPEndPoint payment) {
            return new IPEndPoint(IPAddress.Parse(payment.Ip) , payment.Port);
        }
        public static EndPoint AsEndPoint (this GIPEndPoint payment) {
            return payment.AsIPEndPoint();
        }
    }

}
