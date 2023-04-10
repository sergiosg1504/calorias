using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Windows.Shapes;
using System.Windows;
using System.Runtime.Serialization;

namespace Control_Calorias
{
    [Serializable]
    public class Dia : INotifyPropertyChanged
    {
        [field: NonSerializedAttribute()] public event PropertyChangedEventHandler PropertyChanged;
        private String fecha;
        private double desayuno;
        private double aperitivo;
        private double comida;
        private double merienda;
        private double cena;
        private double otros;
        // Método ON
        protected void OnPropertyChanged(String propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
        // Getters y setters de las variables private
        public String Fecha
        {
            get { return this.fecha; }
            set { this.fecha = value; OnPropertyChanged("fecha"); }
        }
        public double Desayuno
        {
            get { return this.desayuno; }
            set { this.desayuno = value; OnPropertyChanged("desayuno"); }
        }
        public double Aperitivo
        {
            get { return this.aperitivo; }
            set { this.aperitivo = value; OnPropertyChanged("aperitivo"); }
        }
        public double Comida
        {
            get { return this.comida; }
            set { this.comida = value; OnPropertyChanged("comida"); }
        }
        public double Merienda
        {
            get { return this.merienda; }
            set { this.merienda = value; OnPropertyChanged("merienda"); }
        }
        public double Cena
        {
            get { return this.cena; }
            set { this.cena = value; OnPropertyChanged("cena"); }
        }
        public double Otros
        {
            get { return this.otros; }
            set { this.otros = value; OnPropertyChanged("otros"); }
        }
        public double Total_Calorias
        {
            get { return this.desayuno + this.aperitivo + this.comida + this.merienda + this.cena + this.otros; }
        }
        // Constructor de Dia
        public Dia(String fecha, double desayuno, double aperitivo, double comida, double merienda, double cena, double otros)
        {
            this.fecha = fecha;
            this.desayuno = desayuno;
            this.aperitivo = aperitivo;
            this.comida = comida;
            this.merienda = merienda;
            this.cena = cena;
            this.otros = otros;
            Console.WriteLine("des" + desayuno);
            Console.WriteLine("ape" + aperitivo);
        }

    }
}
