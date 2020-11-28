using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMY.HostManager.Bussines
{
    public class ResultModel
    {
        public ResultModel()
        {
            IsSuccess = false;
            Message = "";
        }
        public bool IsSuccess { get; set; }
        public int resultType { get; set; }
        public string Message { get; set; }
        public object Data { get; set; }
    }

}



