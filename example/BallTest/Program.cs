using System;
using System.Windows.Forms;
using AntdUI;

namespace BallTest
{
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            
            // 创建并显示圆球窗体
            var ballForm = new BallLayeredForm();
            ballForm.Show();
            
            // 运行应用程序消息循环
            Application.Run();
        }
    }
}