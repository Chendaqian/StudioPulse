using System.Collections.Generic;
using System.Windows.Controls;
using System.Windows.Media;

namespace StudioPulse
{
    /// <summary>
    /// 把同一个指标的多个文本块统一广播更新
    /// </summary>
    internal class MetricTextBlockCollection : List<TextBlock>
    {
        public Brush Foreground
        {
            set
            {
                foreach (TextBlock textBlock in this)
                {
                    textBlock.Foreground = value;
                }
            }
        }

        public string Text
        {
            set
            {
                foreach (TextBlock textBlock in this)
                {
                    textBlock.Text = value;
                }
            }
        }
    }
}