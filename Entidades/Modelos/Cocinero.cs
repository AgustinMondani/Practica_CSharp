using Entidades.DataBase;
using Entidades.Exceptions;
using Entidades.Files;
using Entidades.Interfaces;


namespace Entidades.Modelos
{
    public delegate void DelegafoDemoraAtencion(double demora);
    public delegate void DelegadoNuevoIngreso(IComestible menu);

    public class Cocinero<T> where T : IComestible, new() 
    {
        private CancellationTokenSource cancellation;
        private int cantPedidosFinalizados;
        private double demoraPreparacionTotal;
        private string nombre;
        private T menu;
        private Task tarea;

        public event DelegafoDemoraAtencion OnDemora;
        public event DelegadoNuevoIngreso OnIngreso;
        public Cocinero(string nombre)
        {
            this.nombre = nombre;
        }

        //No hacer nada
        public bool HabilitarCocina
        {
            get
            {
                return this.tarea is not null && (this.tarea.Status == TaskStatus.Running ||
                    this.tarea.Status == TaskStatus.WaitingToRun ||
                    this.tarea.Status == TaskStatus.WaitingForActivation);
            }
            set
            {
                if (value && !this.HabilitarCocina)
                {
                    this.cancellation = new CancellationTokenSource();
                    this.IniciarIngreso();
                }
                else
                {
                    this.cancellation.Cancel();
                }
            }
        }

        //no hacer nada
        public double TiempoMedioDePreparacion { get => this.cantPedidosFinalizados == 0 ? 0 : this.demoraPreparacionTotal / this.cantPedidosFinalizados; }
        public string Nombre { get => nombre; }
        public int CantPedidosFinalizados { get => cantPedidosFinalizados; }

        private void IniciarIngreso()
        {
            tarea = new Task(() =>
            {
                if (!this.cancellation.IsCancellationRequested)
                {
                    NotificarNuevoIngreso();
                    EsperarProximoIngreso();
                    cantPedidosFinalizados += 1;
                    DataBaseManager.GuardarTicket(nombre, menu);
                }
            });
        }

        private void NotificarNuevoIngreso()
        {
            if(OnIngreso is not null)
            {
                menu = new T();
                menu.InciarPreparacion();
                OnIngreso(menu);
            }
        }
        private void EsperarProximoIngreso()
        {
            int tiempoTrascurrido = 0;
            if(OnDemora is not null)
            {
                while (cancellation.IsCancellationRequested.Equals(false) && !menu.Estado)
                {
                    OnDemora.Invoke(tiempoTrascurrido);
                    Thread.Sleep(1000);
                    tiempoTrascurrido ++;
                }
                this.demoraPreparacionTotal += tiempoTrascurrido;
            }
        }
    }
}
