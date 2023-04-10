using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.Runtime.Serialization;


namespace Control_Calorias
{
    [Serializable]
    public class Modelo
    {
        private ObservableCollection<Dia> misDias { get; set; }
        private Boolean cuadricula;
        private Boolean verTodo;
        private Boolean nightMode;
        public ObservableCollection<Dia> MisDias
        {
            get { return this.misDias; }
            set { this.misDias = value; }
        }
        public Boolean Cuadricula
        {
            get { return this.cuadricula; }
            set { this.cuadricula = value; }
        }
        public Boolean VerTodo
        {
            get { return this.verTodo; }
            set { this.verTodo = value; }
        }
        public Boolean NightMode
        {
            get { return this.nightMode; }
            set { this.nightMode = value; }
        }
        public Modelo()
        {
            misDias = new ObservableCollection<Dia>();
            cuadricula = false;
            verTodo = false;
            nightMode = false;
        }
        public Boolean Aniade(Dia dia)
        {
            foreach (Dia d in misDias)
            {
                if (d.Fecha == dia.Fecha)
                {
                    return false;
                }
            }
            misDias.Add(dia);
            //misDias = new ObservableCollection<Dia>(from diaToOrder in misDias orderby diaToOrder.Fecha.Substring(6,4), diaToOrder.Fecha.Substring(3,2), diaToOrder.Fecha.Substring(0,2) select diaToOrder);
            return true;
        }
        public Boolean Elimina(Dia dia)
        {
            return misDias.Remove(dia);
        }
        public Boolean Modifica(int indice, Dia dia)
        {
            if (misDias.Count < indice)
                return false;
            misDias[indice] = dia;
            return true;
        }
    }
}