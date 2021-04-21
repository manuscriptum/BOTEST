using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Linq.Expressions;
using System.Security.AccessControl;
using System.Threading;
using QuikSharp;
using QuikSharp.DataStructures;
using QuikSharp.DataStructures.Transaction;
using System.IO;
using System.Xml.Serialization;
using System.Globalization;

namespace BOTEST
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Quik _quik;
        private string secCode = "SiM1"; // "SBER";
        private string classCode = "SPBFUT"; // "QJSIM";
        private string clientCode;
        private Tool tool;

        public MainWindow()
        {
            InitializeComponent();
        }
        private void Monitor(string str)
        {
            try
            {
                this.Dispatcher.Invoke(() =>
                {
                    MonitorTextBox.AppendText(str + Environment.NewLine);
                    MonitorTextBox.ScrollToLine(MonitorTextBox.LineCount - 1);
                });
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }
        void Run() // все что стартует по кнопке Запуск
        {
            _quik = new Quik(Quik.DefaultPort, new InMemoryStorage());
            try
            {
                Monitor("Определяем код класса инструмента " + secCode + ", по списку классов" + "...");
                try
                {
                    // classCode = _quik.Class.GetSecurityClass("SPBFUT,TQBR,TQBS,TQNL,TQLV,TQNE,TQOB,QJSIM", secCode).Result;
                }
                catch
                {
                    Monitor("Ошибка определения класса инструмента. Убедитесь, что тикер указан правильно");
                }
                if (classCode != null && classCode != "")
                {
                    Monitor("Определяем код клиента...");
                    Monitor("Читаем значения полей из формы...");
                    Monitor("Class code: " + classCode);
                    Monitor("Security code: " + secCode);
                    clientCode = _quik.Class.GetClientCode().Result;
                    Monitor("Создаем экземпляр инструмента " + secCode + " | " + classCode + "...");
                    tool = new Tool(_quik, secCode, classCode);
                    Monitor("Account ID 1:" + tool.AccountID + " Firm Id 1:" + tool.FirmID + " Client Code:" + clientCode);
                }
            }
            catch
            {
                Monitor("Ошибка получения данных по инструменту.");
                Monitor("Реальный код бумаги " + secCode);
                Monitor("Реальный код класса " + classCode);
            }
            OrderListPrint();
        }

        public void OrderListPrint()
        {
            var orders = _quik.Orders.GetOrders().Result;
            Monitor(" @@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@ ");
            Monitor(" @@@  Список номеров заявок из таблицы <Orders> @@@ ");
            foreach (var order in orders)
            {
                Monitor("@ " + order.OrderNum.ToString());
            }
            Monitor(" __________________________________________________ ");
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Run();
        }
    }
}
