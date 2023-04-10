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
using System.Windows.Media.Animation;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace Control_Calorias
{
    static class Globals
    {
        public static int MARGIN = 120;
        public static int COLUMN_SEPARATION_PROPORTION = 3;
        public static int COLUMN_WIDTH = 25;
        public static int GENERAL_JUMP = 200;
        public static int ESPECIFICO_JUMP = 50;
        public static float DISCONTINUA_THICKNESS = 0.3F;
        public static string BIN_NAME = "datos.bin";
    }
    public partial class MainWindow : Window
    {
        Modelo miModelo;
        Tablas ventana;

        public MainWindow()
        {
            InitializeComponent();
            miModelo = new Modelo();
            // Datos de prueba
            /*miModelo.MisDias.Add(new Dia("05-12-2021", 133, 47, 237, 419, 222, 0));
            miModelo.MisDias.Add(new Dia("06-12-2021", 224, 0, 150, 400, 200, 20));
            miModelo.MisDias.Add(new Dia("07-12-2021", 120, 256, 312, 230, 100, 120));
            miModelo.MisDias.Add(new Dia("08-12-2021", 184, 267, 260, 320, 120, 20));
            miModelo.MisDias.Add(new Dia("09-12-2021", 200, 400, 150, 200, 250, 0));
            miModelo.MisDias.Add(new Dia("10-12-2021", 24, 246, 167, 345, 200, 18));
            miModelo.MisDias.Add(new Dia("11-12-2021", 44, 200, 0, 310, 193, 20));
            miModelo.MisDias.Add(new Dia("12-12-2021", 80, 300, 200, 400, 100, 0));
            miModelo.MisDias.Add(new Dia("13-12-2021", 100, 134, 156, 0, 212, 32));
            miModelo.MisDias.Add(new Dia("14-12-2021", 89, 91, 245, 0, 214, 0));
            miModelo.MisDias.Add(new Dia("15-12-2021", 74, 0, 89, 450, 178, 0));
            calcularPromedioDiario();*/
            ventana = null;
            miModelo.MisDias.CollectionChanged += MisDias_CollectionChanged;
        }
        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            if(ventana == null)
            {
                ventana = new Tablas(miModelo);
                ventana.Closed += ventana_Closed;
                ventana.MuestraDia += ventana_MuestraDia;
                ventana.MuestraGeneral += ventana_MuestraGeneral;
                ventana.Owner = this;
            }
            ventana.Show();
        }
        public void ventana_Closed(object sender, EventArgs e)
        {
            ventana.Close();
        }
        private void MisDias_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            // en e.NewItems estan los dias que se han añadido a lista MisDias
            if(e.NewItems != null)
            {
                foreach (Dia d in e.NewItems)
                {
                    calcularPromedioDiario();
                    if (d.Total_Calorias >= maxCalDia())
                    {
                        redibujarDias();
                        dibujaEtiquetasGeneral();
                        return;
                    }
                    dibujaDiaGeneral(d);
                    d.PropertyChanged += D_PropertyChanged;
                }
            }
            // en e.OldItems estan los dias que se han eliminado de la lista MisDias
            if (e.OldItems != null)
            {
                redibujarDias();
                dibujaEtiquetasGeneral();
                if (miModelo.MisDias.Count == 0)
                    calcularPromedioDiario();
            }
        }

        private void D_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            redibujarDias();
            dibujaEtiquetasGeneral();
        }
        public void ventana_MuestraDia(object sender, MuestraDiaEventArgs e)
        {
            miCanvas.Visibility = Visibility.Hidden;
            miCanvasDia.Visibility = Visibility.Visible;
            Volver.Visibility = Visibility.Visible;
            Promedio_Diario.Visibility = Visibility.Hidden;
            miCanvasDia.Children.Clear();
            dibujaDiaEspecifico(e.MiDia);
        }
        public void ventana_MuestraGeneral(object sender, MuestraGeneralEventArgs e)
        {
            miCanvasDia.Visibility = Visibility.Hidden;
            miCanvas.Visibility = Visibility.Visible;
            Volver.Visibility = Visibility.Hidden;
            Promedio_Diario.Visibility = Visibility.Visible;
        }
        private void Window_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            redibujarDias();
        }
        private void redibujarDias()
        {
            miCanvas.Children.Clear();
            miCanvasTodos.Children.Clear();
            dibujaEtiquetasGeneral();
            foreach (Dia dia in miModelo.MisDias)
            {
                dibujaDiaGeneral(dia);
                dibujaDiaGeneralTodos(dia);
            }
            miCanvasDia.Children.Clear();
            if(ventana == null)
                return;
            else if(ventana.general.SelectedIndex >= 0 && miModelo.MisDias.Count > 0 && ventana.general.SelectedIndex < miModelo.MisDias.Count)
                dibujaDiaEspecifico(miModelo.MisDias[ventana.general.SelectedIndex]);
        }
        private void dibujaDiaGeneral(Dia dia)
        {
            Line l1 = new Line();
            Line l2 = new Line();
            Line l3 = new Line();
            Line l4 = new Line();
            Line l5 = new Line();
            Line l6 = new Line();
            TextBlock fecha = new TextBlock();
            double maximo = maxCalDia();

            if (maximo == 0)
            {
                fecha.TextAlignment = TextAlignment.Center;
                fecha.Width = miGridCanvas.ActualWidth / Globals.COLUMN_WIDTH + 10;
                Canvas.SetLeft(fecha, Globals.MARGIN + (miModelo.MisDias.IndexOf(dia) * Globals.COLUMN_SEPARATION_PROPORTION * miGridCanvas.ActualWidth / Globals.COLUMN_WIDTH) - l1.StrokeThickness / 2);
                Canvas.SetTop(fecha, miCanvas.ActualHeight - (Globals.MARGIN / 2) + 15);
                if (!miModelo.NightMode)
                    fecha.Foreground = Brushes.Black;
                else
                    fecha.Foreground = Brushes.White;
                fecha.Text = dia.Fecha;
                miCanvas.Children.Add(fecha);
                return;
            }
            l1.StrokeThickness =  miGridCanvas.ActualWidth / Globals.COLUMN_WIDTH;
            l2.StrokeThickness = l3.StrokeThickness = l4.StrokeThickness = l5.StrokeThickness = l6.StrokeThickness = l1.StrokeThickness;
            l1.X1 = Globals.MARGIN + (miModelo.MisDias.IndexOf(dia) * Globals.COLUMN_SEPARATION_PROPORTION * miGridCanvas.ActualWidth / Globals.COLUMN_WIDTH);
            l1.X2 = l2.X1 = l2.X2 = l3.X1 = l3.X2 = l4.X1 = l4.X2 = l5.X1 = l5.X2 = l6.X1 = l6.X2 = l1.X1;
            if (l1.X1 >= miCanvas.ActualWidth - l1.StrokeThickness)
                miCanvas.Width = l1.X1 + Globals.MARGIN;
            l1.Y1 = miCanvas.ActualHeight - (Globals.MARGIN / 2);
            l1.Y2 = l1.Y1 - ((miCanvas.ActualHeight - Globals.MARGIN) * dia.Desayuno / maximo);
            l1.Stroke = new SolidColorBrush(Color.FromRgb(10, 57, 231));
            miCanvas.Children.Add(l1);
            l2.Y1 = l1.Y2;
            l2.Y2 = l2.Y1 - ((miCanvas.ActualHeight - Globals.MARGIN) * dia.Aperitivo / maximo);
            l2.Stroke = new SolidColorBrush(Color.FromRgb(0, 195, 19));
            miCanvas.Children.Add(l2);
            l3.Y1 = l2.Y2;
            l3.Y2 = l3.Y1 - ((miCanvas.ActualHeight - Globals.MARGIN) * dia.Comida / maximo );
            l3.Stroke = new SolidColorBrush(Color.FromRgb(255, 221, 0));
            miCanvas.Children.Add(l3);
            l4.Y1 = l3.Y2;
            l4.Y2 = l4.Y1 - ((miCanvas.ActualHeight - Globals.MARGIN) * dia.Merienda / maximo);
            l4.Stroke = new SolidColorBrush(Color.FromRgb(255, 49, 0));
            miCanvas.Children.Add(l4);
            l5.Y1 = l4.Y2;
            l5.Y2 = l5.Y1 - ((miCanvas.ActualHeight - Globals.MARGIN) * dia.Cena / maximo);
            l5.Stroke = new SolidColorBrush(Color.FromRgb(126, 0, 149));
            miCanvas.Children.Add(l5);
            l6.Y1 = l5.Y2;
            l6.Y2 = l6.Y1 - ((miCanvas.ActualHeight - Globals.MARGIN) * dia.Otros / maximo);
            l6.Stroke = new SolidColorBrush(Color.FromRgb(255, 0, 162));
            miCanvas.Children.Add(l6);
            fecha.TextAlignment = TextAlignment.Center;
            fecha.Width = l1.StrokeThickness + 10;
            Canvas.SetLeft(fecha, l1.X1 - l1.StrokeThickness / 2);
            Canvas.SetTop(fecha, l1.Y1 + 15);
            if (!miModelo.NightMode)
                fecha.Foreground = Brushes.Black;
            else
                fecha.Foreground = Brushes.White;
            fecha.Text = dia.Fecha;
            miCanvas.Children.Add(fecha);
        }
        private void dibujaDiaGeneralTodos(Dia dia)
        {
            Line l1 = new Line();
            Line l2 = new Line();
            Line l3 = new Line();
            Line l4 = new Line();
            Line l5 = new Line();
            Line l6 = new Line();
            TextBlock fecha = new TextBlock();
            double maximo = maxCalDia();

            if (maximo == 0)
                return;
            l1.StrokeThickness = miCanvasTodos.ActualWidth / (miModelo.MisDias.Count * 2);
            l2.StrokeThickness = l3.StrokeThickness = l4.StrokeThickness = l5.StrokeThickness = l6.StrokeThickness = l1.StrokeThickness;
            l1.X1 = (miCanvasTodos.ActualWidth / (miModelo.MisDias.Count * 2)) * (((miModelo.MisDias.IndexOf(dia) + 1) * 2) - 1);
            l1.X2 = l2.X1 = l2.X2 = l3.X1 = l3.X2 = l4.X1 = l4.X2 = l5.X1 = l5.X2 = l6.X1 = l6.X2 = l1.X1;
            l1.Y1 = miCanvasTodos.ActualHeight - Globals.MARGIN / 2;
            l1.Y2 = l1.Y1 - ((miCanvasTodos.ActualHeight - Globals.MARGIN) * dia.Desayuno / maximo);
            l1.Stroke = new SolidColorBrush(Color.FromRgb(10, 57, 231));
            miCanvasTodos.Children.Add(l1);
            l2.Y1 = l1.Y2;
            l2.Y2 = l2.Y1 - ((miCanvasTodos.ActualHeight - Globals.MARGIN) * dia.Aperitivo / maximo);
            l2.Stroke = new SolidColorBrush(Color.FromRgb(0, 195, 19));
            miCanvasTodos.Children.Add(l2);
            l3.Y1 = l2.Y2;
            l3.Y2 = l3.Y1 - ((miCanvasTodos.ActualHeight - Globals.MARGIN) * dia.Comida / maximo);
            l3.Stroke = new SolidColorBrush(Color.FromRgb(255, 221, 0));
            miCanvasTodos.Children.Add(l3);
            l4.Y1 = l3.Y2;
            l4.Y2 = l4.Y1 - ((miCanvasTodos.ActualHeight - Globals.MARGIN) * dia.Merienda / maximo);
            l4.Stroke = new SolidColorBrush(Color.FromRgb(255, 49, 0));
            miCanvasTodos.Children.Add(l4);
            l5.Y1 = l4.Y2;
            l5.Y2 = l5.Y1 - ((miCanvasTodos.ActualHeight - Globals.MARGIN) * dia.Cena / maximo);
            l5.Stroke = new SolidColorBrush(Color.FromRgb(126, 0, 149));
            miCanvasTodos.Children.Add(l5);
            l6.Y1 = l5.Y2;
            l6.Y2 = l6.Y1 - ((miCanvasTodos.ActualHeight - Globals.MARGIN) * dia.Otros / maximo);
            l6.Stroke = new SolidColorBrush(Color.FromRgb(255, 0, 162));
            miCanvasTodos.Children.Add(l6);
            fecha.TextAlignment = TextAlignment.Center;
            fecha.Width = l1.StrokeThickness + 10;
            Canvas.SetLeft(fecha, l1.X1 - (l1.StrokeThickness / 2));
            Canvas.SetTop(fecha, miCanvasTodos.ActualHeight - 15);
            fecha.Text = dia.Fecha;
            if (!miModelo.NightMode)
                fecha.Foreground = Brushes.Black;
            else
                fecha.Foreground = Brushes.White;
            fecha.Text = dia.Fecha;
            miCanvasTodos.Children.Add(fecha);
        }
        private double maxCalDia()
        {
            double maximo = 0;
            foreach(Dia dia in miModelo.MisDias)
            {
                if(dia.Total_Calorias > maximo)
                    maximo = dia.Total_Calorias;
            }
            return maximo;
        }
        private void dibujaEtiquetasGeneral()
        {
            double i, maximo = maxCalDia();

            if (maximo == 0 || miModelo.MisDias.Count == 0)
                return;
            for (i = 0; i <= maximo; i = i + Globals.GENERAL_JUMP)
            {
                TextBlock label = new TextBlock();
                Line discontinua = new Line();
                label.Text = i.ToString();
                label.Height = 15;
                Canvas.SetLeft(label, 0);
                Canvas.SetTop(label, (miCanvas.ActualHeight - Globals.MARGIN / 2) - ((miCanvas.ActualHeight - Globals.MARGIN) * i / maximo));
                if (!miModelo.NightMode)
                    label.Foreground = Brushes.Black;
                else
                    label.Foreground = Brushes.White;
                if (!miModelo.VerTodo)
                    miCanvas.Children.Add(label);
                else
                    miCanvasTodos.Children.Add(label);
                if (miModelo.Cuadricula)
                {
                    discontinua.StrokeThickness = Globals.DISCONTINUA_THICKNESS;
                    discontinua.X1 = 15;
                    discontinua.X2 = miCanvas.ActualWidth;
                    if(!miModelo.VerTodo)
                        discontinua.Y1 = discontinua.Y2 = (miCanvas.ActualHeight - Globals.MARGIN / 2) - ((miCanvas.ActualHeight - Globals.MARGIN) * i / maximo);
                    else
                        discontinua.Y1 = discontinua.Y2 = (miCanvasTodos.ActualHeight - Globals.MARGIN / 2) - ((miCanvas.ActualHeight - Globals.MARGIN) * i / maximo);
                    discontinua.StrokeDashArray = new DoubleCollection() { 10, 10 };
                    discontinua.StrokeDashOffset = 60;
                    if (!miModelo.NightMode)
                        discontinua.Stroke = Brushes.Black;
                    else
                        discontinua.Stroke = Brushes.White;
                    if (!miModelo.VerTodo)
                        miCanvas.Children.Add(discontinua);
                    else
                        miCanvasTodos.Children.Add(discontinua);
                }
            }
        }
        private void dibujaDiaEspecifico(Dia dia)
        {
            Line l1 = new Line();
            l1.Name = "l1";
            Line l2 = new Line();
            l2.Name = "l2";
            Line l3 = new Line();
            l3.Name = "l3";
            Line l4 = new Line();
            l4.Name = "l4";
            Line l5 = new Line();
            l5.Name = "l5";
            Line l6 = new Line();
            l6.Name = "l6";
            double maximo = maxCalComida(dia);

            dibujaEtiquetasEspecifico(dia);
            if (maximo == 0)
                return;
            l1.StrokeThickness = l2.StrokeThickness = l3.StrokeThickness = l4.StrokeThickness = l5.StrokeThickness = l6.StrokeThickness = miCanvasDia.ActualWidth / (6 * 2);
            l1.X1 = l1.X2 = (miCanvasDia.ActualWidth / (6 * 2)) * 1;
            l2.X1 = l2.X2 = (miCanvasDia.ActualWidth / (6 * 2)) * 3;
            l3.X1 = l3.X2 = (miCanvasDia.ActualWidth / (6 * 2)) * 5;
            l4.X1 = l4.X2 = (miCanvasDia.ActualWidth / (6 * 2)) * 7; 
            l5.X1 = l5.X2 = (miCanvasDia.ActualWidth / (6 * 2)) * 9;
            l6.X1 = l6.X2 = (miCanvasDia.ActualWidth / (6 * 2)) * 11;
            l1.Y1 = l2.Y1 = l3.Y1 = l4.Y1 = l5.Y1 = l6.Y1 = miCanvasDia.ActualHeight - (Globals.MARGIN / 2);
            l1.Y2 = l1.Y1 - ((miCanvasDia.ActualHeight - Globals.MARGIN) * dia.Desayuno / maximo);
            l2.Y2 = l2.Y1 - ((miCanvasDia.ActualHeight - Globals.MARGIN) * dia.Aperitivo / maximo );
            l3.Y2 = l3.Y1 - ((miCanvasDia.ActualHeight - Globals.MARGIN) * dia.Comida / maximo);
            l4.Y2 = l4.Y1 - ((miCanvasDia.ActualHeight - Globals.MARGIN) * dia.Merienda / maximo);
            l5.Y2 = l5.Y1 - ((miCanvasDia.ActualHeight - Globals.MARGIN) * dia.Cena / maximo);
            l6.Y2 = l6.Y1 - ((miCanvasDia.ActualHeight - Globals.MARGIN) * dia.Otros / maximo);
            l1.Stroke = new SolidColorBrush(Color.FromRgb(10, 57, 231));
            l2.Stroke = new SolidColorBrush(Color.FromRgb(0, 195, 19));
            l3.Stroke = new SolidColorBrush(Color.FromRgb(255, 221, 0));
            l4.Stroke = new SolidColorBrush(Color.FromRgb(255, 49, 0));
            l5.Stroke = new SolidColorBrush(Color.FromRgb(126, 0, 149));
            l6.Stroke = new SolidColorBrush(Color.FromRgb(255, 0, 162));
            miCanvasDia.Children.Add(l1);
            miCanvasDia.Children.Add(l2);
            miCanvasDia.Children.Add(l3);
            miCanvasDia.Children.Add(l4);
            miCanvasDia.Children.Add(l5);
            miCanvasDia.Children.Add(l6);
            // Animacion caida
            DoubleAnimation doubleAnimation = new DoubleAnimation(l1.Y2, l1.Y1, new Duration(TimeSpan.FromSeconds(2.0)));
            l1.BeginAnimation(Grid.HeightProperty, doubleAnimation);
            doubleAnimation.From = l2.Y2;
            doubleAnimation.To = l2.Y1;
            l2.BeginAnimation(Grid.HeightProperty, doubleAnimation);
            doubleAnimation.From = l3.Y2;
            doubleAnimation.To = l3.Y1;
            l3.BeginAnimation(Grid.HeightProperty, doubleAnimation);
            doubleAnimation.From = l4.Y2;
            doubleAnimation.To = l4.Y1;
            l4.BeginAnimation(Grid.HeightProperty, doubleAnimation);
            doubleAnimation.From = l5.Y2;
            doubleAnimation.To = l5.Y1;
            l5.BeginAnimation(Grid.HeightProperty, doubleAnimation);
            doubleAnimation.From = l6.Y2;
            doubleAnimation.To = l6.Y1;
            l6.BeginAnimation(Grid.HeightProperty, doubleAnimation);
        }
        private double maxCalComida(Dia dia)
        {
            double maximo = dia.Desayuno;
            if(maximo < dia.Aperitivo)
                maximo = dia.Aperitivo;
            if(maximo < dia.Comida)
                maximo = dia.Comida;
            if(maximo < dia.Merienda)
                maximo = dia.Merienda;
            if(maximo < dia.Cena)
                maximo = dia.Cena;
            if(maximo < dia.Otros)
                maximo = dia.Otros;
            return maximo;
        }
        private void dibujaEtiquetasEspecifico(Dia dia)
        {
            double i, maximo = maxCalComida(dia);
            TextBlock desayuno = new TextBlock();
            TextBlock aperitivo = new TextBlock();
            TextBlock comida = new TextBlock();
            TextBlock merienda = new TextBlock();
            TextBlock cena = new TextBlock();
            TextBlock otros = new TextBlock();

            desayuno.Text = "DESAYUNO";
            aperitivo.Text = "APERITIVO";
            comida.Text = "COMIDA";
            merienda.Text = "MERIENDA";
            cena.Text = "CENA";
            otros.Text = "OTROS";
            if (!miModelo.NightMode)
            {
                desayuno.Foreground = Brushes.Black;
                aperitivo.Foreground = Brushes.Black;
                comida.Foreground = Brushes.Black;
                merienda.Foreground = Brushes.Black;
                cena.Foreground = Brushes.Black;
                otros.Foreground = Brushes.Black;
            }
            else
            {
                desayuno.Foreground = Brushes.White;
                aperitivo.Foreground = Brushes.White;
                comida.Foreground = Brushes.White;
                merienda.Foreground = Brushes.White;
                cena.Foreground = Brushes.White;
                otros.Foreground = Brushes.White;
            }
            desayuno.Width = aperitivo.Width = comida.Width = merienda.Width = cena.Width = otros.Width = miCanvasDia.ActualWidth / (6 * 2);
            desayuno.TextAlignment = aperitivo.TextAlignment = comida.TextAlignment = merienda.TextAlignment = cena.TextAlignment = otros.TextAlignment = TextAlignment.Center;
            Canvas.SetLeft(desayuno, (miCanvasDia.ActualWidth / (6 * 4) * 1));
            Canvas.SetLeft(aperitivo, (miCanvasDia.ActualWidth / (6 * 4) * 5));
            Canvas.SetLeft(comida, (miCanvasDia.ActualWidth / (6 * 4) * 9));
            Canvas.SetLeft(merienda, (miCanvasDia.ActualWidth / (6 * 4) * 13));
            Canvas.SetLeft(cena, (miCanvasDia.ActualWidth / (6 * 4) * 17));
            Canvas.SetLeft(otros, (miCanvasDia.ActualWidth / (6 * 4) * 21));
            Canvas.SetTop(desayuno, miCanvasDia.ActualHeight - 15);
            Canvas.SetTop(aperitivo, miCanvasDia.ActualHeight - 15);
            Canvas.SetTop(comida, miCanvasDia.ActualHeight - 15);
            Canvas.SetTop(merienda, miCanvasDia.ActualHeight - 15);
            Canvas.SetTop(cena, miCanvasDia.ActualHeight - 15);
            Canvas.SetTop(otros, miCanvasDia.ActualHeight - 15);
            miCanvasDia.Children.Add(desayuno);
            miCanvasDia.Children.Add(aperitivo);
            miCanvasDia.Children.Add(comida);
            miCanvasDia.Children.Add(merienda);
            miCanvasDia.Children.Add(cena);
            miCanvasDia.Children.Add(otros);
            if (maximo == 0)
                return;
            for (i = 0; i <= maximo; i = i + Globals.ESPECIFICO_JUMP)
            {
                TextBlock label = new TextBlock();
                Line discontinua = new Line();

                if (NightMode.Content.ToString() == "🌙")
                    label.Foreground = Brushes.Black;
                else
                    label.Foreground = Brushes.White;
                    
                label.Text = i.ToString();
                label.Height = 15;
                Canvas.SetLeft(label, 0);
                Canvas.SetTop(label, (miCanvasDia.ActualHeight - (Globals.MARGIN / 2)) - ((miCanvasDia.ActualHeight - Globals.MARGIN) * i / maximo));
                miCanvasDia.Children.Add(label);
                if (miModelo.Cuadricula)
                {
                    if (NightMode.Content.ToString() == "🌙")
                        discontinua.Stroke = Brushes.Black;
                    else
                        discontinua.Stroke = Brushes.White;
                    discontinua.StrokeThickness = Globals.DISCONTINUA_THICKNESS;
                    discontinua.X1 = 15;
                    discontinua.X2 = miCanvasDia.ActualWidth;
                    discontinua.Y1 = discontinua.Y2 = (miCanvasDia.ActualHeight - Globals.MARGIN / 2) - ((miCanvasDia.ActualHeight - Globals.MARGIN) * i / maximo);
                    discontinua.StrokeDashArray = new DoubleCollection() { 10, 10 };
                    discontinua.StrokeDashOffset = Globals.MARGIN;
                    miCanvasDia.Children.Add(discontinua);
                }
            }
        }
        private void miCanvas_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            redibujarDias();
        }
        private void Volver_Click(object sender, RoutedEventArgs e)
        {
            miCanvasDia.Visibility = Visibility.Hidden;
            miCanvas.Visibility = Visibility.Visible;
            Volver.Visibility = Visibility.Hidden;
            ventana.general.SelectedIndex = -1;
        }
        private void MenuItem_Click_VerTodos(object sender, RoutedEventArgs e)
        {
            if(!miModelo.VerTodo)
            {
                miModelo.VerTodo = true;
                Control_VerTodos.Header = "Ver scroll";
                miCanvas.Visibility = Visibility.Hidden;
                miCanvasTodos.Visibility = Visibility.Visible;
            }
            else
            {
                miModelo.VerTodo = false;
                Control_VerTodos.Header = "Ver todo";
                miCanvas.Visibility = Visibility.Visible;
                miCanvasTodos.Visibility = Visibility.Hidden;
            }
            redibujarDias();
        }
        private void NightMode_Click(object sender, RoutedEventArgs e)
        {
            ColorAnimation colorAnimation = new ColorAnimation();
            colorAnimation.Duration = new Duration(TimeSpan.FromSeconds(1.5d));

            if (!miModelo.NightMode)
            {
                miModelo.NightMode = true;
                NightMode.Content = "☀️";
                colorAnimation.From = Colors.White;
                colorAnimation.To = Color.FromRgb(40, 40, 40);
                miMainWindow.Background = new SolidColorBrush(Colors.White);
                miMainWindow.Background.BeginAnimation(SolidColorBrush.ColorProperty, colorAnimation);
                miCanvas.Background = new SolidColorBrush(Colors.White);
                miCanvas.Background.BeginAnimation(SolidColorBrush.ColorProperty, colorAnimation);
                miCanvasTodos.Background = new SolidColorBrush(Colors.White);
                miCanvasTodos.Background.BeginAnimation(SolidColorBrush.ColorProperty, colorAnimation);
                miCanvasDia.Background = new SolidColorBrush(Colors.White);
                miCanvasDia.Background.BeginAnimation(SolidColorBrush.ColorProperty, colorAnimation);
                colorAnimation.From = Color.FromRgb(40, 40, 40);
                colorAnimation.To = Colors.White;
                Promedio_Diario.Foreground = new SolidColorBrush(Color.FromRgb(40, 40, 40));
                Promedio_Diario.Foreground.BeginAnimation(SolidColorBrush.ColorProperty, colorAnimation);
                redibujarDias();
                dibujaEtiquetasGeneral();
                if (ventana != null)
                {
                    if (ventana.general.SelectedIndex >= 0)
                        dibujaDiaEspecifico(miModelo.MisDias[ventana.general.SelectedIndex]);
                }
            }
            else
            {
                miModelo.NightMode = false;
                NightMode.Content = "🌙";
                colorAnimation.From = Color.FromRgb(40, 40, 40);
                colorAnimation.To = Colors.White;
                miMainWindow.Background = new SolidColorBrush(Color.FromRgb(40, 40, 40));
                miMainWindow.Background.BeginAnimation(SolidColorBrush.ColorProperty, colorAnimation);
                miCanvas.Background = new SolidColorBrush(Color.FromRgb(40, 40, 40));
                miCanvas.Background.BeginAnimation(SolidColorBrush.ColorProperty, colorAnimation);
                miCanvasTodos.Background = new SolidColorBrush(Color.FromRgb(40, 40, 40));
                miCanvasTodos.Background.BeginAnimation(SolidColorBrush.ColorProperty, colorAnimation);
                miCanvasDia.Background = new SolidColorBrush(Color.FromRgb(40, 40, 40));
                miCanvasDia.Background.BeginAnimation(SolidColorBrush.ColorProperty, colorAnimation);
                colorAnimation.From = Colors.White;
                colorAnimation.To = Color.FromRgb(40, 40, 40);
                Promedio_Diario.Foreground = new SolidColorBrush(Color.FromRgb(40, 40, 40));
                Promedio_Diario.Foreground.BeginAnimation(SolidColorBrush.ColorProperty, colorAnimation);
                redibujarDias();
                dibujaEtiquetasGeneral();
                if (ventana != null)
                {
                    if (ventana.general.SelectedIndex >= 0)
                        dibujaDiaEspecifico(miModelo.MisDias[ventana.general.SelectedIndex]);
                }
            }
        }
        private void calcularPromedioDiario()
        {
            double promedio = 0;
            if (miModelo.MisDias.Count == 0)
            {
                Promedio_Diario.Visibility = Visibility.Collapsed;
            }
            else
            {
                Promedio_Diario.Visibility = Visibility.Visible;
                foreach (Dia dia in miModelo.MisDias)
                {
                    promedio += dia.Total_Calorias;
                }
                promedio /= miModelo.MisDias.Count + 1;
                Promedio_Diario.Content = "Promedio diario : " + promedio.ToString("0.##");
            }
        }
        private void MenuItem_Click_ActivarCuadricula(object sender, RoutedEventArgs e)
        {
            if (!miModelo.Cuadricula)
            {
                miModelo.Cuadricula = true;
                Control_Cuadricula.Header = "Desactivar cuadricula";
                dibujaEtiquetasGeneral();
                if(ventana != null)
                {
                    if (ventana.general.SelectedIndex >= 0)
                        dibujaDiaEspecifico(miModelo.MisDias[ventana.general.SelectedIndex]);
                }
            }
            else
            {
                miModelo.Cuadricula = false;
                Control_Cuadricula.Header = "Activar cuadricula";
                miCanvas.Children.Clear();
                redibujarDias();
                dibujaEtiquetasGeneral();
                if(ventana != null)
                {
                    if (ventana.general.SelectedIndex >= 0)
                        dibujaDiaEspecifico(miModelo.MisDias[ventana.general.SelectedIndex]);
                }
            }
        }
        private void MenuItem_Click_Exportar(object sender, RoutedEventArgs e)
        {
            string fileName = Globals.BIN_NAME;
            IFormatter formatter = new BinaryFormatter();
            Stream stream = new FileStream(fileName, FileMode.Create, FileAccess.Write);
            formatter.Serialize(stream, miModelo);
            stream.Close();
            MessageBoxResult result = System.Windows.MessageBox.Show("Exportación realizada con éxito", "Exportar", MessageBoxButton.OK);
        }

        private void MenuItem_Click_Importar(object sender, RoutedEventArgs e)
        {
            Modelo miModeloAux;
            string fileName = Globals.BIN_NAME;
            IFormatter formatter = new BinaryFormatter();
            Stream stream = new FileStream(fileName, FileMode.Open, FileAccess.Read);
            miModeloAux = (Modelo)formatter.Deserialize(stream);
            miModelo.MisDias.Clear();
            foreach(Dia d in miModeloAux.MisDias)
                miModelo.MisDias.Add(d);
            redibujarDias();
            calcularPromedioDiario();
            stream.Close();
            MessageBoxResult result = System.Windows.MessageBox.Show("Importación realizada con éxito", "Importar", MessageBoxButton.OK);
        }
    }
}