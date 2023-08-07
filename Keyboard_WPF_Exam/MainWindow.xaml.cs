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
using System.Windows.Threading;


namespace Keyboard_WPF_Exam
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {


        int tempTimer = 0;
        int fails = 0;
        Random rnd = new Random();
        string baseString = "1234567890QWER TYUIOPASDFGHJKLZXCV BNMabcdefghijklmnopqrstuvwxyz "; // основная строка
        
        bool flagCaps = true;
        bool flagBackSpace = true;
        bool mesStop = true;

        DispatcherTimer timer = null;
        List<Button> buttons = new List<Button>();
        



        public MainWindow()
        {
            InitializeComponent();
            timer = new DispatcherTimer();
            timer.Interval = new TimeSpan(0, 0, 0, 0, 1000);
            timer.Tick += Timer_Tick;
            buttons.AddRange(new List<Button> { this.A,this.B,this.C,this.D,this.E,this. F,this.G,this.H,this.I,this.J,this.K,this.L,this.M,this.N,this.O,this.P,
                this.Q,this.R,this.S,this.T,this.U,this.V,this.W,this.X,this.Y,this.Z });

            //lineUser.Text = "123456789";
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            tempTimer++;           
            Speed();
        }

        void Speed()
        {
            SpeedChar.Content = Math.Round(((double)lineUser.Text.Length / tempTimer) * 60).ToString(); //вычисляем скорость печатания
        }

       void CapLetters()            //меняем контекст кнопок на заглавные буквы
        {
           foreach(var el in buttons) 
            {
               el.Content = el.Content.ToString().ToUpper();
            }
      
        }
        void LowLetters()
        {
            foreach (var el in buttons)
            {
                el.Content = el.Content.ToString().ToLower();
            }
        } // меняем контекст кнопок на строчные буквы







        private void SliderDifficulty_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)   //значение слайдера передаем в лэйбл
        {
            int difficult = 0;
            Slider s = sender as Slider;
            difficult = (int)s.Value;
            Difficulty.Content = difficult.ToString();
        }


        private void CaseSensetive_Unchecked(object sender, RoutedEventArgs e)
        {
            SliderDifficulty.Maximum = 10;
        } //проверяем чекбокс на усложнение
        private void CaseSensetive_Checked(object sender, RoutedEventArgs e)
        {
            SliderDifficulty.Maximum = 20;
        }//проверяем чекбокс на усложнение


        private void Start_Click(object sender, RoutedEventArgs e)          //начинаем работать) 
        {
            Start.IsEnabled = false;
            SliderDifficulty.IsEnabled = false;
            CaseSensitive.IsEnabled = false;
            Stop.IsEnabled = true;
            tempTimer = 0;
            timer.Start();
           
            CollectSrting(Convert.ToInt32(Difficulty.Content), baseString, (bool)CaseSensitive.IsChecked);   //генерация рандомной строки
            lineUser.IsReadOnly = false;
            lineUser.IsEnabled = true;
            lineUser.Focus();
        }   // 
        private void CollectSrting(int countChar, string baseString, bool flagSensitive)    //каунтчар - наша сложность , базовая коллекция символов , чекбокс усложнения
        {
            string chars = "";
            
            int sensitive = flagSensitive ? 10 : 1;         //если 

            countChar = countChar + sensitive;
            for(int i = 0; i < countChar; i++)
            {
                chars += baseString[rnd.Next(0, baseString.Length)];

            }
            if (!flagSensitive)
            { chars = chars.ToLower();
                countChar =30 + sensitive;
            }
            else 
            {
                countChar = 50 + sensitive * 2;
            }

                
            for (int i = 0; i < countChar; i++)
            {
                linePrograms.Text += chars[rnd.Next(0, chars.Length)];
            }
        }
        private void Stop_Click(object sender, RoutedEventArgs e)
        {
            Start.IsEnabled = true;
            SliderDifficulty.IsEnabled = true;
            CaseSensitive.IsEnabled=true;
            Stop.IsEnabled = false;
            lineUser.Text = "";
            linePrograms.Text = "";
            Fails.Content = 0;
            SpeedChar.Content = 0;
            lineUser.IsReadOnly = true;
            lineUser.IsEnabled = false;
            timer.Stop();
            tempTimer = 0;
            fails = 0;

        }

        private void lineUser_TextChanged(object sender, TextChangedEventArgs e)
        {
            string str = linePrograms.Text.Substring(0, lineUser.Text.Length);
            if (lineUser.Text.Equals(str))
            {
                if (flagBackSpace)
                {
                    Speed();
                }
                lineUser.Foreground = new SolidColorBrush(Colors.Black);
            }
            else
            {
                if (flagBackSpace)
                {
                    fails++;
                }
                lineUser.Foreground = new SolidColorBrush(Colors.Red);
                Fails.Content = fails;
            }
            if (lineUser.Text.Length == linePrograms.Text.Length)
            {
                timer.Stop();
                Speed();
                lineUser.IsReadOnly = true;
                mesStop = false;
            }
        }


        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            lineUser.Focus();
            foreach (var el in (this.Content as Grid).Children)          //приводим елементы  формы к гриду ( у нас все в гридах) и работаем с коллекцией
                                                                         //элементов формы как с гридами
            {
                if (el is Grid)                  //если все таки это грид то 
                {
                    foreach (var item in (el as Grid).Children)          //берем коллекцию детей грида
                    {

                        if (item is Button)             //и если вдруг этот елемент кнопка
                        {
                            if ((item as Button).Name == e.Key.ToString() || (item as Button).Name == e.SystemKey.ToString())                  //и если вдруг имя кнопки совпадает с нашей оригинальной кнопкой
                            {
                                (item as Button).Opacity = 0.5;                             //делаем ету кнопку прозрачненькой
                                if (e.Key.ToString().ToLower() == "LeftShift".ToLower() || e.Key.ToString().ToLower() == "RightShift".ToLower())      //если вдруг мы нажали шифт
                                {
                                    if (flagCaps)
                                    {
                                        CapLetters();// переводим наши буквы в верхний регистр
                                    }

                                    else
                                    {
                                        LowLetters();// переводим наши буквы в нижний регистр
                                    }


                                }

                                else if (e.Key.ToString() == "Capital")         //если нажат капслок
                                {
                                    if (flagCaps)
                                    {
                                        CapLetters();// переводим наши буквы в верхний регистр
                                        flagCaps = false;
                                    }

                                    else
                                    {
                                        LowLetters();
                                        flagCaps = true;
                                    }
                                }
                                else if (e.Key.ToString() == "Back")
                                {
                                    flagBackSpace = false;
                                }
                                else flagBackSpace = true;

                            }

                        }

                    }

                }
            }



        }

        private void Window_KeyUp(object sender, KeyEventArgs e)
        {
            lineUser.Focus();
            foreach(var el in (this.Content as Grid).Children) 
            {
                if(el is Grid)
                {
                    foreach(var item in (el as Grid).Children) 
                    {
                        if(item is Button) 
                        {

                            (item as Button).Opacity = 1;
                            if(e.Key.ToString() == "LeftShift" || e.Key.ToString() == "RightShift")
                            {


                                if (flagCaps)
                                {
                                    LowLetters();

                                }
                                else
                                {
                                    CapLetters();
                                }

                            }
                        }
                    
                    }
                }
            
            }
            if(!mesStop) 
            {

                MessageBox.Show($"Задание выполнено , ошибок {fails} , сложность {Difficulty.Content}");
                mesStop = true;
            }
        }

     

    }
}
