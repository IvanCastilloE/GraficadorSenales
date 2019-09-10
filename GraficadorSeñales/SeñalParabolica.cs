using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraficadorSeñales
{
    class SeñalParabolica:Señal
    {

        public SeñalParabolica()
        {
            Muestras = new List<Muestra>();
            AmplitudMaxima = 0.0;
        }
        override public double evaluar(double tiempo)
        {
            double resultado;
            //Condicion de la formula de la parabola
            if (tiempo >= 0)
            {
                //Para darle un exponente a un numero usa  (Math.Pow(base,exponente)
                resultado = (tiempo * tiempo) / 2.0;

            }
            else
                resultado = 0;

            return resultado;
        }
    }
}
