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
using System.Windows.Shapes;
using System.Windows.Media.Animation;
using Control_Calorias;

namespace Control_Calorias
{
    public delegate void MuestraDiaEventArgsHandler(object sender, MuestraDiaEventArgs e);
    public class MuestraDiaEventArgs : EventArgs
    {
        public Dia MiDia { get; set; }
    }
    public delegate void MuestraGeneralEventArgsHandler(object sender, MuestraGeneralEventArgs e);
    public class MuestraGeneralEventArgs : EventArgs
    {
        public Dia MiDia { get; set; }
    }
    public partial class Tablas : Window
    {
        private Modelo miModelo;
        public event MuestraDiaEventArgsHandler MuestraDia;
        public event MuestraGeneralEventArgsHandler MuestraGeneral;
        private int indice;
        public int General
        {
            get { return this.indice; }
            set { this.indice = value; general.SelectedIndex = this.indice;  }
        }
        public Tablas(Modelo modelo)
        {
            InitializeComponent();
            miModelo = modelo;
            indice = general.SelectedIndex;
            general.ItemsSource = modelo.MisDias;
            TextBox_Desayuno.BorderBrush = Brushes.LightGray;
            TextBox_Aperitivo.BorderBrush = Brushes.LightGray;
            TextBox_Comida.BorderBrush = Brushes.LightGray;
            TextBox_Merienda.BorderBrush = Brushes.LightGray;
            TextBox_Cena.BorderBrush = Brushes.LightGray;
            TextBox_Otros.BorderBrush = Brushes.LightGray;
        }
        void OnMuestraDia(MuestraDiaEventArgs e)
        {
            MuestraDia?.Invoke(this, e);
        }
        void OnMuestraGeneral(MuestraGeneralEventArgs e)
        {
            MuestraGeneral?.Invoke(this, e);
        }
        private void Aniadir_Click(object sender, RoutedEventArgs e)
        {
            Double desayuno, aperitivo, comida, merienda, cena, otros;
            desayuno = aperitivo = comida = merienda = cena = otros = 0;

            if (fechaIsValid(TextFecha.Text))
            {
                if (!TextDesayuno.Text.ToString().Equals(""))
                    desayuno = Convert.ToDouble(TextDesayuno.Text);
                if (!TextAperitivo.Text.ToString().Equals(""))
                    aperitivo = Convert.ToDouble(TextAperitivo.Text);
                if (!TextComida.Text.ToString().Equals(""))
                    comida = Convert.ToDouble(TextComida.Text);
                if (!TextMerienda.Text.ToString().Equals(""))
                    merienda = Convert.ToDouble(TextMerienda.Text);
                if (!TextCena.Text.ToString().Equals(""))
                    cena = Convert.ToDouble(TextMerienda.Text);
                if (!TextOtros.Text.ToString().Equals(""))
                    otros = Convert.ToDouble(TextOtros.Text);
                if (miModelo.Aniade(new Dia(TextFecha.Text, desayuno, aperitivo, comida, merienda, cena, otros)))
                {
                    general.ItemsSource = miModelo.MisDias;
                    Status.Content = "La información se ha añadido";
                    TextFecha.Text = "";
                    TextDesayuno.Text = "";
                    TextAperitivo.Text = "";
                    TextComida.Text = "";
                    TextMerienda.Text = "";
                    TextCena.Text = "";
                    TextOtros.Text = "";
                }
                else
                    Status.Content = "La información NO se ha añadido,\nel dia ya existe";
            }
            else
                Status.Content = "La información NO se ha añadido,\nrevise la fecha";
        }
        private Boolean fechaIsValid(String fecha)
        {
            int i;
            string numeros = "0123456789";
            if(fecha[2] != '-' || fecha[5] != '-' || fecha.Length != 10)
                return false;
            for(i=0; i<10; i++)
            {
                if (i == 2 || i == 5)
                    continue;
                if (!numeros.Contains(fecha[i]))
                    return false;
            }
            return true;
        }
        private void Eliminar_Click(object sender, RoutedEventArgs e)
        {
            miModelo.Elimina((Dia)general.SelectedItem);
        }

        private void Modificar_Click(object sender, RoutedEventArgs e)
        {
            String fecha = miModelo.MisDias[general.SelectedIndex].Fecha;
            if (Modificar.Content.ToString() == "Modificar dia")
            {
                TextBox_Desayuno.Background = Brushes.White;
                TextBox_Aperitivo.Background = Brushes.White;
                TextBox_Comida.Background = Brushes.White; 
                TextBox_Merienda.Background = Brushes.White;
                TextBox_Cena.Background = Brushes.White;
                TextBox_Otros.Background = Brushes.White;
                Modificar.Content = "Confirmar modificación";
            }
            else
            {
                Dia dia = new Dia(fecha, Convert.ToInt32(TextBox_Desayuno.Text), Convert.ToInt32(TextBox_Aperitivo.Text), Convert.ToInt32(TextBox_Comida.Text),
                    Convert.ToInt32(TextBox_Merienda.Text), Convert.ToInt32(TextBox_Cena.Text), Convert.ToInt32(TextBox_Otros.Text));
                miModelo.Modifica(general.SelectedIndex, dia);
                TextBox_Desayuno.BorderBrush = Brushes.LightGray;
                TextBox_Aperitivo.BorderBrush = Brushes.LightGray;
                TextBox_Comida.BorderBrush = Brushes.LightGray;
                TextBox_Merienda.BorderBrush = Brushes.LightGray;
                TextBox_Cena.BorderBrush = Brushes.LightGray;
                TextBox_Otros.BorderBrush = Brushes.LightGray;
                TextBox_Desayuno.Background = Brushes.LightGray;
                TextBox_Aperitivo.Background = Brushes.LightGray;
                TextBox_Comida.Background = Brushes.LightGray;
                TextBox_Merienda.Background = Brushes.LightGray;
                TextBox_Cena.Background = Brushes.LightGray;
                TextBox_Otros.Background = Brushes.LightGray;
            }
            
        }

        private void General_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            MuestraGeneralEventArgs arg;
            MuestraDiaEventArgs arg2;
            int seleccion = general.SelectedIndex;
            
            if(seleccion < 0)
            {
                especifico.Visibility = Visibility.Collapsed;
                Borde_Eliminar.Visibility = Visibility.Collapsed;
                Modificar.Content = "Modificar dia";
                Borde_Modificar.Visibility = Visibility.Collapsed;
                TextBox_Desayuno.DataContext = null;
                TextBox_Aperitivo.DataContext = null;
                TextBox_Comida.DataContext = null;
                TextBox_Merienda.DataContext = null;
                TextBox_Cena.DataContext = null;
                TextBox_Otros.DataContext = null;
                arg = new MuestraGeneralEventArgs();
                OnMuestraGeneral(arg);
            }
            else
            {
                Dia miDia = miModelo.MisDias[seleccion];
                especifico.Visibility = Visibility.Visible;
                Borde_Eliminar.Visibility = Visibility.Visible;
                Borde_Modificar.Visibility = Visibility.Visible;
                TextBox_Desayuno.DataContext = general.SelectedItem;
                TextBox_Aperitivo.DataContext = general.SelectedItem;
                TextBox_Comida.DataContext = general.SelectedItem;
                TextBox_Merienda.DataContext = general.SelectedItem;
                TextBox_Cena.DataContext = general.SelectedItem;
                TextBox_Otros.DataContext = general.SelectedItem;
                arg2 = new MuestraDiaEventArgs();
                arg2.MiDia = miDia;
                OnMuestraDia(arg2);
            }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Hide();
            e.Cancel = true;
        }

        private void especifico_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            DoubleAnimation miDoubleAnimation = new DoubleAnimation(0.0d, 140.0d, new Duration(TimeSpan.FromSeconds(1.5d)));
            especifico.BeginAnimation(Grid.HeightProperty, miDoubleAnimation);
        }
    }
}