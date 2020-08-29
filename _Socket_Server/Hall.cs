using System.Collections.Generic;
using System.Net;

namespace _Socket_Server
{
	public class Hall {
        private static Hall self;
        public static Hall Self {
            get {
                if (self == null) {
                    self = new Hall();
                }
                return self;
            }
        }
    }
}