using Entidades.Enumerados;


namespace Entidades.MetodosDeExtension
{
    public static class IngredientesExtension
    {
        public static double CalcularCostoIngredientes(this List<EIngrediente> ingredientes, int costoInicial)
        {
            double costoFinal = costoInicial;
            foreach(EIngrediente i in ingredientes) 
            {
                costoFinal += (int)i;
            }

            return costoFinal;
        }

        public static List<EIngrediente> IngredientesAleatorios(this Random rand)
        {
            int numeroRandom = rand.Next(1, 6);


            List<EIngrediente> ingredientes = new List<EIngrediente>()
            {
                EIngrediente.QUESO,
                EIngrediente.PANCETA,
                EIngrediente.ADHERESO,
                EIngrediente.HUEVO,
                EIngrediente.JAMON,
            };

            return ingredientes.Take(numeroRandom).ToList();
        }

    }
}
