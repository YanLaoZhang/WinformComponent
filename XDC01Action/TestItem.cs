using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XDC01Action
{
    public class TestItem
    {
        // 测项名称
        public string Name { get; set; } = string.Empty;
        // 测项标准值
        public string Standard { get; set; } = string.Empty;
        // 测项最小值
        public float MinValue { get; set; } = 0.0f;
        // 测项最大值
        public float MaxValue { get; set; } = 0.0f;
        // 测项测量值
        public float Value { get; set; } = 0.0f;
        // 测项非数值测量值
        public string StrVal { get; set; } = string.Empty;
        // 测项结果
        public string Result { get; set; } = string.Empty;
        // 测项ng_item
        public string NgItem { get; set; } = string.Empty;
        // 测项耗时
        public float Duration { get; set; } = 0.0f;

    }
}
