﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediaEncoder.Domain
{
    public enum ItemStatus
    {
        Ready,//任务刚创建完成
        Started,//开始处理
        Completed,//成功
        Failed,//失败
    }
}
