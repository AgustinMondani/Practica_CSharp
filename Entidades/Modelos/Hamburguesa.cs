using Entidades.Enumerados;
using Entidades.Exceptions;
using Entidades.Files;
using Entidades.Interfaces;
using Entidades.MetodosDeExtension;
using System.Text;
using Entidades.DataBase;

namespace Entidades.Modelos
{
    public class Hamburguesa : IComestible
    {

        private double costo;
        private static int costoBase;
        private bool esDoble;
        private bool estado;
        private string imagen;
        List<EIngrediente> ingredientes;
        Random random;


        public string Ticket => $"{this}\nTotal a pagar:{this.costo}";
        public string Imagen { get => imagen; }
        public bool Estado { get => estado;}


        static Hamburguesa() => Hamburguesa.costoBase = 1500;
        public Hamburguesa(bool esDoble)
        {
            this.esDoble = esDoble;
            this.random = new Random();
        }
        public Hamburguesa() : this(false) { }

        private void AgregarIngredientes()
        {
            ingredientes = random.IngredientesAleatorios();
        }

        private string MostrarDatos()
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.AppendLine($"Hamburguesa {(this.esDoble ? "Doble" : "Simple")}");
            stringBuilder.AppendLine("Ingredientes: ");
            this.ingredientes.ForEach(i => stringBuilder.AppendLine(i.ToString()));
            return stringBuilder.ToString();

        }

        public override string ToString() => this.MostrarDatos();

        public void FinalizarPreparacion(string cocinero)
        {
            costo = ingredientes.CalcularCostoIngredientes(costoBase);
            estado = true;
        }

        public void IniciarPreparacion()
        {
            if (!this.Estado)
            {
                int numeroAleatorio = random.Next(1, 9);
                imagen = DataBaseManager.GetImagenComida($"Hamburguesa_{numeroAleatorio}");
                AgregarIngredientes();
            }
        }

        public void InciarPreparacion()
        {
            throw new NotImplementedException();
        }
    }
}